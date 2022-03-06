using System.Collections.Generic;
using System.Linq;
using ExamKeeper.Models;
using ExamKeeper.Views;

namespace ExamKeeper.Tests {

    public class FakeQuestionDB : IMongoQuestion {
        public FakeQuestionDB(List<QuestionView> views, List<QuestionInfo> infos) {
            this.views = views;
            this.infos = infos;

        }
        List<QuestionView> views = new List<QuestionView>();
        List<QuestionInfo> infos = new List<QuestionInfo>();
        public List<QuestionInfo> getQuestion(string eduSubject, List<string> UIDList) {
            return infos.FindAll(o => UIDList.Contains(o.UID)).ToList();
        }

        public void getQuestionDB(string edu) {
            return;
        }

        public List<QuestionView> getQuestionView(string eduSubject, List<string> keys) {
            return views.FindAll(o => keys.Any(key => o.key.Contains(key))).ToList();
        }

        public void addUsage(string eduSubject, List<string> UIDList) {
            views.FindAll(o => UIDList.Contains(o.UID)).ForEach(view => {
                view.usage = (view.usage + 1);
            });
        }

        public QuestionView getView(string eduSubject, string UID) {
            return views.Find(o => o.UID.Equals(UID));
        }

        public List<QuestionView> getQuestionViewByID(string eduSubject, List<string> UIDList) {
            return views.FindAll(o => UIDList.Contains(o.UID));
        }

        public List<QuestionView> getViews(string eduSubject, List<string> UID) {
            throw new System.NotImplementedException();
        }
    }
}