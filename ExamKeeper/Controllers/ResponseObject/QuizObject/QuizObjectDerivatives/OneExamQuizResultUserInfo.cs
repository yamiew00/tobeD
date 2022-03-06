using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamKeeper.Controllers.ResponseObject.QuizObject.QuizObjectDerivatives
{
    public class OneExamQuizResultUserInfo
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName;

        /// <summary>
        /// 座號。這個欄位必須要是string。因為"001" != "01"
        /// 學生只需要輸入姓名與座號，因此在極端情況下會要求學生多輸入好幾個0作為不同人的識別。
        /// </summary>
        public string SeatNo;
    }
}
