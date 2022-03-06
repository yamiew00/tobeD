using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamKeeper.Controllers.RequestObject.QuizObject
{
    public class QuizResultBody: AutoValidateObject
    {
        /// <summary>
        /// jwtToken
        /// </summary>
        [NotEmpty]
        public string JwtToken { get; set; }

        /// <summary>
        /// quizUID
        /// </summary>
        [NotEmpty]
        public string QuizUID { get; set; }
    }
}
