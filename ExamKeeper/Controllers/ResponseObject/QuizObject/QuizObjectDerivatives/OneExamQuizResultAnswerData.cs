using Newtonsoft.Json;

namespace ExamKeeper.Controllers.ResponseObject.QuizObject.QuizObjectDerivatives
{
    /// <summary>
    /// 欄位實際上很多，但只需要分數資訊
    /// </summary>
    public class OneExamQuizResultAnswerData
    {
        [JsonProperty("score")]
        public int Score;
    }
}
