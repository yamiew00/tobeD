using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;
using System.Collections.Generic;

namespace ExamKeeperClassLibrary.Models.QuestionBank.Mongos.QuestionDerivates
{
    public class AnswerInfosSerializer: SerializerBase<IEnumerable<QuestionAnswerInfo>>
    {
        public override IEnumerable<QuestionAnswerInfo> Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            List<QuestionAnswerInfo> list = new List<QuestionAnswerInfo>();

            var bsonReader = context.Reader;
            bsonReader.ReadStartArray();
            //bsonReader的State 會 Type、Value一直迴環往復直到EndorArray
            bsonReader.ReadBsonType();
            while (bsonReader.State == MongoDB.Bson.IO.BsonReaderState.Value)
            {
                var jsonAnswerInfo = BsonSerializer.Deserialize<JsonAnswerInfo>(bsonReader);
                list.Add(new QuestionAnswerInfo()
                {
                    AnswerType = jsonAnswerInfo.AnswerType,
                    AnswerAmount = jsonAnswerInfo.AnswerAmount
                });

                //忽略type讀取的值，但必須要讀才會往下
                bsonReader.ReadBsonType();
            }
            bsonReader.ReadEndArray();

            return list;
        }
    }

    [BsonIgnoreExtraElements]
    public class JsonAnswerInfo
    {
        [BsonElement("answerType")]
        public string AnswerType;

        [BsonElement("answerAmount")]
        public int AnswerAmount;
    }
}
