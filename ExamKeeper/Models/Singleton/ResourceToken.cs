using System;
using System.Collections.Generic;
using ExamKeeper.Utils;
using Utils;

namespace ExamKeeper.Models {
    public interface IServiceToken {
        string getToken();
    }

    public class ResourceTokenSingleton : SendRequest, IServiceToken {
        public ResourceTokenSingleton(IMongoLogger logger) : base(logger) { }
        private static readonly object tokenlock = new object();
        private static ResourceTokenSingleton _instance = null;
        public static ResourceTokenSingleton Instance(IMongoLogger logger) {
            lock(tokenlock) {
                if (_instance == null) {
                    _instance = new ResourceTokenSingleton(logger);
                }
            }
            return _instance;
        }
        // ======================= Service Login ======================= 
        public string serviceCode = AppSetting.ResourceAccount;
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
            string url = APIs.Resource.PlatformAccess;
            Dictionary<string, string> optionHeader = new Dictionary<string, string>() { { SystemSetting.TokenKey, serviceCode } };
            return getAccessToken(url, optionHeader);
        }

        private bool refreshServiceToken() {
            string url = APIs.Resource.PlatformToken(serviceCode);
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

        #region -private class-
        class TokenExpired {
            public string token { get; set; }
            public double expireAt { get; set; }
        }
        #endregion
    }
}