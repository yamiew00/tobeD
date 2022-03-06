using ExamKeeperClassLibrary;

namespace ExamKeeper.Controllers.RequestObject.FunctionalAttributes
{
    public class GradeAttribute: AutoValidateAttribute
    {
        private const string Error = "The {0} property code not matched.";

        public GradeAttribute(): base(Error)
        {

        }

        public override bool IsValid(object value)
        {
            //空值判斷可以處理得更好
            if (value == null)
            {
                return false;
            }

            //必須要是Grade代碼
            return Definitions.GradeCodes.ContainsKey(value.ToString());
        }
    }
}
