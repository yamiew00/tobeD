using System.Collections.Generic;
using System.Linq;
using ExamKeeper.Views;
using Utils;

namespace ExamKeeper.Models {
    /// <summary> 電腦命題 (快速出卷) </summary>
    public class FastPattern : QuestionBase, IDrawUpModel {

        #region -parameters-
        private string year { get; set; }
        private string education { get; set; }
        private string subject { get; set; }
        private string searchKey { get; set; }
        public FastSelection selection { get; private set; }
        public Dictionary<string, FastQuestionGroup> questionGroup { get; private set; }
        public FastQuestionInfo questionInfo { get; private set; }
        private IMongoQuestion questionDB { get; set; }
        private IMongoCache cache { get; set; }

        #endregion
        public FastPattern(IMongoSetting setting, IMongoLogger logger, IMongoCache cache) : base(setting, logger, cache) {
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
            GeneralBookSelection general = getGeneralSelection(education, subject, year);
            if (general != null) {
                selection = general.convert<FastSelection>();
                if (specMeta != null && specMeta.ContainsKey(SpecMeta.Source.ToString())) {
                    selection.sourceMap = MapFormat.filterMap(specMeta[SpecMeta.Source.ToString()], metaDictionary[SubjectMetaType.Source].metadata);
                }
                selection.yearMap = ExamPapeRutils.MapFormat.removeYearmap(selection.yearMap,selection.textbookMap);
            }
           return selection != null;
        }
        public object getSelection() {
            return selection;
        }
        #endregion

        #region -QuestionGroup-
        public bool queryQuestions(string usetype, QuestionPayload request, IMongoQuestion questionDB, bool hasQueryAttribute = true) {
            questionDB.getQuestionDB(request.education);
            // get question views
            List<QuestionView> views = questionDB.getQuestionView(request.EduSubject(), request.keys);
            if (!filterUseableTypes(views, usetype)) {
                return false;
            }
            // get question infos
            List<string> questionIDs = views.Select(o => o.UID).ToList();
            List<QuestionInfo> infos = questionDB.getQuestion(request.EduSubject(), questionIDs);
            filterSource(request.source, ref infos); // filter by source
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
            setFastPatternQuestion(infos);
            return true;
        }

        public object getQuestions() {
            return new {
                searchKey = searchKey,
                    questionGroup = questionGroup
            };
        }
        #endregion

        #region -QuestionDetails-
        /// <summary> 挑選試題頁 </summary>
        public bool queryQuestionInfo(string searchKey) {
            // get cache
            CacheQuestion cache = getCache(searchKey);
            if (cache == null || Compare.EmptyCollection(cache.question)) {
                message.addMessage($"cache null:{searchKey}");
                return false;
            }
            // get Filter
            List<object> filter = getFilter(cache.question);
            if (filter == null) {
                message.addMessage($"getFilter Failed.");
                return false;
            }
            // get chapterMap
            List<BookChapter> chapterMap = getChapterMap(cache.examAttribute);
            // get questions & set chapter question amount
            List<GeneralQuestionInfo> questions = setGeneralQuestionInfo(cache.views, cache.question, ref chapterMap);
            // set response
            questionInfo = new FastQuestionInfo() {
                searchKey = searchKey,
                questions = questions,
                bookChapters = chapterMap,
                filter = filter
            };
            return true;
        }

        public object getQuestionInfo() {
            return questionInfo;
        }
        #endregion

        public object getQuestionsCache(string cacheKey) {
            this.searchKey = cacheKey;
            CacheQuestion cache = getCache(searchKey);
            if (cache == null) {
                return null;
            }
            setFastPatternQuestion(cache.question);
            return getQuestions();
        }

        #region -Pattern Formatter-
        private void setFastPatternQuestion(List<QuestionInfo> infos) {
            IEnumerable<IGrouping<string, QuestionInfo>> group = infos.GroupBy(o => o.metaContent(QuesMeta.Type).name);
            questionGroup = new Dictionary<string, FastQuestionGroup>();
            foreach (IGrouping<string, QuestionInfo> item in group) {
                questionGroup.Add(item.Key, setQuestionGroup(item.ToList()));
            }
        }

        private FastQuestionGroup setQuestionGroup(List<QuestionInfo> infos) {
            CodeMap questionType = infos.First().metaContent(QuesMeta.Type);
            FastQuestionGroup result = new FastQuestionGroup(questionType.code, questionType.name);
            infos.ForEach(q => {
                FastQuestion question = new FastQuestion(q, questionType);
                result.question.Add(question);
                result.setCount(question);
            });
            return result;
        }
        #endregion
    }
}