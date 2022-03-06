using ExamKeeper.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utils;

namespace ExamKeeper.Models.ExamPapeRutils
{
    public static class MapFormat
    {
        /// <summary> 移除無課本的學年度 </summary>
        public static List<CodeMap> removeYearmap(this List<CodeMap> yearMap, Dictionary<string, TextBookMap> textbookMap)
        {
            List<string> removeYearmap = new List<string>();

            foreach (var yearmap in yearMap)
            {
                int count = 0;

                foreach (KeyValuePair<string, TextBookMap> bookMap in textbookMap)
                {
                    foreach (KeyValuePair<string, BookChapter> chapterMap in bookMap.Value.chapterMap)
                    {
                        if (chapterMap.Value.bookID.Contains(yearmap.code)) count++;
                    }
                }

                if (count == 0) removeYearmap.Add(yearmap.code);
            }

            foreach (var yearCode in removeYearmap) yearMap.RemoveAll(o => o.code.Equals(yearCode));

            return yearMap;
        }
    }
}
