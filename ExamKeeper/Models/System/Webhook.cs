using ExamKeeper.Models;

namespace Utils.Setting {
    public class WebhookModel {
        private IMongoSetting setting { get; set; }

        public WebhookModel(IMongoSetting setting) {
            this.setting = setting;
        }
        public string setOTP(Webhook webhook) {
            if (string.IsNullOrWhiteSpace(webhook.otp)) {
                webhook.otp = Format.newGuid();
            }
            setting.setWebhook(webhook);
            return webhook.otp;
        }
        public bool checkOTP(string otp, string key, ref Webhook webhook) {
            webhook = setting.getWebhook(otp, key);
            return webhook != null;
        }
    }
}