using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ExamKeeper.JerryH.JResponse
{
    public class ExamKeeperResonse
    {
        public HttpStatusCode SystemCode { get; set; }

        public bool IsSuccess { get; set; }

        public DateTime SystemNow => DateTime.Now;

        public string Message { get; set; }

        public string Disposal { get; set; }

        /// <summary>
        /// 無泛型的ExamKeeperResonse，代表沒有資料
        /// </summary>
        public string Data => null;
    }
}
