using System;
using System.ComponentModel.DataAnnotations;

namespace Utils {
    public class BaseColumn {
        [StringLength(20)]
        public string maintainer { get; set; }
        // data from another system, not in exam-keeper
        public DateTime? createTime { get; set; }
        public DateTime? updateTime { get; set; }
    }
}