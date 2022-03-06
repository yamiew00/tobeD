namespace ExamKeeper.Controllers.RequestObject
{
    public class NotEmptyAttribute : AutoValidateAttribute
    {
        private const string Error = "The {0} property must not be empty";

        public NotEmptyAttribute(string errorMessage = default): base((errorMessage == default)? Error: errorMessage)
        {

        }

        public override bool IsValid(object value)
        {
            //一般空值判斷
            if (value == null)
            {
                return false;
            }

            //特殊賦值判斷→符合則視為空值
            var type = value.GetType();

            if (CustomDefaultDictionary.TryGetValue(type, out var typeValue))
            {
                if (object.Equals(value, typeValue))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
