using System;
using System.Collections.Generic;
using ExamKeeper.Views;
using Utils;

namespace ExamKeeper.Tests {
    public class FakeQuestions {
        public List<CodeMap> ScoreType { get; private set; }
        public List<QuestionView> fakeViews { get; private set; }
        public List<QuestionInfo> fakeQuestions { get; private set; }
        List<CodeMap> questionTypes { get; set; }
        List<CodeMap> difficulty { get; set; }

        public FakeQuestions() {
            setFakeViews(500);
            prepare();
            setFakeQuestionsByViews();
        }

        private void prepare() {
            ReadJson reader = new ReadJson();
            questionTypes = reader.getQuestionTypes();
            difficulty = MapFormat.toCodeMap<Difficulty>();
            ScoreType = MapFormat.toCodeMap<ScoreType>();
        }

        private void setFakeViews(int amount) {
            fakeViews = new List<QuestionView>();
            for (int i = 0; i < amount; i++) {
                fakeViews.Add(new QuestionView() {
                    UID = Format.newGuid(),
                        image = "Question Image Path",
                        content = "Question Content",
                        key = new List<string>() { getRandomKey() },
                        createTime = DateTime.Now
                });
            }
        }

        private void setFakeQuestionsByViews() {
            fakeQuestions = new List<QuestionInfo>();
            foreach (QuestionView view in fakeViews) {
                fakeQuestions.Add(new QuestionInfo() {
                    UID = view.UID,
                        image = view.image,
                        questionImage = "Question And Options",
                        metadata = getRandomMeta(),
                        answerInfos = new List<AnswerInfos>(),
                        htmlParts = null,
                        updateTime = DateTime.Now
                });
            }
        }

        private string getRandomKey() {
            return string.Concat("key-", RandomHelper.RandomNumber(2));
        }

        public List<QuestionMeta> getRandomMeta() {
            return new List<QuestionMeta>() {
                new QuestionMeta() {
                        code = QuesMeta.Type,
                            name = @"題型",
                            content = new List<CodeMap>() { RandomHelper.RandomPick<CodeMap>(questionTypes) }
                    },
                    new QuestionMeta() {
                    code = QuesMeta.Difficulty,
                    name = @"難易度",
                    content = new List<CodeMap>() { RandomHelper.RandomPick<CodeMap>(difficulty) }
                    }
            };
        }

    }
}