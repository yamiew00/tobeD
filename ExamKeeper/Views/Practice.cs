using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;
using Utils;

namespace ExamKeeper.Views {

    public enum PracticeStatus {
        [Description("未測驗")]
        None, [Description("已建立")]
        Started, [Description("已測驗")]
        Finished,
    }
    public class CreatePractice {
        [Required]
        public string searchKey { get; set; }

        [Required]
        public List<QuestionTypeGroup<string>> questionGroup { get; set; }
    }
    public class ServicePractice : CreatePractice {
        [Required]
        public string year { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public string bookID { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class Practice : BaseColumn {
        public string UID { get; set; }
        public string usetype { get; set; }
        public string name { get; set; }
        public PracticeAttribute attribute { get; set; }
        public List<QuestionTypeGroup<QuestionScore>> questionGroup { get; set; }
        public string source { get; set; } //練習來源:可能是使用者或系統
        public string maintainerUID { get; set; }
        public List<string> getQuestionIDs() {
            return questionGroup.SelectMany(o => o.questionList.Select(q => q.ID)).ToList();
        }
    }

    /// <summary> 學程相關屬性 </summary>
    public class PracticeAttribute : QueryAttribute {
        public string yearName { get; set; }
        public string eduName { get; set; }
        public string subjectName { get; set; }
        public List<string> bookNames { get; set; }

        //========== 查詢後回寫 ==========
        public string version { get; set; }
        public string versionName { get; set; }
        //===============================
    }

    public class ShowPractice {
        public string UID { get; set; }
        public string name { get; set; }
        public string eduSubject { get; set; }
        public string eduSubjectName { get; set; }
        public int amount { get; set; }
        public DateTime createTime { get; set; }
        public string status { get; set; }
        public bool isExam { get; set; } // 是否可進行測驗
        public bool isReport { get; set; } // 是否有測驗結果
        public string examPath { get; set; } // 測驗連結
        public string examReport { get; set; } // 測驗報告

        public string getYear() {
            return UID.Split(Constants.Dash) [0];
        }

        public void setNone() {
            status = PracticeStatus.None.GetEnumDescription();
            isExam = false;
            isReport = false;
            examPath = string.Empty;
            examReport = string.Empty;
        }
    }

    public class PracticeMap {
        public List<CodeMap> eduSubjectMap { get; set; }
        public List<ShowPractice> practice { get; set; }
        public PracticeMap() {
            eduSubjectMap = new List<CodeMap>();
            practice = new List<ShowPractice>();
        }
    }

    [BsonIgnoreExtraElements]
    public class PracticeRecord {
        public string year { get; set; }
        public string UID { get; set; }
        public Guid userUID { get; set; }
        public List<ExamRecord> records { get; set; }

    }

    /// <summary>測驗紀錄</summary>
    public class ExamRecord {
        public string examID { get; set; }
        public string otp { get; set; }
        public DateTime startAt { get; set; }
        public DateTime endAt { get; set; }
        public DateTime createTime { get; set; }
        public DateTime? deliverTime { get; set; } // 紀錄線上測驗回寫的提交時間
        public string resultUrl { get; set; }
        public bool isDelivered() { return !string.IsNullOrWhiteSpace(resultUrl); }
    }

    /// <summary>錯題紀錄</summary>
    [BsonIgnoreExtraElements]
    public class WrongRecord {
        public Guid user { get; set; }
        public string education { get; set; }
        public string subject { get; set; }
        public Dictionary<string, WrongQuestion> questions { get; set; }
    }

    public class WrongQuestion {
        public bool disabled { get; set; } // 我懂了
        public string ID { get; set; }
        public string type { get; set; } // 題型
        public string typeName { get; set; }
        public List<string> keys { get; set; } // 知識向度
        public List<WrongAnswers> records { get; set; }
        public bool containsKey(List<ItemMap> key) {
            if (Compare.EmptyCollection(key)) {
                return false;
            }
            List<string> codes = key.Select(o => o.itemCode).ToList();
            return !Compare.EmptyCollection(codes.Intersect(keys)?.ToList() ?? null);
        }
    }

    public class WrongAnswers {
        public string examUID { get; set; }
        public string examType { get; set; }
        public string UID { get; set; } // 試卷 || 自主練習UID
        public string answer { get; set; } // 正確答案
        public string userAnswer { get; set; } // 作答內容
        public DateTime systemTime { get; set; } // 回寫時間
    }
}