using Newtonsoft.Json;
using System.Collections.Generic;

namespace ExamKeeper.Controllers.ResponseObject.QuizObject.QuizObjectDerivatives
{
    public class OneExamQuizResultContent
    {
        [JsonProperty("id")]
        public string StudentId;

        [JsonProperty("userInfo")]
        public OneExamQuizResultUserInfo UserInfo;

        [JsonProperty("answerData")]
        public List<OneExamQuizResultAnswerData> AnswerDatas;
    }
}
