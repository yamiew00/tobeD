using System;
using System.Collections.Generic;
using System.Linq;
using ExamKeeper.Views;
using Utils;

namespace ExamKeeper.Models {

    /// <summary> 錯題紀錄 </summary>
    public class FixRecordModel {
        protected IMongoExam examDB { get; set; }
        private IMongoQuestion questionDB { get; set; }
        // add fix record
        public FixRecordModel(IMongoExam db) {
            this.examDB = db;
        }
        // query
        public FixRecordModel(IMongoExam db, IMongoQuestion questionDB) {
            this.examDB = db;
            this.questionDB = questionDB;
        }

        /// <summary> 取得使用者錯題紀錄列表 </summary>
        /// <param name="user">使用者UID</param>
        /// <param name="removeDisabled">預設移除標示 "我懂了"</param>
        public List<WrongRecord> userWrongRecord(Guid user, bool removeDisabled = true) {
            List<WrongRecord> result = examDB.getWrongRecord(user);
            if (!Compare.EmptyCollection(result) && removeDisabled) {
                result.ForEach(o => {
                    foreach (var item in o.questions.Where(q => q.Value.disabled).ToList()) {
                        o.questions.Remove(item.Key);
                    }
                });
            }
            return result;
        }

        // <summary> 取得使用者學制科目錯題紀錄列表 </summary>
        /// <param name="user">使用者UID</param>
        /// <param name="edu">學制代碼</param>
        /// <param name="subject">科目代碼</param>
        /// <returns></returns>
        public WrongRecord userWrongRecord(Guid user, string edu, string subject) {
            return userWrongRecord(user)?.Find(o => o.education.Equals(edu) && o.subject.Equals(subject)) ?? null;
        }

        public List<QuestionView> getViews(string education, string subject, List<string> IDs) {
            questionDB.getQuestionDB(education);
            return questionDB.getQuestionViewByID(education + subject, IDs);
        }

        public List<QuestionInfo> getInfos(string education, string subject, List<string> IDs) {
            questionDB.getQuestionDB(education);
            return questionDB.getQuestion(education + subject, IDs);
        }

        // <summary> 更新使用者錯題紀錄 </summary>
        public bool updateRecord(WrongRecord record) {
            return examDB.updateWrongRecord(record);
        }
    }

}