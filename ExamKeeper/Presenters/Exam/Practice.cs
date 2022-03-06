using System;
using System.Collections.Generic;
using System.Linq;
using ExamKeeper.Models;
using ExamKeeper.Resources;
using ExamKeeper.Views;
using Utils;
using Utils.Setting;

namespace ExamKeeper.Presenters {
    public class PracticePresenter : ExamBase {

        public string ErrorMessage { get { return response.message; } }

        public PracticePresenter() : base(new InitMongoCache(MongoCollection.QueryQuestion)) {
            IMongoLogger logger = new InitMongoLog("Practice");
            practiceModel = new PracticeModel(new MongoExam(), ResourceLibrary.Instance(logger));
        }

        #region -Create-
        ///<summary>使用者新增自主練習</summary>
        public void create(UserProfile user, CreatePractice request) {
            CacheQuestion cache = null;
            if (!checkCache(request.searchKey, ref cache)) {
                return;
            }
            create(user, request, cache);
        }

        public object create(UserProfile user, CreatePractice request, CacheQuestion cache, string suffix = "") {
            if (!isStudent(user)) {
                return null;
            }
            List<QuestionTypeGroup<string>> checkedGroup = defaultScore(request.questionGroup);
            if (!checkPayload(cache, ref checkedGroup)) {
                return null;
            }
            //set practice
            Practice practice = new Practice() {
                UID = $"{cache.examAttribute.year}{Constants.Dash}{Format.newGuid()}",
                usetype = user.usetype,
                attribute = Format.objectConvert<ExamPaperAttribute, PracticeAttribute>(practiceModel.setAttributeName(cache.examAttribute)),
                questionGroup = practiceModel.setQuestionScore(request.questionGroup, cache),
                maintainer = user.getMaintainer(),
                maintainerUID = user.UID.ToString(),
                source = MemberType.Account.ToString(),
                createTime = DateTime.Now
            };
            practice.name = defaultName(practice.attribute, practice.getQuestionIDs(), cache) + suffix;
            practiceModel.insert(practice);
            res = new {
                UID = practice.UID,
                name = practice.name
            };
            message.setCode(SystemStatus.Succeed);
            return res;
        }

        ///<summary>錯題庫建立自主練習</summary>
        /*public object create(UserProfile user, List<QuestionTypeGroup<string>> questionGroup, CacheQuestion cache, PracticeAttribute attribute) {
            if (!isStudent(user)) {
                return null;
            }
            List<QuestionTypeGroup<string>> checkedGroup = defaultScore(questionGroup);
            //set practice
            Practice practice = new Practice() {
                UID = $"{attribute.year}{Constants.Dash}{Format.newGuid()}",
                usetype = user.usetype,
                attribute = attribute,
                questionGroup = practiceModel.setQuestionScore(questionGroup, cache),
                maintainer = user.getMaintainer(),
                maintainerUID = user.UID.ToString(),
                source = MemberType.Account.ToString(),
                createTime = DateTime.Now
            };
            practice.name = defaultName(practice.attribute, practice.getQuestionIDs(), cache) + @"_錯題練習";
            practiceModel.insert(practice);
            return new {
                UID = practice.UID,
                    name = practice.name
            };
        }*/

        ///<summary>對接服務新增自主練習</summary>
        public void create(ServiceProfile service, ServicePractice request) {
            CacheQuestion cache = null;
            List<QuestionTypeGroup<string>> checkedGroup = defaultScore(request.questionGroup);
            if (!checkPayload(request.searchKey, ref checkedGroup, ref cache)) {
                return;
            }
            // =============== attribute exist in cache ==========
            string education = cache.examAttribute.education;
            string subject = cache.examAttribute.subject;
            // ===================================================

            TextBook textbook = practiceModel.getTextBook(request.year, education, subject);
            if (textbook == null || !textbook.bookInfo.Any(o => o.code.Equals(request.bookID))) {
                message.setCode(SystemStatus.CacheExpired, CustomString.ReadFailed(@"課本資訊", request.bookID));
                return;
            }

            //set practice
            Practice practice = new Practice() {
                UID = $"{request.year}{Constants.Dash}{Format.newGuid()}",
                usetype = service.usetype,
                attribute = setQueryAttribute(request.bookID, textbook),
                questionGroup = practiceModel.setQuestionScore(request.questionGroup, cache),
                maintainer = service.getDesc(),
                maintainerUID = service.account,
                source = MemberType.Service.ToString(),
                createTime = DateTime.Now
            };
            practice.name = request.name;
            practiceModel.insert(practice);
            res = new {
                type = SystemItemType.Practice.ToString(),
                UID = practice.UID
            };
            message.setCode(SystemStatus.Succeed);
        }

        private PracticeAttribute setQueryAttribute(string bookID, TextBook textbook) {
            TextBookInfo book = textbook.bookInfo.Find(o => o.code.Equals(bookID));
            PracticeAttribute result = new PracticeAttribute() {
                year = book.year.ToString(),
                education = book.education,
                subject = book.subject,
                bookIDs = new List<string>() { bookID },
                bookNames = new List<string>() { book.bookName },
                version = book.version
            };
            // names from selections
            result.yearName = textbook.yearMap.findName(result.year);
            result.eduName = textbook.eduMap.findName(result.education);
            result.subjectName = textbook.subjectMap.findName(result.subject);
            result.versionName = textbook.versionMap.findName(result.version);
            return result;
        }

        #endregion

        #region -Query-
        /// <summary> 使用者查詢自主練習 </summary>
        public void query(UserProfile user, string year) {
            if (!isStudent(user)) {
                return;
            }
            setUser(user);
            PracticeMap result = query(year, user.UID.ToString());
            // set status by records
            if (!Compare.EmptyCollection(result.practice)) {
                List<ShowPractice> practiceList = new List<ShowPractice>();
                foreach (IGrouping<string, ShowPractice> group in result.practice.GroupBy(p => p.getYear())) {
                    List<PracticeRecord> records = practiceModel.getPracticeRecords(group.Key, user.UID);
                    practiceList.AddRange(setPracticeMapRecord(group.ToList(), records));
                }
                result.practice = practiceList;
            }
            res = result;
            message.setCode(SystemStatus.Succeed);
        }

        private List<ShowPractice> setPracticeMapRecord(List<ShowPractice> practiceList, List<PracticeRecord> records) {
            practiceList.ForEach(practice => {
                PracticeRecord record = records.Find(o => o.UID.Equals(practice.UID));
                if (record != null) {
                    // ============= practice.status 有先後順序不可調整 =============
                    // 未逾期且未完成測驗 (保險起見抓一小時內可測驗的)
                    ExamRecord unfinished = record.records.Find(r => !r.isDelivered() && r.endAt > DateTime.Now.AddHours(-1));
                    if (unfinished != null) {
                        practice.status = PracticeStatus.Started.GetEnumDescription();
                        practice.isExam = true;
                        practice.examPath = Utils.APIs.OneExam.start(unfinished.examID, user.account);
                    }
                    // 回報紀錄
                    ExamRecord reported = record.records.Find(r => r.isDelivered());
                    if (reported != null) {
                        practice.status = PracticeStatus.Finished.GetEnumDescription();
                        practice.isReport = true;
                        practice.examReport = reported.resultUrl;
                    }
                    // =============================================================
                }
            });
            return practiceList;
        }

        /// <summary> 對接服務查詢自主練習清單 </summary>
        public void query(ServiceProfile service) {
            query(DateTimes.currentSchoolYear(1), service.account);
        }

        /// <summary> 對接服務查詢自主練習明細 </summary>
        public void query(ServiceProfile service, string practiceUID) {
            if (!ExamChecker.checkUID(practiceUID)) {
                message.setCode(SystemStatus.BadRequest, CustomString.TypeError(practiceUID));
                return;
            }
            Practice practice = practiceModel.getPractice(ExamChecker.getYear(practiceUID), practiceUID);
            if (practice == null) {
                message.setCode(SystemStatus.BadRequest, CustomString.NotFound(@"自主練習", practiceUID));
                return;
            }

            QuestionIDPayload questionPayload = new QuestionIDPayload() {
                education = practice.attribute.education,
                subject = practice.attribute.subject,
                keys = practice.getQuestionIDs()
            };
            ServiceQueryQuestion questionModel = new ServiceQueryQuestion(service);

            res = new PracticeInfo() {
                UID = practiceUID,
                practiceInfo = setExamAttributes(practice),
                questionGroup = practice.questionGroup,
                questionInfo = questionModel.queryQuestion(questionPayload)
            };
            message.setCode(SystemStatus.Succeed);
        }

        #endregion

        #region - private methods-
        private PracticeMap query(string year, string userUID) {
            if (string.IsNullOrWhiteSpace(year)) {
                year = DateTimes.currentSchoolYear(1);
            }
            List<Practice> practice = new List<Practice>();
            List<CodeMap> yearMap = ExamChecker.getYearMap(year);
            foreach (CodeMap item in yearMap) {
                int intYear = Convert.ToInt16(item.code);
                practice.AddRange(practiceModel.getPracticeList(userUID, item.code).OrderByDescending(o => o.createTime));
            }
            PracticeMap result = setPracticeMap(practice);
            res = result;
            message.setCode(SystemStatus.Succeed);
            return result;
        }

        // 一般使用者限學生使用
        private bool isStudent(UserProfile user) {
            if (SystemIdentity.Student.ToString().Equals(user.identity)) {
                return true;
            }
            message.setCode(SystemStatus.Forbidden, "Identity Not Allowed.");
            return false;
        }

        private List<QuestionTypeGroup<string>> defaultScore(List<QuestionTypeGroup<string>> groups) {
            int score = 100 / groups.Sum(q => q.questionList.Count);
            groups.ForEach(g => {
                g.scoreType = ScoreType.PerQuestion.ToString();
                g.score = score;
            });
            return groups;
        }

        private string defaultName(PracticeAttribute attributes, List<string> questions, CacheQuestion cache) {
            string name = $"{attributes.yearName}{attributes.versionName}{Format.toString(attributes.bookNames)}";
            List<QuestionInfo> questionInfos = cache.question.FindAll(o => questions.Contains(o.UID));
            List<string> chapters = questionInfos.Select(o => o.metaContentCode(QuesMeta.Chapter)).Distinct().ToList();
            chapters.RemoveAll(o => string.IsNullOrWhiteSpace(o));
            if (!Compare.EmptyCollection(chapters)) {
                name += $"({Format.toString(chapters)})";
            }
            return name;
        }

        private PracticeMap setPracticeMap(List<Practice> practice) {
            PracticeMap result = new PracticeMap();
            practice.ForEach(p => {
                ShowPractice temp = new ShowPractice() {
                UID = p.UID,
                name = p.name,
                eduSubject = p.attribute.education + p.attribute.subject,
                eduSubjectName = p.attribute.eduName + p.attribute.subjectName,
                amount = p.getQuestionIDs().Count(),
                createTime = (DateTime) p.createTime
                };
                temp.setNone();
                result.practice.Add(temp);
            });
            result.eduSubjectMap = result.practice.Select(o => new CodeMap(o.eduSubject, o.eduSubjectName)).ToList().doDistinct();
            return result;
        }
        #endregion
    }
}