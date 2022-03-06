using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Linq;

namespace tobeD
{
    class Program
    {
        static void Main(string[] args)
        {
            MongoClient client = new MongoClient("mongodb+srv://question-bank-admin:mk+nfFx*wmBgR4G#@ruledata.tmqy1.mongodb.net/{0}?retryWrites=true&w=majority");
            var database = client.GetDatabase("QuestionBank");
            var collection = database.GetCollection<ECH>("QuestionECH");

            var result = collection.Find(x => true).ToEnumerable();

            Console.WriteLine($"count = {result.Count()}");

            while (true)
            {
                Console.ReadLine();
            }
        }

        [BsonIgnoreExtraElements]
        public class ECH
        {
            [BsonId]
            [BsonElement("_id")]
            public ObjectId objectId;
        }
    }
}
