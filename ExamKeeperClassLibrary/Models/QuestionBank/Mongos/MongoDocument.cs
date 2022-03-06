using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ExamKeeperClassLibrary.Models.QuestionBank.Mongos
{
    [BsonIgnoreExtraElements]
    public class MongoDocument
    {
        [BsonId]
        [BsonElement("_id")]
        public ObjectId _id;
    }
}
