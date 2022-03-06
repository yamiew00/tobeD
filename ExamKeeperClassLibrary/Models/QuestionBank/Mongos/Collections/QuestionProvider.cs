using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamKeeperClassLibrary.Models.QuestionBank.Mongos.Collections
{
    /// <summary>
    /// 題目提供者(資料來自QuestionBank_MongoDB)
    /// 用法: QuestionProvider["ECH"]
    /// </summary>
    public class QuestionProvider
    {
        /// <summary>
        /// 按科目分類的QuestionCollection
        /// </summary>
        public readonly Dictionary<string, QuestionCollection> QuestionCollections = new Dictionary<string, QuestionCollection>();

        /// <summary>
        /// 取單一科目的Collection
        /// </summary>
        /// <param name="subject"></param>
        /// <returns></returns>
        public QuestionCollection this[string subject]
        {
            get => QuestionCollections[subject];
        }

        public QuestionProvider(IEnumerable<string> eduSubjects, MongoDataStream<Question> mongoDataStream) 
        {
            QuestionCollections = eduSubjects.ToDictionary(subject => subject, 
                                                           subject => new QuestionCollection(subject, mongoDataStream));
        }
    }
}
