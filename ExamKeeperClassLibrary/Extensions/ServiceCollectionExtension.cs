using ExamKeeperClassLibrary.Models.Configs;
using ExamKeeperClassLibrary.Models.QuestionBank.Mongos;
using ExamKeeperClassLibrary.Models.QuestionBank.Mongos.Collections;
using ExamKeeperClassLibrary.Models.ResourceLibrary;
using ExamKeeperClassLibrary.Models.ResourceLibrary.Resources;
using ExamKeeperClassLibrary.Models.ResourceLibrary.SubjectMeta;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace ExamKeeperClassLibrary.Extensions
{
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// 注入類別庫相關物件
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void InjectLibrary(this IServiceCollection services, IConfiguration configuration)
        {
            //1. Configs
            var configs = CreateConfig(connectionString: configuration.GetConnectionString("ConfigDB"),
                                                        enviroment: configuration.GetConnectionString("enviroment"),
                                                        version: configuration.GetSection("version").Value);
            services.AddSingleton<Configs>(configs);

            //2. Definition(資源平台)
            resourceContext resourceContext = new resourceContext(configs["resourceLibrary_ConnectionString"] + "resource");

            Dictionary<(string, string), Definition> Definitions = resourceContext.Definitions.ToDictionary(item => (item.Type, item.Code));

            services.AddSingleton<Dictionary<(string, string), Definition>>(Definitions);

            //3.處理「定義」物件, 這裡的subjectMetaContext處理不太好，會在DefinitionLibrary內部填充資料
            SubjectMetaContext subjectMetaContext = new SubjectMetaContext(configs["resourceLibrary_ConnectionString"] + "subjectMeta");
            var definitionLibrary = new DefinitionLibrary(resourceContext, 
                                                          subjectMetaContext);
            services.AddSingleton<DefinitionLibrary>(definitionLibrary);

            //4.載入 ResourceLibrary 資料庫內容
            BookLibrary library = new BookLibrary(configs["resourceLibrary_ConnectionString"], 
                                                  definitionLibrary, 
                                                  subjectMetaContext,
                                                  resourceContext);
            services.AddSingleton<BookLibrary>(library);

            //5. QuestionBank的題目與索引
            QuestionProvider questionProvider;
            if (bool.Parse(configuration.GetSection("IsIgnoreIndexCreating").Value))
            {
                questionProvider = new QuestionProvider(Enumerable.Empty<string>(), default);
            }
            else
            {
                MongoClient questionBankClient = new MongoClient(configs["QuestionBank_Mongo_ConnectionString"] + "connectTimeoutMS=60000;" + "socketTimeoutMS=120000;" + "?connect=replicaSet");
                var dataBase = questionBankClient.GetDatabase("QuestionBank"); //寫死

                MongoDataStream<Question> mongoDataStream = new MongoDataStream<Question>(dataBase, definitionLibrary.EduSubjects);
                questionProvider = new QuestionProvider(definitionLibrary.EduSubjects, mongoDataStream);

                //這裡怎麼多監聽一次???
                //mongoDataStream.Activate();
            }

            services.AddSingleton<QuestionProvider>(questionProvider);
        }

        /// <summary>
        /// 去資料庫拿取，生成此專案config檔所有內容。
        /// </summary>
        /// <param name="connectionString">連線字串</param>
        /// <param name="enviroment"> dev/uat/release </param>
        /// <param name="version">版本號</param>
        /// <returns></returns>
        private static Configs CreateConfig(string connectionString, string enviroment, string version)
        {
            //config資料庫
            project_configContext configContext = new project_configContext(connectionString,
                                                                            Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.18-mysql"));
            var examkeeperConfigs = configContext.ExamkeeperConfigs.Where(config => config.Version == version);

            return enviroment switch
            {
                "dev" => new Configs(examkeeperConfigs.ToDictionary(config => config.Key, config => config.Dev)),
                "uat" => new Configs(examkeeperConfigs.ToDictionary(config => config.Key, config => config.Uat)),
                "release" => new Configs(examkeeperConfigs.ToDictionary(config => config.Key, config => config.Release)),
                _ => throw new System.Exception("環境變數錯誤。")
            };
        }
    }
}
