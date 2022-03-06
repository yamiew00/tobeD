using ExamKeeper.Controllers.ResponseObject.QuizObject.QuizObjectDerivatives;
using Newtonsoft.Json;

namespace ExamKeeper.Controllers.ResponseObject.QuizObject
{
    public class OneExamQuiz
    {
        [JsonProperty("status")]
        public string Status;

        [JsonProperty("content")]
        public OneExamQuizContent Content;
    }
}
