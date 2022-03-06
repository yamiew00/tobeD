namespace ExamKeeperClassLibrary.Models.QuestionBank.Mongos.QuestionDerivates
{
    /// <summary>
    /// Question的metadata內容
    /// </summary>
    public class MetaData
    {
        public string Knowledge { get; set; }

        public string Source { get; set; }

        public string QuestionType { get; set; }

        public string Difficulty { get; set; }
    }
}
