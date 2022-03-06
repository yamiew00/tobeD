using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamKeeper.Controllers.RequestObject.QueryObject
{
    public class QueryQuestionTypeBody
    {
        public string EduSubject { get; set; }

        public IEnumerable<string> Knowledges { get; set; }

        public IEnumerable<string> BookIDs { get; set; }

        public IEnumerable<string> Sources { get; set; }
    }
}
