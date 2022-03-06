using MongoDB.Bson.Serialization.Attributes;

namespace ExamKeeperClassLibrary.Models.QuestionBank.Mongos.QuestionDerivates
{
    [BsonIgnoreExtraElements]
    public class QuestionMetaContent
    {
        [BsonElement("code")]
        public string Code;

        [BsonElement("name")]
        public string Name;
    }
}
