using Utils;
using Utils.Setting;

namespace ExamKeeper.Presenters {
    public class MemberAuth : PresenterBase {
        public bool allowTypesetting(UserProfile profile) {
            SystemIdentity identity = profile.getIdentity();
            switch (identity) {
                case SystemIdentity.Admin:
                case SystemIdentity.Editor:
                case SystemIdentity.Teacher:
                    return true;
                default:
                    return false;
            }
        }
    }
}