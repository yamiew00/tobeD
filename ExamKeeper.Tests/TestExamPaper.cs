using System.Collections.Generic;
using System.Linq;
using ExamKeeper.Models;
using ExamKeeper.Presenters;
using ExamKeeper.Views;
using Utils;
using Xunit;

namespace ExamKeeper.Tests {
    public class CreateExamTest {
        private FakeQuestions testQuestions = new FakeQuestions();
        private ExamPaperModel fakeModel { get; set; }
        private CreateExam newExamPaper { get; set; }
        private List<QuestionInfo> chosenQuestions { get; set; }

        public CreateExamTest() {
            fakeModel = new ExamPaperModel(new FakeExamDB(), new FakeResource());
        }

        [Fact]
        public void QuestionInit() {
            Assert.Equal(500, testQuestions.fakeQuestions.Count());
        }

        [Fact]
        public void TestCreateExamPaper() {
            // cache
            CacheQuestion cache = new CacheQuestion() {
                examAttribute = new ExamPaperAttribute() {
                year = "109",
                education = "J",
                subject = "CT",
                bookIDs = new List<string>() { "109N-JCTB03" },
                drawUp = "FastPattern"
                },
                views = testQuestions.fakeViews,
                question = testQuestions.fakeQuestions
            };

            //Questions
            List<QuestionInfo> questions = RandomHelper.RandomPick<QuestionInfo>(testQuestions.fakeQuestions, 20);
            IEnumerable<IGrouping<string, QuestionInfo>> picked = questions.GroupBy(o => o.metaContent("QUES_TYPE").code);
            List<QuestionTypeGroup<string>> questionGroup = new List<QuestionTypeGroup<string>>();

            foreach (IGrouping<string, QuestionInfo> item in picked) {
                questionGroup.Add(new QuestionTypeGroup<string>() {
                    typeCode = item.Key,
                        typeName = string.Empty,
                        scoreType = RandomHelper.RandomPick<CodeMap>(testQuestions.ScoreType).code,
                        score = 5,
                        questionList = item.Select(o => o.UID).ToList()
                });
            }
            List<QuestionTypeGroup<QuestionScore>> examQuestions = fakeModel.setQuestionScore(questionGroup, cache);
            examQuestions.ForEach(group => {
                group.questionList.ForEach(question => {
                    switch (group.GetScoreType()) {
                        case ScoreType.PerQuestion:
                            Assert.Equal(group.score, question.score);
                            break;
                        case ScoreType.PerAnswer:
                            decimal score = (group.score * question.answerAmount);
                            Assert.Equal(score, question.score);
                            break;
                    }
                });
            });
        }

        [Fact]
        public void TestPayload() {
            // checkPayload(ref CreateExam request, ref CacheQuestion cache)
            EditExamPaper presenter = new EditExamPaper(FakeUsers.fakeUser, new FakeCache(), fakeModel);

        }
    }
    public class EditExamTest {

    }
}