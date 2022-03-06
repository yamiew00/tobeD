namespace ExamKeeper.Controllers.ResponseObject.QuizObject
{
    public class QuizStudent
    {
        [CsvHelper.Configuration.Attributes.Name("學生座號")]
        public string Seat { get; set; }

        [CsvHelper.Configuration.Attributes.Name("學生姓名")]
        public string Name { get; set; }

        [CsvHelper.Configuration.Attributes.Name("學生成績")]
        public int Score { get; set; }

        [CsvHelper.Configuration.Attributes.Name("學生連結")]
        public string Url { get; set; }
    }
}
