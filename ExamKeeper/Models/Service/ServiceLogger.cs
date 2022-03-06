using System;
using System.Text.Json;
using Utils;
using Utils.Setting;

namespace ExamKeeper.Models {
    public class ServiceLogger {
        public MongoLogger logger { get; private set; }

        public ServiceLogger() {
            logger = new InitMongoLog(UtilsMongo.DataBase.Logger);
        }
        private static readonly object loggerLock = new object();
        private static ServiceLogger _instance = null;
        public static ServiceLogger Instance() {
            lock(loggerLock) {
                if (_instance == null) {
                    _instance = new ServiceLogger();
                }
            }
            return _instance;
        }
        public void insertPayloadLog(string auth, string controller, object request, object response) {
            PayloadLog log = new PayloadLog() {
                authorization = auth,
                controller = controller,
                request = toString(request),
                response = toString(response),
                systemTime = DateTime.Now
            };
            logger.logPayload(log);
        }
        private string toString(object data) {
            if (data == null) {
                return string.Empty;
            }
            return GZip.compress(JsonSerializer.Serialize(data));
        }
    }
}