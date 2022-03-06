using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExamKeeperClassLibrary.Models
{
    /// <summary>
    /// MongoDatabase。
    /// 可以對其中任意一個Collection做CRUD。
    /// </summary>
    public class MongoDatabase
    {
        /// <summary>
        /// 資料庫實體
        /// </summary>
        private readonly IMongoDatabase Database;

        public MongoDatabase(IMongoClient client, string databaseName)
        {
            Database = client.GetDatabase(databaseName);
        }

        /// <summary>
        /// 取得型別T的Collection
        /// </summary>
        /// <typeparam name="T">Document型別</typeparam>
        /// <returns>collection實體</returns>
        private IMongoCollection<T> GetCollection<T>()
        {
            return Database.GetCollection<T>(typeof(T).Name);
        }

        /// <summary>
        /// 回傳符合條件的全部資料。(有使用索引為佳)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IEnumerable<T> Find<T>(Expression<Func<T, bool>> filter)
        {
            return GetCollection<T>().Find(filter).ToEnumerable();
        }

        /// <summary>
        /// 回傳一個符合條件的項目
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        public T FindOne<T>(Expression<Func<T, bool>> filter)
        {
            return GetCollection<T>().Find(filter).Limit(1).ToList().FirstOrDefault();
        }

        /// <summary>
        /// 新增一筆資料。失敗時將拋出exception.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        public void InsertOne<T>(T data)
        {
            GetCollection<T>().InsertOne(data);
        }

        public void UpsertOne<T>(Expression<Func<T, bool>> filter, T data)
        {
            GetCollection<T>().ReplaceOne<T>(filter: filter, 
                                             options: new ReplaceOptions { IsUpsert = true },
                                             replacement: data);
        }
    }
}
