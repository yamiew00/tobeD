using System.IO;
using ExamKeeper;
using Microsoft.Extensions.Configuration;

namespace Utils.Setting {
    #region -SystemStatus-
    public enum SystemStatus {
        Start = 1,
        Succeed = 200,
        Failed = 501,
        // Auth
        BadRequest = 400,
        DataNull = 402,
        TokenError = 401,
        Forbidden = 403,
        // Format
        OverLimit = 601,
        ErrorType = 602,
        // CRUD
        ReadFailed = 701,
        InsertFailed = 702,
        UpdateFailed = 703,
        //Error
        CacheExpired = 801,
        OTPExpired = 802,
        Connection = 803,
        Exception = 9999,
    }
    #endregion

    #region - MongoDB -
    public enum MongoCollection {
        SpecMeta,
        Views,
        Question,
        ExamPaper,
        Practice,
        PracticeRecord,
        WrongRecord,
        ServiceOTP,
        // Cache
        QueryQuestion,
        Selections,
        FixQuestion,
        //Export
        UserExport, // 試卷匯出資訊(for user)
        UserTypesetting, // 匯出格式(for user's export exam)
        ExportRecord, // 試卷匯出紀錄(for export system)
        // Tags
        TagBindings,
        UserPreference,
        UserFavorite,
        // Payload
        Payloads,
        Webhooks,
        TransactionLogs,
        SystemLog,
        Exception
    }
    public class UtilsMongo {
        public static readonly string url = Startup.Configuration.GetConnectionString("MongoDB:Url");
        public static class DataBase {
            public static readonly string Logger = Startup.Configuration.GetConnectionString("MongoDB:DataBase:Log");
            public static readonly string ExamPaper = Startup.Configuration.GetConnectionString("MongoDB:DataBase:ExamPaper");
            public static readonly string ExamExport = Startup.Configuration.GetConnectionString("MongoDB:DataBase:ExamExport");
            public static readonly string Elementary = Startup.Configuration.GetConnectionString("MongoDB:DataBase:Elementary");
            public static readonly string Junior = Startup.Configuration.GetConnectionString("MongoDB:DataBase:Junior");
            public static readonly string Senior = Startup.Configuration.GetConnectionString("MongoDB:DataBase:Senior");
            public static readonly string Settings = Startup.Configuration.GetConnectionString("MongoDB:DataBase:Settings");
            public static readonly string Cache = Startup.Configuration.GetConnectionString("MongoDB:DataBase:Cache");
        }
    }
    #endregion

    #region -Firebase (not using, just for utils)-
    public static class FirebaseInfo {
        public static readonly string WebApiKey = Startup.Configuration.GetValue<string>("");
        public static readonly string Project = Startup.Configuration.GetValue<string>("");
        public static string filePath(string folder) {
            string path = Path.GetFullPath(string.Format(Startup.Configuration.GetValue<string>(""), folder));
            newFolder(path);
            return path;
        }
        private static void newFolder(string path) {
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }
        }
    }
    #endregion
}