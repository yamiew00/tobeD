using System;
using System.Collections.Generic;
using System.Linq;
using ExamKeeper.Models;
using ExamKeeper.Resources;
using ExamKeeper.Views;
using Utils;
using Utils.Setting;
using static ExamKeeper.Utils.APIs;

namespace ExamKeeper.Presenters {
    public class PracticeExam : ExamBase {
        private string UID { get; set; }
        private OneExamModel oneexamModel { get; set; }
        private QuestionModel questionModel { get; set; }
        private FixRecordModel recordModel { get; set; }
        private WebhookModel webhook { get; set; }

        public PracticeExam(UserProfile user, string practiceUID) : base(new InitMongoCache(MongoCollection.QueryQuestion)) {
            this.UID = practiceUID;
            IMongoLogger logger = new InitMongoLog("PracticeExam");
            MongoExam examDB = new MongoExam();
            practiceModel = new PracticeModel(examDB, ResourceLibrary.Instance(logger));
            oneexamModel = new OneExamModel(logger);
            recordModel = new FixRecordModel(examDB);
            webhook = new WebhookModel(new MongoSetting());
            questionModel = new QuestionModel(new MongoQuestion());
            setUser(user);
        }

        /// <summary> 建立測驗 </summary>
        public void createExam(string oneclubToken) {
            // check Payload
            Practice practice = practiceModel.getPractice(ExamChecker.getYear(UID), UID);
            if (practice == null) {
                message.setCode(SystemStatus.BadRequest, CustomString.NotFound(@"自主練習", UID));
                return;
            }
            if (!user.UID.ToString().Equals(practice.maintainerUID)) {
                message.setCode(SystemStatus.BadRequest, CustomString.NotMatch(@"使用者"));
                return;
            }
            if (create(oneclubToken, practice)) {
                message.setCode(SystemStatus.Succeed);
            }
        }

        /// <summary> 測驗紀錄 </summary>
        public OneExamResponse<string> setExamNotice(string otp, ExamNotice notice) {
            PracticeRecord record = null;
            Webhook webhookLog = null;
            Practice practice = null;
            if (!checkPayload(otp, notice, ref record, ref webhookLog, ref practice)) {
                return new OneExamResponse<string>() {
                    status = "error",
                        content = message.TakeMessage()
                };
            }
            // update record
            int idx = record.records.FindIndex(rec => rec.examID.Equals(notice.examId));
            if (idx > -1) {
                record.records[idx].deliverTime = DateTime.Now;
                record.records[idx].resultUrl = OneExam.report(notice.examId, notice.userId);
            }
            practiceModel.upsertPracticeRecord(record);
            // error answers
            if (!Compare.EmptyCollection(notice.wrongRecord)) {
                wrongRecord(notice.examId, practice, notice.wrongRecord);
            }
            return new OneExamResponse<string>() {
                status = "success"
            };
        }

        private bool checkPayload(string otp, ExamNotice notice, ref PracticeRecord record, ref Webhook webhookLog, ref Practice practice) {
            if (!webhook.checkOTP(otp, notice.examId, ref webhookLog)) {
                message.addMessage(CustomString.NotFound("OTP"));
                return false;
            }
            this.UID = webhookLog.searchKey; // practice UID
            practice = practiceModel.getPractice(ExamChecker.getYear(UID), UID);
            if (practice == null) {
                message.addMessage(CustomString.NotFound(@"自主練習", UID));
                return false;
            }
            Guid userUID = new Guid(practice.maintainerUID);
            record = practiceModel.getPracticeRecord(ExamChecker.getYear(UID), practice.UID, userUID);
            if (record == null) {
                message.addMessage(CustomString.NotFound(@"練習紀錄", UID));
                return false;
            }
            if (Compare.EmptyCollection(record.records) || !record.records.Any(o => o.examID.Equals(notice.examId))) {
                message.addMessage(CustomString.NotFound(@"測驗UID", notice.examId));
                return false;
            }
            return true;
        }

        /// <summary>自主練習錯題回報紀錄</summary>
        private void wrongRecord(string examUID, Practice practice, List<WrongAnswer> wrongRecord) {
            Guid examUser = new Guid(practice.maintainerUID); // 自主練習只有建立者可以作測驗
            List<WrongRecord> wrongRecords = recordModel.userWrongRecord(examUser);
            WrongRecord record = wrongRecords.Find(o => o.education.Equals(practice.attribute.education) && o.subject.Equals(practice.attribute.subject));
            if (record == null) {
                record = new WrongRecord() {
                user = examUser,
                education = practice.attribute.education,
                subject = practice.attribute.subject,
                questions = new Dictionary<string, WrongQuestion>()
                };
            }

            // get QuestionViews
            questionModel.setEduSubject(practice.attribute.education, practice.attribute.subject);
            List<QuestionView> views = questionModel.getView(wrongRecord.Select(o => o.questionID).ToList());
            List<QuestionInfo> infos = questionModel.get(wrongRecord.Select(o => o.questionID).ToList());

            // set records
            foreach (WrongAnswer item in wrongRecord) {
                WrongAnswers wrongAnswers = new WrongAnswers() {
                    examUID = examUID,
                    examType = SystemItemType.Practice.ToString(),
                    UID = practice.UID,
                    answer = item.answer,
                    userAnswer = item.userAnswer,
                    systemTime = DateTime.Now
                };
                if (record.questions.ContainsKey(item.questionID)) {
                    record.questions[item.questionID].disabled = false;
                    record.questions[item.questionID].records.Add(wrongAnswers);
                } else {
                    List<string> keys = views.Find(o => o.UID.Equals(item.questionID))?.key ?? null;
                    CodeMap questionType = infos.Find(o => o.UID.Equals(item.questionID)).metaContent(QuesMeta.Type);
                    record.questions.Add(item.questionID, new WrongQuestion() {
                        ID = item.questionID,
                            disabled = false,
                            type = questionType.code,
                            typeName = questionType.name,
                            keys = keys,
                            records = new List<WrongAnswers>() { wrongAnswers }
                    });
                }
            }

            // update data
            recordModel.updateRecord(record);
        }

        /// <summary>建立測驗並紀錄測驗ID(PracticeRecord)</summary>
        /// <param name="oneclubToken"></param>
        /// <param name="practice"></param>
        private bool create(string oneclubToken, Practice practice) {
            string otp = Format.newGuid();
            CreateExamInfos infos = oneexamModel.createExam(oneclubToken, practice, otp);
            if (infos == null) {
                message.setCode(SystemStatus.Connection, oneexamModel.ErrorMessage);
                return false;
            }
            // set examID record
            PracticeRecord records = practiceModel.getPracticeRecord(ExamChecker.getYear(UID), practice.UID, user.UID);
            if (records == null) {
                records = new PracticeRecord() {
                UID = practice.UID,
                userUID = user.UID,
                year = ExamChecker.getYear(UID),
                records = new List<ExamRecord>()
                };
            }
            records.records.Add(new ExamRecord() {
                examID = infos.examId,
                    otp = webhook.setOTP(setWebhook(otp, infos.examId, practice)),
                    startAt = infos.startAt,
                    endAt = infos.endAt,
                    createTime = infos.createDate
            });
            // add Attendees
            if (!oneexamModel.addAttendees(oneclubToken, infos.examId, user)) {
                message.setCode(SystemStatus.Connection, @"加入受測名單失敗");
                return false;
            }
            if (!practiceModel.upsertPracticeRecord(records)) {
                message.setCode(SystemStatus.Failed, @"建立測驗紀錄失敗");
                return false;
            }
            res = OneExam.start(infos.examId, user.account);
            return true;
        }

        private Webhook setWebhook(string otp, string examID, Practice practice) {
            return new Webhook() {
                type = "ExamResult",
                    otp = otp,
                    key = examID,
                    searchKey = practice.UID
            };
        }
    }
}