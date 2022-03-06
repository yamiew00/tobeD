using System.Collections.Generic;
using ExamKeeper.Utils;
using ExamKeeper.Views;
using Utils;
using Utils.Setting;

namespace ExamKeeper.Models {
    /// <summary> 對接線上測驗 </summary>
    public class OneExamModel : SendRequest {
        private string serviceCode = "OnePaper"; // 寫死的
        public OneExamModel(IMongoLogger logger) : base(logger) { }

        #region -Create Exam-
        /// <summary>建立測驗</summary>
        /// <param name="oneclubToken">使用者OneClubToken</param>
        /// <param name="practice">自主練習資訊</param>
        /// <returns></returns>
        public CreateExamInfos createExam(string oneclubToken, Practice practice, string otp) {
            string url = APIs.OneExam.create;
            Dictionary<string, string> optionHeader = new Dictionary<string, string>() { { SystemSetting.TokenKey, oneclubToken } };
            OneExamResponse<CreateExamInfos> response = sendOneExam<CreateExamInfos>(url, optionHeader, setCreateExam(practice, otp));
            if (response == null) {
                return null;
            }
            return response.content;
        }

        private CreateOneExam setCreateExam(Practice practice, string otp) {
            CreateOneExam payload = new CreateOneExam() {
                examName = practice.name + @"_自主練習",
                paperId = practice.UID,
                examPeriod = getExamPeriod(practice.getQuestionIDs().Count),
                service = serviceCode,
                paperType = SystemItemType.Practice.ToString().ToLower(),
                responseAPI = $"{AppSetting.URL}ExamResult?otp={otp}"
            };
            return payload;
        }

        /// <summary>指定作答時間</summary>
        /// <param name="questionAmount">題數</param>
        /// <returns></returns>
        private int getExamPeriod(int questionAmount) {
            int defaultMinute = 10;
            if (questionAmount > 9) {
                defaultMinute = 25;
            }
            if (questionAmount > 24) {
                defaultMinute = 40;
            }
            if (questionAmount > 39) {
                defaultMinute = 100;
            }
            return defaultMinute;
        }
        #endregion

        #region -Add Attendees-
        /// <summary>加入受測者名單</summary>
        public bool addAttendees(string oneclubToken, string examID, UserProfile user) {
            string url = APIs.OneExam.attendees;
            Dictionary<string, string> optionHeader = new Dictionary<string, string>() { { SystemSetting.TokenKey, oneclubToken } };
            OneExamResponse<List<string>> response = sendOneExam<List<string>>(url, optionHeader, setExamAttendees(examID, user));
            return response.isSuccess();
        }

        /// <summary>加入受測者名單</summary>
        private ExamAttendees setExamAttendees(string examID, UserProfile user) {
            return new ExamAttendees() {
                examId = examID,
                    userInfos = new List<ExamUserInfos>() {
                        new ExamUserInfos() {
                        userId = user.account,
                        userName = user.name,
                        seatNo = "none"
                        }
                        }
            };
        }

        #endregion

    }
}