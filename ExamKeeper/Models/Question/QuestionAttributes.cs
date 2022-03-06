using System.Collections.Generic;
using System.Linq;
using ExamKeeper.Views;

namespace ExamKeeper.Models {

    public class QuestionAttributes {
        private IMongoQuestion questionDB { get; set; }

        public QuestionAttributes(IMongoQuestion questionDB) {
            this.questionDB = questionDB;
        }
        public void setQuestionDB(string edu) {
            questionDB.getQuestionDB(edu);
        }
        public void setUsageCount(string edu, string subject, List<string> questionID) {
            questionDB.getQuestionDB(edu);
            questionDB.addUsage(edu + subject, questionID);
        }
        public Dictionary<string, PathAndMeta> getDownloadPath(string edu, string subject, List<string> questionUID) {
            List<QuestionInfo> questions = questionDB.getQuestion(edu + subject, questionUID);
            return questions.ToDictionary(o => o.UID, o => new PathAndMeta(o.questionDoc, o.metadata));
        }
    }
    public class PathAndMeta {
        public PathAndMeta(string path, List<QuestionMeta> metadata) {
            this.path = path;
            this.metadata = metadata;
        }
        public string path { get; set; }
        public List<QuestionMeta> metadata { get; set; }
    }
}