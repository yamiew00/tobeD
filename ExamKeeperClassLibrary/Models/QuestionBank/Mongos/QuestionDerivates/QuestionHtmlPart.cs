using MongoDB.Bson.Serialization.Attributes;

namespace ExamKeeperClassLibrary.Models.QuestionBank.Mongos.QuestionDerivates
{
    [BsonIgnoreExtraElements]
    public class QuestionHtmlPart
    {
        [BsonElement("content")]
        public string Content;

        [BsonElement("answer")]
        public string Answer;

        [BsonElement("analyze")]
        public string Analyze;
    }
}
