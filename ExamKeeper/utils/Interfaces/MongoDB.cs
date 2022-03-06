using System;
using System.Collections.Generic;
using Utils.Setting;

namespace Utils {
    public interface IMongoCache {
        void insertCache(string key, object value, int expireMin);
        void insertCache(MongoCollection collection, string key, object value, int expireMin);
        void insertCaches(string key, object value, int expireMin, int chunkSize);
        void updateCache(string key, object value, int expireMin);
        void updateCache(MongoCollection collection, string key, object value, int expireMin);
        T getCache<T>(string key, int extendMin = 0);
        T getCache<T>(MongoCollection collection, string key, int extendMin = 0);
        T getCaches<T>(string key);
        void clearCache(string key);
        void extendCache(string key, int extendMin);
    }

    public interface IMongoLogger {
        void setKey(string action, string logKey);
        void logException(string action, Exception ex);
        void setStatus(LogStatus status, string value = "");
        void addEvent(LogStatus status, string value);
        void insert(MongoCollection collection);
        void logPayload(string url, Dictionary<string, string> optionHeader, object response);
    }
}