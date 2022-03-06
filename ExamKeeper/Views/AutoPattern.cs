using System.Collections.Generic;
using Utils;

namespace ExamKeeper.Views {

    public class AutoQuestionGroup : CodeMap {
        // 難易度分組
        public List<AutoQuestion> question { get; set; }
        public AutoQuestionGroup(string code, string name) {
            this.code = code;
            this.name = name;
            this.question = new List<AutoQuestion>();
        }
    }

    public class AutoQuestion {
        public string UID { get; set; }
        public string difficulty { get; set; }
        public string difficultyName { get; set; }
        public string quesType { get; set; }
        public string quesTypeName { get; set; }
        public int answerAmount { get; set; }
        public List<CodeMap> keys { get; set; }
        public AutoQuestion(QuestionInfo question) {
            this.UID = question.UID;
            CodeMap difficultyType = question.metaContent(QuesMeta.Difficulty);
            this.difficulty = difficultyType.code;
            this.difficultyName = difficultyType.name;
            CodeMap questionType = question.metaContent(QuesMeta.Type);
            this.quesType = questionType.code;
            this.quesTypeName = questionType.name;
            this.answerAmount = question.getAnswerAmount();
            this.keys = question.metaContents(QuesMeta.Knowledge).doDistinct();
        }
    }
}