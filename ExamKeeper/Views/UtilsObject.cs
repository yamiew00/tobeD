using System;
using System.Collections.Generic;
using ExamKeeper.Views;

namespace Utils.Setting {
    public class UserProfile {
        public Guid UID { get; set; } // 系統UID
        public string name { get; set; } // 姓名
        public string identity { get; set; } // 身分
        public string email { get; set; } //電子信箱
        public string account { get; set; } // 可識別帳號 (沿用oneclub帳號)
        public string usetype { get; set; } // 可用資料範圍
        public AttributeMap organization { get; set; }
        public string status { get; set; } // 狀態
        public DateTime lastLogin { get; set; } // 最後登入時間
        public bool isActive() {
            return AccountStatus.Active.ToString().Equals(status);
        }
        public string getMaintainer() {
            return string.Format("[{0}] {1}", account, name);
        }
        public bool checkIdentity(SystemIdentity type) {
            return type.Equals(getIdentity());
        }
        public SystemIdentity getIdentity() {
            return ExtensionHelper.GetFromName<SystemIdentity>(identity);
        }
        public SystemOrganization getOrganization() {
            if (organization == null) {
                return SystemOrganization.None;
            }
            return ExtensionHelper.GetFromName<SystemOrganization>(organization.type);
        }
    }
    public class LoginUser {
        public UserProfile userProfile { get; set; }
        public Preference preference { get; set; }
        public List<CodeMap> examQueryMap { get; set; }
        public string token { get; set; }
        public long expireAt { get; set; }
    }
}