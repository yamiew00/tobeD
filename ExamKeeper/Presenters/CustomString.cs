namespace ExamKeeper.Resources {
    // 使用VS Code開發, 故未使用resx
    public static class CustomString {

        // =========== System Settings ===========
        public const string System_NotMatch = "登入資訊與授權系統不相符";
        public const string System_NotService = "非授權服務，無法使用雲端題庫相關功能";
        // =========== System Authority ===========
        public const string Auth_NotAllow = "功能尚未開放，無法使用";
        // =========== Check Data ===========
        public static string NotFound(string name, string value = null) {
            return $"{name}資料：{value}不存在, 請確認輸入內容。";
        }
        public static string DataNull(string name) {
            return $"{name}無資料";
        }
        public static string DataDuplicated(string name, string value = null) {
            return $"[{name}資料重複]{value} ";
        }
        public static string ReadFailed(string name, string value = null) {
            return $"[{name}讀取失敗]{value}";
        }
        public static string TypeError(string name, string value = null) {
            return $"[無法識別{name}]{value}";
        }
        public static string UpdateError(string name, string value = null) {
            return $"[{name}資料更新失敗]{value}";
        }
        public static string NotMatch(string name, string value1, string value2) {
            return $"[{name}資料不符]{value1}::{value2}";
        }
        public static string NotMatch(string name) {
            return $"[{name}資料不符]";
        }
        public static string Exceed(string name, string value = "") {
            return $"[{name}資料超出使用範圍]{value}";
        }
        public static string Required(string name) {
            return $"[必須輸入{name}]";
        }
        public static string Connection(string name) {
            return $"[{name}連接異常]";
        }
        public static string Expired(string name) {
            return $"[{name}資料逾期]";
        }
        public static string Locked(string name) {
            return $"[{name}資料已封存]";
        }

        public static class Meta {
            public const string ANSWER_TYPE = "作答方式";
            public const string CHAPTER = "章節";
            public const string LESSON = "課次";
            public const string BOOK = "冊次";
        }
    }
}