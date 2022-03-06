using System.Collections.Generic;
using System.Linq;
using ExamKeeper.Views;
using Utils;

namespace ExamKeeper.Models {
    /// <summary> 試卷 & 自主練習共用 </summary>
    public class ExamBaseModel {
        protected IMongoExam examDB { get; set; }
        protected IResourceLibrary resource { get; private set; }
        public ExamBaseModel(IMongoExam db, IResourceLibrary resourceLibrary) {
            this.examDB = db;
            this.resource = resourceLibrary;
        }

        /// <summary> 回填屬性中文名稱 </summary>
        public ExamPaperAttribute setAttributeName(ExamPaperAttribute attributes) {
            TextBook infos = resource.getYearTextBook(attributes.year, attributes.education, attributes.subject);
            if (infos == null) {
                return attributes;
            }
            // payloads
            attributes.yearName = infos.yearMap.Find(o => o.code.Equals(attributes.year)).name;
            attributes.eduName = infos.eduMap.Find(o => o.code.Equals(attributes.education)).name;
            attributes.subjectName = infos.subjectMap.Find(o => o.code.Equals(attributes.subject)).name;
            attributes.versionName = infos.versionMap.Find(o => o.code.Equals(attributes.version)).name;
            List<string> bookCodes = attributes.bookIDs.Select(o => o.Substring(o.Length - 3)).ToList();
            List<CodeMap> books = MapFormat.filterMap(bookCodes, infos.bookMap);
            attributes.bookNames = books.Select(o => o.name).ToList();
            // default
            attributes.examType = ExamType.General.ToString();
            if (string.IsNullOrWhiteSpace(attributes.name)) {
                attributes.name = $"{attributes.yearName}{attributes.eduName}{attributes.subjectName}測驗";
            }
            return attributes;
        }

        /// <summary> 計算各題分數 </summary>
        /// <param name="questionGroup">試題配分 & ID</param>
        /// <param name="cache">暫存試題資料</param>
        /// <returns></returns>
        public List<QuestionTypeGroup<QuestionScore>> setQuestionScore(List<QuestionTypeGroup<string>> questionGroup, CacheQuestion cache) {
            List<QuestionTypeGroup<QuestionScore>> result = new List<QuestionTypeGroup<QuestionScore>>();
            questionGroup.ForEach(group => {
                Dictionary<string, int> answerDic = getQuestionAnswerAmount(group.questionList, cache);
                List<QuestionScore> scoreQuestion = new List<QuestionScore>();
                int seq = 0;
                group.questionList.ForEach(question => {
                    switch (group.GetScoreType()) {
                        case ScoreType.PerQuestion:
                            scoreQuestion.Add(new QuestionScore() {
                                ID = question,
                                    sequence = ++seq,
                                    score = group.score,
                                    answerAmount = answerDic[question]
                            });
                            break;
                        case ScoreType.PerAnswer:
                            scoreQuestion.Add(new QuestionScore() {
                                ID = question,
                                    sequence = ++seq,
                                    score = (group.score * answerDic[question]),
                                    answerAmount = answerDic[question]
                            });
                            break;
                    }
                });
                result.Add(new QuestionTypeGroup<QuestionScore>() {
                    typeCode = group.typeCode,
                        typeName = group.typeName,
                        score = group.score,
                        scoreType = group.scoreType,
                        questionList = scoreQuestion
                });
            });
            return result;
        }
        private Dictionary<string, int> getQuestionAnswerAmount(List<string> IDs, CacheQuestion cache) {
            List<QuestionInfo> questions = cache.question.FindAll(cache => IDs.Contains(cache.UID));
            return questions.ToDictionary(x => x.UID, x => x.getAnswerAmount());
        }
    }
}