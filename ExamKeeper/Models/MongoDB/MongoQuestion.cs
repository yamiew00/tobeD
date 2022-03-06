using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using ExamKeeper.Views;
using MongoDB.Driver;
using Utils;
using Utils.Setting;

namespace ExamKeeper.Models {

    public interface IMongoQuestion {
        /// <summary>取得學制對應DB</summary>
        void getQuestionDB(string edu);
        /// <summary>使用關鍵欄位查詢試題(知識向度等)</summary>
        List<QuestionView> getQuestionView(string eduSubject, List<string> keys);
        /// <summary>使用ID查詢試題</summary>
        List<QuestionView> getQuestionViewByID(string eduSubject, List<string> UID);
        /// <summary>使用ID查詢試題</summary>
        List<QuestionInfo> getQuestion(string eduSubject, List<string> UIDList);
        /// <summary>累加試題使用數</summary>
        void addUsage(string eduSubject, List<string> UIDList);
        QuestionView getView(string eduSubject, string UID);
    }

    [ExcludeFromCodeCoverage]
    public class MongoQuestion : IMongoQuestion {
        public MongoHelper questionDB { get; set; }
        private string education { get; set; }
        public List<QuestionView> getQuestionView(string eduSubject, List<string> keys) {
            FilterDefinitionBuilder<QuestionView> filterBuilder = Builders<QuestionView>.Filter;
            FilterDefinition<QuestionView> filter = filterBuilder.Where(e => !e.disabled) & filterBuilder.AnyIn(e => e.key, keys);
            return questionDB.Find<QuestionView>(MongoCollection.Views, filter, eduSubject);
        }
        public List<QuestionView> getQuestionViewByID(string eduSubject, List<string> IDs) {
            FilterDefinitionBuilder<QuestionView> filterBuilder = Builders<QuestionView>.Filter;
            FilterDefinition<QuestionView> filter = filterBuilder.In(o => o.UID, IDs);
            return questionDB.Find<QuestionView>(MongoCollection.Views, filter, eduSubject);
        }

        public List<QuestionInfo> getQuestion(string eduSubject, List<string> UIDList) {

            if (UIDList.Count > 1000) {
                Dictionary<string, List<QuestionInfo>> dic = new Dictionary<string, List<QuestionInfo>>();
                List<List<string>> groups = Splitter.groupList<string>(UIDList, 5);
                Parallel.ForEach(groups, group => {
                    dic.Add(group[0], getQuestionByUID(eduSubject, group));
                });
                return Format.valueList<string, QuestionInfo>(dic);
            } else {
                return getQuestionByUID(eduSubject, UIDList);
            }
        }
        private List<QuestionInfo> getQuestionByUID(string eduSubject, List<string> UIDList) {
            FilterDefinition<QuestionInfo> filter = Builders<QuestionInfo>.Filter.In(e => e.UID, UIDList);
            return questionDB.Find<QuestionInfo>(MongoCollection.Question, filter, eduSubject);
        }

        public QuestionView getView(string eduSubject, string UID) {
            FilterDefinition<QuestionView> filter = Builders<QuestionView>.Filter.Where(e => e.UID.Equals(UID));
            return questionDB.FindOne<QuestionView>(MongoCollection.Views, filter, false, eduSubject);
        }

        public void addUsage(string eduSubject, List<string> UIDList) {
            FilterDefinition<QuestionView> filter = Builders<QuestionView>.Filter.In(e => e.UID, UIDList);
            UpdateDefinition<QuestionView> update = Builders<QuestionView>.Update.Inc("usage", 1);
            questionDB.UpdateAll<QuestionView>(MongoCollection.Views, filter, update, eduSubject);
        }

        public void getQuestionDB(string edu) {
            if (edu.Equals(education)) {
                return;
            }
            switch (edu) {
                case "E":
                    questionDB = new InitMongoDB(UtilsMongo.DataBase.Elementary);
                    break;
                case "J":
                    questionDB = new InitMongoDB(UtilsMongo.DataBase.Junior);
                    break;
                case "H":
                    questionDB = new InitMongoDB(UtilsMongo.DataBase.Senior);
                    break;
                default:
                    throw new System.Exception($"education [{edu}] database does not exist.");
            }
            edu = education;
        }
    }
}