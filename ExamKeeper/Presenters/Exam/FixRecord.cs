using System.Collections.Generic;
using System.Linq;
using ExamKeeper.Models;
using ExamKeeper.Resources;
using ExamKeeper.Views;
using Utils;
using Utils.Setting;

namespace ExamKeeper.Presenters {
    public class FixRecord : PresenterBase {

        private FixRecordModel model { get; set; }
        private BookSelectionModel bookModel { get; set; }
        private List<WrongRecord> records { get; set; }
        private IMongoCache questionCache { get; set; }
        private PracticePresenter practice = new PracticePresenter();

        public FixRecord(UserProfile user) {
            model = new FixRecordModel(new MongoExam(), new MongoQuestion());
            bookModel = new BookSelectionModel(ResourceLibrary.Instance(new InitMongoLog("FixRecord")), new MongoSetting(), new InitMongoCache(MongoCollection.QueryQuestion));
            records = model.userWrongRecord(user.UID);
            questionCache = new InitMongoCache(MongoCollection.FixQuestion);
            setUser(user);
        }

        /// <summary> 範圍篩選 </summary>
        public void get() {
            if (Compare.EmptyCollection(records)) {
                message.setCode(SystemStatus.DataNull);
                return;
            }
            EduSubject eduSubject = bookModel.resource.getEduSubject(string.Empty);
            Dictionary<string, string> eduDic = eduSubject.eduMap.toCodeDic();
            Dictionary<string, string> subjectDic = eduSubject.eduSubject.SelectMany(o => o.Value).ToList().doDistinct().toCodeDic();
            FixSelection result = new FixSelection() {
                year = ExamChecker.getYearMap(DateTimes.currentSchoolYear())
            };
            foreach (IGrouping<string, WrongRecord> rec in records.GroupBy(o => o.education)) {
                result.eduMap.Add(new CodeMap(rec.Key, eduDic[rec.Key]));
                List<CodeMap> subjectMap = new List<CodeMap>();
                foreach (WrongRecord record in rec.ToList()) {
                    subjectMap.Add(new CodeMap(record.subject, subjectDic[record.subject]));
                }
                result.subjectMap.Add(rec.Key, subjectMap);
            }
            res = result;
            message.setCode(SystemStatus.Succeed);
        }

        /// <summary> 錯題列表 </summary>
        public void get(string eduSubject, string year) {
            string education = eduSubject.Substring(0, 1);
            string subject = eduSubject.Substring(1, 2);
            WrongRecord record = records.Find(o => o.education.Equals(education) && o.subject.Equals(subject));
            // get GeneralBookSelection
            GeneralBookSelection selection = bookModel.getGeneralSelection(education, subject, year);
            if (selection == null) {
                message.setCode(SystemStatus.DataNull, CustomString.ReadFailed(@"章節資訊"));
            }
            // set response
            res = getBookQuestions(education, subject, year, record);
            message.setCode(SystemStatus.Succeed);
        }

        #region -歸納錯題所屬冊次章節-
        /// <summary> 歸納錯題所屬冊次章節 </summary>
        /// <param name="education"></param>
        /// <param name="subject"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        private FixBookSelection getBookQuestions(string education, string subject, string year, WrongRecord record) {
            // get BookSelection
            GeneralBookSelection baseSelection = bookModel.getGeneralSelection(education, subject, year);
            if (baseSelection == null) {
                return null;
            }
            FixBookSelection selection = new FixBookSelection() {
                versionMap = baseSelection.versionMap,
                bookMap = new Dictionary<string, TextBookMap<BookChapter<FixChapter>>>()
            };

            // get Question Views & infos
            List<QuestionView> views = model.getViews(education, subject, record.questions.Keys.ToList());
            List<QuestionInfo> infos = model.getInfos(education, subject, record.questions.Keys.ToList());
            infos.ForEach(o => o.decompress());
            // set Response
            foreach (KeyValuePair<string, TextBookMap> item in baseSelection.textbookMap) {
                Dictionary<string, List<CodeMap>> bookMap = item.Value.bookMap;
                List<CodeMap> bookList = item.Value.bookList;
                Dictionary<string, BookChapter<FixChapter>> chapterMap = new Dictionary<string, BookChapter<FixChapter>>();
                foreach (KeyValuePair<string, BookChapter> chapter in item.Value.chapterMap) {
                    BookChapter<FixChapter> fixChapter = setBookChapter(chapter.Value, record, views, infos);
                    if (fixChapter != null) {
                        fixChapter.bookDesc = bookList.findName(fixChapter.bookID);
                        chapterMap.Add(chapter.Key, fixChapter);
                    } else {
                        foreach (KeyValuePair<string, List<CodeMap>> map in bookMap) {
                            map.Value.RemoveAll(o => o.code.Equals(chapter.Key));
                        }
                    }
                }

                selection.bookMap.Add(item.Key, new TextBookMap<BookChapter<FixChapter>>() {
                    bookMap = bookMap,
                        chapterMap = chapterMap
                });
            }
            selection.searchKey = setCache(education, subject, year, views, infos);
            return selection;
        }

        private BookChapter<FixChapter> setBookChapter(BookChapter bookChapter, WrongRecord record, List<QuestionView> views, List<QuestionInfo> infos) {
            BookChapter<FixChapter> fixChapter = Format.objectConvert<BookChapter, BookChapter<FixChapter>>(bookChapter);
            List<WrongQuestion> questions = record.questions.Select(o => o.Value).ToList();
            bool hasQuestion = false;
            bookChapter.chapters.ForEach(c => {
                List<WrongQuestion> list = questions.FindAll(o => o.containsKey(c.knowledgeList));
                if (!Compare.EmptyCollection(list)) {
                    hasQuestion = true;
                    List<FixQuestion> questions = new List<FixQuestion>();
                    foreach (WrongQuestion question in list) {
                        WrongAnswers ans = question.records.OrderByDescending(o => o.systemTime).FirstOrDefault();
                        QuestionInfo info = infos.Find(o => o.UID.Equals(question.ID));
                        questions.Add(new FixQuestion() {
                            ID = question.ID,
                                image = views.Find(o => o.UID.Equals(question.ID)).image,
                                questionImage = info.getImages(),
                                htmlParts = info.htmlParts,
                                typeCode = question.type,
                                typeName = question.typeName,
                                answer = ans.answer,
                                userAnswer = ans.userAnswer
                        });
                    }
                    fixChapter.chapters.Find(o => o.Desc.Equals(c.Desc)).questions = questions;
                    fixChapter.amount += questions.Count;
                }
            });
            if (!hasQuestion) {
                return null;
            }
            // 計算題數
            for (int i = 0; i < fixChapter.chapters.Count; i++) {
                FixChapter item = fixChapter.chapters[i];
                item.amount = item.getAmount() + (fixChapter.chapters.FindAll(o => item.code.Equals(o.parentCode))?.Sum(o => o.getAmount()) ?? 0);
            }
            // 移除空章節
            fixChapter.chapters.RemoveAll(o => o.amount == 0);
            return fixChapter;
        }
        #endregion

        /// <summary> 標示"我懂了" </summary>
        public void setUnderstand(string eduSubject, string ID) {
            if (Compare.EmptyCollection(records)) {
                message.setCode(SystemStatus.DataNull);
                return;
            }
            string education = eduSubject.Substring(0, 1);
            string subject = eduSubject.Substring(1, 2);
            WrongRecord rec = records.Find(o => o.education.Equals(education) && o.subject.Equals(subject));
            if (rec == null || !rec.questions.ContainsKey(ID)) {
                message.setCode(SystemStatus.BadRequest, CustomString.ReadFailed(@"錯題紀錄"));
                return;
            }
            // update
            rec.questions[ID].disabled = true;
            model.updateRecord(rec);
            message.setCode(SystemStatus.Succeed);
        }

        /// <summary> 建立練習 </summary>
        public void createPractice(FixPractice request) {

            CacheQuestion cache = getCache(request.searchKey);
            if (cache == null) {
                message.setCode(SystemStatus.BadRequest, CustomString.Expired("錯題資訊"));
                return;
            }
            cache.examAttribute.bookIDs = request.bookIDs;
            cache.examAttribute.version = request.version;
            res = practice.create(user, request, cache, @"_錯題練習");
            if (res == null) {
                message.setCode(SystemStatus.Failed, practice.ErrorMessage);
                return;
            }
            message.setCode(SystemStatus.Succeed);
        }

        #region -Cache-
        private string setCache(string education, string subject, string year, List<QuestionView> views, List<QuestionInfo> infos) {
            string cacheKey = Format.newGuid();
            CacheQuestion cache = new CacheQuestion() {
                examAttribute = new ExamPaperAttribute() {
                year = year,
                education = education,
                subject = subject
                },
                views = views,
                question = infos
            };
            questionCache.insertCache(cacheKey, cache, 60);
            return cacheKey;
        }
        private CacheQuestion getCache(string cacheKey) {
            return questionCache.getCache<CacheQuestion>(cacheKey);
        }
        #endregion

    }

    public class FixSelection {
        public List<CodeMap> year { get; set; }
        public List<CodeMap> eduMap { get; set; }
        public Dictionary<string, List<CodeMap>> subjectMap { get; set; }
        public FixSelection() {
            eduMap = new List<CodeMap>();
            subjectMap = new Dictionary<string, List<CodeMap>>();
        }
    }

    public class FixPractice : CreatePractice {
        public string version { get; set; }
        public List<string> bookIDs { get; set; }
    }
}