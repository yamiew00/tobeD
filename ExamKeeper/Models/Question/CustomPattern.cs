using System.Collections.Generic;
using System.Linq;
using ExamKeeper.Views;
using Utils;
using Utils.Setting;

namespace ExamKeeper.Models {
    /// <summary> 手動命題 (全數自選題) </summary>
    public class CustomPattern : QuestionBase, IDrawUpModel {

        #region -parameters-
        private string year { get; set; }
        private string education { get; set; }
        private string subject { get; set; }
        private string searchKey { get; set; }
        public CustomSelection selection { get; private set; }
        private IMongoCache cache { get; set; }
        private UserProfile user { get; set; }
        #endregion

        #region -results-
        public Dictionary<string, CustomQuestionGroup> questionGroup { get; private set; }
        public CustomQuestionInfo questionInfo { get; private set; }
        #endregion
        public CustomPattern(UserProfile user, IMongoSetting setting, IMongoLogger logger, IMongoCache cache) : base(setting, logger, cache) {
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
            GeneralBookSelection general = getGeneralSelection(education, subject, year);
            if (general != null) {
                selection = general.convert<CustomSelection>();
                if (specMeta != null && specMeta.ContainsKey(SpecMeta.Source.ToString())) {
                    selection.sourceMap = MapFormat.filterMap(specMeta[SpecMeta.Source.ToString()], metaDictionary[SubjectMetaType.Source].metadata);
                    selection.yearMap = ExamPapeRutils.MapFormat.removeYearmap(selection.yearMap, selection.textbookMap);
                }
            }
            return selection != null;
        }
        public object getSelection() {
            return selection;
        }
        #endregion

        #region -QuestionInfos-
        public bool queryQuestions(string usetype, QuestionPayload request, IMongoQuestion questionDB, bool hasQueryAttribute = true) {
            questionDB.getQuestionDB(request.education);
            // get views
            List<QuestionView> views = questionDB.getQuestionView(request.EduSubject(), request.keys);
            if (!filterUseableTypes(views, usetype)) {
                return false;
            }
            // get questionInfos
            List<string> questionIDs = views.Select(o => o.UID).ToList();
            List<QuestionInfo> questionInfos = questionDB.getQuestion(request.EduSubject(), questionIDs);
            filterSource(request.source, ref questionInfos); // filter by source
            // get question infos
            if (hasQueryAttribute) {
                setChapter(ref questionInfos, request);
            }
            // set cache
            searchKey = insertQueryCache(views, questionInfos, request);
            // set response
            setCustomPatternQuestion(questionInfos);
            return true;
        }

        private string insertQueryCache(List<QuestionView> views, List<QuestionInfo> questionInfos, QuestionPayload request) {
            ExamPaperAttribute examAttribute = new ExamPaperAttribute() {
                year = request.year,
                education = request.education,
                subject = request.subject,
                version = request.getVersionCode(),
                bookIDs = request.bookIDs
            };
            CacheQuestion cache = new CacheQuestion() {
                views = views,
                question = questionInfos,
                examAttribute = examAttribute
            };
            return insertCache(cache);
        }
        #endregion

        #region -Pattern Formatter-
        private void setCustomPatternQuestion(List<QuestionInfo> infos) {
            IEnumerable<IGrouping<string, QuestionInfo>> group = infos.GroupBy(o => o.metaContent(QuesMeta.Type).name);
            questionGroup = new Dictionary<string, CustomQuestionGroup>();
            foreach (IGrouping<string, QuestionInfo> item in group) {
                questionGroup.Add(item.Key, setQuestionGroup(item.ToList()));
            }
        }

        private CustomQuestionGroup setQuestionGroup(List<QuestionInfo> infos) {
            CodeMap questionType = infos.First().metaContent(QuesMeta.Type);
            CustomQuestionGroup result = new CustomQuestionGroup(questionType.code, questionType.name);
            infos.ForEach(q => {
                CustomQuestion question = new CustomQuestion(q, questionType);
                result.question.Add(question);
                result.setCount(question);
            });
            return result;
        }

        private void setCustomQuestionInfo(string searchKey, List<QuestionView> views, List<QuestionInfo> questionInfos, List<BookChapter> chapterMap = null) {
            // get filter map
            List<object> filter = getFilter(questionInfos);
            List<GeneralQuestionInfo> questions = setGeneralQuestionInfo(views, questionInfos, ref chapterMap);
            questionInfo = new CustomQuestionInfo() {
                searchKey = searchKey,
                filter = filter,
                questions = questions,
                bookChapters = chapterMap
            };
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
            setCustomPatternQuestion(cache.question);
            return getQuestions();
        }

        public bool queryQuestionInfo(string searchKey) {
            // get cache
            CacheQuestion cache = getCache(searchKey);
            if (cache == null || Compare.EmptyCollection(cache.question)) {
                message.addMessage($"cache null:{searchKey}");
                return false;
            }
            setCustomQuestionInfo(searchKey, cache.views, cache.question, getChapterMap(cache.examAttribute));
            return true;
        }

        public object getQuestionInfo() {
            return questionInfo;
        }
    }
}