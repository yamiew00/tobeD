using System;
using System.Collections.Generic;
using ExamKeeper.Views;

namespace ExamKeeper.Models {
    public class PracticeModel : ExamBaseModel {
        public PracticeModel(IMongoExam db, IResourceLibrary resourceLibrary) : base(db, resourceLibrary) { }

        #region -Query-
        public List<Practice> getPracticeList(string userUID, string year) {
            return examDB.getPracticeList(userUID, year);
        }
        public Practice getPractice(string year, string UID) {
            return examDB.getPractice(UID, year);
        }
        public PracticeRecord getPracticeRecord(string year, string UID, Guid user) {
            return examDB.getPracticeRecord(UID, year, user);
        }
        public List<PracticeRecord> getPracticeRecords(string year, Guid user) {
            return examDB.getPracticeRecord(year, user);
        }
        public TextBook getTextBook(string year, string edu, string subject) {
            return resource.getYearTextBook(year, edu, subject);
        }
        #endregion

        #region -update-
        public bool upsertPracticeRecord(PracticeRecord record) {
            return examDB.upsertPracticeRecord(record);
        }
        #endregion

        #region -insert-
        public void insert(Practice practice) {
            examDB.insert(practice);
        }
        #endregion
    }
}