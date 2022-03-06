using System.Collections.Generic;
using Utils;
using Utils.Setting;

namespace ExamKeeper.Tests {
    public class FakeCache : IMongoCache {
        private List<FakeCachePara> data = new List<FakeCachePara>();
        public void clearCache(string key) {
            data.RemoveAll(o => o.key.Equals(key));
        }

        public void extendCache(string key, int extendMin) {
            //測試用沒有延展需要
            return;
        }

        public T getCache<T>(string key) {
            FakeCachePara result = data.Find(o => o.key.Equals(key));
            if (result != null) {
                return (T) result.data;
            }
            return default(T);
        }

        public T getCache<T>(string key, int extendMin = 0) {
            return getCache<T>(key);
        }

        public T getCache<T>(MongoCollection collection, string key, int extendMin = 0) {
            return getCache<T>(key);
        }

        public T getCaches<T>(string key) {
            return getCache<T>(key);
        }

        public void insertCache(string key, object value, int expireMin) {
            data.Add(new FakeCachePara() {
                key = key,
                    data = value
            });
        }

        public void insertCache(MongoCollection collection, string key, object value, int expireMin) {
            data.Add(new FakeCachePara() {
                key = key,
                    data = value
            });
        }

        public void insertCaches(string key, object value, int expireMin, int chunkSize) {
            data.Add(new FakeCachePara() {
                key = key,
                    data = value
            });
        }
        public void updateCache(string key, object value, int expireMin) {
            int idx = data.FindIndex(o => o.key.Equals(key));
            if (idx > -1) {
                data[idx] = new FakeCachePara() {
                    key = key,
                    data = value
                };
            }
        }

        public void updateCache(MongoCollection collection, string key, object value, int expireMin) {
            int idx = data.FindIndex(o => o.key.Equals(key));
            if (idx > -1) {
                data[idx] = new FakeCachePara() {
                    key = key,
                    data = value
                };
            }
        }

        class FakeCachePara {
            public string key { get; set; }
            public object data { get; set; }
        }
    }
}