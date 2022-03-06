using System.Collections.Generic;
using System.Linq;
using ExamKeeper.Models;
using ExamKeeper.Resources;
using ExamKeeper.Views;
using Utils;
using Utils.Setting;

namespace ExamKeeper.Presenters {
    public class ServiceQueryQuestion : PresenterBase {

        #region -MongoDB-
        private IMongoCache cacheDB { get; set; }
        private IMongoQuestion questionDB { get; set; }
        #endregion

        private ServiceQuestionModel model { get; set; }
        private AutoPattern autoModel { get; set; }
        #region -settings-
        private List<SystemIdentity> AllowIdentity = new List<SystemIdentity>() { SystemIdentity.Student, SystemIdentity.Teacher };
        private UserProfile serviceUser { get; set; }
        #endregion

        public ServiceQueryQuestion(ServiceProfile service, string identity = "") {
            setServiceUser(service, identity);
            cacheDB = new InitMongoCache(MongoCollection.QueryQuestion);
            questionDB = new MongoQuestion();
            model = new ServiceQuestionModel(cacheDB);
            autoModel = new AutoPattern(serviceUser, new MongoSetting(), new InitMongoLog("ServiceQueryQuestion"), cacheDB);
            message.setCode(SystemStatus.Start);
        }

        private void setServiceUser(ServiceProfile service, string identity) {
            serviceUser = new UserProfile() {
                name = service.name,
                identity = string.IsNullOrWhiteSpace(identity) ? SystemIdentity.Guest.ToString() : identity,
                account = service.account,
                usetype = service.usetype,
                organization = new AttributeMap() {
                code = service.code,
                name = service.name,
                type = service.type
                }
            };
        }

        public List<ServiceQuestion> queryQuestion(QuestionIDPayload request) {
            List<QuestionInfo> questions = model.queryQuestions(request, questionDB);
            List<QuestionType> questionTypes = autoModel.resourceAPI.getQuestionTypes();
            if (Compare.EmptyCollection(questionTypes)) {
                message.setCode(SystemStatus.Exception, CustomString.ReadFailed(CustomString.Meta.ANSWER_TYPE));
                return null;
            }
            if (Compare.EmptyCollection(questions) || !setAnswerType(ref questions, questionTypes)) {
                return null;
            }
            List<ServiceQuestion> serviceQuestions = setServiceQuestions(questions, questionTypes);
            res = new {
                searchKey = model.searchKey,
                question = serviceQuestions
            };
            message.setCode(SystemStatus.Succeed);
            return serviceQuestions;
        }

        private bool setAnswerType(ref List<QuestionInfo> questions, List<QuestionType> questionTypes) {
            Dictionary<string, CodeMap> answerTypeDic = questionTypes.ToDictionary(type => type.code, type => new CodeMap(type.groupCode, type.groupDesc));
            questions.ForEach(question => {
                string questionType = question.metaContentCode(QuesMeta.Type);
                if (answerTypeDic.ContainsKey(questionType)) {
                    question.metadata.Insert(0, new QuestionMeta() {
                        code = QuesMeta.AnswerType,
                            name = CustomString.Meta.ANSWER_TYPE,
                            content = new List<CodeMap>() { answerTypeDic[questionType] }
                    });
                }
            });
            return true;
        }

        public void getQuestionCache(string searchKey) {
            List<QuestionInfo> questions = model.getCache(searchKey);
            if (questions == null) {
                message.setCode(SystemStatus.Failed, "Cache Expired.");
                return;
            }
            List<QuestionType> questionTypes = autoModel.resourceAPI.getQuestionTypes();
            res = setServiceQuestions(questions, questionTypes);
            message.setCode(SystemStatus.Succeed);
        }

        private List<ServiceQuestion> setServiceQuestions(List<QuestionInfo> questions, List<QuestionType> types) {
            List<ServiceQuestion> result = new List<ServiceQuestion>();
            Dictionary<string, string> typeDic = types.GroupBy(o => o.groupCode).ToDictionary(x => x.Key, x => x.ToList().FirstOrDefault().groupDesc);
            questions.ForEach(question => {
                question.decompress();
                result.Add(new ServiceQuestion() {
                    UID = question.UID,
                        image = question.image,
                        questionImage = question.getImages(),
                        metadata = question.metadata,
                        answerInfos = setServiceAnswers(question.answerInfos, typeDic),
                        htmlParts = question.htmlParts,
                        updateTime = question.updateTime
                });
            });
            return result;
        }

        private List<ServiceAnswers> setServiceAnswers(List<AnswerInfos> ans, Dictionary<string, string> typeDic) {
            if (Compare.EmptyCollection(ans)) {
                return null;
            }
            List<ServiceAnswers> result = new List<ServiceAnswers>();
            ans.ForEach(a => {
                ServiceAnswers serviceAns = Format.objectConvert<AnswerInfos, ServiceAnswers>(a);
                serviceAns.answerTypeName = typeDic.ContainsKey(a.answerType) ? typeDic[a.answerType] : string.Empty;
                result.Add(serviceAns);
            });
            return result;
        }
        public void autoPatternQuery(ServicePayload request) {
            if (!checkPayload(request)) {
                return;
            }
            QuestionPayload payload = new QuestionPayload() {
                education = request.education,
                subject = request.subject,
                keys = request.keys

            };
            // service智能命題不走範圍查詢, 是直接使用知識項度, 故無法反推查詢條件
            if (!autoModel.queryQuestions(serviceUser.usetype, payload, questionDB, false)) {
                message.setCode(SystemStatus.Failed, autoModel.getErrorMessage());
                return;
            }
            res = autoModel.getQuestions();
            message.setCode(SystemStatus.Succeed);
        }

        public void autoPatternCache(string searchKey) {
            res = autoModel.getQuestionsCache(searchKey);
            if (res == null) {
                message.setCode(SystemStatus.Failed, "Cache Expired.");
            } else {
                message.setCode(SystemStatus.Succeed);
            }
        }

        private bool checkPayload(ServicePayload request) {
            SystemIdentity identity = ExtensionHelper.GetFromName<SystemIdentity>(request.identity);
            if (!AllowIdentity.Contains(identity)) {
                message.setCode(SystemStatus.Forbidden, CustomString.System_NotMatch);
                return false;
            }
            return true;
        }
    }
}