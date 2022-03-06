using System.Collections.Generic;
using ExamKeeper.Models;
using ExamKeeper.Resources;
using ExamKeeper.Views;
using Utils;
using Utils.Setting;

namespace ExamKeeper.Presenters {
    public class AnomalyReport : PresenterBase {
        public AnomalyReport(UserProfile user) {
            setUser(user);
        }
        public void related() {
            List<CodeMap> typeMap = MapFormat.toCodeMap<AnomalyType>();
            typeMap.RemoveAll(o => o.code.Equals("Management")); // for cms
            res = typeMap;
            message.setCode(SystemStatus.Succeed);
        }

        public void report(AnomalyPayload request) {
            if (checkPayload(request)) {
                ReportAnomaly payload = Format.objectConvert<AnomalyPayload, ReportAnomaly>(request);
                payload.itemType = SystemItemType.Question.ToString(); // 目前固定只回報試題
                payload.user = user.getMaintainer();
                payload.userIdentity = user.identity;
                // send
                CMSSingleton CMS = CMSSingleton.Instance(new InitMongoLog("ReportAnomaly"));
                string errorMessage = CMS.sendAnomaly(payload);
                if (!string.IsNullOrWhiteSpace(errorMessage)) {
                    message.setCode(SystemStatus.Connection, errorMessage);
                } else {
                    message.setCode(SystemStatus.Succeed);
                }
            }
        }

        public bool checkPayload(AnomalyPayload request) {
            AnomalyType type = ExtensionHelper.GetFromName<AnomalyType>(request.anomalyType);
            if (!ExtensionHelper.checkName<AnomalyType>(request.anomalyType)) {
                message.setCode(SystemStatus.BadRequest, CustomString.TypeError("AnomalyType"));
                return false;
            }
            QuestionModel questionModel = new QuestionModel(new MongoQuestion());
            questionModel.setEduSubject(request.education, request.subject);
            if (!questionModel.exist(request.UID)) {
                message.setCode(SystemStatus.BadRequest, CustomString.NotFound("UID", request.UID));
                return false;
            }
            return true;
        }
    }
}