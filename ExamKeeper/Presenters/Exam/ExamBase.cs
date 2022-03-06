using System;
using System.Collections.Generic;
using System.Linq;
using ExamKeeper.Models;
using ExamKeeper.Resources;
using ExamKeeper.Utils;
using ExamKeeper.Views;
using Utils;
using Utils.Setting;

namespace ExamKeeper.Presenters {
    public class ExamBase : PresenterBase {
        private IMongoCache questionCache { get; set; }
        protected ExamPaperModel examModel { get; set; }
        protected PracticeModel practiceModel { get; set; }
        public ExamBase(IMongoCache cache) {
            questionCache = cache;
            message.setCode(SystemStatus.Start);
        }

        protected bool checkPayload(string searchKey, ref List<QuestionTypeGroup<string>> questionGroup, ref CacheQuestion cache) {
            if (!checkCache(searchKey, ref cache)) {
                return false;
            }
            return checkPayload(cache, ref questionGroup);
        }

        protected bool checkCache(string searchKey, ref CacheQuestion cache) {
            cache = questionCache.getCaches<CacheQuestion>(searchKey);
            if (cache == null) {
                message.setCode(SystemStatus.CacheExpired, CustomString.ReadFailed(@"查詢紀錄"));
                return false;
            }
            if (Compare.EmptyCollection(cache.views) || Compare.EmptyCollection(cache.question) || cache.examAttribute == null) {
                message.setCode(SystemStatus.CacheExpired, CustomString.ReadFailed(@"暫存紀錄遺失，無法驗證試題資訊"));
                return false;
            }
            return true;
        }

        protected bool checkPayload(CacheQuestion cache, ref List<QuestionTypeGroup<string>> questionGroup) {
            bool result = true;
            List<string> allowIDs = cache.views.Select(q => q.UID).ToList();
            Dictionary<string, string> allowTypes = MapFormat.toCodeDic(cache.question.Select(o => o.metaContent(QuesMeta.Type)).ToList().doDistinct());
            List<string> errorIDs = new List<string>();
            List<string> errorQuestionTypes = new List<string>();
            List<string> errorScoreTypes = new List<string>();
            foreach (QuestionTypeGroup<string> item in questionGroup) {
                errorIDs.AddRange(item.questionList.Except(allowIDs).ToList());
                if (!allowTypes.ContainsKey(item.typeCode)) {
                    errorQuestionTypes.Add(item.typeCode);
                } else {
                    item.typeName = allowTypes[item.typeCode];
                }
                if (string.IsNullOrWhiteSpace(item.scoreType)) {
                    errorScoreTypes.Add("Empty Type");
                } else if (!ExtensionHelper.checkName<ScoreType>(item.scoreType)) {
                    errorScoreTypes.Add(item.scoreType);
                }
            }
            if (!Compare.EmptyCollection(errorQuestionTypes)) {
                message.setCode(SystemStatus.BadRequest, CustomString.Exceed(@"題型"));
                result = false;
            }
            if (!Compare.EmptyCollection(errorIDs)) {
                message.setCode(SystemStatus.BadRequest, CustomString.Exceed(@"試題ID", Format.toString(errorIDs)));
                result = false;
            }
            if (!Compare.EmptyCollection(errorScoreTypes)) {
                message.setCode(SystemStatus.BadRequest, CustomString.Exceed(@"配分方式"));
                result = false;
            }
            return result;
        }
        public void setQuestionTypeName(List<QuestionTypeGroup<string>> questionGroup, CacheQuestion cache) {
            List<string> allowIDs = cache.views.Select(q => q.UID).ToList();
            Dictionary<string, string> allowTypes = MapFormat.toCodeDic(cache.question.Select(o => o.metaContent(QuesMeta.Type)).ToList().doDistinct());
            List<string> errorIDs = new List<string>();
            List<string> errorQuestionTypes = new List<string>();
            List<string> errorScoreTypes = new List<string>();
            foreach (QuestionTypeGroup<string> item in questionGroup) {
                item.typeName = allowTypes[item.typeCode];
            }
        }

        protected ExamAttributes setExamAttributes(Practice practice) {
            return new ExamAttributes() {
                UID = practice.UID,
                    name = practice.name,
                    questionAmount = practice.getQuestionIDs().Count(),
                    year = practice.attribute.year,
                    yearName = practice.attribute.yearName,
                    education = practice.attribute.education,
                    educationName = practice.attribute.eduName,
                    subject = practice.attribute.subject,
                    subjectName = practice.attribute.subjectName,
                    maintainer = practice.maintainer,
                    createTime = (DateTime) practice.createTime,
                    lastUpdateTime = (DateTime) (practice.updateTime ?? practice.createTime)
            };
        }

        /// <summary> 試卷基本檢核(使用者) </summary>
        /// <param name="examUID">試卷UID</param>
        /// <param name="exam">回出試卷資訊</param>
        /// <param name="action">目前操作</param>
        protected bool checkExamExist(string examUID, ref ExamPaper exam, PaperAction action = PaperAction.Read) {
            if (!checkExamExist(examUID, user.usetype, ref exam)) {
                return false;
            }
            switch (action) {
                case PaperAction.Read:
                case PaperAction.Copy:
                    if (!exam.isPublic && !exam.maintainerUID.Equals(user.UID.ToString())) {
                        message.setCode(SystemStatus.BadRequest, CustomString.Exceed(@"試卷", "Private Exam Paper."));
                        return false;
                    }
                    break;
                case PaperAction.Edit:
                    if (exam.isLock) {
                        message.setCode(SystemStatus.BadRequest, CustomString.Locked(@"試卷"));
                        return false;
                    }
                    if (!exam.maintainerUID.Equals(user.UID.ToString())) {
                        message.setCode(SystemStatus.BadRequest, CustomString.Exceed(@"試卷", "Not Exam Paper Author."));
                        return false;
                    }
                    break;
            }
            return true;
        }

        /// <summary> 試卷基本檢核 </summary>
        /// <param name="UID">試卷UID</param>
        ///  /// <param name="usetype">使用範圍</param>
        /// <param name="exam">試卷資訊</param>
        protected bool checkExamExist(string UID, string usetype, ref ExamPaper exam) {
            if (!ExamChecker.checkUID(UID)) {
                message.setCode(SystemStatus.BadRequest, CustomString.TypeError("UID"));
                return false;
            }
            exam = examModel.get(UID);
            if (exam == null) {
                message.setCode(SystemStatus.DataNull, CustomString.NotFound("ExamPaper"));
                return false;
            }
            List<string> useableType = SystemSetting.GetUseTypeRange(usetype);
            if (!useableType.Contains(exam.usetype)) {
                message.setCode(SystemStatus.BadRequest, CustomString.Exceed(@"試卷", "Out Of UseType Range."));
                return false;
            }
            return true;
        }
    }
}