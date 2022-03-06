using System.Collections.Generic;
using ExamKeeper.Models;
using Utils;

namespace ExamKeeper.Views {
    public class CustomSelection : GeneralBookSelection {
        public List<CodeMap> sourceMap { get; set; }
    }
    public class CustomQuestionGroup : CodeMap {
        public List<CustomQuestion> question { get; set; }
        public FastCount count { get; set; }
        public CustomQuestionGroup(string code, string name) {
            this.code = code;
            this.name = name;
            this.question = new List<CustomQuestion>();
            this.count = new FastCount();
        }
        public void setCount(CustomQuestion question) {
            count.add(question.difficulty, question.answerAmount);
        }
    }
    public class CustomQuestion {
        public string UID { get; set; }
        public string difficulty { get; set; }
        public string quesType { get; set; }
        public string quesTypeName { get; set; }
        public int answerAmount { get; set; }
        public CustomQuestion(QuestionInfo question, CodeMap questionType) {
            this.UID = question.UID;
            this.difficulty = question.metaContent(QuesMeta.Difficulty).code;
            this.quesType = questionType.code;
            this.quesTypeName = questionType.name;
            this.answerAmount = question.getAnswerAmount();
        }
    }
    public class CustomQuestionInfo {
        public string searchKey { get; set; }
        public List<object> filter { get; set; }
        public List<GeneralQuestionInfo> questions { get; set; }
        public List<BookChapter> bookChapters { get; set; }
    }
}