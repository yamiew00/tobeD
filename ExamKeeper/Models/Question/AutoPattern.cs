using System;
using System.Collections.Generic;
using System.Linq;
using ExamKeeper.Views;
using Utils;
using Utils.Setting;

namespace ExamKeeper.Models {
    /// <summary> 智能命題 (自動出卷) </summary>
    public class AutoPattern : QuestionBase, IDrawUpModel {

        #region -parameters-
        private string year { get; set; }
        private string education { get; set; }
        private string subject { get; set; }
        private string searchKey { get; set; }
        public GeneralBookSelection selection { get; private set; }
        private IMongoCache cache { get; set; }
        private UserProfile user { get; set; }
        #endregion

        #region -results-
        public Dictionary<string, AutoQuestionGroup> questionGroup { get; private set; }
        #endregion
        public AutoPattern(UserProfile user, IMongoSetting setting, IMongoLogger logger, IMongoCache cache) : base(setting, logger, cache) {
            this.user = user;
            this.cache = cache;
        }
        public string getErrorMessage() {
            return ErrorMessage;
        }
        public void setPara(PatternPayload para) {
            this.year = para.year;
            this.education = para.education;
            this.subject = para.subject;
        }

        #region -Selection-
        public bool setSelection() {
            selection = getGeneralSelection(education, subject, year);
            selection.yearMap = ExamPapeRutils.MapFormat.removeYearmap(selection.yearMap, selection.textbookMap);
            return selection != null;
        }
        public object getSelection() {
            return selection;
        }
        #endregion

        #region -QuestionGroup-
        public bool queryQuestions(string usetype, QuestionPayload request, IMongoQuestion questionDB, bool hasQueryAttribute = true) {
            questionDB.getQuestionDB(request.education);
            List<QuestionView> views = questionDB.getQuestionView(request.EduSubject(), request.keys);
            if (!filterUseableTypes(views, usetype)) {
                return false;
            }
            List<string> questionIDs = views.Select(o => o.UID).ToList();
            List<QuestionInfo> infos = questionDB.getQuestion(request.EduSubject(), questionIDs);
            // filter by question-type: 學生限用單一選擇題&是非題
            if (user.checkIdentity(SystemIdentity.Student)) {
                List<QuestionType> questionTypes = resourceAPI.getQuestionTypes();
                List<string> singleSelection = questionTypes.FindAll(o => o.groupCode.Equals("SS") || o.groupCode.Equals("YN")).Select(o => o.code).ToList();
                infos.RemoveAll(o => !singleSelection.Contains(o.metaContentCode(QuesMeta.Type)));
            }
            if (hasQueryAttribute) {
                setChapter(ref infos, request);
            }
            if (!Compare.EmptyCollection(infos)) {
                // insert cache
                searchKey = insertCache(new CacheQuestion {
                    examAttribute = new ExamPaperAttribute() {
                            year = request.year,
                                education = request.education,
                                subject = request.subject,
                                version = request.getVersionCode(),
                                bookIDs = request.bookIDs
                        },
                        views = views,
                        question = infos
                });
            }
            setAutoPatternQuestion(infos);
            return true;
        }
        #endregion

        public object getQuestions() {
            return new {
                searchKey = searchKey,
                    questionGroup = questionGroup
            };
        }

        public object getQuestionsCache(string searchKey) {
            this.searchKey = searchKey;
            CacheQuestion cache = getCache(searchKey);
            if (cache == null) {
                return null;
            }
            setAutoPatternQuestion(cache.question);
            return getQuestions();
        }

        public bool queryQuestionInfo(string searchKey) {
            throw new NotImplementedException("undone");
        }

        public object getQuestionInfo() {
            throw new NotImplementedException("undone");
        }

        #region -Pattern Formatter-
        private void setAutoPatternQuestion(List<QuestionInfo> infos) {
            IEnumerable<IGrouping<Difficulty, QuestionInfo>> group = infos.GroupBy(o => ExamChecker.setDifficultyCode(o.metaContent(QuesMeta.Difficulty).code));
            questionGroup = new Dictionary<string, AutoQuestionGroup>();
            foreach (IGrouping<Difficulty, QuestionInfo> item in group) {
                questionGroup.Add(item.Key.ToString(), setQuestionGroup(item.Key, item.ToList()));
            }
        }

        private AutoQuestionGroup setQuestionGroup(Difficulty difficulty, List<QuestionInfo> infos) {
            AutoQuestionGroup result = new AutoQuestionGroup(difficulty.ToString(), difficulty.GetEnumDescription());
            infos.ForEach(q => {
                AutoQuestion question = new AutoQuestion(q);
                result.question.Add(question);
            });
            return result;
        }
        #endregion
    }
}