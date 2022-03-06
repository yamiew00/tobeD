using System;
using System.Collections.Generic;
using System.Linq;
using ExamKeeper.Models;
using ExamKeeper.Resources;
using ExamKeeper.Views;
using Utils;
using Utils.Setting;

namespace ExamKeeper.Presenters {
    public class EditExamPaper : ExamBase {
        private IMongoSetting settingDB { get; set; }
        private QuestionAttributes questionAttr { get; set; }

        public EditExamPaper(UserProfile user) : base(new InitMongoCache(MongoCollection.QueryQuestion)) {
            IMongoLogger logger = new InitMongoLog("EditExamPaper");
            examModel = new ExamPaperModel(new MongoExam(), ResourceLibrary.Instance(logger));
            questionAttr = new QuestionAttributes(new MongoQuestion());
            settingDB = new MongoSetting();
            setUser(user);
        }
        public EditExamPaper(UserProfile user, IMongoCache cache, ExamPaperModel model) : base(cache) {
            this.examModel = model;
        }

        ///<summary>新增試卷，並回傳設定屬性</summary>
        public void create(CreateExam request) {
            CacheQuestion cache = null;
            List<QuestionTypeGroup<string>> checkedGroup = request.questionGroup;
            if (!checkPayload(request) || !checkPayload(request.searchKey, ref checkedGroup, ref cache)) {
                return;
            }
            //set exam-paper
            ExamPaper examPaper = new ExamPaper() {
                UID = $"{cache.examAttribute.year}{Constants.Dash}{Format.newGuid()}",
                isPublic = false,
                usetype = user.usetype,
                attribute = examModel.setAttributeName(cache.examAttribute),
                questionGroup = examModel.setQuestionScore(checkedGroup, cache),
                maintainer = user.getMaintainer(),
                maintainerUID = user.UID.ToString(),
                createTime = DateTime.Now
            };
            examPaper.attribute.drawUp = request.drawUp;
            examPaper.tags = getTagCodes(user.UID.ToString());
            examModel.insert(examPaper);
            // Question Usage Count
            setUsage(cache.examAttribute.education, cache.examAttribute.subject, checkedGroup);
            // set Response
            res = examModel.getRelated(examPaper.UID, examPaper.attribute);
            message.setCode(SystemStatus.Succeed);
        }

        ///<summary>修改試卷</summary>
        public void edit(string examUID, CreateExam request) {
            ExamPaper exam = null;
            if (!checkExamExist(examUID, ref exam, PaperAction.Edit)) {
                return;
            }
            CacheQuestion cache = null;
            List<QuestionTypeGroup<string>> checkedGroup = request.questionGroup;
            if (!checkPayload(request) || !checkPayload(request.searchKey, ref checkedGroup, ref cache)) {
                return;
            }
            //set exam-paper
            exam.usetype = user.usetype;
            exam.questionGroup = examModel.setQuestionScore(checkedGroup, cache);
            exam.tags = getTagCodes(user.UID.ToString());
            exam.maintainer = user.getMaintainer();
            exam.maintainerUID = user.UID.ToString();
            exam.updateTime = DateTime.Now;
            examModel.update(exam);
            // Question Usage Count
            setUsage(cache.examAttribute.education, cache.examAttribute.subject, checkedGroup);
            // set Response
            res = examModel.getRelated(exam.UID, exam.attribute);
            message.setCode(SystemStatus.Succeed);
        }

        private void setUsage(string education, string subject, List<QuestionTypeGroup<string>> checkedGroup) {
            List<string> allQuestions = checkedGroup.SelectMany(o => o.questionList).ToList();
            questionAttr.setUsageCount(education, subject, allQuestions);
        }

        private bool checkPayload(CreateExam request) {
            if (!ExtensionHelper.checkName<DrawUpPattern>(request.drawUp)) {
                message.setCode(SystemStatus.BadRequest, CustomString.TypeError(@"出卷方式"));
                return false;
            }
            return true;
        }

        private List<string> getTagCodes(string binding) {
            List<TagBinding> tags = settingDB.getTags(binding);
            if (Compare.EmptyCollection(tags)) {
                return null;
            }
            return tags.Select(o => o.code).ToList();
        }

        /// <summary> 修改試卷可異動屬性 </summary>
        public void update(EditExam request) {
            ExamPaper exam = null;
            if (!checkPayload(request, ref exam)) {
                return;
            }
            exam.attribute.name = request.name;
            exam.attribute.examType = request.examType;
            exam.maintainer = user.getMaintainer();
            exam.updateTime = DateTime.Now;
            updateScore(request.questionGroup, ref exam);
            examModel.update(exam);
            message.setCode(SystemStatus.Succeed);
        }

        private void updateScore(List<QuestionTypeGroup<string>> questions, ref ExamPaper exam) {
            if (questions != null) {
                exam.questionGroup = examModel.setQuestionScore(questions, exam.questionGroup);
            }
        }

        /// <summary> 試卷公開設定 </summary>
        public void setPublic(PublicExam request) {
            ExamPaper exam = null;
            if (!checkExamExist(request.examUID, ref exam)) {
                return;
            }
            exam.isPublic = request.isPublic;
            exam.maintainer = user.getMaintainer();
            exam.updateTime = DateTime.Now;
            examModel.update(exam);
            message.setCode(SystemStatus.Succeed);
        }

        /// <summary> 試卷收藏設定 </summary>
        public void setFavorite(FavoriteExam request) {
            ExamPaper exam = null;
            if (!checkExamExist(request.examUID, ref exam)) {
                return;
            }
            //get UserFavorites
            List<UserFavorites> favorites = settingDB.getUserFavorites(user.UID);
            UserFavorites favoriteExam = null;
            if (favorites != null) {
                favoriteExam = favorites.Find(f => f.itemType.Equals(SystemItemType.ExamPaper.ToString()));
            }
            if (favoriteExam == null) {
                favoriteExam = new UserFavorites() {
                userUID = user.UID,
                itemType = SystemItemType.ExamPaper.ToString(),
                items = new List<FavoriteIem>()
                };
            }
            if (request.isAdd) {
                favoriteExam.items.Add(new FavoriteIem() {
                    year = exam.attribute.year,
                        UID = exam.UID,
                        createTime = DateTime.Now
                });
                exam.favorites++;
            } else {
                exam.favorites--;
                favoriteExam.items.RemoveAll(o => exam.UID.Equals(o.UID));
            }
            examModel.update(exam);
            settingDB.upsert(favoriteExam);
            message.setCode(SystemStatus.Succeed);
        }
        private bool checkPayload(EditExam request, ref ExamPaper exam) {
            bool result = true;
            if (string.IsNullOrWhiteSpace(request.name)) {
                result = false;
                message.setCode(SystemStatus.BadRequest, CustomString.Required(@"試卷名稱"));
            }
            if (!ExtensionHelper.checkName<ExamType>(request.examType)) {
                result = false;
                message.setCode(SystemStatus.BadRequest, CustomString.TypeError(@"試卷考試別"));
            }
            if (!ExtensionHelper.checkName<OutputType>(request.outputType)) {
                result = false;
                message.setCode(SystemStatus.BadRequest, CustomString.TypeError(@"試卷輸出方式"));
            }
            if (result) {
                return checkExamExist(request.examUID, ref exam);
            }
            return result;
        }
    }
}