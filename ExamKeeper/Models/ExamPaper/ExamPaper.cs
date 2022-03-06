using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamKeeper.Utils;
using ExamKeeper.Views;
using Utils;

namespace ExamKeeper.Models {
    public class ExamPaperModel : ExamBaseModel {
        public ExamPaperModel(IMongoExam db, IResourceLibrary resourceLibrary) : base(db, resourceLibrary) { }

        #region -Query-
        public ExamPaper get(string UID) {
            string year = UID.Split(Constants.Dash) [0];
            return examDB.get(year, UID);
        }
        public Dictionary<string, CodeMap> getQuestionTypePrintCode() {
            List<QuestionType> questionTypeInfos = resource.getQuestionTypes();
            return questionTypeInfos.ToDictionary(o => o.code, o => new CodeMap(o.printCode, o.printDesc));
        }

        /// <summary>取得使用者試卷清單</summary>
        /// <param name="year">年度</param>
        /// <param name="userUID">使用者UID</param>
        /// <param name="usetypes">允許使用範圍</param>
        /// <param name="UIDList">收藏試卷UID</param>
        public List<ExamPaper> getPersonalExamPaper(string year, Guid userUID, List<string> usetypes, List<string> UIDList = null) {
            List<ExamPaper> result = new List<ExamPaper>();
            // own 
            result.AddRange(examDB.getUserExamPaper(year, userUID.ToString()));
            // favorite
            if (!Compare.EmptyCollection(UIDList)) {
                List<ExamPaper> favoriteExam = examDB.get(year, UIDList);
                favoriteExam.RemoveAll(o => !o.isPublic || !o.tags.Contains(SystemSetting.SystemTag)); // remove private & no tag
                if (!Compare.EmptyCollection(favoriteExam)) {
                    favoriteExam.RemoveAll(o => !usetypes.Contains(o.usetype)); // remove by usetype
                }
                result.AddRange(favoriteExam);
            }
            return result;
        }

        /// <summary>取得公開卷清單</summary>
        /// <param name="year">年度</param>
        /// <param name="usetypes">允許使用範圍</param>
        public List<ExamPaper> getPublicExamPaper(string year, List<string> usetypes) {
            List<ExamPaper> result = new List<ExamPaper>();
            // get all public
            result.AddRange(examDB.getPublic(year));
            // usetype
            if (!Compare.EmptyCollection(result)) {
                result = result.FindAll(o => usetypes.Contains(o.usetype));
            }
            return result;
        }

        #endregion

        #region -update-
        /// <summary> 修改試題分數 </summary>
        /// <param name="questionGroup">試題配分 & ID</param>
        /// <param name="result">現有試題資料</param>
        /// <returns></returns>
        public List<QuestionTypeGroup<QuestionScore>> setQuestionScore(List<QuestionTypeGroup<string>> questionGroup, List<QuestionTypeGroup<QuestionScore>> result) {
            Parallel.ForEach(result, group => {
                QuestionTypeGroup<string> scoreInfo = questionGroup.Find(o => o.typeCode.Equals(group.typeCode));
                group.scoreType = scoreInfo.scoreType;
                group.score = scoreInfo.score;
                group.questionList.ForEach(question => {
                    switch (group.GetScoreType()) {
                        case ScoreType.PerQuestion:
                            question.score = group.score;
                            break;
                        case ScoreType.PerAnswer:
                            question.score = group.score * question.answerAmount;
                            break;
                    }
                });
            });
            return result;
        }

        /// <summary> 取得修改試卷用屬性 </summary>
        public ExamPaperRelated getRelated(string UID, ExamPaperAttribute attributes) {
            return new ExamPaperRelated() {
                examUID = UID,
                    defaultName = attributes.name,
                    examType = MapFormat.toCodeMap<ExamType>(),
                    outputType = MapFormat.toCodeMap<OutputType>(),
                    attribute = attributes
            };
        }
        #endregion

        #region -insert-
        public void insert(ExamPaper examPaper) {
            examDB.insert(examPaper);
        }
        public void update(ExamPaper examPaper) {
            examDB.update(examPaper);
        }
        public string createOTP(string examUID) {
            ServiceOTP otp = new ServiceOTP() {
                examUID = examUID,
                optCode = Format.newGuid(),
                systemTime = DateTime.Now,
                expireAt = DateTimes.toTimestamp(DateTime.Now.AddHours(3))
            };
            examDB.insertOTP(otp);
            return otp.optCode;
        }
        #endregion
    }

    public class ExamPaperRelated {
        public string examUID { get; set; }
        public List<CodeMap> examType { get; set; }
        public List<CodeMap> outputType { get; set; }
        public string defaultName { get; set; }
        public ExamPaperAttribute attribute { get; set; } // 複製/編輯的時候回傳
    }
}