using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ExamKeeper.Controllers.RequestObject
{
    public class AutoValidateObject
    {
        public AutoValidateObject()
        {
            SetProperties();
        }

        /// <summary>
        /// 指定Properties的值
        /// </summary>
        public void SetProperties()
        {
            var properties = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var property in properties)
            {

                var attributes = property.GetCustomAttributes<AutoValidateAttribute>(true);

                if (!attributes.Any())
                {
                    continue;
                }

                foreach (var attribute in attributes)
                {
                    if (CustomDefaultDictionary.TryGetValue(property.PropertyType, out var value))
                    {
                        property.SetValue(this, value);
                        continue;
                    }

                    property.SetValue(this, default);
                }

            }
        }

        /// <summary>
        /// 自定義預設值字典。
        /// </summary>
        public static readonly Dictionary<Type, object> CustomDefaultDictionary = new Dictionary<Type, object>()
        {
            {typeof(int), int.MinValue },
            {typeof(DateTime), DateTime.MinValue}
        };
    }
}
