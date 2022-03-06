using System;
using System.Collections.Generic;
using System.Linq;
using ExamKeeper.Models;
using ExamKeeper.Views;
using Utils;

namespace ExamKeeper.Tests {

    public class FakeExamDB : IMongoExam {
        /// <summary>examPaper:<year, List<ExamPaper>></summary>
        public Dictionary<string, List<ExamPaper>> examPaper = new Dictionary<string, List<ExamPaper>>();
        /// <summary>practiceList:<year, List<Practice>></summary>
        public Dictionary<string, List<Practice>> practiceDic = new Dictionary<string, List<Practice>>();
        /// <summary>practiceList:<year, List<PracticeRecord>></summary>
        public Dictionary<string, List<PracticeRecord>> practiceRec = new Dictionary<string, List<PracticeRecord>>();
        public List<ServiceOTP> otpList = new List<ServiceOTP>();

        #region -ExamPaper-
        public ExamPaper get(string year, string UID) {
            if (examPaper.ContainsKey(year)) {
                return examPaper[year]?.Find(o => o.UID.Equals(UID)) ?? null;
            }
            return null;
        }
        public List<ExamPaper> get(string year, List<string> UIDList) {
            if (!examPaper.ContainsKey(year)) {
                return null;
            }
            return examPaper[year].FindAll(o => UIDList.Contains(o.UID)).ToList();
        }

        public List<ExamPaper> getPublic(string year) {
            return examPaper[year].FindAll(o => o.isPublic).ToList();
        }

        public List<ExamPaper> getUserExamPaper(string year, string userID) {
            if (!examPaper.ContainsKey(year)) {
                return null;
            }
            return examPaper[year].FindAll(o => o.maintainerUID.Equals(userID)).ToList();
        }

        public void insert(ExamPaper exam) {
            if (!examPaper.ContainsKey(exam.attribute.year)) {
                examPaper.Add(exam.attribute.year, new List<ExamPaper>() { exam });
            } else {
                examPaper[exam.attribute.year].Add(exam);
            }
        }
        public void update(ExamPaper exam) {
            if (examPaper.ContainsKey(exam.attribute.year)) {
                int index = examPaper[exam.attribute.year].FindIndex(o => o.UID.Equals(exam.UID));
                if (index >= 0) {
                    examPaper[exam.attribute.year][index] = exam;
                }
            }
        }
        #endregion

        public void insertOTP(ServiceOTP otp) {
            otpList.Add(otp);
        }

        public ServiceOTP getOTP(string otpCode) {
            return otpList.Find(o => o.optCode.Equals(otpCode));
        }

        #region -Practice-
        public void insert(Practice practice) {
            if (!practiceDic.ContainsKey(practice.attribute.year)) {
                practiceDic.Add(practice.attribute.year, new List<Practice>() { practice });
            } else {
                practiceDic[practice.attribute.year].Add(practice);
            }
        }
        public List<Practice> getPracticeList(string userUID, string year) {
            if (practiceDic.ContainsKey(year)) {
                return practiceDic[year].FindAll(o => o.maintainerUID.Equals(userUID));
            }
            return null;
        }
        public Practice getPractice(string UID, string year) {
            if (practiceDic.ContainsKey(year)) {
                return practiceDic[year].Find(o => o.UID.Equals(UID));
            }
            return null;
        }
        public PracticeRecord getPracticeRecord(string UID, string year, Guid userUID) {
            if (practiceRec.ContainsKey(year)) {
                return practiceRec[year].Find(o => o.UID.Equals(UID) && o.userUID.Equals(userUID));
            }
            return null;
        }
        public List<PracticeRecord> getPracticeRecord(string year, Guid userUID) {
            if (practiceRec.ContainsKey(year)) {
                return practiceRec[year].FindAll(o => o.userUID.Equals(userUID));
            }
            return null;
        }
        public bool upsertPracticeRecord(PracticeRecord record) {
            if (practiceRec.ContainsKey(record.year)) {
                int index = practiceRec[record.year].FindIndex(e => e.UID.Equals(record.UID) && e.userUID.Equals(record.userUID));
                if (index >= 0) {
                    practiceRec[record.year][index] = record;
                } else {
                    practiceRec[record.year].Add(record);
                }
            } else {
                practiceRec.Add(record.year, new List<PracticeRecord>() { record });
            }
            return true;
        }

        public void addExportInfo(ExportPaper exportPaper, string year) {
            throw new NotImplementedException();
        }

        public void addExportRecord(ExportRecord record) {
            throw new NotImplementedException();
        }

        public List<ExportRecord> getExportRecords(string status) {
            throw new NotImplementedException();
        }

        public ExportRecord getExportRecord(string UID) {
            throw new NotImplementedException();
        }

        public List<ExportRecord> getExportRecords(List<string> UID) {
            throw new NotImplementedException();
        }

        public ExportPaper getExportPaper(string UID, string year) {
            throw new NotImplementedException();
        }

        public List<ExportPaper> getExportPapers(Guid userUID, string year) {
            throw new NotImplementedException();
        }

        public void updateExportRecord(ExportRecord record) {
            throw new NotImplementedException();
        }

        public List<WrongRecord> getWrongRecord(Guid userUID) {
            throw new NotImplementedException();
        }

        public bool updateWrongRecord(WrongRecord record) {
            throw new NotImplementedException();
        }
        #endregion
    }
}