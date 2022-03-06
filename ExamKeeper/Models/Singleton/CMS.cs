using System;
using System.Collections.Generic;
using ExamKeeper.Utils;
using ExamKeeper.Views;
using Utils;
using Utils.Setting;

namespace ExamKeeper.Models {

    public class CMSSingleton : SendRequest {
        public CMSSingleton(IMongoLogger logger) : base(logger) {
            this.logger = logger;
        }
        private static readonly object cmslock = new object();
        private static CMSSingleton _instance = null;
        public static CMSSingleton Instance(IMongoLogger logger) {
            lock(cmslock) {
                if (_instance == null) {
                    _instance = new CMSSingleton(logger);
                }
            }
            return _instance;
        }

        public UserProfile getUserProfile(string token) {
            string url = APIs.CMS.userProfile;
            Dictionary<string, string> optionHeader = new Dictionary<string, string>() { { SystemSetting.TokenKey, token } };
            Response<UserProfile> response = send<UserProfile>(url, optionHeader);
            if (response.isSuccess) {
                return response.data;
            }
            return null;
        }

        public ServiceProfile getServiceProfile(string token) {
            string url = APIs.CMS.serviceProfile;
            Dictionary<string, string> optionHeader = new Dictionary<string, string>() { { SystemSetting.TokenKey, token } };
            Response<ServiceProfile> response = send<ServiceProfile>(url, optionHeader);
            if (response?.isSuccess ?? false) {
                return response.data;
            }
            return null;
        }

        public Response<LoginUser> userLogin(string token) {
            string url = APIs.CMS.clientLogin;
            Dictionary<string, string> optionHeader = new Dictionary<string, string>() { { SystemSetting.TokenKey, getToken() } };
            var request = new { token = token };
            return send<LoginUser>(url, optionHeader, request);
        }

        public string sendAnomaly(ReportAnomaly payload) {
            try {
                string url = APIs.CMS.serviceAnomaly;
                Dictionary<string, string> optionHeader = new Dictionary<string, string>() { { SystemSetting.TokenKey, getToken() } };
                Response<string> response = send<string>(url, optionHeader, payload);
                return response.message;
            } catch (Exception ex) {
                return ex.Message;
            }
        }

        #region -CMS Service Token-
        // ======================= Service Login ======================= 
        public string serviceCode = AppSetting.ServiceCode;
        public string serviceToken { get; private set; }
        private double expireAt { get; set; }
        //======================================================

        public string getToken() {
            if (string.IsNullOrWhiteSpace(serviceToken)) {
                createServiceToken();
            } else if (expireAt <= new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds()) {
                if (!refreshServiceToken()) {
                    createServiceToken(); // refresh failed, get new one
                }
            }
            if (string.IsNullOrWhiteSpace(serviceToken)) {
                throw new Exception("ServiceToken Error!");
            }
            return serviceToken;
        }

        private bool createServiceToken() {
            string url = APIs.CMS.serviceAccess;
            Dictionary<string, string> optionHeader = new Dictionary<string, string>() { { SystemSetting.TokenKey, serviceCode } };
            return getAccessToken(url, optionHeader);
        }

        private bool refreshServiceToken() {
            string url = APIs.CMS.serviceRefresh(serviceCode);
            Dictionary<string, string> optionHeader = new Dictionary<string, string>() { { SystemSetting.TokenKey, serviceToken } };
            return getAccessToken(url, optionHeader);
        }

        private bool getAccessToken(string url, Dictionary<string, string> optionHeader) {
            Response<TokenExpired> response = send<TokenExpired>(url, optionHeader, true);
            if (response.isSuccess) {
                TokenExpired tokenInfo = response.data;
                expireAt = tokenInfo.expireAt;
                serviceToken = AesCrypt.AesDecrypt(tokenInfo.token, serviceCode, response.disposal);
                return true;
            }
            return false;
        }

        class TokenExpired {
            public string token { get; set; }
            public double expireAt { get; set; }
        }
        #endregion
    }
}