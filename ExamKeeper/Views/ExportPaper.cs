using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;
using Utils;

/// <summary> 試卷匯出 </summary>
namespace ExamKeeper.Views {

    public enum ExportStatus {
        None,
        [Description("等待中")]
        Waiting,
        [Description("開始組卷")]
        Start,
        [Description("組卷中")]
        Convert,
        [Description("存檔中")]
        Saving,
        [Description("組卷完成")]
        Finished,
        [Description("組卷發生錯誤")]
        Error,
        [Description("系統異常")]
        Exception
    }
    public class ExportPayload {
        [Required]
        public string examUID { get; set; }

        [Required]
        public string outputType { get; set; }
        public Typesetting typesetting { get; set; }
        public OnlineSetting onlineSetting { get; set; }
    }

    [BsonIgnoreExtraElements]

    public class ExportPaper : ExportPayload {
        public string UID { get; set; } // 組卷紀錄識別碼
        public string name { get; set; }
        public string year { get; set; }
        public Guid user { get; set; }
        public List<ExportQuestionTypeGroup<ExportQuestions>> questionGroup { get; set; }
        public int getQuestionAmount() {
            return questionGroup.Sum(o => o.questionList.Count());
        }
    }

    public class ExportQuestionTypeGroup<T> : QuestionTypeGroup<T> {
        public string printCode { get; set; }
        public string printName { get; set; }
    }

    /// <summary> 查詢組卷紀錄回傳物件 </summary>
    public class ExportPaperQuery : ExportStatusPayload {
        public string examUID { get; set; }
        public string examName { get; set; } // 試卷名稱
        public string paperName { get; set; } // 組卷名稱
        public int amount { get; set; } // 總題數
        public string eduSubjectName { get; set; } // 學制科目 (使用者組卷時自行填入的，非資源庫定義資料)
        public DateTime createTime { get; set; }
        public DateTime lastUpdateTime { get; set; } // 最後更新時間

        public ExportPaperQuery(ExportPaper export, ExportRecord record) {
            UID = export.UID;
            examUID = export.examUID;
            examName = export.name;
            paperName = export.typesetting.paperName;
            amount = export.getQuestionAmount();
            eduSubjectName = export.typesetting.eduSubject;
            if (record != null) {
                status = ExtensionHelper.GetFromName<ExportStatus>(record.status).GetEnumDescription();
                createTime = record.systemTime;
                lastUpdateTime = record.updateTime ?? record.systemTime;
                message = record.message;
                downloadUrl = record.downloadUrl;
            } else {
                message = @"組卷紀錄遺失，請重新匯出";
            }
        }
    }

    public class ExportQuestions {
        public int sequence { get; set; }
        public string ID { get; set; }
        public string url { get; set; } // 下載路徑
        public List<QuestionMeta> metadata { get; set; } // 試題相關屬性
    }

    [BsonIgnoreExtraElements]
    public class ExportRecord : ExportStatusPayload {
        public string examUID { get; set; }
        public string year { get; set; } // 匯出時間，跟試卷學年度無關
        public Guid user { get; set; }
        public DateTime systemTime { get; set; }
        public DateTime? updateTime { get; set; }
        public void setStatus(ExportStatus status, string message = "") {
            this.status = status.ToString();
            if (!string.IsNullOrWhiteSpace(message)) {
                this.message = message;
            }
        }
        public string getUpdateTimeString() {
            return updateTime?.ToString("yyyy-MM-dd-HH-mm-ss") ?? string.Empty;
        }
    }

    public class ExportStatusPayload {
        [Required]
        public string UID { get; set; }

        [Required]
        public string status { get; set; } // 狀態，還沒想好要怎麼寫
        public string downloadUrl { get; set; } // 下載路徑，由組卷程式回寫
        public string message { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class UserExamTypesetting {
        public Guid userUID { get; set; }
        public string examUID { get; set; }
        public Typesetting typesetting { get; set; } // 紙本卷設定
        public OnlineSetting onlinesetting { get; set; } // 線上測驗設定
        public UserExamTypesetting() { }
        public UserExamTypesetting(Guid userUID, string examUID) {
            this.userUID = userUID;
            this.examUID = examUID;
        }
    }
}