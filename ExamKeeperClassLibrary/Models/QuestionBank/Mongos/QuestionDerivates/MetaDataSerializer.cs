﻿using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;
using System.Collections.Generic;
using System.Linq;

namespace ExamKeeperClassLibrary.Models.QuestionBank.Mongos.QuestionDerivates
{
    /// <summary>
    /// 解metaData用的工具
    /// </summary>
    public class MetaDataDeserializer : SerializerBase<MetaData>
    {
        public override MetaData Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var knowledge = string.Empty;
            var source = string.Empty;
            var questionType = string.Empty;
            var difficulty = string.Empty;

            var bsonReader = context.Reader;

            bsonReader.ReadStartArray();

            //bsonReader的State 會 Type、Value一直迴環往復直到EndorArray
            bsonReader.ReadBsonType();
            while (bsonReader.State == MongoDB.Bson.IO.BsonReaderState.Value)
            {
                var jsonMetaData = BsonSerializer.Deserialize<JsonMetaData>(bsonReader);
                //知識向度
                if (jsonMetaData.Code == "KNOWLEDGE")
                {
                    knowledge = jsonMetaData.Content.FirstOrDefault().Code;
                }
                else if (jsonMetaData.Code == "SOURCE")
                {
                    source = jsonMetaData.Content.FirstOrDefault().Code;
                }
                else if (jsonMetaData.Code == "QUES_TYPE")
                {
                    questionType = jsonMetaData.Content.FirstOrDefault().Code;
                }
                else if (jsonMetaData.Code == "DIFFICULTY")
                {
                    difficulty = jsonMetaData.Content.FirstOrDefault().Code;
                }

                //忽略type讀取的值，但必須要讀才會往下
                bsonReader.ReadBsonType();
            }
            bsonReader.ReadEndArray();

            return new MetaData()
            {
                Knowledge = knowledge,
                Source = source,
                QuestionType = questionType,
                Difficulty = difficulty
            };
        }
    }

    public class JsonMetaData
    {
        [BsonElement("code")]
        public string Code;

        [BsonElement("name")]
        public string Name;

        [BsonElement("content")]
        public IEnumerable<QuestionMetaContent> Content;
    }
}
