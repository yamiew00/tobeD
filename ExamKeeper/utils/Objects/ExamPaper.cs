using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;

namespace Utils {
    /// <summary> 新增試卷Payload </summary>
    public class CreateExam {
        [Required]
        public string searchKey { get; set; }

        [Required]
        public string drawUp { get; set; }

        [Required]
        public List<QuestionTypeGroup<string>> questionGroup { get; set; }
    }
    /// <summary> 修改試卷Payload </summary>
    public class EditExam {
        [Required]
        public string examUID { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public string examType { get; set; }
        public string outputType { get; set; } // 幫檢核但不存檔
        public List<QuestionTypeGroup<string>> questionGroup { get; set; } /// 有帶入才修改
    }
    /// <summary> 公開試卷Payload </summary>
    public class PublicExam {
        [Required]
        public string examUID { get; set; }

        [Required]
        public bool isPublic { get; set; }
    }
    /// <summary> 搜尋公開試卷 </summary>
    public class QueryPublicExam {
        [Required]
        public string field { get; set; }

        [Required]
        public string content { get; set; }
    }
    /// <summary> 收藏試卷 </summary>
    public class FavoriteExam {
        [Required]
        public string examUID { get; set; }

        [Required]
        public string year { get; set; }

        [Required]
        public string education { get; set; }

        [Required]
        public string subject { get; set; }
        public bool isAdd { get; set; } // add or remove
    }
    public class QuestionTypeGroup<T> {
        public string typeCode { get; set; }
        public string typeName { get; set; }
        public string scoreType { get; set; }
        public decimal score { get; set; }
        public List<T> questionList { get; set; }
        public ScoreType GetScoreType() {
            return ExtensionHelper.GetFromName<ScoreType>(scoreType);
        }
    }
    /// <summary> 學程相關屬性 </summary>
    public class QueryAttribute {
        [Required]
        public string year { get; set; }

        [Required]
        public string education { get; set; }

        [Required]
        public string subject { get; set; }

        [Required]
        public List<string> bookIDs { get; set; }
    }

    /// <summary> 試卷屬性 </summary>
    [BsonIgnoreExtraElements]
    public class ExamPaperAttribute : QueryAttribute {
        [Required]
        public string name { get; set; }

        [Required]
        public string examType { get; set; }
        public string drawUp { get; set; }
        public string yearName { get; set; }
        public string eduName { get; set; }
        public string subjectName { get; set; }
        public List<string> bookNames { get; set; }

        //========== 查詢後回寫 ==========
        public string version { get; set; }
        public string versionName { get; set; }
        //===============================
    }

    [BsonIgnoreExtraElements]
    public class ExamPaper : ExamPaper<QuestionScore> {
        public List<string> getQuestionIDs() {
            return questionGroup.SelectMany(o => o.questionList.Select(q => q.ID)).ToList();
        }
    }

    #region -複製時加回傳試題難易度-  
    public class ExamPaper<T> : BaseColumn {
        public string UID { get; set; }
        public bool isPublic { get; set; }
        public string usetype { get; set; }
        public ExamPaperAttribute attribute { get; set; }
        public List<QuestionTypeGroup<T>> questionGroup { get; set; }
        public string maintainerUID { get; set; }
        public List<string> tags { get; set; }
        public int favorites { get; set; } // 收藏量
        // ============ Lock ============
        public bool isLock { get; set; }
        public string lockMaintainer { get; set; }
        public DateTime lockTime { get; set; }
        // ==============================
    }
    public class CopyQuestionScore : QuestionScore {
        public string difficulty { get; set; } // 方便前端帶入資料，加回傳難易度
    }
    #endregion

    public class QuestionScore {
        public int sequence { get; set; }
        public string ID { get; set; }
        public int answerAmount { get; set; }
        public decimal score { get; set; } // 單題分數 (要算每答分數再/answerAmount)
    }

    /// ======================================== 試卷列表顯示用 ========================================
    public class ShowExamPaper {
        public string UID { get; set; }
        public string name { get; set; }
        public string eduSubjectName { get; set; }
        public string examTypeName { get; set; }
        public string drawUpName { get; set; }
        public bool isFavorite { get; set; }
        public int favorites { get; set; } // 收藏量
        public ExamPaperDetails attributes { get; set; }
        public List<string> tags { get; set; }
        public string maintainer { get; set; }
        public DateTime createTime { get; set; }
        public DateTime? updateTime { get; set; }
        // ============== 下載 ==============
        public string download { get; set; }
        public string downloadName { get; set; }
        // =================================
        public ShowExamPaper(ExamPaper exam) {
            this.UID = exam.UID;
            this.name = exam.attribute.name;
            this.eduSubjectName = exam.attribute.eduName + exam.attribute.subjectName;
            this.examTypeName = ExtensionHelper.GetFromName<ExamType>(exam.attribute.examType).GetEnumDescription();
            this.drawUpName = ExtensionHelper.GetFromName<DrawUpPattern>(exam.attribute.drawUp).GetEnumDescription();
            this.attributes = Format.objectConvert<ExamPaperAttribute, ExamPaperDetails>(exam.attribute);
            this.maintainer = exam.maintainer;
            this.createTime = (DateTime) exam.createTime;
            this.updateTime = exam.updateTime ?? exam.createTime;
            this.attributes.isPublic = exam.isPublic;
            this.tags = exam.tags;
            this.attributes.questionAmount = exam.questionGroup.Sum(o => o.questionList.Count());
            this.attributes.score = exam.questionGroup.Sum(o => o.questionList.Sum(q => q.score));
            this.isFavorite = false;
            this.favorites = exam.favorites;
        }
    }

    public class ExamPaperDetails : ExamPaperAttribute {
        public bool isPublic { get; set; }
        public int questionAmount { get; set; }
        public decimal score { get; set; }
    }
    public class ExamAttributes {
        public string UID { get; set; }
        public string name { get; set; }
        public string eduSubjectName { get { return educationName + subjectName; } }
        public int questionAmount { get; set; }
        public string year { get; set; }
        public string yearName { get; set; }
        public string education { get; set; }
        public string educationName { get; set; }
        public string subject { get; set; }
        public string subjectName { get; set; }
        public List<string> bookNames { get; set; }
        public string maintainer { get; set; }
        public DateTime createTime { get; set; }
        public DateTime lastUpdateTime { get; set; }
    }

    public class ServiceExamPaper : ExamAttributes {
        public string examTypeName { get; set; }
        public string drawUpName { get; set; }
        public bool isPublic { get; set; }
        public decimal score { get; set; }
        public string examType { get; set; }
        public string drawUp { get; set; }
        public List<string> tags { get; set; }
    }
    /// ===============================================================================================
}