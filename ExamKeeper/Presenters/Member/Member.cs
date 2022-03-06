using ExamKeeper.Models;
using ExamKeeper.Views;
using ExamKeeperClassLibrary;
using Utils;
using Utils.Setting;

namespace ExamKeeper.Presenters
{
    public class Member : MemberAuth
    {
        private MemberModel model { get; set; }

        #region -MongoDB-
        private IMongoLogger loggerDB { get; set; }
        #endregion

        public Member()
        {
            loggerDB = new InitMongoLog("Member");
            model = new MemberModel(loggerDB);
            setUser(user);
        }

        public void login(string token)
        {
            LoginModel loginModel = new LoginModel(loggerDB);
            if (!loginModel.DoLogin(token))
            {
                message.setCode(SystemStatus.TokenError, loginModel.getMessage);
                return;
            }
            LoginUser loginUser = loginModel.loginUser;
            loginUser.preference = model.getPreference(loginUser.userProfile.UID);
            loginUser.examQueryMap = MapFormat.toCodeMap<ExamField>();
            res = loginUser;
            message.setCode(SystemStatus.Succeed);
        }

        public void get(UserProfile user)
        {
            LoginUser userInfo = new LoginUser()
            {
                userProfile = user,
                preference = model.getPreference(user.UID)
            };
            res = userInfo;
            message.setCode(SystemStatus.Succeed);
        }

        public void getMainMenu(UserProfile user, DefinitionLibrary definitionLibrary)
        {
            EduSubject eduSubjectMap = model.getEduSubject(getSubjectType(user));
            res = new MainMenu()
            {
                eduMap = eduSubjectMap.eduMap,
                eduSubject = eduSubjectMap.eduSubject,
                eduGrade = eduSubjectMap.eduGrade,
                outputMap = MapFormat.toCodeMap<OutputType>(),
                patternMap = MapFormat.toCodeMap<DrawUpPattern>(),
                publisherMap = definitionLibrary.Publisher
            };
            message.setCode(SystemStatus.Succeed);
        }

        /// <summary>TODO</summary>
        /// <param name="user"></param>
        public void getTypesetting(UserProfile user)
        {
            SystemIdentity userIdentity = user.getIdentity();
            UserTypesetting result = new UserTypesetting()
            {
                UID = user.UID,
                isTeacher = SystemIdentity.Teacher.Equals(userIdentity),
                identityName = userIdentity.GetEnumDescription(),
                account = user.account,
                name = user.name,
                organizationName = user.organization.name
            };
            if (allowTypesetting(user))
            {
                // get typesetting
                UserPreference userPreference = model.getPreference(user.UID);
                result.paperSizeMap = MapFormat.toCodeMap<PaperSize>();
                result.wordSettingMap = MapFormat.toCodeMap<WordSetting>();
                result.paperContent = MapFormat.toCodeMap<PaperContent>();
                result.analyzeContent = MapFormat.toCodeMap<AnalyzeContent>();
                result.advancedSetting = MapFormat.toCodeMap<AdvancedSetting>();
                result.typesetting = (userPreference == null) ? default : userPreference.typesetting;
            }
            res = result;
            message.setCode(SystemStatus.Succeed);
        }

        private string getSubjectType(UserProfile user)
        {
            switch (user.getOrganization())
            {
                case SystemOrganization.Agency:
                case SystemOrganization.Tutoring:
                    return string.Empty;
                case SystemOrganization.None:
                case SystemOrganization.School:
                default:
                    return "N";
            }
        }
    }
}