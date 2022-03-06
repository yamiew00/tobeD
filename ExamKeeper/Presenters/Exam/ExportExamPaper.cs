using System.Collections.Generic;
using System.Linq;
using ExamKeeper.Models;
using ExamKeeper.Resources;
using ExamKeeper.Views;
using Utils;
using Utils.Setting;

namespace ExamKeeper.Presenters {
    public class ExportExamPaper : ExamBase {

        private IMongoSetting settingDB { get; set; }
        private QuestionAttributes questionModel { get; set; }
        private ExportModel exportModel { get; set; }

        public ExportExamPaper(UserProfile user) : base(new InitMongoCache(MongoCollection.QueryQuestion)) {
            IMongoLogger logger = new InitMongoLog("ExportExamPaper");
            examModel = new ExamPaperModel(new MongoExam(), ResourceLibrary.Instance(logger));
            questionModel = new QuestionAttributes(new MongoQuestion());
            exportModel = new ExportModel(new MongoExport());
            settingDB = new MongoSetting();
            setUser(user);
        }

        #region -get related-
        /// <summary> 匯出試卷相關設定 </summary>
        public void related(string examUID = "") {

            // ============= prepare =============
            UserExamTypesetting lastSetting = null;
            if (!string.IsNullOrWhiteSpace(examUID)) {
                lastSetting = exportModel.getLastSetting(user.UID, examUID);
            }
            UserPreference preference = settingDB.getUserPreference(user.UID);
            // ===================================

            ExportRelated result = new ExportRelated() {
                outputMap = MapFormat.toCodeMap<OutputType>(),
                settings = new Dictionary<string, object>()
            };
            // 紙本卷設定
            result.settings.Add(OutputType.Files.ToString(), getFileSettings(preference, lastSetting));
            // 線上測驗設定
            result.settings.Add(OutputType.Online.ToString(), getOnlineSettings(preference, lastSetting));
            res = result;
            message.setCode(SystemStatus.Succeed);
        }

        /// <summary> 紙本卷設定 </summary>
        private TypesettingMaps getFileSettings(UserPreference preference, UserExamTypesetting lastsetting) {
            Typesetting setting = preference.typesetting; // default
            if (lastsetting != null && lastsetting.typesetting != null) {
                setting = lastsetting.typesetting;
            }
            TypesettingMaps result = new TypesettingMaps() {
                paperSizeMap = MapFormat.toCodeMap<PaperSize>(),
                wordSettingMap = MapFormat.toCodeMap<WordSetting>(),
                paperContent = MapFormat.toCodeMap<PaperContent>(),
                analyzeContent = MapFormat.toCodeMap<AnalyzeContent>(),
                advancedSetting = MapFormat.toCodeMap<AdvancedSetting>(),
                typesetting = setting
            };
            return result;
        }

        /// <summary> 線上測驗卷設定 </summary>
        private OnlineSettingMaps getOnlineSettings(UserPreference preference, UserExamTypesetting lastsetting) {
            List<string> advanced = preference.typesetting?.advanced ?? null; // default
            if (lastsetting != null && lastsetting.onlinesetting != null) {
                advanced = lastsetting.onlinesetting.advanced;
            } else if (!Compare.EmptyCollection(advanced)) {
                advanced.RemoveAll(o => !o.StartsWith("Change")); // 只留線上測驗可用的設定
            }
            List<CodeMap> maps = MapFormat.toCodeMap<AdvancedSetting>();
            maps.RemoveAll(o => !o.code.StartsWith("Change")); // 只留線上測驗可用的設定
            OnlineSettingMaps result = new OnlineSettingMaps() {
                advancedSetting = maps,
                advanced = advanced
            };
            return result;
        }
        #endregion

        #region -Export Exam Paper-
        /// <summary> 寫入匯出試卷清單 </summary>
        public void start(ExportPayload request) {
            ExamPaper exam = null;
            if (!checkPayload(request, ref exam)) {
                return;
            }
            questionModel.setQuestionDB(exam.attribute.education);
            string UID = Format.newGuid();
            string errorMsg = string.Empty;
            switch (ExtensionHelper.GetFromName<OutputType>(request.outputType)) {
                case OutputType.Files:
                    if (!PaperChecker.checkTypesetting(request.typesetting, ref errorMsg)) {
                        message.setCode(SystemStatus.BadRequest, errorMsg);
                        return;
                    }
                    exportFiles(UID, request, exam);
                    break;
                case OutputType.Online:
                    if (!PaperChecker.checkTypesetting(request.onlineSetting, ref errorMsg)) {
                        message.setCode(SystemStatus.BadRequest, errorMsg);
                        return;
                    }
                    exportOnline(request);
                    break;
            }
            res = new {
                UID = UID,
                examUID = exam.UID
            };
            message.setCode(SystemStatus.Succeed);
        }

        private void exportFiles(string UID, ExportPayload request, ExamPaper exam) {
            Dictionary<string, CodeMap> printCodeDic = examModel.getQuestionTypePrintCode();
            ExportPaper exportPaper = new ExportPaper() {
                UID = UID,
                outputType = OutputType.Files.ToString(),
                examUID = request.examUID,
                typesetting = request.typesetting,
                name = exam.attribute.name,
                year = exam.attribute.year,
                user = user.UID,
                questionGroup = setQuestions(exam.attribute, exam.questionGroup, printCodeDic)
            };
            // add export record
            exportModel.exportRecord(exportPaper, user.UID);
            exportModel.fileSetting(user.UID, request.examUID, request.typesetting);
        }

        private void exportOnline(ExportPayload request) {
            exportModel.onlineSetting(user.UID, request.examUID, request.onlineSetting);
        }

        private bool checkPayload(ExportPayload request, ref ExamPaper exam) {
            if (!checkExamExist(request.examUID, ref exam)) {
                return false;
            }
            if (!ExtensionHelper.checkName<OutputType>(request.outputType)) {
                message.setCode(SystemStatus.BadRequest, CustomString.TypeError(@"輸出方式"));
                return false;
            }
            return true;
        }
        #endregion

        /// <summary> 讀取近兩年組卷紀錄(個人) </summary>
        public void getExportList() {
            // get personal export infos
            List<ExportPaper> exports = exportModel.getExportPaper(user.UID);
            if (Compare.EmptyCollection(exports)) {
                message.setCode(SystemStatus.DataNull);
                return;
            }
            List<string> UIDs = exports.Select(o => o.UID).ToList();
            // get records by UID
            Dictionary<string, ExportRecord> recordDic = exportModel.getExportRecords(UIDs);
            // set result
            List<ExportPaperQuery> result = new List<ExportPaperQuery>();
            exports.ForEach(export => {
                ExportRecord rec = recordDic.ContainsKey(export.UID) ? recordDic[export.UID] : null;
                result.Add(new ExportPaperQuery(export, rec));
            });
            res = result.OrderByDescending(o => o.lastUpdateTime); // 新到舊
            message.setCode(SystemStatus.Succeed);
        }

        /// <summary> 查詢單筆組卷紀錄 </summary>
        public void getExport(string UID) {
            ExportRecord record = exportModel.getExportRecord(UID);
            if (record == null) {
                message.setCode(SystemStatus.BadRequest, CustomString.NotFound(@"匯出紀錄"));
            }
            ExportPaper paper = exportModel.getExportPaper(record);
            if (record == null) {
                message.setCode(SystemStatus.BadRequest, CustomString.NotFound(@"匯出試卷"));
            }
            res = getExportProcess(record, paper.typesetting.eduSubject);
            message.setCode(SystemStatus.Succeed);
        }

        private ExportProcess getExportProcess(ExportRecord rec, string eduSubjectName) {
            ExportStatus status = ExtensionHelper.GetFromName<ExportStatus>(rec.status);
            ExportProcess result = new ExportProcess() {
                UID = rec.UID,
                examUID = rec.examUID,
                status = rec.status,
                statusDesc = status.GetEnumDescription(),
                message = rec.message
            };

            switch (ExtensionHelper.GetFromName<ExportStatus>(rec.status)) {
                case ExportStatus.Waiting:
                    List<ExportRecord> waiting = exportModel.getExportRecords(ExportStatus.Waiting);
                    result.waitingPaper = waiting.FindAll(o => o.systemTime > rec.systemTime).Count() + 1;
                    int sec = result.waitingPaper * 30; // 一張卷估30秒
                    result.minutes = sec / 60;
                    result.seconds = sec % 60;
                    break;
                case ExportStatus.Finished:
                    result.download = rec.downloadUrl;
                    result.downloadName = $"{rec.getUpdateTimeString()}{eduSubjectName}試卷.zip"; // 2022-01-12-14-23-55國中國文試卷.zip
                    break;
                    // 目前不做其他處理
                    // case ExportStatus.Start:
                    // case ExportStatus.Convert:
                    // case ExportStatus.Saving:
                    // case ExportStatus.Error:
                    // case ExportStatus.Exception:
                    //     break;
            }
            return result;
        }

        private List<ExportQuestionTypeGroup<ExportQuestions>> setQuestions(
            ExamPaperAttribute attributes,
            List<QuestionTypeGroup<QuestionScore>> questionGroup,
            Dictionary<string, CodeMap> printCodeDic) {
            List<ExportQuestionTypeGroup<ExportQuestions>> result = new List<ExportQuestionTypeGroup<ExportQuestions>>();
            questionGroup.ForEach(o => {
                result.Add(new ExportQuestionTypeGroup<ExportQuestions>() {
                    typeCode = o.typeCode,
                        typeName = o.typeName,
                        printCode = printCodeDic[o.typeCode].code,
                        printName = printCodeDic[o.typeCode].name,
                        scoreType = o.scoreType,
                        score = o.score,
                        questionList = setQuestions(attributes, o.questionList)
                });
            });
            return result;
        }

        private List<ExportQuestions> setQuestions(ExamPaperAttribute attributes, List<QuestionScore> questions) {
            // get path
            List<string> UID = questions.Select(o => o.ID).ToList();
            Dictionary<string, PathAndMeta> pathDic = questionModel.getDownloadPath(attributes.education, attributes.subject, UID);
            // set ExportQuestions
            List<ExportQuestions> result = new List<ExportQuestions>();
            questions.ForEach(q => {
                result.Add(new ExportQuestions() {
                    sequence = q.sequence,
                        ID = q.ID,
                        url = pathDic[q.ID].path,
                        metadata = pathDic[q.ID].metadata,
                });
            });
            return result;
        }
    }

    public class ExportRelated {
        public List<CodeMap> outputMap { get; set; }
        public Dictionary<string, object> settings { get; set; }
    }

    /// <summary> 匯出進度 </summary>
    public class ExportProcess {
        public string UID { get; set; }
        public string examUID { get; set; }
        public string status { get; set; } // 狀態
        public string statusDesc { get; set; }
        public int waitingPaper { get; set; } // 等待中的卷數
        public int minutes { get; set; } // 預估時間 (分)
        public int seconds { get; set; } // 預估時間 (秒)
        public string download { get; set; }
        public string downloadName { get; set; }
        public string message { get; set; }
    }
}