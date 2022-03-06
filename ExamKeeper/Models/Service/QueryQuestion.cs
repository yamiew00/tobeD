using System.Collections.Generic;
using ExamKeeper.Views;
using Utils;

namespace ExamKeeper.Models {
    public class ServiceQuestionModel {
        protected MessageModel message = new MessageModel();
        private IMongoCache cache { get; set; }
        public string ErrorMessage { get { return message.TakeMessage(); } }
        public string searchKey { get; private set; }

        public ServiceQuestionModel(IMongoCache cache) {
            this.cache = cache;
        }
        public List<QuestionInfo> queryQuestions(QuestionIDPayload request, IMongoQuestion questionDB) {
            questionDB.getQuestionDB(request.education);
            List<QuestionInfo> infos = questionDB.getQuestion(request.EduSubject(), request.keys);
            if (!Compare.EmptyCollection(infos)) {
                // insert cache
                searchKey = insertCache(new CacheQuestion {
                    question = infos
                });
            }
            return infos;
        }
        public string insertCache(CacheQuestion para) {
            string cacheKey = Format.newGuid();
            cache.insertCache(cacheKey, para, 360);
            return cacheKey;
        }
        public List<QuestionInfo> getCache(string key) {
            return cache.getCaches<CacheQuestion>(key)?.question ?? null;
        }
    }
}