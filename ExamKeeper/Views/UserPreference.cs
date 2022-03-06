using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;
using Utils;

namespace ExamKeeper.Views {

    public class MainMenu : EduSubject {
        public List<CodeMap> outputMap { get; set; }
        public List<CodeMap> patternMap { get; set; }

        public Dictionary<string, string> publisherMap { get; set; }
    }

    public class Preference {
        public string education { get; set; }
        public string subject { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class UserPreference : Preference {
        public Guid UID { get; set; }
        public Typesetting typesetting { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class UserFavorites {
        public Guid userUID { get; set; }
        public string itemType { get; set; }
        public List<FavoriteIem> items { get; set; }
    }

    public class FavoriteIem {
        public string year { get; set; }
        public string UID { get; set; }
        public DateTime createTime { get; set; }
    }
    public class TypesettingMaps {
        public List<CodeMap> paperSizeMap { get; set; } // 紙張大小
        public List<CodeMap> wordSettingMap { get; set; } // 排版
        public List<CodeMap> paperContent { get; set; } // 匯出卷類
        public List<CodeMap> analyzeContent { get; set; } // 解析卷匯出項目
        public List<CodeMap> advancedSetting { get; set; } // 進階設定項目
        public Typesetting typesetting { get; set; }
    }
    public class OnlineSettingMaps : OnlineSetting {
        public List<CodeMap> advancedSetting { get; set; } // 進階設定項目
    }
    /// <summary> 設定頁面初始值 </summary>
    public class UserTypesetting : TypesettingMaps {
        public Guid UID { get; set; }
        public bool isTeacher { get; set; }
        public string identityName { get; set; }
        public string account { get; set; }
        public string name { get; set; }
        public string organizationName { get; set; }
    }
    /// <summary> 紙本卷設定 </summary>
    public class Typesetting {
        // title
        public string schoolName { get; set; }

        [Required]
        public string paperName { get; set; }
        public string teacherSign { get; set; }
        public string grade { get; set; }
        public string room { get; set; }
        public string eduSubject { get; set; }
        public string studentSign { get; set; }
        // exam paper format
        public string paperSize { get; set; }
        public string wordSetting { get; set; }
        public List<string> paperContents { get; set; }
        public List<string> analyzeContent { get; set; } // 解析卷匯出項目
        [Range(1, 5)]
        public int amount { get; set; } // 出幾卷
        public List<string> advanced { get; set; }
    }
    /// <summary> 線上測驗設定 </summary>
    public class OnlineSetting {
        public List<string> advanced { get; set; }
    }

    #region -format component-
    public enum PaperSize {
        None,
        A4,
        A3,
        B4,
        B5
    }
    /// <summary> doc排版設定 </summary>
    /// V:vertical, H:horizontal
    /// S:Single, D:Double
    public enum WordSetting {
        None,
        [Description("直書單欄")]
        HSS,
        [Description("直書雙欄")]
        HSD,
        [Description("橫書單欄")]
        VHS,
        [Description("橫書雙欄A")]
        VHD,
        [Description("橫書雙欄B")]
        HHD       
    }
    public enum PaperContent {
        None,
        [Description("題目卷")]
        Question,
        [Description("答案卷")]
        Answer,
        [Description("解析卷")]
        Analyze,
        [Description("閱卷")]
        Read,
        [Description("作答紙")]
        AnswerPaper
    }
    public enum AnalyzeContent {
        None,
        [Description("題目")]
        Question,
        [Description("答案")]
        Answer,
        [Description("解析")]
        Analyze,
        [Description("難易度")]
        Difficulty,
        [Description("出處")]
        Source,
        [Description("知識向度")]
        Knowledge
    }

    public enum AdvancedSetting {
        None,
        [Description("題號連續")]
        Continuous,
        [Description("插入組距")]
        AddSpace,
        [Description("相同題目，不同題序")]
        ChangeOrder,
        [Description("相同題目，選項隨機排列")]
        ChangeOption
    }

    #endregion
}