using ExamKeeperClassLibrary.Models.ResourceLibrary.Resources;
using ExamKeeperClassLibrary.Models.ResourceLibrary.SubjectMeta;
using System.Collections.Generic;
using System.Linq;

namespace ExamKeeperClassLibrary
{
    /// <summary>
    /// 資源庫中所有的定義
    /// </summary>
    public class DefinitionLibrary
    {
        /// <summary>
        /// 學制+科目
        /// </summary>
        public IEnumerable<string> EduSubjects { get; private set; }

        /// <summary>
        /// 出版社
        /// </summary>
        public Dictionary<string, string> Publisher { get; private set; }

        /// <summary>
        /// 冊次名稱
        /// </summary>
        public Dictionary<string, Dictionary<string, string>> VolumeName { get; private set; }

        /// <summary>
        /// 出處
        /// </summary>
        public Dictionary<string, string> Source { get; private set; }

        /// <summary>
        /// 題型
        /// </summary>
        public Dictionary<string, string> QuestionType { get; private set; }

        /// <summary>
        /// 難易度
        /// </summary>
        public Dictionary<string, string> Difficulty { get; private set; }

        public DefinitionLibrary(resourceContext resourceContext, 
                                 SubjectMetaContext subjectMetaContext)
        {
            //載入所有科目
            EduSubjects = resourceContext.Subjects
                                         .Select(subject => subject.EduCode + subject.Code);
            subjectMetaContext.Populate(EduSubjects);

            //載入出版社
            Publisher = resourceContext.Definitions
                                       .Where(def => def.Type == "PUBLISHER")
                                       .ToDictionary(item => item.Code, item => item.Name);

            //載入所有科目的冊次
            VolumeName = new Dictionary<string, Dictionary<string, string>>();
            foreach (var eduSubject in EduSubjects)
            {
                VolumeName[eduSubject] = subjectMetaContext.GetMeta(eduSubject)
                                                           .Where(item => item.MetaType == "BOOK_NAME")
                                                           .ToDictionary(item => item.Code, 
                                                                         item => item.Name);
            }

            //出處
            Source = resourceContext.Sources
                                    .Where(source => source.MetaType == "SOURCE")
                                    .ToDictionary(source => source.Code, source => source.Name);

            //題型
            QuestionType = resourceContext.QuestionTypes
                                          .ToDictionary(item => item.Code, item => item.Name);

            //難易度
            Difficulty = resourceContext.Definitions
                                        .Where(def => def.Type == "DIFFICULTY" && def.Name != "test")
                                        .ToDictionary(diff => diff.Code, diff => diff.Name);
        }
    }
}
