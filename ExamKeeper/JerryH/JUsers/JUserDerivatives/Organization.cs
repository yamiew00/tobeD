using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamKeeper.JerryH.JUsers.JUserDerivatives
{
    public class Organization
    {
        [JsonProperty("type")]
        public string Type;

        [JsonProperty("code")]
        public string Code;

        [JsonProperty("name")]
        public string Name;
    }
}
