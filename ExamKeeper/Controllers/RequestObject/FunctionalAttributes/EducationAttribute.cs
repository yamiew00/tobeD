using ExamKeeperClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamKeeper.Controllers.RequestObject.FunctionalAttributes
{
    public class EducationAttribute : AutoValidateAttribute
    {
        private const string Error = "The {0} property code not matched.";

        public EducationAttribute(): base(Error)
        {
                
        }

        public override bool IsValid(object value)
        {
            //空值判斷可以處理得更好
            if(value == null)
            {
                return false;
            }

            //必須要是Education代碼
            return Definitions.EducationCodes.ContainsKey(value.ToString());
        }
    }
}
