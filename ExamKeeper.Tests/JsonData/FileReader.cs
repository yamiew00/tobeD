using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Utils;

namespace ExamKeeper.Tests {
    public class ReadJson {
        private string rootPath { get; set; }

        public ReadJson() {
            rootPath = Path.GetFullPath(@"../../../JsonData");
        }
        public List<CodeMap> getQuestionTypes() {
            try {
                string files = Path.Combine(rootPath, "QuestionTypes.json");
                if (File.Exists(files)) {
                    List<FakeQuestionType> questionTypes = JsonSerializer.Deserialize<List<FakeQuestionType>>(File.ReadAllText(files));
                    return questionTypes.Select(o => new CodeMap(o.code, o.name)).ToList();
                }
            } catch {
                // ignore
            }
            return new List<CodeMap>();
        }
    }
}