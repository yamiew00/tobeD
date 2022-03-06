using System.Collections.Generic;

namespace Utils {

    /// <summary> 格式檢核 </summary>
    public static class ExamChecker {
        public static List<CodeMap> getYearMap(string yearString, int yearCount = 3) {
            try {
                List<CodeMap> result = new List<CodeMap>();
                int year = 0;
                if (!int.TryParse(yearString, out year)) {
                    return null;
                }
                for (int i = 0; i < yearCount; i++) {
                    result.Add(new CodeMap((year - i).ToString(), $"{year - i}學年度"));
                }
                return result;
            } catch {
                return null;
            }
        }
        public static bool checkUID(string value) {
            if (string.IsNullOrWhiteSpace(value)) {
                return false;
            }
            if (!value.Contains(Constants.Dash)) {
                return false;
            }
            if (value.Length != 36) {
                return false;
            }
            string[] valueArr = value.Split(Constants.Dash);
            return valueArr[0].Length == 3 && valueArr[1].Length == 32;
        }
        public static string getYear(string value) {
            return value.Split(Constants.Dash) [0];
        }
        public static Difficulty setDifficultyCode(string originCode) {
            Difficulty difficulty = ExtensionHelper.GetFromName<Difficulty>(originCode);
            switch (difficulty) {
                case Difficulty.BEGIN:
                    return Difficulty.BEGIN;
                case Difficulty.BASIC:
                case Difficulty.INTERMEDIATE:
                case Difficulty.ADVANCED:
                    return Difficulty.INTERMEDIATE;
                case Difficulty.EXPERT:
                    return Difficulty.EXPERT;
                default:
                    return Difficulty.None;
            }
        }
    }
}