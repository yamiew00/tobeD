using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExamKeeper.Controllers.RequestObject
{
    public class AutoValidateAttribute: ValidationAttribute
    {
        public AutoValidateAttribute(string errorMessage): base(errorMessage)
        {

        }

        /// <summary>
        /// 自定義預設值字典。
        /// </summary>
        protected static readonly Dictionary<Type, object> CustomDefaultDictionary = AutoValidateObject.CustomDefaultDictionary;
    }
}
