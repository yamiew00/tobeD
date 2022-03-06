using System.Collections.Generic;
using Utils;

namespace ExamKeeper.Views {

    public class TextBookMap : TextBookMap<BookChapter> { }
    public class TextBookMap<T> {
        public Dictionary<string, List<CodeMap>> bookMap { get; set; } // <課綱, 課本列表>
        public List<CodeMap> bookList { get { return Format.valueList<string, CodeMap>(bookMap); } } // 課本列表
        public Dictionary<string, T> chapterMap { get; set; }
        public Dictionary<string, string> getBookDescDic() {
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (KeyValuePair<string, List<CodeMap>> item in bookMap) {
                item.Value.ForEach(book => {
                    result.Add(book.code, book.name);
                });
            }
            return result;
        }
    }

    public class BookChapter : BookChapter<Chapter> { }
    public class BookChapter<T> {

        public string bookID { get; set; }
        public string bookName { get; set; }
        public string bookDesc { get; set; } // 第n冊，非完整課名
        // public BookAttribute bookInfo { get; set; } //課本相關屬性, 目前不使用
        public int amount { get; set; } // 試題數量
        public List<T> chapters { get; set; }
    }

    // public class BookAttribute {
    //     public string year { get; set; }
    //     public string version { get; set; }
    //     public string education { get; set; }
    //     public string subject { get; set; }
    //     public string book { get; set; }
    //     public string bookName { get; set; } //從冊名表取得
    //     public string curriculum { get; set; }
    // }

    public class Chapter {
        public int hierarchy { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string parentCode { get; set; }
        public bool hasKnowledge { get { return !Compare.EmptyCollection(knowledgeList); } }
        public List<ItemMap> knowledgeList { get; set; }
        public string Desc { get { return $"[{code}] {name}"; } }
        public int amount { get; set; } // 試題數量
        public bool isParent() {
            return string.IsNullOrWhiteSpace(parentCode);
        }
    }
    public class ItemMap {
        public string itemCode { get; set; }
        public string itemName { get; set; }
    }

    /// <summary> 錯題重測功能 </summary>
    #region -Selection- 
    public class FixBookSelection {
        public string searchKey { get; set; }
        public List<CodeMap> versionMap { get; set; }
        /// <summary> <版本, 課本選單> </summary>
        public Dictionary<string, TextBookMap<BookChapter<FixChapter>>> bookMap { get; set; }
    }

    public class FixChapter {
        public int hierarchy { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string parentCode { get; set; }
        public string Desc { get { return $"[{code}] {name}"; } }
        public int amount { get; set; } // 試題數量
        public List<FixQuestion> questions { get; set; }
        public bool isParent { get { return string.IsNullOrWhiteSpace(parentCode); } }
        public int getAmount() {
            return questions?.Count ?? 0;
        }
    }
    public class FixQuestion {
        public string ID { get; set; }
        public string image { get; set; }
        public QuestionImages questionImage { get; set; }
        public QuestionHtml htmlParts { get; set; }
        public string typeCode { get; set; } // 題型
        public string typeName { get; set; }
        public string answer { get; set; }
        public string userAnswer { get; set; }
    }
    #endregion
}