using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamKeeper.Resources;
using ExamKeeper.Views;
using Utils;
using Utils.Setting;

namespace ExamKeeper.Models {

    /// <summary> 處理課本章節結構 </summary>
    public class BookSelectionModel {
        public BookSelectionModel(ResourceLibrary resource, IMongoSetting settingDB, IMongoCache cache) {
            this.resource = resource;
            this.settingDB = settingDB;
            this.cache = cache;
        }
        public ResourceLibrary resource { get; private set; }
        private IMongoSetting settingDB { get; set; }
        private IMongoCache cache { get; set; }
        protected MessageModel message = new MessageModel();
        public string ErrorMessage { get { return message.TakeMessage(); } }

        #region -parameters-
        private MongoCollection controller = MongoCollection.Selections;
        private TextBook textBook { get; set; }
        public Dictionary<string, SubjectMeta> metaDictionary { get; private set; }
        public Dictionary<string, List<string>> specMeta { get; private set; }
        private string key { get; set; }
        #endregion

        #region -GeneralSelection-
        public GeneralBookSelection getGeneralSelection(string edu, string subject, string year) {
            key = $"GeneralSelection_{year}{edu}{subject}";
            GeneralBookSelection result = getSelectionCache(key);
            if (result != null) {
                return result;
            }
            return updateCache(edu, subject, year);
        }

        public GeneralBookSelection getSelectionCache(string key) {
            SelectionCache cacheData = cache.getCache<SelectionCache>(controller, key);
            if (cacheData == null) {
                return null;
            }
            textBook = cacheData.textBook;
            metaDictionary = cacheData.metaDictionary;
            specMeta = cacheData.specMeta;
            return cacheData.generalSelection;
        }

        private GeneralBookSelection updateCache(string edu, string subject, string year) {
            if (!getFromResourceLibrary(year, edu, subject)) {
                return null;
            }
            // cacheData
            SelectionCache cacheData = new SelectionCache() {
                textBook = textBook, // must before "setGeneralSelection(year, edu + subject)",
                metaDictionary = metaDictionary
            };
            GeneralBookSelection result = setGeneralSelection(year, edu + subject);
            cacheData.specMeta = specMeta;
            cacheData.generalSelection = result;
            if (result != null) {
                cache.updateCache(controller, key, cacheData, 60 * 24); // keep one day
            }
            return result;
        }

        private bool getFromResourceLibrary(string year, string edu, string subject) {
            textBook = resource.getYearTextBook(year, edu, subject);
            if (textBook == null) {
                message.addLine(CustomString.ReadFailed(@"資源平台", $"{year}年度{edu}{subject}課本資訊"));
            }
            metaDictionary = resource.getEduSubjectMeta(edu, subject);
            if (metaDictionary == null) {
                message.addLine(CustomString.ReadFailed(@"資源平台", $"{edu}{subject}屬性資料"));
            } else if (!metaDictionary.ContainsKey(SubjectMetaType.Source)) {
                message.addLine(CustomString.ReadFailed(@"資源平台", $"{edu}{subject}出處資料"));
            }
            return !message.hasValue;
        }

        #region -Format-
        private GeneralBookSelection setGeneralSelection(string year, string eduSubject) {
            specMeta = settingDB.getMetaDictionary(year, eduSubject);
            GeneralBookSelection result = new GeneralBookSelection() {
                yearMap = textBook.yearMap,
                curriculumMap = textBook.curriculumMap,
            };

            // set version
            List<CodeMap> versionMap = resetVersionName(textBook.versionMap);
            List<string> version = textBook.bookInfo.Select(o => o.version).ToList();
            result.versionMap = MapFormat.filterMap(version, versionMap);

            //set chapters
            result.textbookMap = new Dictionary<string, TextBookMap>();
            IEnumerable<IGrouping<string, TextBookInfo>> versionGroup = textBook.bookInfo.GroupBy(o => o.version).ToList();
            foreach (IGrouping<string, TextBookInfo> book in versionGroup) {
                List<TextBookInfo> versionBooks = book.ToList();
                TextBookMap textBookMap = new TextBookMap();
                //chapter
                textBookMap.chapterMap = setChapterMap(versionBooks);
                if (result == null) {
                    message.addLine(CustomString.ReadFailed(@"資源平台", @"課本章節資料"));
                    return null;
                }
                textBookMap.bookMap = new Dictionary<string, List<CodeMap>>();
                foreach (IGrouping<string, TextBookInfo> item in book.GroupBy(o => o.curriculum)) {
                    List<string> existBookCode = item.Select(o => o.bookCode).ToList();
                    textBookMap.bookMap.Add(item.Key, MapFormat.filterMap(existBookCode, textBook.bookMap));
                }
                result.textbookMap.Add(book.Key, textBookMap);
            }
            return result;
        }
        private List<CodeMap> resetVersionName(List<CodeMap> versionMap) {
            versionMap.ForEach(version => {
                if (!"N".Equals(version.code)) {
                    version.name = @"適" + version.name.Substring(0, 1);
                }
            });
            return versionMap;
        }
        private Dictionary<string, BookChapter> setChapterMap(List<TextBookInfo> books) {
            ConcurrentDictionary<string, BookChapter> chapter = new ConcurrentDictionary<string, BookChapter>();
            bool finished = true;
            Parallel.ForEach(books, (book, state) => {
                BookChapter bookChapter = resource.getBookChapter(book.code);
                if (bookChapter == null) {
                    finished = false;
                    state.Break();
                }
                chapter.TryAdd(book.bookCode, bookChapter);
            });
            return finished ? chapter.ToDictionary(o => o.Key, o => o.Value) : null;
        }

        #endregion

        #endregion

        class SelectionCache {
            public GeneralBookSelection generalSelection { get; set; }
            public TextBook textBook { get; set; }
            public Dictionary<string, SubjectMeta> metaDictionary { get; set; }
            public Dictionary<string, List<string>> specMeta { get; set; }
        }

    }

}