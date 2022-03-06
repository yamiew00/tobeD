using System.Collections.Generic;
using ExamKeeper.Models;
using ExamKeeper.Views;

namespace ExamKeeper.Tests {
    public class FakeResource : IResourceLibrary {
        public BookChapter getBookChapter(string bookID) {
            throw new System.NotImplementedException();
        }

        public EduSubject getEduSubject(string subjectAttr) {
            throw new System.NotImplementedException();
        }

        public Dictionary<string, SubjectMeta> getEduSubjectMeta(string edu, string subject) {
            throw new System.NotImplementedException();
        }

        public List<QuestionType> getQuestionTypes() {
            throw new System.NotImplementedException();
        }

        public TextBook getYearTextBook(string year, string edu, string subject) {
            throw new System.NotImplementedException();
        }
    }
}