using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace ExamKeeperClassLibrary.Models.ExamPaper
{
    [BsonIgnoreExtraElements]
    public class ServiceOTP
    {
        /// <summary>
        /// 物件id
        /// </summary>
        [BsonId]
        [BsonElement("_id")]
        public ObjectId _id { get; set; }

        [BsonElement("examUID")]
        public string ExamUID { get; set; }

        [BsonElement("optCode")]
        public string OptCode { get; set; }

        [BsonElement("userUID")]
        public List<string> UserUIDs { get; set; }

        [BsonElement("service")]
        public string Service { get; set; }

        [BsonElement("systemTime")]
        public DateTime SystemTime { get; set; }

        [BsonElement("expireAt")]
        public long ExpireAt { get; set; }
    }
}
