using System.Collections.Generic;

namespace ExamKeeperClassLibrary
{
    /// <summary>
    /// configs。用來取代ExamKeeperConfig類別
    /// </summary>
    /// <see cref="ExamKeeperConfig">
    public class Configs
    {
        private readonly Dictionary<string, string> Dictionary = new Dictionary<string, string>();

        public string this[string key]
        {
            get
            {
                return Dictionary[key];
            }
        }

        public Configs(Dictionary<string, string> dictionary)
        {
            Dictionary = dictionary;
        }
    }
}
