using System;
using ExamKeeper.Views;
using Utils;
using Utils.Setting;

namespace ExamKeeper.Models {
    public class MemberModel {
        private ResourceLibrary resourceAPI { get; set; }
        private IMongoSetting setting { get; set; }

        public MemberModel(IMongoLogger logger) {
            resourceAPI = ResourceLibrary.Instance(logger);
            setting = new MongoSetting();
        }

        public EduSubject getEduSubject(string type) {
            return resourceAPI.getEduSubject(type);
        }

        public bool updatePreference(UserPreference preference) {
            return setting.upsert(preference);
        }
        public UserPreference getPreference(Guid userUID) {
            return setting.getUserPreference(userUID);
        }
    }
}