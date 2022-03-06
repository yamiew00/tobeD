using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ExamKeeper.Views;
using MongoDB.Driver;
using Utils;
using Utils.Setting;

namespace ExamKeeper.Models {

    public interface IMongoExport {
        void addExportInfo(ExportPaper exportPaper, string year);
        void addExportRecord(ExportRecord record);
        List<ExportRecord> getExportRecords(string status);
        ExportRecord getExportRecord(string UID);
        List<ExportRecord> getExportRecords(List<string> UID);
        List<ExportRecord> getExportRecords(Guid userUID);
        ExportPaper getExportPaper(string UID, string year);
        List<ExportPaper> getExportPapers(Guid userUID, string year);
        void updateExportRecord(ExportRecord record);
        UserExamTypesetting getLastSetting(Guid userUID, string examUID);
        void setLastSetting(UserExamTypesetting setting);
    }

    [ExcludeFromCodeCoverage]
    public class MongoExport : IMongoExport {
        public MongoHelper examDB = new InitMongoDB(UtilsMongo.DataBase.ExamExport);

        /// ========== ExportPaper ==========
        public void addExportInfo(ExportPaper exportPaper, string year) {
            examDB.Insert<ExportPaper>(MongoCollection.UserExport, exportPaper, year);
        }
        public ExportPaper getExportPaper(string UID, string year) {
            FilterDefinition<ExportPaper> filter = Builders<ExportPaper>.Filter.Where(e => e.UID.Equals(UID));
            return examDB.FindOne<ExportPaper>(MongoCollection.UserExport, filter, false, year);
        }
        public List<ExportPaper> getExportPapers(Guid userUID, string year) {
            FilterDefinition<ExportPaper> filter = Builders<ExportPaper>.Filter.Where(e => e.user.Equals(userUID));
            return examDB.Find<ExportPaper>(MongoCollection.UserExport, filter, year);
        }
        /// ========== ExportRecord ==========
        public void addExportRecord(ExportRecord record) {
            examDB.Insert<ExportRecord>(MongoCollection.ExportRecord, record);
        }
        /// <summary> 取得指定狀態紀錄 </summary>
        public List<ExportRecord> getExportRecords(string status) {
            FilterDefinition<ExportRecord> filter = Builders<ExportRecord>.Filter.Where(e => e.status.Equals(status));
            return examDB.Find<ExportRecord>(MongoCollection.ExportRecord, filter);
        }
        public List<ExportRecord> getExportRecords(List<string> UID) {
            FilterDefinition<ExportRecord> filter = Builders<ExportRecord>.Filter.In(e => e.UID, UID);
            return examDB.Find<ExportRecord>(MongoCollection.ExportRecord, filter);
        }
        public List<ExportRecord> getExportRecords(Guid user) {
            FilterDefinition<ExportRecord> filter = Builders<ExportRecord>.Filter.Where(e => e.user.Equals(user));
            return examDB.Find<ExportRecord>(MongoCollection.ExportRecord, filter);
        }
        public void updateExportRecord(ExportRecord record) {
            FilterDefinition<ExportRecord> filter = Builders<ExportRecord>.Filter.Where(e => e.UID.Equals(record.UID));
            examDB.ReplaceOne<ExportRecord>(MongoCollection.ExportRecord, filter, record);
        }
        public ExportRecord getExportRecord(string UID) {
            FilterDefinition<ExportRecord> filter = Builders<ExportRecord>.Filter.Where(e => e.UID.Equals(UID));
            return examDB.FindOne<ExportRecord>(MongoCollection.ExportRecord, filter);
        }
        public UserExamTypesetting getLastSetting(Guid userUID, string examUID) {
            return examDB.FindOne<UserExamTypesetting>(MongoCollection.UserTypesetting, filter(userUID, examUID));
        }
        public void setLastSetting(UserExamTypesetting setting) {
            examDB.UpsertInsert<UserExamTypesetting>(MongoCollection.UserTypesetting, filter(setting.userUID, setting.examUID), setting);
        }
        private FilterDefinition<UserExamTypesetting> filter(Guid userUID, string examUID) {
            return Builders<UserExamTypesetting>.Filter.Where(e => e.userUID.Equals(userUID) && e.examUID.Equals(examUID));
        }
    }
}