using System;
using System.Collections.Generic;
using System.Linq;
using ExamKeeper.Models;
using ExamKeeper.Resources;
using ExamKeeper.Views;
using Utils;
using Utils.Setting;

namespace ExamKeeper.Presenters {
    public class ServiceExam : ExamBase {

        public ServiceExamModel service { get; set; }
        public ServiceProfile profile { get; set; }

        public ServiceExam(ServiceProfile profile) : base(new InitMongoCache(MongoCollection.QueryQuestion)) {
            this.profile = profile;
            service = new ServiceExamModel(new MongoExam(), new MongoExport(), new MongoSetting());
            IMongoLogger logger = new InitMongoLog("ServiceExam");
            examModel = new ExamPaperModel(new MongoExam(), ResourceLibrary.Instance(logger));
            message.setCode(SystemStatus.Start);
        }

        /// <summary> 查詢試卷列表 </summary>
        /// <param name="year">學年度</param>
        /// <param name="tag">指定標籤</param>
        public void queryList(string year, string tag) {
            if (string.IsNullOrWhiteSpace(year)) {
                year = DateTimes.currentSchoolYear(1);
            }
            List<ExamPaper> serviceExam = service.getExamPaperList(profile, year);
            if (!Compare.EmptyCollection(serviceExam) && !string.IsNullOrWhiteSpace(tag)) {
                serviceExam = serviceExam.FindAll(exam => exam.tags.Contains(tag));
            }
            if (Compare.EmptyCollection(serviceExam)) {
                message.setCode(SystemStatus.DataNull);
                return;
            }
            res = service.examPaperFlatten(serviceExam);
            message.setCode(SystemStatus.Succeed);
        }

        /// <summary>查詢試卷明細</summary>
        /// <param name="UID">試卷UID</param>
        /// <param name="optCode">OTP授權碼</param>
        /// <param name="user">指定使用者</param>/
        public void queryExamPaper(string UID, string optCode, string user, Guid? author) {
            ExamPaper exampaper = null;
            if (!checkExamExist(UID, profile.usetype, ref exampaper)) {
                return;
            }
            if (!exampaper.isPublic && !checkOTP(exampaper, optCode, user)) {
                return;
            }
            // query questions
            ServiceQueryQuestion questionModel = new ServiceQueryQuestion(profile);
            QuestionIDPayload questionPayload = new QuestionIDPayload() {
                education = exampaper.attribute.education,
                subject = exampaper.attribute.subject,
                keys = exampaper.getQuestionIDs()
            };
            res = new ExamPaperInfo() {
                UID = UID,
                examPaperInfo = service.flatten(exampaper),
                questionGroup = exampaper.questionGroup,
                questionInfo = questionModel.queryQuestion(questionPayload),
                setting = getOnlineSetting(UID, author)
            };
            message.setCode(SystemStatus.Succeed);
        }

        private OnlineSetting getOnlineSetting(string UID, Guid? author) {
            if (author == null || Guid.Empty.Equals(author)) {
                return null;
            }
            UserExamTypesetting setting = service.getLastSetting(UID, (Guid) author);
            return setting?.onlinesetting ?? null;
        }

        /// <summary> 封存試卷 </summary>
        /// <param name="UID">試卷UID</param>
        public void setLock(string UID) {
            ExamPaper exampaper = null;
            if (!checkExamExist(UID, profile.usetype, ref exampaper)) {
                return;
            }
            service.setLock(exampaper, profile.getDesc());
            message.setCode(SystemStatus.Succeed);
        }

        #region -Export Exam Paper-
        /// <summary> 取得待組試卷 </summary>
        public void getWaiting() {
            List<ExportRecord> records = service.getExamExport();
            if (Compare.EmptyCollection(records)) {
                message.setCode(SystemStatus.DataNull);
                return;
            }
            ExportRecord record = records.OrderBy(o => o.systemTime).First(); // 取得最早資料
            ExportPaper exportPaper = service.getExportPaper(record);
            if (exportPaper == null) {
                // 資料異常，寫回記錄檔
                record.setStatus(ExportStatus.Error, @"無法取得組卷版面設定");
                message.setCode(SystemStatus.Failed, "Can't find ExportPaper");
            } else {
                // 回傳記錄檔
                res = exportPaper;
                record.setStatus(ExportStatus.Start);
                message.setCode(SystemStatus.Succeed);
            }
            service.setExportRecord(record); // update record status
        }

        /// <summary> 更新組卷狀態 </summary>
        public void setStatus(ExportStatusPayload request) {
            ExportRecord record = null;
            if (!checkPayload(request, ref record)) {
                message.setCode(SystemStatus.BadRequest);
                return;
            }
            record.status = request.status;
            record.downloadUrl = request.downloadUrl;
            record.message = request.message;
            record.updateTime = DateTime.Now;
            service.setExportRecord(record); // update record status
            message.setCode(SystemStatus.Succeed);
        }

        private bool checkPayload(ExportStatusPayload request, ref ExportRecord record) {
            if (!ExtensionHelper.checkName<ExportStatus>(request.status)) {
                message.addMessage(CustomString.TypeError("status", request.status));
                return false;
            }
            ExportStatus status = ExtensionHelper.GetFromName<ExportStatus>(request.status);
            if (ExportStatus.Finished.Equals(status) && string.IsNullOrWhiteSpace(request.downloadUrl)) {
                message.addMessage(CustomString.Required("DownloadUrl"));
                return false;
            }
            record = service.getExportRecord(request.UID);
            if (record == null) {
                message.addMessage(CustomString.NotFound("Record", request.UID));
                return false;
            }
            return true;
        }
        #endregion

        #region -OTP-
        private bool checkOTP(ExamPaper exampaper, string optCode, string user) {
            if (string.IsNullOrWhiteSpace(optCode)) {
                message.setCode(SystemStatus.DataNull, CustomString.Required("opt code"));
                return false;
            }
            ServiceOTP otp = service.getOTP(optCode);
            if (otp == null) {
                message.setCode(SystemStatus.DataNull, CustomString.NotMatch("opt"));
                return false;
            }
            DateTime expireAtTime = DateTimes.UnixTimeStampToDateTime(otp.expireAt);
            if (DateTime.Now > expireAtTime) {
                message.setCode(SystemStatus.OTPExpired, CustomString.Expired("opt"));
                return false;
            }
            if (!otp.examUID.Equals(exampaper.UID)) {
                message.setCode(SystemStatus.BadRequest, CustomString.NotMatch("exampaper UID"));
                return false;
            }
            if (!Compare.EmptyCollection(otp.userUID) && !otp.userUID.Contains(user)) {
                message.setCode(SystemStatus.BadRequest, CustomString.Exceed("user"));
                return false;
            }
            return true;
        }
        public void insertOTP(ServiceOTP request) {
            ExamPaper exam = null;
            if (!checkExamExist(request.examUID, profile.usetype, ref exam)) {
                return;
            }
            request.service = profile.code;
            request.systemTime = DateTime.Now;
            service.addOTP(request);
            message.setCode(SystemStatus.Succeed);
        }
        #endregion
    }
}