using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Utils;

namespace ExamKeeper.Views {
    /// <summary> 出卷範圍篩選Payload集合 </summary>
    public class PatternPayload {
        public string year { get; set; }

        [Required]
        public string education { get; set; }

        [Required]
        public string subject { get; set; }
    }

    /// <summary> 出卷試題查詢Payload集合 </summary>
    public class QuestionPayload : QueryAttribute {
        [Required]
        public List<string> keys { get; set; }
        public List<string> source { get; set; } // 出處
        public string EduSubject() { return education + subject; }
        public string getVersionCode() {
            if (Compare.EmptyCollection(bookIDs)) {
                return string.Empty;
            }
            return bookIDs[0].Substring(3, 1);
        }
    }

    /// <summary> 試題分布查詢 </summary>
    public class ChartPayload {
        [Required]
        public string searchKey { get; set; }

        [Required]
        public List<string> questions { get; set; }
    }

    public class Charts {
        public Charts() {
            typeMap = new List<CodeMap>();
            chart = new Dictionary<string, ChartInfos>();
        }
        public List<CodeMap> typeMap { get; set; }
        public Dictionary<string, ChartInfos> chart { get; set; }
    }
    public class ChartInfos {
        public string code { get; set; }
        public string name { get; set; }
        public int total { get; set; }
        public List<ChartItem> item { get; set; }
        public ChartInfos(QuestionMeta meta) {
            code = meta.code;
            name = meta.name;
            total = 0;
            item = new List<ChartItem>();
            addItem(meta.content, QuesMeta.Difficulty.Equals(meta.code));
        }
        public void addItem(List<CodeMap> meta, bool isDifficulty) {
            meta.ForEach(meta => {
                if (meta != null) {
                    total++;
                    if (isDifficulty) {
                        Difficulty difficulty = ExamChecker.setDifficultyCode(meta.code);
                        meta.code = difficulty.ToString();
                        meta.name = difficulty.GetEnumDescription();
                    }
                    int index = item.FindIndex(o => o.code.Equals(meta.code));
                    if (index < 0) {
                        item.Add(new ChartItem() {
                            code = meta.code,
                                name = meta.name,
                                count = 1
                        });
                    } else {
                        item[index].count++;
                    }
                }
            });
        }
    }
    public class ChartItem {
        public string code { get; set; }
        public string name { get; set; }
        public int count { get; set; }
        public decimal percent { get; set; }
    }
}