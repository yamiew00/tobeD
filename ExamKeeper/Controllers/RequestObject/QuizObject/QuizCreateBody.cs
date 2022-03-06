using ExamKeeper.Controllers.RequestObject.FunctionalAttributes;
using ExamKeeperClassLibrary;
using System;
using System.ComponentModel.DataAnnotations;

namespace ExamKeeper.Controllers.RequestObject.QuizObject
{
    public class QuizCreateBody : AutoValidateObject
    {
        /// <summary>
        /// jwtToken
        /// </summary>
        [NotEmpty]
        public string JwtToken { get; set; }

        /// <summary>
        /// 測驗名稱
        /// </summary>
        [NotEmpty]
        public string QuizName { get; set; }


        /// <summary>
        /// 測驗開始時間
        /// </summary>
        [NotEmpty]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 考卷ID
        /// </summary>
        [NotEmpty]
        public string ExamID { get; set; }


        /// <summary>
        /// 測驗結束時間
        /// </summary>
        [NotEmpty]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 作答時間
        /// </summary>
        [NotEmpty]
        public int Duration { get; set; }

        /// <summary>
        /// 是否馬上批改
        /// </summary>
        [NotEmpty]
        public bool IsAutoCheck { get; set; }

        /// <summary>
        /// 受測者學制
        /// </summary>
        [NotEmpty]
        [Education]
        public string EducationCode { get; set; }

        /// <summary>
        /// 受測者年級
        /// </summary>
        [NotEmpty]
        [Grade]
        public string GradeCode { get; set; }
    }
}
