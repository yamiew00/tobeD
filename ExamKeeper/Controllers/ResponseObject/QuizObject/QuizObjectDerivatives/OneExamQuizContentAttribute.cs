using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamKeeper.Controllers.ResponseObject.QuizObject.QuizObjectDerivatives
{
    public class OneExamQuizContentAttribute
    {
        [JsonProperty("subject")]
        public string Subject;
    }
}
