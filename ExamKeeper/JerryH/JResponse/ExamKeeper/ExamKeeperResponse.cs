using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ExamKeeper.JerryH.JResponse
{
    public class ExamKeeperResponse<T>
    {
        public HttpStatusCode SystemCode { get; set; }

        public bool IsSuccess { get; set; }

        public DateTime SystemNow { get; set; }

        public string Message { get; set; }

        public string Disposal { get; set; }

        public T Data { get; set; }
    }
}
