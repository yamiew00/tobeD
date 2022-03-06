using ExamKeeper.JerryH.JUsers.JUserDerivatives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamKeeper.JerryH.JUsers
{
    public class JUserProfile
    {
        [JsonProperty("oneClubUID")]
        public Guid OneClubUID;

        [JsonProperty("uid")]
        public Guid UID;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("identity")]
        public string Identity;

        [JsonProperty("email")]
        public string Email;

        [JsonProperty("account")]
        public string Account;

        [JsonProperty("usetype")]
        public string UseType;

        [JsonProperty("status")]
        public string Status;

        [JsonProperty("organization")]
        public Organization Organization;

        [JsonProperty("lastLogin")]
        public DateTime LastLogin;

        /// <summary>
        /// 未實作功能的欄位
        /// </summary>
        [JsonIgnore]
        public object education;

        /// <summary>
        /// 未實作功能的欄位
        /// </summary>
        [JsonIgnore]
        public object Subject;
    }
}
