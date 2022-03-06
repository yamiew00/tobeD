using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ExamKeeper.Views;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Utils;
using Utils.Setting;

namespace ExamKeeper.Models {

    public interface IMongoSetting {
        Dictionary<string, List<string>> getMetaDictionary(string year, string eduSubject);
        List<TagBinding> getTags(string binding);
        bool upsert(UserPreference preference);
        UserPreference getUserPreference(Guid UID);
        List<UserFavorites> getUserFavorites(Guid UID);
        UserFavorites getUserFavorite(Guid UID, string type);
        bool upsert(UserFavorites preference);
        void setWebhook(Webhook key);
        Webhook getWebhook(string otp, string key);
    }

    [ExcludeFromCodeCoverage]
    public class MongoSetting : IMongoSetting {
        private MongoHelper settings = new InitMongoDB(UtilsMongo.DataBase.Settings);
        private static List<string> ignoreYear = new List<string>() { "Source" };
        public Dictionary<string, List<string>> getMetaDictionary(string year, string eduSubject) {
            FilterDefinition<SettingMeta> filter = Builders<SettingMeta>.Filter.Where(e => e.eduSubject.Equals(eduSubject));
            List<SettingMeta> currentMeta = settings.Find(MongoCollection.SpecMeta, filter);
            if (currentMeta == null) {
                return new Dictionary<string, List<string>>();
            }
            // set result
            Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();
            foreach (IGrouping<string, SettingMeta> item in currentMeta.GroupBy(meta => meta.type)) {
                if (ignoreYear.Contains(item.Key)) {
                    List<string> codes = item.SelectMany(o => Splitter.valueList(o.data)).Distinct().ToList();
                    result.Add(item.Key, codes);
                } else {
                    SettingMeta meta = item.Where(meta => meta.year.Equals(year)).FirstOrDefault();
                    if (meta != null) {
                        result.Add(item.Key, Splitter.valueList(meta.data));
                    }
                }
            }
            return result;
        }

        public List<TagBinding> getTags(string binding) {
            FilterDefinition<TagBinding> filter = Builders<TagBinding>.Filter.Where(e => e.binding.Equals(binding));
            return settings.Find(MongoCollection.TagBindings, filter);
        }

        #region -UserPreference-
        public bool upsert(UserPreference preference) {
            FilterDefinition<UserPreference> filter = Builders<UserPreference>.Filter.Where(e => e.UID.Equals(preference.UID));
            return settings.UpsertInsert<UserPreference>(MongoCollection.UserPreference, filter, preference);
        }
        public UserPreference getUserPreference(Guid UID) {
            FilterDefinition<UserPreference> filter = Builders<UserPreference>.Filter.Where(e => e.UID.Equals(UID));
            return settings.FindOne<UserPreference>(MongoCollection.UserPreference, filter);
        }
        public List<UserFavorites> getUserFavorites(Guid UID) {
            FilterDefinition<UserFavorites> filter = Builders<UserFavorites>.Filter.Where(e => e.userUID.Equals(UID));
            return settings.Find<UserFavorites>(MongoCollection.UserFavorite, filter);
        }
        public UserFavorites getUserFavorite(Guid UID, string type) {
            FilterDefinition<UserFavorites> filter = Builders<UserFavorites>.Filter.Where(e => e.userUID.Equals(UID) && e.itemType.Equals(type));
            return settings.FindOne<UserFavorites>(MongoCollection.UserFavorite, filter);
        }
        public bool upsert(UserFavorites favorites) {
            FilterDefinition<UserFavorites> filter = Builders<UserFavorites>.Filter.Where(e => e.userUID.Equals(favorites.userUID) && e.itemType.Equals(favorites.itemType));
            return settings.UpsertInsert<UserFavorites>(MongoCollection.UserFavorite, filter, favorites);
        }
        #endregion

        public void setWebhook(Webhook key) {
            settings.Insert<Webhook>(MongoCollection.Webhooks, key);
        }
        public Webhook getWebhook(string otp, string key) {
            FilterDefinition<Webhook> filter = Builders<Webhook>.Filter.Where(e => e.otp.Equals(otp) && e.key.Equals(key));
            return settings.FindAndModify<Webhook>(MongoCollection.Webhooks, filter);
        }
    }

    [BsonIgnoreExtraElements]
    public class SettingMeta {
        public string year { get; set; }
        public string eduSubject { get; set; }
        public string type { get; set; }
        public string data { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class TagBinding {
        public Guid tagUID { get; set; }
        public string type { get; set; } // user or service
        public string code { get; set; }
        public string binding { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class Webhook {
        public string type { get; set; }
        public string otp { get; set; }
        public string key { get; set; }
        public string searchKey { get; set; }
    }
}