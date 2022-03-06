using System.Text.Json;
using ExamKeeper.Resources;
using Utils;
using Utils.Setting;

namespace ExamKeeper.Models
{
    public class LoginModel
    {
        private CMSSingleton CMS { get; set; }
        public UserProfile userProfile { get; private set; }
        public LoginUser loginUser { get; private set; }
        private MessageModel message { get; set; }
        public string getMessage { get { return message.TakeMessage(); } }
        public string ProfileString { get { return JsonSerializer.Serialize(userProfile); } }

        public LoginModel(IMongoLogger logger)
        {
            CMS = CMSSingleton.Instance(logger);
            message = new MessageModel();
        }
        public bool isLogin(string token)
        {
            userProfile = CMS.getUserProfile(token);
            return userProfile != null;
        }
        public bool DoLogin(string token)
        {
            //走CMS的Client/login嘗試登入
            Response<LoginUser> response = CMS.userLogin(token);
            if (response == null)
            {
                message.addMessage(CustomString.Connection("CMS Login"));
                return false;
            }
            if (!response.isSuccess)
            {
                message.addMessage(response.message);
                return false;
            }
            loginUser = response.data;
            return true;
        }
    }
}