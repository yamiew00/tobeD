using ExamKeeperClassLibrary.Models.ExamPaper.ExamPaperDerivatives;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExamKeeperClassLibrary.Models.ExamPaper
{
    [BsonIgnoreExtraElements]
    public class ExamPaper
    {
        /// <summary>
        /// 物件id
        /// </summary>
        [BsonId]
        [BsonElement("_id")]
        public ObjectId _id { get; set; }

        [BsonElement("UID")]
        public string UID;

        [BsonElement("attribute")]
        public ExamPaperAttribute Attribute;
    }
}
