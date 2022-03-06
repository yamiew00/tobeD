using System.Collections.Generic;
using ExamKeeper.Utils;
using ExamKeeper.Views;
using Utils;

namespace ExamKeeper.Models {
    public interface IResourceLibrary {
        /// <summary>學制科目</summary>
        EduSubject getEduSubject(string subjectAttr);
        /// <summary>單一年度學制科目列表</summary>
        TextBook getYearTextBook(string year, string edu, string subject);
        /// <summary>學制科目關聯屬性 (包含基本屬性：難易度等)</summary>
        Dictionary<string, SubjectMeta> getEduSubjectMeta(string edu, string subject);
        /// <summary> 課本章節 </summary>
        BookChapter getBookChapter(string bookID);
        /// <summary> 題型資訊 </summary>
        List<QuestionType> getQuestionTypes();
    }
    public class ResourceLibrary : SendRequest, IResourceLibrary {
        private ResourceTokenSingleton resourceToken { get; set; }
        public ResourceLibrary(IMongoLogger logger) : base(logger) {
            this.logger = logger;
            this.resourceToken = ResourceTokenSingleton.Instance(logger);
        }
        private static readonly object resourcelock = new object();
        private static ResourceLibrary _instance = null;
        public static ResourceLibrary Instance(IMongoLogger logger) {
            lock(resourcelock) {
                if (_instance == null) {
                    _instance = new ResourceLibrary(logger);
                }
            }
            return _instance;
        }
        public TextBook getYearTextBook(string year, string edu, string subject) {
            string url = APIs.Resource.TextBook(year, edu, subject);
            Dictionary<string, string> optionHeader = new Dictionary<string, string>() { { SystemSetting.TokenKey, resourceToken.getToken() } };
            Response<TextBook> response = send<TextBook>(url, optionHeader);
            if (response.isSuccess) {
                return response.data;
            }
            return null;
        }
        public Dictionary<string, SubjectMeta> getEduSubjectMeta(string edu, string subject) {
            string url = APIs.Resource.EduSubjectMeta(edu + subject);
            Dictionary<string, string> optionHeader = new Dictionary<string, string>() { { SystemSetting.TokenKey, resourceToken.getToken() } };
            Response<Dictionary<string, SubjectMeta>> response = send<Dictionary<string, SubjectMeta>>(url, optionHeader);
            if (response.isSuccess) {
                return response.data;
            }
            return null;
        }
        public BookChapter getBookChapter(string bookID) {
            string url = APIs.Resource.BookChapter(bookID);
            Dictionary<string, string> optionHeader = new Dictionary<string, string>() { { SystemSetting.TokenKey, resourceToken.getToken() } };
            Response<BookChapter> response = send<BookChapter>(url, optionHeader);
            if (response.isSuccess) {
                return response.data;
            }
            return null;
        }

        public EduSubject getEduSubject(string type) {
            string url = APIs.Resource.EduSubject(type);
             Dictionary<string, string> optionHeader = new Dictionary<string, string>() { { SystemSetting.TokenKey, resourceToken.getToken() } };
            Response<EduSubject> response = send<EduSubject>(url, optionHeader);
            if (response.isSuccess) {
                return response.data;
            }
            return null;
        }

        public List<QuestionType> getQuestionTypes() {
            string url = APIs.Resource.QuestionType;
            Dictionary<string, string> optionHeader = new Dictionary<string, string>() { { SystemSetting.TokenKey, resourceToken.getToken() } };
            Response<List<QuestionType>> response = send<List<QuestionType>>(url, optionHeader);
            if (response.isSuccess) {
                return response.data;
            }
            return null;
        }

    }

    /// <summary> 資源庫"學制科目"相關屬性 </summary>
    public static class SubjectMetaType {
        public static readonly string Source = "SOURCE";
    }
}