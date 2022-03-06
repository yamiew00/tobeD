using System;
using System.Collections.Generic;
using System.Linq;
using ExamKeeper.Views;
using Utils;

namespace ExamKeeper.Models {
    public enum OPTStatus {
        NotExist,
        Expired,
        Available
    }
    public class ServiceExamModel {
        private IMongoExam examDB { get; set; }
        private IMongoSetting settingDB { get; set; }
        private IMongoExport exportDB { get; set; }
        public ServiceExamModel(IMongoExam db, IMongoExport export, IMongoSetting setting) {
            this.examDB = db;
            this.settingDB = setting;
            this.exportDB = export;
        }

        /// <summary>系統查詢預設卷</summary>
        /// <param name="yearRange">預設查3年內的資訊</param>
        public List<ExamPaper> getExamPaperList(ServiceProfile service, string year, int yearRange = 3) {
            // get tags 
            List<TagBinding> tags = settingDB.getTags(service.account);
            if (Compare.EmptyCollection(tags)) {
                return null;
            }
            // query exam paper
            List<ExamPaper> result = new List<ExamPaper>();
            int intYear = Convert.ToInt16(year);
            for (int i = 0; i < yearRange; i++) {
                result.AddRange(examDB.getPublic((intYear - i).ToString()));
            }
            if (!Compare.EmptyCollection(result)) {
                List<string> allowTag = tags.Select(o => o.code).ToList();
                result = result.FindAll(exam => !Compare.EmptyCollection(exam.tags) && allowTag.Intersect(exam.tags).Count() > 0);
            }
            return result;
        }

        /// <summary>系統讀取試卷列表格式</summary>
        public List<ServiceExamPaper> examPaperFlatten(List<ExamPaper> paper) {
            if (Compare.EmptyCollection(paper)) {
                return null;
            }
            List<ServiceExamPaper> result = new List<ServiceExamPaper>();
            paper.ForEach(o => {
                result.Add(flatten(o));
            });
            return result.OrderByDescending(o => o.lastUpdateTime).ToList();
        }
        public ServiceExamPaper flatten(ExamPaper exampaper) {
            ShowExamPaper paper = new ShowExamPaper(exampaper);
            return new ServiceExamPaper() {
                UID = paper.UID,
                    name = paper.name,
                    isPublic = paper.attributes.isPublic,
                    questionAmount = paper.attributes.questionAmount,
                    score = paper.attributes.score,
                    year = paper.attributes.year,
                    yearName = paper.attributes.yearName,
                    education = paper.attributes.education,
                    educationName = paper.attributes.eduName,
                    subject = paper.attributes.subject,
                    subjectName = paper.attributes.subjectName,
                    bookNames = paper.attributes.bookNames,
                    examType = paper.attributes.examType,
                    examTypeName = paper.examTypeName,
                    drawUp = paper.attributes.drawUp,
                    drawUpName = paper.drawUpName,
                    tags = paper.tags,
                    maintainer = paper.maintainer,
                    createTime = paper.createTime,
                    lastUpdateTime = paper.updateTime ?? paper.createTime

            };
        }

        public ExamPaper getExamPaper(string year, string UID) {
            return examDB.get(year, UID);
        }

        public bool checkExist(string year, string examUID) {
            return examDB.get(year, examUID) != null;
        }

        #region -Edit-
        public void setLock(ExamPaper exampaper, string maintainer) {
            exampaper.isLock = true;
            exampaper.lockMaintainer = maintainer;
            exampaper.lockTime = DateTime.Now;
            examDB.update(exampaper);
        }
        #endregion

        #region -Export-
        public List<ExportRecord> getExamExport(ExportStatus status = ExportStatus.Waiting) {
            return exportDB.getExportRecords(status.ToString());
        }
        public ExportPaper getExportPaper(ExportRecord record) {
            return exportDB.getExportPaper(record.UID, record.year);
        }
        public void setExportRecord(ExportRecord record) {
            exportDB.updateExportRecord(record);
        }
        public ExportRecord getExportRecord(string UID) {
            return exportDB.getExportRecord(UID);
        }
        public UserExamTypesetting getLastSetting(string UID, Guid userUID) {
            return exportDB.getLastSetting(userUID, UID);
        }
        #endregion

        #region -OTP-
        public OPTStatus checkOTP(OTPPayload request) {
            ServiceOTP otp = examDB.getOTP(request.optCode);
            if (otp == null) {
                return OPTStatus.NotExist;
            }
            if (DateTimes.UnixTimeStampToDateTime(otp.expireAt) < DateTime.Now) {
                return OPTStatus.Expired;
            }
            return OPTStatus.Available;
        }
        public void addOTP(ServiceOTP otp) {
            examDB.insertOTP(otp);
        }
        public ServiceOTP getOTP(string otp) {
            return examDB.getOTP(otp);
        }
        #endregion
    }
}