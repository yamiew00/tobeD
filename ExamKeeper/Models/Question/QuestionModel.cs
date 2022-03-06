using System.Collections.Generic;
using ExamKeeper.Views;

namespace ExamKeeper.Models {
    public class QuestionModel {

        private IMongoQuestion questionDB { get; set; }
        private string education { get; set; }
        private string eduSubject { get; set; }
        public QuestionModel(IMongoQuestion db) {
            questionDB = db;
        }
        public void setEduSubject(string edu, string subject) {
            this.education = edu;
            this.eduSubject = edu + subject;
            questionDB.getQuestionDB(education);
        }
        public bool exist(string UID) {
            QuestionView view = questionDB.getView(eduSubject, UID);
            return view != null;
        }
        public List<QuestionInfo> get(List<string> IDs) {
            return questionDB.getQuestion(eduSubject, IDs);
        }
        public List<QuestionView> getView(List<string> IDs) {
            return questionDB.getQuestionViewByID(eduSubject, IDs);
        }
    }
}