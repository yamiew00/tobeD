using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace ExamKeeperClassLibrary.Models.QuestionBank.Mongos.QuestionDerivates
{
    [BsonIgnoreExtraElements]
    public class QuestionMetaData
    {
        [BsonElement("code")]
        public string Code;

        [BsonElement("name")]
        public string Name;

        [BsonElement("content")]
        public IEnumerable<QuestionMetaContent> Content;


        public string Knowledge;
    }
}
