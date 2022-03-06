using System;
using MongoDB.Bson.Serialization.Attributes;
using Utils;

namespace ExamKeeper.Views {
    //// <summary> 服務登入 </summary>
    [BsonIgnoreExtraElements]
    public class ServiceProfile : BaseColumn {
        public string code { get; set; }
        public string account { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string usetype { get; set; }
        public string status { get; set; }
        public string getDesc() {
            return $"[{code}] {name}";
        }
    }

    [BsonIgnoreExtraElements]
    public class ServiceToken {
        public string account { get; set; }
        public string prev { get; set; }
        public string token { get; set; }
        public DateTime createTime { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("expireAt")]
        public DateTime expireAt { get; set; }
        public bool reset() {
            // 30分鐘內逾期的話延後2小時
            if (this.expireAt.ToLocalTime() < DateTime.Now.AddMinutes(30)) {
                this.expireAt = DateTime.Now.AddHours(2);
                return true;
            }
            return false;
        }
    }
    public class TokenExpired {
        public TokenExpired(string token, DateTime expireTime) {
            this.token = token;
            this.expireAt = new DateTimeOffset(expireTime.ToUniversalTime()).ToUnixTimeSeconds();
        }
        public string token { get; private set; }
        public double expireAt { get; private set; }
    }
}