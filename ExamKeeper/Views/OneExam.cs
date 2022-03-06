using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

/// <summary> for OneExam </summary>
namespace ExamKeeper.Views {
    public class PracticeExamPayload {
        [Required]
        public string UID { get; set; }

        [Required]
        public string oneclubToken { get; set; }
    }
    public class CreateOneExam {
        public string examName { get; set; }
        public string paperId { get; set; }
        public int examPeriod { get; set; } // 單位:分鐘
        public string service { get; set; }
        public string paperType { get; set; }
        public string responseAPI { get; set; } // 回寫用
        /*
        // 測驗有開但目前沒使用的
        public DateTime startAt { get; set; }
        public DateTime endAt { get; set; }
        */
    }

    /// <summary> 測驗完成通知 (欄位是線上測驗開的) </summary>
    public class ExamNotice {
        public string examId { get; set; }
        public string userId { get; set; }
        public string status { get; set; }
        public string message { get; set; }
        public List<WrongAnswer> wrongRecord { get; set; } // 錯題欄位
    }

    public class WrongAnswer {
        public string questionID { get; set; } // 試題ID
        public string answer { get; set; } // 正確答案
        public string userAnswer { get; set; } // 作答內容
    }

    public class ExamAttendees {
        public string examId { get; set; }
        public List<ExamUserInfos> userInfos { get; set; }
    }

    public class ExamUserInfos {
        public string userId { get; set; }
        public string userName { get; set; }
        public string seatNo { get; set; }
    }

    public class OneExamResponse<T> {
        public string status { get; set; }
        public T content { get; set; }
        public bool isSuccess() {
            return string.Compare("success", status, true) == 0;
        }
    }

    public class CreateExamInfos {
        public string examId { get; set; }
        public List<InfoMap> info { get; set; }
        public DateTime createDate { get; set; }
        /*
        // 試卷資料都是從雲端題庫帶出去的... 不另外接
        public ExamPaperAttribute attribute { get; set; } 
        "examPeriod": 10,
        "service": "test",// 服務名稱
        "paperId": "110-01f890bfac06440395e99f454362ccc1",
        "paperType": "exam",//試卷類型
        "questions": [
        */
        public List<string> userIds { get; set; } // 測驗者名單
        public DateTime startAt { get; set; }
        public DateTime endAt { get; set; }
        public string status { get; set; }
        public bool includingWriting { get; set; } // 批改使用
    }

    public class InfoMap {
        public string title { get; set; }
        public string content { get; set; }
    }
}