using ExamKeeperClassLibrary.Models.QuestionBank.Mongos.Indexes;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamKeeperClassLibrary.Models.QuestionBank.Mongos.Collections
{
    /// <summary>
    /// 處理題目的索引以及即時更新
    /// </summary>
    public class QuestionCollection
    {
        public string Subject { get; private set; }

        /// <summary>
        /// 索引表
        /// </summary>
        public QuestionIndex QuestionIndex;

        /// <summary>
        /// 資料是否可以存取
        /// </summary>
        private Task DataPreparedTask;

        private readonly MongoDataStream<Question> MongoDataStream;

        public QuestionCollection(string subject, MongoDataStream<Question> mongoDataStream)
        {
            Subject = subject;
            MongoDataStream = mongoDataStream;
            DataPreparedTask = Init(mongoDataStream.GetCollection(subject));
        }

        /// <summary>
        /// 從資料庫拿所有資料回來
        /// </summary>
        /// <param name="mongoCollection"></param>
        /// <returns></returns>
        private async Task Init(IMongoCollection<Question> mongoCollection)
        {
            await Task.Run(() =>
            {
                var start = Environment.TickCount;
                var data = mongoCollection.Find(item => true).ToEnumerable();
                
                QuestionIndex = QuestionIndex.CreateIndexes(data);

                //開始逐一監聽(不等候)
                var changeStream = MongoDataStream[Subject];
                changeStream.SubscribeOnInsert(OnInsert());

                changeStream.ChangeStreamOnSingleCollection();

                System.Diagnostics.Debug.WriteLine($"{Subject} is done.");
            });
        }

        /// <summary>
        /// Insert事件發生時的處理
        /// </summary>
        /// <returns></returns>
        public Action<Question> OnInsert()
        {
            return (question) =>
            {
                DataPreparedTask = Task.Run(() =>
                {
                    QuestionIndex.InsertIndex(question);
                });
            };
        }

        public bool IsCompleted()
        {
            return DataPreparedTask.IsCompleted;
        }

        public async Task<int> GetQuestionAmount()
        {
            await DataPreparedTask;
            return QuestionIndex.GetQuestionAmount();
        }

        public async Task<IEnumerable<ObjectId>> GetByBooks(IEnumerable<string> bookIDs)
        {
            await DataPreparedTask;
            return QuestionIndex.GetByBooks(bookIDs);
        }

        public async Task<IEnumerable<ObjectId>> GetByKnowledges(IEnumerable<string> knowledges)
        {
            await DataPreparedTask;
            return QuestionIndex.GetByKnowledges(knowledges);
        }

        public async Task<IEnumerable<ObjectId>> GetBySources(IEnumerable<string> sources)
        {
            await DataPreparedTask;
            return QuestionIndex.GetBySources(sources);
        }

        public async Task<IEnumerable<string>> GetSourcesKey()
        {
            await DataPreparedTask;
            return QuestionIndex.GetSourcesKey();
        }

        public async Task<IEnumerable<Question>> GetQuestionById(IEnumerable<ObjectId> objectIds)
        {
            await DataPreparedTask;
            return QuestionIndex.GetQuestionById(objectIds);
        }
    }
}
