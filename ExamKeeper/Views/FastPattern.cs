using System.Collections.Generic;
using ExamKeeper.Models;
using Utils;

namespace ExamKeeper.Views {
    public class FastSelection : GeneralBookSelection {
        public List<CodeMap> sourceMap { get; set; }
    }
    public class FastQuestionGroup : CodeMap {
        public List<FastQuestion> question { get; set; }
        public FastCount count { get; set; }
        public FastQuestionGroup(string code, string name) {
            this.code = code;
            this.name = name;
            this.question = new List<FastQuestion>();
            this.count = new FastCount();
        }
        public void setCount(FastQuestion question) {
            count.add(question.difficulty, question.answerAmount);
        }
    }
    public class FastQuestion {
        public string UID { get; set; }
        public string difficulty { get; set; }
        public string quesType { get; set; }
        public string quesTypeName { get; set; }
        public int answerAmount { get; set; }
        public FastQuestion(QuestionInfo question, CodeMap questionType) {
            this.UID = question.UID;
            this.difficulty = question.metaContent(QuesMeta.Difficulty).code;
            this.quesType = questionType.code;
            this.quesTypeName = questionType.name;
            this.answerAmount = question.getAnswerAmount();
        }
    }
    public class FastCount : CountPara {
        public Dictionary<string, CountPara> difficulty { get; set; }
        public FastCount() {
            difficulty = new Dictionary<string, CountPara>();
        }
        public void add(string difficultyCode, int answerAmount) {
            question++;
            answer += answerAmount;
            // 難易度
            difficultyCode = ExamChecker.setDifficultyCode(difficultyCode).ToString();
            if (difficulty.ContainsKey(difficultyCode)) {
                difficulty[difficultyCode].add(answerAmount);
            } else {
                CountPara newPara = new CountPara();
                newPara.add(answerAmount);
                difficulty.Add(difficultyCode, newPara);
            }
        }
    }
    public class FastQuestionInfo {
        public string searchKey { get; set; }
        public List<object> filter { get; set; }
        public List<GeneralQuestionInfo> questions { get; set; }
        public List<BookChapter> bookChapters { get; set; }
    }
}