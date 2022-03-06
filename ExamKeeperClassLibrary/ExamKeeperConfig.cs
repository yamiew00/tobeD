using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace ExamKeeperClassLibrary
{
    /// <summary>
    /// Configuration(待棄用，未來要更新為Configs)
    /// <see cref="Configs"/>
    /// </summary>
    public class ExamKeeperConfig
    {
        private readonly IConfiguration Configuration;

        public ExamKeeperConfig(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// MongoDB連線字串
        /// </summary>
        public string MongoConnectionString => Configuration.GetConnectionString("JMongo:Url");

        public string OneExamApiQuiz => Configuration.GetConnectionString("JAPIs:OneExam:Quiz");
    }
}
