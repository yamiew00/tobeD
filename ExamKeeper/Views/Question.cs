using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using Utils;

namespace ExamKeeper.Views {
    public static class QuesMeta {
        public const string Type = "QUES_TYPE";
        public const string Difficulty = "DIFFICULTY";
        public const string Source = "SOURCE";
        public const string SubSource = "SUB_SOURCE";
        public const string Knowledge = "KNOWLEDGE";
        public const string Learn = "LEARN_CONTENT";
        public const string AnswerType = "ANSWER_TYPE"; // 作答方式，由題型反推
        public const string Chapter = "CHAPTER"; // 章節代碼，由向度反推
        public const string Lesson = "LESSON"; // 課次代碼，由向度反推
        public const string Book = "BOOK"; // 課本代碼，由向度反推
    }

    [BsonIgnoreExtraElements]
    public class QuestionView {
        public string UID { get; set; }
        public bool disabled { get; set; }
        public string image { get; set; } // 試題圖檔(完整內容)
        public string content { get; set; } // 預覽字串
        public string soundtrack2 { get; set; } // 正常速度音檔路徑
        public string usetype { get; set; }
        public int usage { get; set; } //試卷使用次數
        public List<string> key { get; set; } // 查詢key
        public DateTime createTime { get; set; }
        public DateTime? updateTime { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class QuestionInfo {
        public string UID { get; set; }
        public string image { get; set; } // 試題圖檔(完整內容)
        public string questionImage { get; set; } // 試題圖檔(僅試題+選項)
        public string questionDoc { get; set; } // 試題文檔(完整)
        public List<QuestionMeta> metadata { get; set; }
        public List<AnswerInfos> answerInfos { get; set; } //答案內容
        public List<SortPath> detailImage { get; set; } // 試題圖檔(解析細項)
        public List<SortPath> soundtracks { get; set; } // 試題音檔
        public QuestionHtml htmlParts { get; set; }
        public DateTime updateTime { get; set; } //最後更新時間
        public CodeMap metaContent(string type) {
            return metadata.Find(o => o.code.Equals(type)).content[0];
        }
        public List<CodeMap> metaContents(string type) {
            return metadata.Find(o => o.code.Equals(type)).content;
        }
        public string metaContentCode(string type) {
            return metadata.Find(o => o.code.Equals(type))?.content[0].code ?? string.Empty;
        }
        public void decompress() {
            if (htmlParts != null) {
                htmlParts.content = GZip.decompress(htmlParts.content);
                htmlParts.answer = GZip.decompress(htmlParts.answer);
                htmlParts.analyze = GZip.decompress(htmlParts.analyze);
            }
        }

        public QuestionImages getImages() {
            return new QuestionImages() {
                question = detailImage.Find(o => o.code.Equals("QuestionOverall.gif"))?.path,
                    content = detailImage.Find(o => o.code.Equals("QuestionContent.gif"))?.path,
                    answer = detailImage.Find(o => o.code.Equals("Answer.gif"))?.path,
                    analyze = detailImage.Find(o => o.code.Equals("Analyze.gif"))?.path,
                    subQuestions = getSubImages()
            };
        }

        private List<SubQuestionImages> getSubImages() {
            List<SubQuestionImages> result = new List<SubQuestionImages>();
            SubQuestionImages temp = null;
            bool prevContent = false;
            detailImage.ForEach(img => {
                if (img.code.StartsWith("QuestionContent")) {
                    if (temp != null) {
                        result.Add(temp);
                    }
                    prevContent = true;
                    temp = new SubQuestionImages() {
                        question = img.path
                    };
                } else if (img.code.StartsWith("options")) {
                    if (temp == null) {
                        temp = new SubQuestionImages(); // 聽力題可能沒有試題內文
                    }
                    temp.options.Add(img.path);
                } else if (img.code.StartsWith("SubQuestion")) {
                    if (temp != null && !prevContent) {
                        result.Add(temp);
                    }
                    temp = new SubQuestionImages() {
                        question = img.path
                    };
                    prevContent = false;
                }
            });
            result.Add(temp);
            return result;
        }

        public int getAnswerAmount() {
            int result = 0;
            if (answerInfos == null) {
                return result;
            }
            foreach (AnswerInfos info in answerInfos) {
                result += info.answerAmount;
            }
            return result;
        }
        public List<string> getAnswerText() {
            List<string> result = new List<string>();
            if (Compare.EmptyCollection(answerInfos)) {
                return result;
            }
            foreach (AnswerInfos info in answerInfos) {
                if (!Compare.EmptyCollection(info.answer)) {
                    result.AddRange(info.answer);
                }
            }
            return result;
        }
    }

    public class QuestionImages {
        public string question { get; set; }
        public string content { get; set; }
        public List<SubQuestionImages> subQuestions { get; set; }
        public string answer { get; set; }
        public string analyze { get; set; }
    }
    public class SubQuestionImages {
        public string question { get; set; }
        public List<string> options { get; set; }
        public SubQuestionImages() {
            options = new List<string>();
        }
    }
    public class QuestionMeta {
        public string code { get; set; }
        public string name { get; set; }
        public List<CodeMap> content { get; set; }
    }
    public class AnswerInfos {
        public int index { get; set; } // 題號
        public string answerType { get; set; } // 作答方式 (代碼)
        public int answerAmount { get; set; } // 答案數
        public List<string> answer { get; set; } // 答案內容 (文字)
        public List<int> position { get; set; } // 答案位置 (選擇題才處理)
    }
    public class QuestionHtml {
        //every string has been compressed
        public string content { get; set; }
        public string answer { get; set; }
        public string analyze { get; set; }
    }

    /// <summary> sorted PathMap </summary>
    public class SortPath : PathMap {
        public string orderby { get; set; }
    }

    public class AnomalyPayload {
        public string education { get; set; }
        public string subject { get; set; }
        public string UID { get; set; }
        public string anomalyType { get; set; }
        public string description { get; set; }
    }

    public class ReportAnomaly : AnomalyPayload {
        public string itemID { get { return UID; } }
        public string itemType { get; set; }
        public string remark { get; set; }
        public string user { get; set; }
        public string userIdentity { get; set; }
    }
}