using System;
using System.Collections.Generic;
using System.Linq;
using ExamKeeper.Models;
using ExamKeeper.Views;
using Utils;
using Utils.Setting;

namespace ExamKeeper.Presenters {
    public class QuestionChart : PresenterBase {
        private List<string> chartType = new List<string>() { QuesMeta.Difficulty, QuesMeta.Lesson, QuesMeta.Type, QuesMeta.Knowledge, QuesMeta.Book };

        #region -MongoDB-
        private IMongoCache cacheDB { get; set; }
        #endregion
        private QuestionBase model { get; set; }
        public QuestionChart(UserProfile user) {
            model = new QuestionBase(new MongoSetting(), new InitMongoLog("QuestionChart"), new InitMongoCache(MongoCollection.QueryQuestion));
            setUser(user);
        }

        public void getChart(ChartPayload request) {
            CacheQuestion cache = model.getCache(request.searchKey);
            if (cache == null) {
                message.addMessage($"cache null:{request.searchKey}");
                return;
            }
            Dictionary<string, List<QuestionMeta>> questionDic = cache.question.ToDictionary(o => o.UID, o => o.metadata);
            Dictionary<string, ChartInfos> counter = new Dictionary<string, ChartInfos>();
            // count
            request.questions.ForEach(q => {
                if (questionDic.ContainsKey(q)) {
                    List<QuestionMeta> meta = questionDic[q];
                    meta.RemoveAll(m => !chartType.Contains(m.code));
                    meta.ForEach(m => {
                        if (counter.ContainsKey(m.code)) {
                            counter[m.code].addItem(m.content, QuesMeta.Difficulty.Equals(m.code));
                        } else {
                            counter.Add(m.code, new ChartInfos(m));
                        }
                    });
                }
            });
            // set response
            Charts result = new Charts();
            foreach (KeyValuePair<string, ChartInfos> type in counter) {
                result.typeMap.Add(new CodeMap(type.Value.code, type.Value.name));
                type.Value.item.ForEach(item => {
                    item.percent = Math.Round(item.count * ((decimal) 100 / (decimal) type.Value.total), 2);
                });
            }
            result.chart = counter;
            res = result;
            message.setCode(SystemStatus.Succeed);
        }

    }
}