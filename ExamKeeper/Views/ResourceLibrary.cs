using System.Collections.Generic;
using Utils;

namespace ExamKeeper.Views {
    public class EduSubject {

        /// <summary>
        /// 學制
        /// </summary>
        public List<CodeMap> eduMap { get; set; }

        /// <summary>
        /// 學制 + 科目
        /// </summary>
        public Dictionary<string, List<CodeMap>> eduSubject { get; set; }

        /// <summary>
        /// 年級
        /// </summary>
        public Dictionary<string, List<CodeMap>> eduGrade { get; set; }
    }

    public class TextBook {
        public List<CodeMap> yearMap { get; set; }
        public List<CodeMap> eduMap { get; set; }
        public List<CodeMap> subjectMap { get; set; }
        public List<CodeMap> versionMap { get; set; }
        public List<CodeMap> bookMap { get; set; }
        public List<CodeMap> curriculumMap { get; set; }
        public List<TextBookInfo> bookInfo { get; set; }
    }

    public class TextBookInfo {
        public string code { get; set; }
        public string name { get; set; }
        public string curriculum { get; set; }
        public int year { get; set; }
        public string version { get; set; }
        public string education { get; set; }
        public string subject { get; set; }
        public string bookCode { get; set; }
        public string bookName { get; set; }
    }

    public class SubjectMeta {
        public string name { get; set; }
        public List<CodeMap> metadata { get; set; }
    }

    public class QuestionType {
        public string code { get; set; }
        public string name { get; set; }
        public string groupCode { get; set; }
        public string printCode { get; set; }
        public string groupDesc { get; set; }
        public string printDesc { get; set; }
        public string title { get; set; }
    }
}