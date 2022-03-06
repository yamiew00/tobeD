using MongoDB.Bson;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ExamKeeperClassLibrary.Models.QuestionBank.Mongos.Indexes
{
    /// <summary>
    /// 索引表。
    /// 檢索的方法可以寫得更好。
    /// </summary>
    public class QuestionIndex
    {

        /// <summary>
        /// 需要存的最少資料。
        /// </summary>
        public ConcurrentDictionary<ObjectId, Question> RawData;

        private ConcurrentDictionary<string, ConcurrentBag<ObjectId>> BookIdIndex = new ConcurrentDictionary<string, ConcurrentBag<ObjectId>>();

        private ConcurrentDictionary<string, ConcurrentBag<ObjectId>> KnowledgeIndex = new ConcurrentDictionary<string, ConcurrentBag<ObjectId>>();

        private ConcurrentDictionary<string, ConcurrentBag<ObjectId>> SourceIndex = new ConcurrentDictionary<string, ConcurrentBag<ObjectId>>();

        private ConcurrentDictionary<string, ConcurrentBag<ObjectId>> QuestionTypeIndex = new ConcurrentDictionary<string, ConcurrentBag<ObjectId>>();


        private QuestionIndex()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="questions"></param>
        /// <returns></returns>
        public static QuestionIndex CreateIndexes(IEnumerable<Question> questions)
        {
            QuestionIndex indexInstance = new QuestionIndex();

            indexInstance.RawData = new ConcurrentDictionary<ObjectId, Question>(questions.ToDictionary(x => x._id));

            foreach (var question in questions)
            {
                //BookId
                if (indexInstance.BookIdIndex.TryGetValue(question.BookID, out var bookIdBag))
                {
                    bookIdBag.Add(question._id);
                }
                else
                {
                    indexInstance.BookIdIndex[question.BookID] = new ConcurrentBag<ObjectId>() { question._id };
                }

                //KnowledgeIndex
                if (indexInstance.KnowledgeIndex.TryGetValue(question.MetaData.Knowledge, out var knowledgeBag))
                {
                    knowledgeBag.Add(question._id);
                }
                else
                {
                    indexInstance.KnowledgeIndex[question.MetaData.Knowledge] = new ConcurrentBag<ObjectId>() { question._id };
                }

                //Source
                if (indexInstance.SourceIndex.TryGetValue(question.MetaData.Source, out var sourceBag))
                {
                    sourceBag.Add(question._id);
                }
                else
                {
                    indexInstance.SourceIndex[question.MetaData.Source] = new ConcurrentBag<ObjectId>() { question._id };
                }

                //QuestionType
                if (indexInstance.QuestionTypeIndex.TryGetValue(question.MetaData.QuestionType, out var typeBag))
                {
                    typeBag.Add(question._id);
                }
                else
                {
                    indexInstance.QuestionTypeIndex[question.MetaData.QuestionType] = new ConcurrentBag<ObjectId>() { question._id };
                }
            }
            return indexInstance;
        }

        public void InsertIndex(Question question)
        {
            RawData.TryAdd(question._id, question);

            //重複性很高

            //BookId
            if (BookIdIndex.TryGetValue(question.BookID, out var bookIdBag))
            {
                bookIdBag.Add(question._id);
            }
            else
            {
                BookIdIndex[question.BookID] = new ConcurrentBag<ObjectId>() { question._id };
            }

            //KnowledgeIndex
            if (KnowledgeIndex.TryGetValue(question.MetaData.Knowledge, out var knowledgeBag))
            {
                knowledgeBag.Add(question._id);
            }
            else
            {
                KnowledgeIndex[question.MetaData.Knowledge] = new ConcurrentBag<ObjectId>() { question._id };
            }

            //Source
            if (SourceIndex.TryGetValue(question.MetaData.Source, out var sourceBag))
            {
                sourceBag.Add(question._id);
            }
            else
            {
                SourceIndex[question.MetaData.Source] = new ConcurrentBag<ObjectId>() { question._id };
            }

            //QuestionType
            if (QuestionTypeIndex.TryGetValue(question.MetaData.QuestionType, out var typeBag))
            {
                typeBag.Add(question._id);
            }
            else
            {
                QuestionTypeIndex[question.MetaData.QuestionType] = new ConcurrentBag<ObjectId>() { question._id };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bookIDs"></param>
        /// <returns></returns>
        public IEnumerable<ObjectId> GetByBooks(IEnumerable<string> bookIDs)
        {
            return BookIdIndex.Where(x => bookIDs.Contains(x.Key)).SelectMany(x => x.Value);
        }

        public IEnumerable<ObjectId> GetByKnowledges(IEnumerable<string> knowledges)
        {
            return KnowledgeIndex.Where(x => knowledges.Contains(x.Key)).SelectMany(x => x.Value);
        }

        public IEnumerable<ObjectId> GetBySources(IEnumerable<string> knowledges)
        {
            return SourceIndex.Where(x => knowledges.Contains(x.Key)).SelectMany(x => x.Value);
        }

        public int GetQuestionAmount()
        {
            return RawData.Count;
        }

        public IEnumerable<string> GetSourcesKey()
        {
            return SourceIndex.Keys;
        }

        public IEnumerable<Question> GetQuestionById(IEnumerable<ObjectId> objectIds)
        {
            return objectIds.Select(obj => RawData[obj]);
        }
    }
}
