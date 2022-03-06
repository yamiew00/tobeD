using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace ExamKeeperClassLibrary.Models.QuestionBank.Mongos.QuestionDerivates
{
    [BsonIgnoreExtraElements]
    public class QuestionAnswerInfo
    {
        [BsonElement("index")]
        public int Index;

        [BsonElement("answerType")]
        public string AnswerType;

        [BsonElement("answerAmount")]
        public int AnswerAmount;

        [BsonElement("answer")]
        public IEnumerable<string> Answer;

        [BsonElement("position")]
        public IEnumerable<int> Position;
    }
}
