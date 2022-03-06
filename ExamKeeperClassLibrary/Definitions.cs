using System.Collections.Generic;

namespace ExamKeeperClassLibrary
{
    /// <summary>
    /// 定義代碼表(待棄用，應該要用資料庫管理)
    /// </summary>
    public static class Definitions
    {
        /// <summary>
        /// 學制
        /// </summary>
        public readonly static Dictionary<string, string> EducationCodes = new Dictionary<string, string>()
        {
            { "E", "國小"},
            { "J", "國中"},
            { "H", "高中"}
        };

        /// <summary>
        /// 年級
        /// </summary>
        public readonly static Dictionary<string, string> GradeCodes = new Dictionary<string, string>()
        {
            { "G01", "一年級"},
            { "G02", "二年級"},
            { "G03", "三年級"},
            { "G04", "四年級"},
            { "G05", "五年級"},
            { "G06", "六年級"},
            { "G07", "七年級"},
            { "G08", "八年級"},
            { "G09", "九年級"}
        };
    }
}
