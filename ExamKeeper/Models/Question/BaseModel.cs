using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ExamKeeper.Resources;
using ExamKeeper.Views;
using Utils;

namespace ExamKeeper.Models {

    public interface IDrawUpModel {
        void setPara(PatternPayload para);
        bool setSelection();
        string getErrorMessage();
        object getSelection();
        bool queryQuestions(string usetype, QuestionPayload request, IMongoQuestion questionDB, bool hasQueryAttribute = true);
        object getQuestions();
        object getQuestionsCache(string searchKey);
        bool queryQuestionInfo(string searchKey);
        object getQuestionInfo();
    }

    public class QuestionBase {
        private BookSelectionModel bookModel { get; set; }
        protected MessageModel message = new MessageModel();
        public ResourceLibrary resourceAPI { get; private set; }
        private IMongoCache cache { get; set; }
        public string ErrorMessage { get { return message.TakeMessage(); } }

        #region -parameters-
        public Dictionary<string, SubjectMeta> metaDictionary { get { return bookModel.metaDictionary; } }
        public Dictionary<string, List<string>> specMeta {
            get { return bookModel.specMeta; }
        }
        #endregion

        #region -Settings-
        public static readonly List<string> QuestionFilterMeta = new List<string>() {
            QuesMeta.Type, QuesMeta.Source, QuesMeta.SubSource, QuesMeta.Knowledge, QuesMeta.Learn, QuesMeta.Difficulty
        };
        #endregion

        public QuestionBase(IMongoSetting settings, IMongoLogger logger, IMongoCache cache) {
            bookModel = new BookSelectionModel(ResourceLibrary.Instance(logger), settings, cache);
            this.resourceAPI = ResourceLibrary.Instance(logger);
            this.cache = cache;
        }

        #region -GeneralSelection-
        public GeneralBookSelection getGeneralSelection(string edu, string subject, string year = "") {
            if (string.IsNullOrWhiteSpace(year)) {
                year = DateTimes.currentSchoolYear(); //default
            }
            GeneralBookSelection result = bookModel.getGeneralSelection(edu, subject, year);
            if (result == null) {
                message.addMessage(bookModel.ErrorMessage);
            }
            return result;
        }
        #endregion

        #region -QuestionInfos-
        protected void filterSource(List<string> source, ref List<QuestionInfo> infos) {
            // filter by source
            if (!Compare.EmptyCollection(source)) {
                ConcurrentBag<QuestionInfo> filtered = new ConcurrentBag<QuestionInfo>();
                Parallel.ForEach(infos, info => {
                    if (source.Contains(info.metaContentCode(QuesMeta.Source))) {
                        filtered.Add(info);
                    }
                });
                infos = filtered.ToList();
            }
        }
        protected List<GeneralQuestionInfo> setGeneralQuestionInfo(List<QuestionView> views, List<QuestionInfo> questions, ref List<BookChapter> chapterList) {
            List<GeneralQuestionInfo> questionInfos = new List<GeneralQuestionInfo>();
            List<string> knowledgeCodes = new List<string>();
            // set questions
            foreach (QuestionInfo question in questions) {
                QuestionView view = views.Find(o => o.UID.Equals(question.UID));
                questionInfos.Add(new GeneralQuestionInfo() {
                    UID = question.UID,
                        image = view.image,
                        content = view.content,
                        answer = Format.toString(question.getAnswerText()),
                        metadata = question.metadata
                });
                List<CodeMap> knowledges = question.metaContents(QuesMeta.Knowledge);
                knowledgeCodes.AddRange(knowledges.Select(o => o.code).ToList().Distinct());
            }
            if (!Compare.EmptyCollection(chapterList)) {
                // count chapter questions
                foreach (BookChapter book in chapterList) {
                    // 節
                    Parallel.ForEach(book.chapters, chapter => {
                        if (chapter.hasKnowledge) {
                            List<string> chapterKnowledge = chapter.knowledgeList.Select(o => o.itemCode).ToList();
                            chapter.amount = knowledgeCodes.FindAll(o => chapterKnowledge.Contains(o)).Count();
                        }
                    });
                    // 章
                    book.chapters.FindAll(o => o.isParent()).ForEach(parent => {
                        if (parent.amount <= 0) {
                            parent.amount = book.chapters.FindAll(o => parent.code.Equals(o.parentCode)).Sum(child => child.amount);
                        }
                    });
                }
                // 冊
                chapterList.ForEach(book => {
                    book.amount = book.chapters.FindAll(o => o.isParent()).Sum(c => c.amount);
                });
            }
            return questionInfos;
        }
        public void setChapter(ref List<QuestionInfo> questions, QuestionPayload request) {
            // get chapter map
            Dictionary<string, BookChapter> chapters = getChapterMap(request);
            if (chapters.Count == 0) { return; }
            // format <knowledge, chapterCode> Dictionary
            Dictionary<string, KnowledgeBook> knowledgeMap = getKnowledgeMap(chapters);
            if (knowledgeMap == null) { return; }
            // set question metadata (add chapter)
            Parallel.ForEach(questions, question => {
                List<CodeMap> knowledges = question.metaContents(QuesMeta.Knowledge);
                if (!Compare.EmptyCollection(knowledges)) {
                    QuestionMeta chapter = new QuestionMeta() {
                        code = QuesMeta.Chapter,
                        name = CustomString.Meta.CHAPTER,
                        content = new List<CodeMap>()
                    };
                    QuestionMeta lesson = new QuestionMeta() {
                        code = QuesMeta.Lesson,
                        name = CustomString.Meta.LESSON,
                        content = new List<CodeMap>()
                    };
                    QuestionMeta book = new QuestionMeta() {
                        code = QuesMeta.Book,
                        name = CustomString.Meta.BOOK,
                        content = new List<CodeMap>()
                    };
                    knowledges.ForEach(k => {
                        if (knowledgeMap.ContainsKey(k.code)) {
                            book.content.Add(knowledgeMap[k.code].book);
                            lesson.content.Add(knowledgeMap[k.code].lesson);
                            if (knowledgeMap[k.code].chapter != null) {
                                chapter.content.Add(knowledgeMap[k.code].chapter);
                            }
                        }
                    });
                    question.metadata.Add(book);
                    question.metadata.Add(lesson);
                    if (!Compare.EmptyCollection(chapter.content)) {
                        question.metadata.RemoveAll(o => o.code.Equals(QuesMeta.Chapter));
                        question.metadata.Add(chapter);
                    }
                }
            });
        }

        protected List<BookChapter> getChapterMap(ExamPaperAttribute attributes) {
            List<BookChapter> result = new List<BookChapter>();
            QuestionPayload payload = new QuestionPayload() {
                year = attributes.year,
                education = attributes.education,
                subject = attributes.subject,
                bookIDs = attributes.bookIDs
            };
            Dictionary<string, BookChapter> chapterMap = getChapterMap(payload);
            return chapterMap.Values.OrderBy(o => o.bookID).ToList();
        }

        /// <summary> get chapter map from selection cache </summary>
        public Dictionary<string, BookChapter> getChapterMap(QuestionPayload request) {
            // get chapter map
            string key = $"GeneralSelection_{request.year}{request.education}{request.subject}";
            GeneralBookSelection selection = bookModel.getSelectionCache(key);
            if (selection == null) {
                // cache data expired
                selection = getGeneralSelection(request.education, request.subject, request.year);
            }
            Dictionary<string, BookChapter> chapters = new Dictionary<string, BookChapter>();
            foreach (KeyValuePair<string, TextBookMap> bookMap in selection.textbookMap) {
                Dictionary<string, string> bookDescDic = bookMap.Value.getBookDescDic();
                foreach (KeyValuePair<string, BookChapter> chapterMap in bookMap.Value.chapterMap) {
                    if (request.bookIDs.Contains(chapterMap.Value.bookID)) {
                        chapterMap.Value.bookDesc = bookDescDic[chapterMap.Key];
                        chapters.Add(chapterMap.Key, chapterMap.Value);
                    }
                }
            }
            return chapters;
        }

        /// <summary> get chapter map from selection cache </summary>
        private Dictionary<string, KnowledgeBook> getKnowledgeMap(Dictionary<string, BookChapter> chapters) {
            try {
                Dictionary<string, KnowledgeBook> knowledgesDic = new Dictionary<string, KnowledgeBook>();
                foreach (KeyValuePair<string, BookChapter> chapterMap in chapters) {
                    // ==================== by books ====================
                    Dictionary<string, CodeMap> lessonMap = new Dictionary<string, CodeMap>();
                    Dictionary<string, string> chapterLessonMap = new Dictionary<string, string>();
                    List<string> items = new List<string>();
                    // ==================================================
                    chapterMap.Value.chapters.ForEach(o => {
                        if (o.hasKnowledge) {
                            foreach (ItemMap item in o.knowledgeList) {
                                knowledgesDic.Add(item.itemCode, new KnowledgeBook() {
                                    book = new CodeMap(chapterMap.Key, chapterMap.Value.bookDesc),
                                        lesson = o.isParent() ? new CodeMap(o.code, o.name) : null,
                                        chapter = o.isParent() ? null : new CodeMap(o.code, o.name)
                                });
                                items.Add(item.itemCode);
                            }
                        }
                        if (!o.isParent()) {
                            chapterLessonMap.Add(o.code, o.parentCode);
                        } else {
                            lessonMap.Add(o.code, new CodeMap(o.code, o.name));
                        }
                    });
                    // set lesson
                    foreach (string item in items) {
                        if (knowledgesDic[item].chapter != null && chapterLessonMap.ContainsKey(knowledgesDic[item].chapter.code)) {
                            knowledgesDic[item].lesson = lessonMap[chapterLessonMap[knowledgesDic[item].chapter.code]];
                        }
                    }
                }
                return knowledgesDic;
            } catch (Exception ex) {
                message.addLine(CustomString.ReadFailed(@"課本章節資料", ex.Message));
                return null;
            }
        }

        class KnowledgeBook {
            public CodeMap book { get; set; }
            public CodeMap lesson { get; set; }
            public CodeMap chapter { get; set; }
        }

        #endregion

        #region -Cache-
        public string insertCache(CacheQuestion para) {
            string cacheKey = Format.newGuid();
            cache.insertCaches(cacheKey, para, 120, para.getChunkSize()); // 資料量太大，拆分存
            return cacheKey;
        }
        public CacheQuestion getCache(string key) {
            return cache.getCaches<CacheQuestion>(key);
        }
        #endregion

        #region -QuestionFilter-
        protected List<object> getFilter(List<QuestionInfo> questions) {
            try {
                Dictionary<string, CodeMap> groupDic = getQuestionGroup();
                if (groupDic == null) {
                    message.addMessage("Get QuestionGroup Info Failed.");
                    return null;
                }

                #region -Filter框架-
                Dictionary<string, List<CodeMap>> meta = new Dictionary<string, List<CodeMap>>();
                Dictionary<string, string> metaName = new Dictionary<string, string>();
                QuestionFilterMeta.ForEach(item => {
                    meta.Add(item, new List<CodeMap>());
                });
                Dictionary<string, CodeMap> subsourceDic = new Dictionary<string, CodeMap>();
                questions.ForEach(o => {
                    // set source dictionary
                    CodeMap source = o.metaContent(QuesMeta.Source);
                    string subsource = o.metaContentCode(QuesMeta.SubSource);
                    if (source != null && !string.IsNullOrWhiteSpace(subsource) && !subsourceDic.ContainsKey(subsource)) {
                        subsourceDic.Add(subsource, source);
                    }
                    // set metaMap
                    QuestionMeta answerType = new QuestionMeta() {
                        code = QuesMeta.AnswerType,
                        name = CustomString.Meta.ANSWER_TYPE
                    };
                    o.metadata.ForEach(m => {
                        if (!metaName.ContainsKey(m.code)) {
                            metaName.Add(m.code, m.name);
                        }
                        if (meta.ContainsKey(m.code)) {
                            meta[m.code].AddRange(m.content);
                            meta[m.code] = MapFormat.doDistinct(meta[m.code]);
                        }
                        // set AnswerType
                        if (QuesMeta.Type.Equals(m.code)) {
                            string questionType = m.content.FirstOrDefault().code;
                            if (groupDic.ContainsKey(questionType)) {
                                answerType.content = new List<CodeMap>() {
                                    groupDic[questionType]
                                };
                            }
                        }
                    });
                    o.metadata.Insert(0, answerType);
                });
                List<CodeMap> questionTypeMap = new List<CodeMap>();
                QuestionMainFilter questionFilter = null;
                List<CodeMap> sourceMap = new List<CodeMap>();
                QuestionMainFilter sourceFilter = null;
                List<object> filter = new List<object>();
                foreach (KeyValuePair<string, List<CodeMap>> item in meta) {
                    if (Compare.EmptyCollection(item.Value)) {
                        continue;
                    }
                    switch (item.Key) {
                        case QuesMeta.Type:
                            questionTypeMap.AddRange(item.Value);
                            questionFilter = new QuestionMainFilter() {
                                code = item.Key,
                                name = metaName[item.Key]
                            };
                            break;
                        case QuesMeta.Source:
                            sourceFilter = new QuestionMainFilter() {
                                code = item.Key,
                                name = metaName[item.Key]
                            };
                            break;
                        case QuesMeta.SubSource:
                            sourceMap.AddRange(item.Value);
                            break;
                        case QuesMeta.Difficulty:
                            filter.Add(new QuestionFilter() {
                                code = item.Key,
                                    name = metaName[item.Key],
                                    map = sortDifficulty(item.Value)
                            });
                            break;
                        default:
                            filter.Add(new QuestionFilter() {
                                code = item.Key,
                                    name = metaName[item.Key],
                                    map = item.Value
                            });
                            break;
                    }
                }
                #endregion

                #region -階層處理-
                questionFilter.map = setHierarchyFilters(MapFormat.doDistinct(questionTypeMap), groupDic);
                filter.Insert(0, questionFilter);
                sourceFilter.map = setHierarchyFilters(MapFormat.doDistinct(sourceMap), subsourceDic);
                filter.Insert(1, sourceFilter);
                #endregion
                return filter;
            } catch {
                return null;
            }
        }

        private List<QuestionSubFilter> setHierarchyFilters(List<CodeMap> subMap, Dictionary<string, CodeMap> groupDic) {
            List<QuestionSubFilter> sub = new List<QuestionSubFilter>();
            subMap.ForEach(o => {
                string groupCode = groupDic[o.code].code;
                int index = sub.FindIndex(t => t.code.Equals(groupCode));
                if (index > -1) {
                    sub[index].subMap.Add(o);
                } else {
                    sub.Add(new QuestionSubFilter() {
                        code = groupDic[o.code].code,
                            name = groupDic[o.code].name,
                            subMap = new List<CodeMap>() { o }
                    });
                }
            });
            return sub;
        }
        #endregion

        #region -Rules-
        public Dictionary<string, CodeMap> getQuestionGroup() {
            List<QuestionType> questionTypes = resourceAPI.getQuestionTypes();
            if (Compare.EmptyCollection(questionTypes)) {
                return null;
            }
            return questionTypes.ToDictionary(x => x.code, x => new CodeMap(x.groupCode, x.groupDesc));
        }

        public bool filterUseableTypes(List<QuestionView> views, string usetype) {
            if (Compare.EmptyCollection(views)) {
                message.addMessage("No Questions.");
                return false;
            }
            QuestionUseType type = ExtensionHelper.GetFromName<QuestionUseType>(usetype);
            List<string> useableTypes = new List<string>();
            switch (type) {
                case QuestionUseType.General:
                    useableTypes.Add(QuestionUseType.General.ToString());
                    break;
                case QuestionUseType.Premium:
                    useableTypes.Add(QuestionUseType.General.ToString());
                    useableTypes.Add(QuestionUseType.Premium.ToString());
                    break;
                default:
                    break;
            }
            if (Compare.EmptyCollection(useableTypes)) {
                message.addMessage("No Useable Types.");
                return false;
            }
            // filter by usetype
            views = views.FindAll(o => useableTypes.Contains(o.usetype));
            if (Compare.EmptyCollection(views)) {
                message.addMessage("No Useable Questions.");
                return false;
            }
            return true;
        }

        protected List<CodeMap> sortDifficulty(List<CodeMap> map) {
            List<string> sort = MapFormat.toCodeMap<Difficulty>().Select(o => o.code).ToList();
            map = map.OrderBy(item => sort.IndexOf(item.code)).ToList();
            return map;
        }
        #endregion

    }

    /// <summary> 單年度課本範圍篩選 </summary>
    public class GeneralBookSelection {
        public List<CodeMap> curriculumMap { get; set; }
        public List<CodeMap> yearMap { get; set; }
        public List<CodeMap> versionMap { get; set; }
        /// <summary> <版本, 課本選單> </summary>
        public Dictionary<string, TextBookMap> textbookMap { get; set; }
        public T convert<T>() {
            string data = JsonSerializer.Serialize(this);
            return JsonSerializer.Deserialize<T>(data);
        }
    }

    public class CountPara {
        public int question { get; set; }
        public int answer { get; set; }
        public void add(int answerAmount) {
            question++;
            answer += answerAmount;
        }
    }

    public class CacheQuestion {
        public ExamPaperAttribute examAttribute { get; set; }
        public List<QuestionView> views { get; set; }
        public List<QuestionInfo> question { get; set; }
        public int getChunkSize() {
            if (question.Count < 800) {
                return 1;
            }
            return question.Count / 800;
        }
    }

    /// <summary> 試題明細 </summary>
    public class GeneralQuestionInfo {
        public string UID { get; set; }
        public string image { get; set; }
        public string content { get; set; }
        public string answer { get; set; }
        public List<QuestionMeta> metadata { get; set; }
    }

    #region -GeneralQuestionFilter-
    // 一階篩選
    public class QuestionFilter : CodeMap {
        public List<CodeMap> map { get; set; }
    }

    // 二階篩選
    public class QuestionMainFilter : CodeMap {
        public List<QuestionSubFilter> map { get; set; }
    }
    public class QuestionSubFilter : CodeMap {
        public List<CodeMap> subMap { get; set; }
    }
    #endregion
}