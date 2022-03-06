using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;
using Utils;

namespace ExamKeeper.Views {
    #region -Query-
    public class QuestionIDPayload {
        [Required]
        public string education { get; set; }

        [Required]
        public string subject { get; set; }

        [Required]
        public List<string> keys { get; set; }
        public string EduSubject() {
            return education + subject;
        }
    }

    public class ServicePayload : QuestionIDPayload {
        [Required]
        public string identity { get; set; }
    }
    public class ExamPaperInfo {
        public string UID { get; set; }
        public ServiceExamPaper examPaperInfo { get; set; }
        public List<QuestionTypeGroup<QuestionScore>> questionGroup { get; set; }
        public List<ServiceQuestion> questionInfo { get; set; }
        public OnlineSetting setting { get; set; } // 進階設定，可能為null
    }
    public class PracticeInfo {
        public string UID { get; set; }
        public ExamAttributes practiceInfo { get; set; }
        public List<QuestionTypeGroup<QuestionScore>> questionGroup { get; set; }
        public List<ServiceQuestion> questionInfo { get; set; }
    }
    public class ServiceQuestion {
        public string UID { get; set; }
        public string image { get; set; } // 試題圖檔(完整內容)
        public QuestionImages questionImage { get; set; }
        public List<QuestionMeta> metadata { get; set; }
        public List<ServiceAnswers> answerInfos { get; set; }
        public QuestionHtml htmlParts { get; set; }
        public DateTime updateTime { get; set; } //最後更新時間
    }
    public class ServiceAnswers : AnswerInfos {
        public string answerTypeName { get; set; } // 線上測驗顯示用
    }
    #endregion

    #region -OTP-
    public class OTPPayload {
        [Required]
        public string examUID { get; set; } // 試卷UID
        [Required]
        public string optCode { get; set; } // 授權碼
        public List<string> userUID { get; set; } // 使用者UID
    }

    [BsonIgnoreExtraElements]
    public class ServiceOTP : OTPPayload {
        public string service { get; set; }
        public DateTime systemTime { get; set; }

        [Required]
        [Range(double.Epsilon, double.MaxValue)]
        public double expireAt { get; set; } // 逾期時間
    }
    #endregion
}