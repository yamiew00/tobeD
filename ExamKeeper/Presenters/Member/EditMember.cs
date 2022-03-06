using ExamKeeper.Models;
using ExamKeeper.Resources;
using ExamKeeper.Views;
using Utils;
using Utils.Setting;

namespace ExamKeeper.Presenters
{
    public class EditMember : MemberAuth
    {
        private MemberModel model { get; set; }

        #region -MongoDB-
        private IMongoLogger loggerDB { get; set; }
        #endregion

        public EditMember()
        {
            loggerDB = new InitMongoLog("Member");
            model = new MemberModel(loggerDB);
            message.setCode(SystemStatus.Start);
        }

        public void savePreference(UserProfile user, Preference preference)
        {
            // get 
            UserPreference userPreference = model.getPreference(user.UID);
            if (userPreference == null)
            {
                userPreference = new UserPreference()
                {
                    UID = user.UID
                };
            }
            userPreference.education = preference.education;
            userPreference.subject = preference.subject;
            save(userPreference);
        }

        public void saveTypesetting(UserProfile user, Typesetting setting)
        {
            string errorMessage = string.Empty;
            if (!PaperChecker.checkTypesetting(setting, ref errorMessage))
            {
                message.setCode(SystemStatus.BadRequest, errorMessage);
                return;
            }
            UserPreference userPreference = model.getPreference(user.UID);
            if (userPreference == null)
            {
                userPreference = new UserPreference()
                {
                    UID = user.UID
                };
            }
            userPreference.typesetting = setting;
            save(userPreference);
        }

        private void save(UserPreference userPreference)
        {
            if (model.updatePreference(userPreference))
            {
                message.setCode(SystemStatus.Succeed);
            }
            else
            {
                message.setCode(SystemStatus.UpdateFailed);
            }
        }
    }
}