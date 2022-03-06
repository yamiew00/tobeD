using System;
using System.Collections.Generic;

#nullable disable

namespace ExamKeeperClassLibrary.Models.Configs
{
    public partial class ExamkeeperConfig
    {
        public string Version { get; set; }
        public string Key { get; set; }
        public string Dev { get; set; }
        public string Uat { get; set; }
        public string Release { get; set; }
    }
}
