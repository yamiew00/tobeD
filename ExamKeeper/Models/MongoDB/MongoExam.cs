using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ExamKeeper.Views;
using MongoDB.Driver;
using Utils;
using Utils.Setting;

namespace ExamKeeper.Models {

    public interface IMongoExam {
        void insert(ExamPaper exam);
        ExamPaper get(string year, string UID);
        List<ExamPaper> get(string year, List<string> UIDList);
        List<ExamPaper> getPublic(string year);
        List<ExamPaper> getUserExamPaper(string year, string userID);
        void update(ExamPaper exam);
        ServiceOTP getOTP(string code);
        void insertOTP(ServiceOTP otp);
        void insert(Practice practice);
        List<Practice> getPracticeList(string userUID, string year);
        Practice getPractice(string UID, string year);
        PracticeRecord getPracticeRecord(string UID, string year, Guid userUID);
        List<PracticeRecord> getPracticeRecord(string year, Guid userUID);
        bool upsertPracticeRecord(PracticeRecord record);
        List<WrongRecord> getWrongRecord(Guid userUID);
        bool updateWrongRecord(WrongRecord record);
    }

    [ExcludeFromCodeCoverage]
    public class MongoExam : IMongoExam {
        public MongoHelper examDB = new InitMongoDB(UtilsMongo.DataBase.ExamPaper);

        #region -Query-
        public ExamPaper get(string year, string UID) {
            FilterDefinition<ExamPaper> filter = Builders<ExamPaper>.Filter.Where(e => e.UID.Equals(UID));
            return examDB.FindOne<ExamPaper>(MongoCollection.ExamPaper, filter, false, year);
        }
        public List<ExamPaper> get(string year, List<string> UIDList) {
            FilterDefinition<ExamPaper> filter = Builders<ExamPaper>.Filter.In(e => e.UID, UIDList);
            return examDB.Find<ExamPaper>(MongoCollection.ExamPaper, filter, year);
        }
        public List<ExamPaper> getPublic(string year) {
            FilterDefinition<ExamPaper> filter = Builders<ExamPaper>.Filter.Where(o => o.isPublic);
            return examDB.Find<ExamPaper>(MongoCollection.ExamPaper, filter, year);
        }
        public List<ExamPaper> getUserExamPaper(string year, string userUID) {
            FilterDefinition<ExamPaper> filter = Builders<ExamPaper>.Filter.Where(e => e.maintainerUID.Equals(userUID));
            return examDB.Find<ExamPaper>(MongoCollection.ExamPaper, filter, year);
        }
        #endregion

        #region -Edit-
        public void insert(ExamPaper exam) {
            examDB.Insert<ExamPaper>(MongoCollection.ExamPaper, exam, exam.attribute.year);
        }
        public void update(ExamPaper exam) {
            FilterDefinition<ExamPaper> filter = Builders<ExamPaper>.Filter.Where(e => e.UID.Equals(exam.UID));
            examDB.ReplaceOne<ExamPaper>(MongoCollection.ExamPaper, filter, exam, exam.attribute.year);
        }
        public void insertOTP(ServiceOTP otp) {
            examDB.Insert(MongoCollection.ServiceOTP, otp);
        }
        public ServiceOTP getOTP(string otpCode) {
            FilterDefinition<ServiceOTP> filter = Builders<ServiceOTP>.Filter.Where(e => e.optCode.Equals(otpCode));
            return examDB.FindOne<ServiceOTP>(MongoCollection.ServiceOTP, filter);
        }
        #endregion

        #region -Practice-
        public void insert(Practice practice) {
            examDB.Insert<Practice>(MongoCollection.Practice, practice, practice.attribute.year);
        }
        public List<Practice> getPracticeList(string userUID, string year) {
            FilterDefinition<Practice> filter = Builders<Practice>.Filter.Where(e => e.maintainerUID.Equals(userUID));
            return examDB.Find<Practice>(MongoCollection.Practice, filter, year);
        }
        public Practice getPractice(string UID, string year) {
            FilterDefinition<Practice> filter = Builders<Practice>.Filter.Where(e => e.UID.Equals(UID));
            return examDB.FindOne<Practice>(MongoCollection.Practice, filter, false, year);
        }
        public PracticeRecord getPracticeRecord(string UID, string year, Guid userUID) {
            FilterDefinition<PracticeRecord> filter = Builders<PracticeRecord>.Filter.Where(e => e.UID.Equals(UID) && e.userUID.Equals(userUID));
            return examDB.FindOne<PracticeRecord>(MongoCollection.PracticeRecord, filter, false, year);
        }
        public List<PracticeRecord> getPracticeRecord(string year, Guid userUID) {
            FilterDefinition<PracticeRecord> filter = Builders<PracticeRecord>.Filter.Where(e => e.userUID.Equals(userUID));
            return examDB.Find<PracticeRecord>(MongoCollection.PracticeRecord, filter, year);
        }
        public bool upsertPracticeRecord(PracticeRecord record) {
            FilterDefinition<PracticeRecord> filter = Builders<PracticeRecord>.Filter.Where(e => e.UID.Equals(record.UID) && e.userUID.Equals(record.userUID));
            return examDB.UpsertInsert<PracticeRecord>(MongoCollection.PracticeRecord, filter, record, record.year);
        }
        #endregion

        #region -WrongRecord-
        public List<WrongRecord> getWrongRecord(Guid userUID) {
            FilterDefinition<WrongRecord> filter = Builders<WrongRecord>.Filter.Where(e => e.user.Equals(userUID));
            return examDB.Find(MongoCollection.WrongRecord, filter);
        }
        public bool updateWrongRecord(WrongRecord record) {
            FilterDefinition<WrongRecord> filter =
                Builders<WrongRecord>.Filter.Where(e =>
                    e.user.Equals(record.user) &&
                    e.education.Equals(record.education) &&
                    e.subject.Equals(record.subject));
            return examDB.UpsertInsert<WrongRecord>(MongoCollection.WrongRecord, filter, record);
        }
        #endregion
    }
}