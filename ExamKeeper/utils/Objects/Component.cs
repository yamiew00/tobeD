using System.ComponentModel;

namespace Utils {
    /// <summary> 帳戶狀態 </summary>
    public enum AccountStatus {
        Active,
        Suspended
    }
    /// <summary> 身分別 </summary>
    public enum SystemIdentity {
        None,
        [Description(@"系統管理員")]
        Admin,
        [Description(@"教師")]
        Teacher,
        [Description(@"編輯")]
        Editor,
        [Description(@"訪客")]
        Guest,
        [Description(@"學生")]
        Student
    }
    /// <summary> 機構類別 </summary>
    public enum SystemOrganization {
        None,
        [Description(@"權屬機關")]
        Agency,
        [Description(@"學校")]
        School,
        [Description(@"補習班")]
        Tutoring,
        [Description(@"家教系統")]
        Live
    }
    /// <summary> 啟用規格屬性(處理查詢範圍使用) </summary>
    public enum SpecMeta {
        Source,
        BookID
    }
    /// <summary> 出卷方式 </summary>
    public enum DrawUpPattern {
        None,
        [Description(@"電腦命題")]
        FastPattern,
        [Description(@"智能命題")]
        AutoPattern,
        [Description(@"手動命題")]
        CustomPattern,
        [Description(@"知識向度命題")]
        KnowledgePattern
    }
    /// <summary> 試題難易度 </summary>
    public enum Difficulty {
        None,
        [Description("易")]
        BEGIN = 1,
        [Description("中偏易")]
        BASIC = 2,
        [Description("中")]
        INTERMEDIATE = 3,
        [Description("中偏難")]
        ADVANCED = 4,
        [Description("難")]
        EXPERT = 5
    }
    /// <summary> 配分方式 </summary>
    public enum ScoreType {
        None,
        [Description("每題配分")]
        PerQuestion,
        [Description("每答配分")]
        PerAnswer
    }
    /// <summary> 考試別 </summary>
    public enum ExamType {
        None,
        [Description("平時考")]
        General,
        [Description("段考")]
        Exam,
        [Description("隨堂測驗")]
        Quiz,
        [Description("其他")]
        Others,
    }
    /// <summary> 輸出方式 </summary>
	public enum OutputType {
        None,
        [Description("線上測驗")]
        Online,
        [Description("紙本卷類")]
        Files,
        [Description("速測")]
        Quick,
    }
    /// <summary>雲端題庫資訊使用範圍</summary>
    public enum QuestionUseType {
        None,
        [Description("通用版")]
        General,
        [Description("專業版")]
        Premium
    }
    /// <summary>標籤類別</summary>
    public enum TagType {
        None,
        [Description("對接服務")]
        Service
    }
    /// <summary>系統成員類別</summary>
    public enum MemberType {
        None,
        [Description("對接服務")]
        Service,
        [Description("個人帳號")]
        Account,
    }
    /// <summary> 資料類別 </summary>
    public enum SystemItemType {
        None,
        [Description("試題")]
        Question,
        [Description("試卷")]
        ExamPaper,
        [Description("自主練習")]
        Practice
    }
    /// <summary> 錯誤回報錯誤類別 </summary>
    public enum AnomalyType {
        None,
        [Description("試題內容有誤")]
        Content,
        [Description("答案有誤")]
        Answer,
        [Description("超出課綱範圍")]
        Range,
        [Description("試題修改建議")]
        Suggestion,
        [Description("管理人員停用")]
        Management
    }
    /// <summary> 試卷搜尋欄位 </summary>
    public enum ExamField {
        None,
        [Description("試卷名稱")]
        Name,
        [Description("命題教師")]
        Author,
    }
    /// <summary> 試卷操作(檢核用) </summary>
    public enum PaperAction {
        Read,
        Copy,
        Edit
    }
}