using Newtonsoft.Json;
using System;

namespace ExamKeeper.Controllers.ResponseObject.QuizObject.QuizObjectDerivatives
{
    public class OneExamQuizContent
    {
        [JsonProperty("quizId")]
        public string QuizId;

        [JsonProperty("createDate")]
        public DateTime CreateDate;

        [JsonProperty("quizCode")]
        public int QuizCode;

        [JsonProperty("schoolYear")]
        public string SchoolYear;

        [JsonProperty("attribute")]
        public OneExamQuizContentAttribute Attribute;
    }
}
