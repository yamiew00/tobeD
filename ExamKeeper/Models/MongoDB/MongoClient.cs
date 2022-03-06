using System.Diagnostics.CodeAnalysis;
using Connection;
using MongoDB.Driver;
using Utils;
using Utils.Setting;

namespace ExamKeeper.Models {

    [ExcludeFromCodeCoverage]
    public sealed class SystemMongo {
        private static IMongoClient _client;
        private static readonly object connectionLock = new object();
        public static IMongoClient Connection {
            get {
                lock(connectionLock) {
                    if (_client == null) {
                        _client = new MongoClient(Decrypt.get(UtilsMongo.url));
                    }
                }
                return _client;
            }
        }
    }

    [ExcludeFromCodeCoverage]

    public class InitMongoDB : MongoHelper {
        public InitMongoDB(string database) {
            string dbName = Decrypt.get(database);
            IMongoDatabase _database = SystemMongo.Connection.GetDatabase(dbName);
            setDatebase(_database);
        }
    }

    [ExcludeFromCodeCoverage]
    public class InitMongoLog : MongoLogger {
        public InitMongoLog(string action) : base(new InitMongoDB(UtilsMongo.DataBase.Logger), action) { }
    }

    [ExcludeFromCodeCoverage]
    public class InitMongoCache : CacheHelper {
        public InitMongoCache(MongoCollection collection) : base(new InitMongoDB(UtilsMongo.DataBase.Cache), collection) { }
    }
}