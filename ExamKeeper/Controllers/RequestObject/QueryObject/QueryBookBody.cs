using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamKeeper.Controllers.RequestObject.QueryObject
{
    public class QueryBookBody: AutoValidateObject
    {
        /// <summary>
        /// 學制科目
        /// </summary>
        [NotEmpty]
        public string Edusubject { get; set; }

        /// <summary>
        /// 出版社。預設值是南一
        /// </summary>
        public List<string> Publishers { get; set; }
    }
}
