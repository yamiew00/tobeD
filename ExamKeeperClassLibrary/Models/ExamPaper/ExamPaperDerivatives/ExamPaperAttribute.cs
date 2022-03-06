using MongoDB.Bson.Serialization.Attributes;

namespace ExamKeeperClassLibrary.Models.ExamPaper.ExamPaperDerivatives
{
    [BsonIgnoreExtraElements]
    public class ExamPaperAttribute
    {
        [BsonElement("name")]
        public string Name;
    }
}
