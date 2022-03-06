using System.Collections.Generic;
using System.Linq;
using ExamKeeper.Models;
using ExamKeeper.Views;
using Utils;
using Utils.Setting;

namespace ExamKeeper.Presenters {
    public class CopyQuestion : ExamBase {
        private QuestionModel questionModel { get; set; }

        #region -parameters-
        private ExamPaper exampaper { get; set; }
        #endregion

        public CopyQuestion(UserProfile user) : base(new InitMongoCache(MongoCollection.QueryQuestion)) {
            questionModel = new QuestionModel(new MongoQuestion());
            IMongoLogger logger = new InitMongoLog("CopyQuestion");
            examModel = new ExamPaperModel(new MongoExam(), ResourceLibrary.Instance(logger));
            setUser(user);
        }

        public bool getExamPaper(string examUID, string action) {
            if (!ExtensionHelper.checkName<PaperAction>(action)) {
                message.setCode(SystemStatus.BadRequest, "Action does not exist.");
                return false;
            }
            ExamPaper exam = null;
            if (checkExamExist(examUID, ref exam, ExtensionHelper.GetFromName<PaperAction>(action))) {
                exampaper = exam;
                return true;
            }
            return false;
        }

        public void getQuestions() {
            QueryQuestion queryModel = new QueryQuestion(user, exampaper.attribute.drawUp);
            QuestionPayload payload = new QuestionPayload() {
                year = exampaper.attribute.year,
                education = exampaper.attribute.education,
                subject = exampaper.attribute.subject,
                bookIDs = exampaper.attribute.bookIDs,

            };
            // get exam questions
            List<string> IDs = exampaper.getQuestionIDs();
            questionModel.setEduSubject(payload.education, payload.subject);
            List<QuestionInfo> examQuestions = questionModel.get(IDs);
            // set keys & source from questions
            setKeysAndSource(examQuestions, ref payload);
            // check disabled questions
            List<AmountMap> disabledQuestion = checkDisabled(examQuestions, IDs);
            // set response
            res = new CopyResponse() {
                examUID = exampaper.UID,
                pattern = exampaper.attribute.drawUp,
                examPaper = setCopyExamPaper(exampaper, examQuestions),
                questionInfo = queryModel.queryQuestion(payload),
                disabled = disabledQuestion
            };
            message.setCode(SystemStatus.Succeed);
        }

        private void setKeysAndSource(List<QuestionInfo> examQuestions, ref QuestionPayload payload) {
            List<string> keys = new List<string>();
            List<string> source = new List<string>();
            examQuestions.ForEach(q => {
                keys.AddRange(q.metaContents(QuesMeta.Knowledge).Select(m => m.code));
                source.AddRange(q.metaContents(QuesMeta.Source).Select(m => m.code));
            });
            payload.keys = keys.Distinct().ToList();
            payload.source = source.Distinct().ToList();
        }

        private List<AmountMap> checkDisabled(List<QuestionInfo> examQuestions, List<string> IDs) {
            List<QuestionView> views = questionModel.getView(IDs);
            List<AmountMap> result = null;
            if (views.Any(o => o.disabled)) {
                result = new List<AmountMap>();
                IDs = views.FindAll(o => o.disabled).Select(o => o.UID).ToList();
                // remove disabled questions from exampaper
                foreach (QuestionTypeGroup<QuestionScore> group in exampaper.questionGroup) {
                    group.questionList.RemoveAll(o => IDs.Contains(o.ID));
                }
                // count by knowledge
                Dictionary<string, QuestionInfo> dic = examQuestions.ToDictionary(o => o.UID, o => o);
                List<CodeMap> knowledges = new List<CodeMap>();
                foreach (string ID in IDs) {
                    knowledges.AddRange(dic[ID].metaContents(QuesMeta.Knowledge));
                }
                foreach (IGrouping<string, CodeMap> group in knowledges.GroupBy(o => o.code)) {
                    result.Add(new AmountMap() {
                        code = group.Key,
                            name = group.FirstOrDefault().name,
                            amount = group.Count()
                    });
                }
            }
            return result;
        }

        private ExamPaper<CopyQuestionScore> setCopyExamPaper(ExamPaper exampaper, List<QuestionInfo> questions) {
            Dictionary<string, QuestionInfo> dic = questions.ToDictionary(o => o.UID, o => o);
            ExamPaper<CopyQuestionScore> result = Format.objectConvert<ExamPaper, ExamPaper<CopyQuestionScore>>(exampaper);
            result.questionGroup.ForEach(o => {
                o.questionList.ForEach(q => {
                    if (dic.ContainsKey(q.ID)) {
                        q.difficulty = ExamChecker.setDifficultyCode(dic[q.ID].metaContent(QuesMeta.Difficulty).code).ToString();
                    }
                });
            });
            return result;
        }
    }

    public class CopyResponse {
        public string examUID { get; set; }
        public string pattern { get; set; }
        public ExamPaper<CopyQuestionScore> examPaper { get; set; }
        public object questionInfo { get; set; }
        public List<AmountMap> disabled { get; set; }
    }
}