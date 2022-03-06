using System;
using System.Collections.Generic;
using System.Linq;
using ExamKeeper.Models;
using ExamKeeper.Resources;
using ExamKeeper.Utils;
using ExamKeeper.Views;
using Utils;
using Utils.Setting;
using static ExamKeeper.Utils.APIs;

namespace ExamKeeper.Presenters {
    public class QueryExamPaper : ExamBase {
        private IMongoSetting settingDB { get; set; }
        private ExportModel exportModel { get; set; }

        public QueryExamPaper(UserProfile user) : base(new InitMongoCache(MongoCollection.QueryQuestion)) {
            setUser(user);
            IMongoLogger logger = new InitMongoLog("QueryExamPaper");
            examModel = new ExamPaperModel(new MongoExam(), ResourceLibrary.Instance(logger));
            exportModel = new ExportModel(new MongoExport());
            settingDB = new MongoSetting();
        }

        public void getPersonal(string year) {
            ExamPaperList result = new ExamPaperList();
            List<string> useableType = new List<string>();
            if (!setDefault(year, ref result, ref useableType)) {
                return;
            }
            List<ExamPaper> examList = new List<ExamPaper>();
            List<string> favorites = new List<string>();
            foreach (CodeMap resultYear in result.yearMap) {
                List<string> favoriteExam = getFavoriteExam();
                favorites.AddRange(favoriteExam);
                examList.AddRange(examModel.getPersonalExamPaper(resultYear.code, user.UID, useableType, favoriteExam));
            }
            // sort by createTime time
            examList = examList.OrderByDescending(o => o.createTime).ToList();
            // set response
            if (setExamListResponse(result, examList, favorites)) {
                message.setCode(SystemStatus.Succeed);
            } else {
                message.setCode(SystemStatus.Exception);
            }
        }

        ///<summary>搜尋公開試卷</summary>
        public void getPublic(string field, string content) {
            ExamPaperList result = new ExamPaperList();
            List<string> useableType = new List<string>();
            if (!setDefault(DateTimes.currentSchoolYear(1), ref result, ref useableType)) {
                return;
            }
            if (!ExtensionHelper.checkName<ExamField>(field)) {
                message.setCode(SystemStatus.BadRequest, CustomString.TypeError(field));
                return;
            }
            List<ExamPaper> examList = new List<ExamPaper>();
            List<string> favorites = new List<string>();
            foreach (CodeMap resultYear in result.yearMap) {
                List<string> favoriteExam = getFavoriteExam();
                favorites.AddRange(favoriteExam);
                List<ExamPaper> examPapers = examModel.getPublicExamPaper(resultYear.code, useableType);
                // filter
                if (!Compare.EmptyCollection(examPapers) && !string.IsNullOrWhiteSpace(content)) {
                    switch (ExtensionHelper.GetFromName<ExamField>(field)) {
                        case ExamField.Author:
                            examPapers = examPapers.FindAll(o => o.maintainer.Contains(content));
                            break;
                        case ExamField.Name:
                            examPapers = examPapers.FindAll(o => o.attribute.name.Contains(content));
                            break;
                    }
                }
                examList.AddRange(examPapers);
            }
            // sort by favorite count
            examList = examList.OrderByDescending(o => o.favorites).ThenByDescending(o => o.createTime).ToList();
            // set response
            if (setExamListResponse(result, examList, favorites)) {
                message.setCode(SystemStatus.Succeed);
            } else {
                message.setCode(SystemStatus.Exception);
            }
        }

        ///<summary>取得試卷預覽路徑</summary>
        public void getPreview(string examUID) {
            // check
            ExamPaper exampaper = null;
            if (!checkExamExist(examUID, ref exampaper)) {
                return;
            }
            // set OTP
            string otp = string.Empty;
            if (!exampaper.isPublic) {
                otp = examModel.createOTP(examUID);
            }
            res = OneExam.preview(examUID, otp);
            message.setCode(SystemStatus.Succeed);
        }

        private bool setDefault(string year, ref ExamPaperList result, ref List<string> useableType) {
            if (string.IsNullOrWhiteSpace(year)) {
                year = DateTimes.currentSchoolYear(1);
            }
            result = new ExamPaperList() {
                typeMap = MapFormat.toCodeMap<ExamType>(),
                outputMap = MapFormat.toCodeMap<OutputType>()
            };
            result.yearMap = ExamChecker.getYearMap(year);
            if (result.yearMap == null) {
                message.setCode(SystemStatus.BadRequest, "year error.");
                return false;
            }
            useableType = SystemSetting.GetUseTypeRange(user.usetype);
            if (Compare.EmptyCollection(useableType)) {
                message.setCode(SystemStatus.Forbidden, "No Useable Types.");
                return false;
            }
            return true;
        }

        private bool getYearMap(string yearString, ref ExamPaperList result) {
            int year = 0;
            if (!int.TryParse(yearString, out year)) {
                return false;
            }
            for (int i = 0; i < 3; i++) {
                result.yearMap.Add(new CodeMap((year - i).ToString(), $"{year - i}學年度"));
            }
            return true;
        }

        private List<string> getFavoriteExam() {
            UserFavorites favorites = settingDB.getUserFavorite(user.UID, SystemItemType.ExamPaper.ToString());
            if (favorites != null && !Compare.EmptyCollection(favorites.items)) {
                return favorites.items.Select(o => o.UID).ToList();
            }
            return new List<string>();
        }

        private bool setExamListResponse(ExamPaperList result, List<ExamPaper> examList, List<string> favorites) {
            try {
                List<CodeMap> eduSubject = new List<CodeMap>();
                List<CodeMap> drawUpPattern = new List<CodeMap>();
                List<ExportRecord> exports = exportModel.getExportRecords(user.UID);
                if (!Compare.EmptyCollection(exports)) {
                    exports = exports.OrderByDescending(o => o.updateTime).ToList();
                }
                examList.ForEach(exam => {
                    ShowExamPaper temp = new ShowExamPaper(exam);
                    if (favorites.Contains(temp.UID)) {
                        temp.isFavorite = true;
                    }
                    eduSubject.Add(new CodeMap(exam.attribute.education + exam.attribute.subject, temp.eduSubjectName));
                    drawUpPattern.Add(new CodeMap(temp.attributes.drawUp, temp.drawUpName));
                    int idx = exports.FindIndex(o => o.examUID.Equals(exam.UID) && o.status.Equals(ExportStatus.Finished.ToString()));
                    if (idx > -1) {
                        temp.download = exports[idx].downloadUrl;
                        temp.downloadName = $"{DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")}{exam.attribute.eduName + exam.attribute.subjectName}試卷.zip";
                    }
                    result.examPaper.Add(temp);
                });
                result.eduSubjectMap = eduSubject.doDistinct();
                result.patternMap = drawUpPattern.doDistinct(false);
                res = result;
                return true;
            } catch (Exception ex) {
                message.addMessage(ex.Message);
                return false;
            }
        }
    }

    public class ExamPaperList {
        public ExamPaperList() {
            yearMap = new List<CodeMap>();
            examPaper = new List<ShowExamPaper>();
        }
        public List<CodeMap> yearMap { get; set; } //近三年列表
        public List<CodeMap> eduSubjectMap { get; set; }
        public List<CodeMap> typeMap { get; set; }
        public List<CodeMap> outputMap { get; set; }
        public List<CodeMap> patternMap { get; set; }
        public List<ShowExamPaper> examPaper { get; set; }
    }

}