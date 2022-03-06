using Newtonsoft.Json;
using System;

namespace ExamKeeper.JerryH.JResponse
{
    public class CMSResponseBody<T>
    {
        [JsonProperty("systemCode")]
        public string SystemCode;

        [JsonProperty("isSuccess")]
        public bool IsSuccess;

        [JsonProperty("systemNow")]
        public DateTime SystemNow;

        [JsonProperty("message")]
        public string Message;

        [JsonProperty("disposal")]
        public string Disposal;

        [JsonProperty("data")]
        public T Data;
    }
}
