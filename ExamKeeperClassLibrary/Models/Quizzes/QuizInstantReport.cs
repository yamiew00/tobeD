using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExamKeeperClassLibrary.Models.Quizzes
{
    [BsonIgnoreExtraElements]
    public class QuizInstantReport
    {
        /// <summary>
        /// 物件id
        /// </summary>
        [BsonId]
        [BsonElement("_id")]
        [BsonIgnoreIfDefault]
        public ObjectId _id { get; set; }

        [BsonElement("quizUID")]
        public string QuizUID { get; set; }

        [BsonElement("completedNumber")]
        public int CompletedNumber { get; set; }

        [BsonElement("csvFileUrl")]
        public string CSVFileUrl { get; set; }
    }
}
