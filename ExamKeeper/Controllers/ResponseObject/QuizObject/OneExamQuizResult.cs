using ExamKeeper.Controllers.ResponseObject.QuizObject.QuizObjectDerivatives;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ExamKeeper.Controllers.ResponseObject.QuizObject
{
    public class OneExamQuizResult
    {
        [JsonProperty("status")]
        public string Status;

        [JsonProperty("content")]
        public IEnumerable<OneExamQuizResultContent> Content;

    }
}
