using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Utils;

namespace ExamKeeper.Utils {

    public static class AppSetting {
        public static readonly string appSetting = Startup.Configuration.GetValue<string>("appSetting");
        public static readonly string ServiceCode = Startup.Configuration.GetValue<string>("ServiceCode");
        public static readonly string ResourceAccount = Startup.Configuration.GetValue<string>("ResourceAccount");
        public static string URL { get { return Connection.Decrypt.get(Startup.Configuration.GetValue<string>("Webhook")); } }
    }

    #region - System Setting -
    public static class SystemSetting {
        public static readonly string TokenKey = "Authorization";
        public static readonly string UserProfile = "UserProfile";
        public static readonly string env = "ASPNETCORE_ENVIRONMENT";
        public static readonly string SystemTag = "examkeeper";
        public static List<string> GetUseTypeRange(string usetype) {
            QuestionUseType type = ExtensionHelper.GetFromName<QuestionUseType>(usetype);
            List<string> useableTypes = new List<string>();
            switch (type) {
                case QuestionUseType.General:
                    useableTypes.Add(QuestionUseType.General.ToString());
                    break;
                case QuestionUseType.Premium:
                    useableTypes.Add(QuestionUseType.General.ToString());
                    useableTypes.Add(QuestionUseType.Premium.ToString());
                    break;
                default:
                    break;
            }
            return useableTypes;
        }
    }
    #endregion

    #region -APIs-
    [ExcludeFromCodeCoverage]
    public static class APIs {
        public static class CMS {
            static string url { get { return Connection.Decrypt.get(Startup.Configuration.GetConnectionString("APIs:CMS")); } }
            public static string userProfile {
                get { return url + "Member/UserProfile"; }
            }
            public static string clientLogin {
                get { return url + "Client/Login"; }
            }
            public static string serviceProfile {
                get { return url + "Service/Profile"; }
            }
            public static string serviceAccess {
                get { return url + "Service/Access"; }
            }
            public static string serviceRefresh(string serviceCode) {
                return url + $"Service/RefreshToken?service={serviceCode}";
            }
            public static string serviceAnomaly {
                get { return url + "Service/Anomaly/Report"; }
            }
        }
        public static class Resource {
            static string platform { get { return Connection.Decrypt.get(Startup.Configuration.GetConnectionString("APIs:Resource:Platform")); } }
            static string resource { get { return Connection.Decrypt.get(Startup.Configuration.GetConnectionString("APIs:Resource:ResourceLibrary")); } }

            public static string PlatformAccess {
                get { return platform + "Service/Access"; }
            }
            public static string PlatformToken(string serviceCode) {
                return platform + $"Service/Token?service={serviceCode}";
            }
            public static string TextBook(string year, string edu, string subject) {
                return resource + $"Open/{year}/EduSubjectBooks?edu={edu}&subject={subject}&includeSelection=true";
            }
            public static string EduSubjectMeta(string eduSubject) {
                return resource + $"Platform/SubjectMeta/{eduSubject}";
            }
            public static string BookChapter(string bookID) {
                return resource + $"Open/{bookID}/Chapter";
            }
            public static string EduSubject(string subjectAttr) {
                string result = resource + $"Open/Selection/EduSubject";
                if (!string.IsNullOrWhiteSpace(subjectAttr)) {
                    result += $"?subjectAttr={subjectAttr}";
                }
                return result;
            }
            public static string QuestionType {
                get { return resource + $"Open/QuestionType"; }
            }
        }
        public static class OneExam {
            static string api { get { return Connection.Decrypt.get(Startup.Configuration.GetConnectionString("APIs:OneExam:Function")); } }
            static string web { get { return Connection.Decrypt.get(Startup.Configuration.GetConnectionString("APIs:OneExam:Website")); } }
            public static string create {
                get { return api + "exam"; }
            }
            public static string attendees {
                get { return api + "exam/attendees"; }
            }
            public static string start(string examID, string userID) {
                return web + $"user/answer/{examID}/{userID}";
            }
            public static string report(string examID, string userID) {
                return web + $"user/report/{examID}/{userID}";
            }
            public static string preview(string examID, string otp) {
                return web + $"paper/preview/{examID}?otp={otp}";
            }
        }
    }
    #endregion
}