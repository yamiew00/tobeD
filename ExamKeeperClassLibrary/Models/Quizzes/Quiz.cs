using ExamKeeperClassLibrary.Models.ResourceLibrary.Resources;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ExamKeeperClassLibrary.Models.Quizzes
{
    [BsonIgnoreExtraElements]
    public class Quiz
    {
        /// <summary>
        /// 物件id
        /// </summary>
        [BsonId]
        [BsonElement("_id")]
        [JsonIgnore]
        public ObjectId _id { get; set; }

        /// <summary>
        /// 測驗id
        /// </summary>
        [BsonElement("quizUID")]
        public string QuizUID { get; set; }

        /// <summary>
        /// 考卷uid
        /// </summary>
        [BsonElement("examUID")]
        public string ExamUID { get; set; }

        [BsonElement("examName")]
        public string ExamName { get; set; }

        /// <summary>
        /// 測驗碼
        /// </summary>
        [BsonElement("quizCode")]
        public int QuizCode { get; set; }

        /// <summary>
        /// 用戶id
        /// </summary>
        [BsonElement("userUID")]
        public Guid UserUID { get; set; }

        /// <summary>
        /// 測驗名稱
        /// </summary>
        [BsonElement("quizName")]
        public string QuizName { get; set; }

        /// <summary>
        /// 學年度
        /// </summary>
        [BsonElement("schoolYear")]
        [JsonIgnore]
        public string SchoolYear { get; set; }

        /// <summary>
        /// 測驗開始時間
        /// </summary>
        [BsonElement("startTime")]
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 測驗結束時間
        /// </summary>
        [BsonElement("endTime")]
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 作答時間
        /// </summary>
        [BsonElement("duration")]
        public int Duration { get; set; }

        /// <summary>
        /// 是否自動批改
        /// </summary>
        [JsonIgnore]
        [BsonElement("isAutoCheck")]
        public bool IsAutoCheck { get; set; }

        /// <summary>
        /// 學制
        /// </summary>
        [BsonElement("education")]
        public string Education { get; set; }

        /// <summary>
        /// 科目
        /// </summary>
        [BsonElement("subject")]
        public string Subject { get; set; }

        /// <summary>
        /// 年級
        /// </summary>
        [BsonElement("grade")]
        public string Grade { get; set; }

        /// <summary>
        /// 資料製造時間
        /// </summary>
        [BsonElement("createTime")]
        [JsonIgnore]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 學制科目
        /// </summary>
        public string EduSubjectName { get; set; }

        /// <summary>
        /// 前端的轉換要求。
        /// </summary>
        /// <param name="definitions"></param>
        public Quiz EduSubjectFormat(Dictionary<(string, string), Definition> definitions)
        {
            var edu = definitions[("EDU", Education)].Name;
            var subjectName = definitions[("SUBJECT", Subject)].Name;
            EduSubjectName = edu + subjectName;
            return this;
        }
    }
}
