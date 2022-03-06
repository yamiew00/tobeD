using MongoDB.Driver;

namespace ExamKeeperClassLibrary.Models
{
    /// <summary>
    /// database context。
    /// 使用此類別時，需確定類別與Collection名稱完全一致！
    /// </summary>
    public class MongoContext
    {
        private MongoClient Client;

        /// <summary>
        /// Quizzes資料庫
        /// </summary>
        public MongoDatabase Quizzes { get; private set; }

        public MongoDatabase ExamPaper { get; private set; }

        public MongoContext(string connectionString)
        {
            Client = new MongoClient(connectionString);

            //Databases
            Quizzes = new MongoDatabase(Client, "Quizzes");
            ExamPaper = new MongoDatabase(Client, "ExamPaper");
        }

        public IMongoDatabase GetDatabase(string database)
        {
            return Client.GetDatabase(database);
        }
    }
}
