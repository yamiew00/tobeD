using System.Linq;
using System.Text;
using ExamKeeper.Resources;
using ExamKeeper.Views;
using Utils;

namespace ExamKeeper.Presenters {
    /// <summary> 試卷排版檢核 </summary>
    public static class PaperChecker {
        public static bool checkTypesetting(Typesetting setting, ref string errorMessage) {
            StringBuilder msg = new StringBuilder();
            if (setting == null) {
                errorMessage = CustomString.Required("Typesetting");
                return false;
            }
            if (!string.IsNullOrWhiteSpace(setting.paperSize)) {
                if (!ExtensionHelper.checkName<PaperSize>(setting.paperSize)) {
                    msg.AppendLine(CustomString.TypeError(@"紙張大小"));
                }
            }
            if (!string.IsNullOrWhiteSpace(setting.wordSetting)) {
                if (!ExtensionHelper.checkName<WordSetting>(setting.wordSetting)) {
                    msg.AppendLine(CustomString.TypeError(@"排版方式"));
                }
            }
            if (Compare.EmptyCollection(setting.paperContents)) {
                msg.AppendLine(CustomString.Required(@"輸出卷別"));
            } else {
                if (!ExtensionHelper.checkNames<PaperContent>(setting.paperContents)) {
                    msg.AppendLine(CustomString.TypeError(@"輸出卷項目"));
                }
                if (setting.paperContents.Contains(PaperContent.Analyze.ToString())) {
                    if (Compare.EmptyCollection(setting.analyzeContent)) {
                        msg.AppendLine(CustomString.Required(@"解析卷匯出項目"));
                    } else if (!ExtensionHelper.checkNames<AnalyzeContent>(setting.analyzeContent)) {
                        msg.AppendLine(CustomString.TypeError(@"解析卷匯出項目"));
                    }
                }
            }

            if (setting.amount > 1 && (Compare.EmptyCollection(setting.advanced) || !setting.advanced.Any(o => o.StartsWith("Change")))) {
                msg.AppendLine(@"輸出一種以上的試卷，至少需選取 [變換題序] 或 [變換選項] 其一");
            }
            if (!Compare.EmptyCollection(setting.advanced) &&
                !ExtensionHelper.checkNames<AdvancedSetting>(setting.advanced)) {
                msg.AppendLine(CustomString.TypeError(@"進階設定項目"));
            }
            errorMessage = msg.ToString();
            return string.IsNullOrWhiteSpace(errorMessage);
        }

        public static bool checkTypesetting(OnlineSetting setting, ref string errorMessage) {
            StringBuilder msg = new StringBuilder();
            if (setting != null && !Compare.EmptyCollection(setting.advanced)) {
                if (!ExtensionHelper.checkNames<AdvancedSetting>(setting.advanced)) {
                    msg.AppendLine(CustomString.TypeError(@"進階設定項目"));
                }
                if (setting.advanced.Any(o => !o.StartsWith("Change"))) {
                    msg.AppendLine(@"線上測驗卷只允許變換順序相關設定");
                }
            }
            errorMessage = msg.ToString();
            return string.IsNullOrWhiteSpace(errorMessage);
        }
    }
}