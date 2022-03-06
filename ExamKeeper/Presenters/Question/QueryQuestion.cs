using ExamKeeper.Models;
using ExamKeeper.Resources;
using ExamKeeper.Views;
using Utils;
using Utils.Setting;

namespace ExamKeeper.Presenters {
    public class QueryQuestion : PresenterBase {
        private IDrawUpModel model { get; set; }

        #region -MongoDB-
        private IMongoSetting settingDB { get; set; }
        private IMongoLogger loggerDB { get; set; }
        private IMongoCache cacheDB { get; set; }
        private IMongoQuestion questionDB { get; set; }
        #endregion

        #region -parameters-
        private string currentPattern { get; set; }
        #endregion

        public QueryQuestion(UserProfile user, string pattern) {
            currentPattern = pattern;
            settingDB = new MongoSetting();
            loggerDB = new InitMongoLog("QueryQuestion");
            cacheDB = new InitMongoCache(MongoCollection.QueryQuestion);
            questionDB = new MongoQuestion();
            setUser(user);
        }
        public void getSelection(PatternPayload para) {
            if (!getPatternModel()) {
                return;
            }
            model.setPara(para);
            if (!model.setSelection()) {
                message.setCode(SystemStatus.Failed, model.getErrorMessage());
                return;
            }
            res = model.getSelection();
            message.setCode(SystemStatus.Succeed);
        }

        public object queryQuestion(QuestionPayload request) {
            if (!getPatternModel()) {
                return null;
            }
            if (!model.queryQuestions(user.usetype, request, questionDB)) {
                message.setCode(SystemStatus.Failed, model.getErrorMessage());
                return null;
            }
            res = model.getQuestions();
            message.setCode(SystemStatus.Succeed);
            return res;
        }

        public void queryQuestionInfo(string searchKey) {
            if (!getPatternModel()) {
                return;
            }
            if (!model.queryQuestionInfo(searchKey)) {
                message.setCode(SystemStatus.Failed, model.getErrorMessage());
                return;
            }
            res = model.getQuestionInfo();
            message.setCode(SystemStatus.Succeed);
        }

        public void getQuestionCache(string searchKey) {
            if (!getPatternModel()) {
                return;
            }
            res = model.getQuestionsCache(searchKey);
            if (res == null) {
                message.setCode(SystemStatus.Failed, "Cache Expired.");
                return;
            }
            message.setCode(SystemStatus.Succeed);
        }

        private bool getPatternModel() {
            //確認傳入的string是否符合enum(沒必要這麼做, 可以在外部存取)
            if (!ExtensionHelper.checkName<DrawUpPattern>(currentPattern)) {
                message.setCode(SystemStatus.ErrorType, CustomString.TypeError(@"出卷方式"));
                return false;
            }
            DrawUpPattern pattern = ExtensionHelper.GetFromName<DrawUpPattern>(currentPattern);
            switch (pattern) {
                case DrawUpPattern.FastPattern:
                    model = new FastPattern(settingDB, loggerDB, cacheDB);
                    return true;
                case DrawUpPattern.AutoPattern:
                    model = new AutoPattern(user, settingDB, loggerDB, cacheDB);
                    return true;
                case DrawUpPattern.CustomPattern:
                    model = new CustomPattern(user, settingDB, loggerDB, cacheDB);
                    return true;
                case DrawUpPattern.KnowledgePattern:
                default:
                    message.setCode(SystemStatus.Forbidden, CustomString.Auth_NotAllow);
                    return false;
            }
        }
    }

}