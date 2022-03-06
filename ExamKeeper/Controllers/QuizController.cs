using ExamKeeper.Controllers.RequestObject.QuizObject;
using ExamKeeper.Controllers.ResponseObject.QuizObject;
using ExamKeeper.JerryH.Jutils;
using ExamKeeperClassLibrary;
using ExamKeeperClassLibrary.Extensions;
using ExamKeeperClassLibrary.FileProvider;
using ExamKeeperClassLibrary.Models;
using ExamKeeperClassLibrary.Models.ExamPaper;
using ExamKeeperClassLibrary.Models.Quizzes;
using ExamKeeperClassLibrary.Models.ResourceLibrary.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ExamKeeper.Controllers
{
    /// <summary>
    /// 測驗的路由。
    /// 1.只有老師才可以使用速驗
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class QuizController : ControllerUtil
    {
        /// <summary>
        /// csv資料夾路徑
        /// </summary>
        private readonly static string CSV_ROOT_PATH = Path.Combine(Directory.GetCurrentDirectory(), CSV_FOLDER_NAME);

        /// <summary>
        /// 存放csv的資料夾名稱
        /// </summary>
        private const string CSV_FOLDER_NAME = "QuizCSV";

        private readonly Configs Configs;

        /// <summary>
        /// 資源平台的定義表
        /// </summary>
        private readonly Dictionary<(string, string), Definition> Definitions;

        public QuizController(Configs configs, Dictionary<(string, string), Definition> definitions)
        {
            Configs = configs;
            Definitions = definitions;
        }

        /// <summary>
        /// 靜態建構式。
        /// </summary>
        static QuizController()
        {
            //只需要建一次資料夾，且不應該刪掉它
            CSVManager.AddDirectory("csv root Folder", CSV_ROOT_PATH);
        }

        /// <summary>
        /// 建立測驗。與OneExam對接。
        /// https://dev.azure.com/oneclass-rd/OneExam/_wiki/wikis/OneExam.wiki/81/OneExam-API-Document
        /// </summary>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] QuizCreateBody quizCreatebody)
        {
            try
            {
                //mongoContext, 之後再統整注入方式
                MongoContext context = new MongoContext(Config.MongoConnectionString);

                //先去找exam。這邊必須要指定collection名字，因為是照年份切的
                var year = quizCreatebody.ExamID.Substring(0, 3);
                var exam = context.GetDatabase("ExamPaper")
                                  .GetCollection<ExamPaper>("ExamPaper" + year)
                                  .Find(x => x.UID == quizCreatebody.ExamID).Limit(1)
                                  .FirstOrDefault();

                //確認考卷存在
                if(exam == null)
                {
                    //回傳格式還沒固定
                    return Ok(new
                    {
                        systemCode = "0400",
                        isSuccess = false,
                        systemNow = DateTime.Now,
                        diposal = string.Empty,
                        data = "無此測驗"
                    });
                }

                //新增OTP, 尚未使用ttl
                var otpCode = Guid.NewGuid().ToString();
                context.ExamPaper.InsertOne<ServiceOTP>(new ServiceOTP()
                {
                    ExamUID = quizCreatebody.ExamID,
                    OptCode = otpCode.ToString(),
                    UserUIDs = new List<string>() { new Guid(JUser.OneClubUID.ToString()).ToString() },
                    Service = "OneTest",   //寫死
                    SystemTime = DateTime.Now,
                    ExpireAt = DateTime.Now.AddDays(1).DateTimeToUnixTimeStamp()
                });

                //打oneExam API 拿測驗id
                var oneExamQuiz = await APISender.Create()
                                                 .AddUri(Config.OneExamApiQuiz)
                                                 .AddAuthorization(quizCreatebody.JwtToken)
                                                 .AddRequestBody(new
                                                 {
                                                     examName = quizCreatebody.QuizName,
                                                     paperId = quizCreatebody.ExamID,                       //沒時間做id的exists檢查，直接通過
                                                     examPeriod = quizCreatebody.Duration,
                                                     service = "onePaper",                              //寫死的
                                                     paperType = "exam",                                //寫死的
                                                     startAt = quizCreatebody.StartTime,
                                                     endAt = quizCreatebody.EndTime,
                                                     education = quizCreatebody.EducationCode,
                                                     grade = int.Parse(quizCreatebody.GradeCode.Substring(2, 1)), //年級代碼轉換
                                                     isAnnounced = quizCreatebody.IsAutoCheck,
                                                     otp = otpCode,
                                                     userAuthId = JUser.OneClubUID
                                                 })
                                                 .PostAsync<OneExamQuiz>();

                //新增測驗進MongoDB
                var newQuiz = new Quiz()
                {
                    QuizUID = oneExamQuiz.Content.QuizId,
                    QuizCode = oneExamQuiz.Content.QuizCode,
                    UserUID = JUser.UID,
                    QuizName = quizCreatebody.QuizName,
                    ExamUID = quizCreatebody.ExamID,
                    ExamName = exam.Attribute.Name,
                    SchoolYear = oneExamQuiz.Content.SchoolYear,
                    StartTime = quizCreatebody.StartTime,
                    EndTime = quizCreatebody.EndTime,
                    Duration = quizCreatebody.Duration,
                    IsAutoCheck = quizCreatebody.IsAutoCheck,
                    Education = quizCreatebody.EducationCode,
                    Subject = oneExamQuiz.Content.Attribute.Subject,
                    Grade = quizCreatebody.GradeCode,
                    CreateTime = DateTime.Now
                };
                context.Quizzes.InsertOne(newQuiz);

                return Ok(new
                {
                    systemCode = "0200",
                    isSuccess = true,
                    systemNow = DateTime.Now,
                    diposal = string.Empty,
                    data = newQuiz
                });
            }
            catch (Exception ex)
            {
                //回傳格式還沒固定
                return Ok(new
                {
                    systemCode = "0400",
                    isSuccess = false,
                    systemNow = DateTime.Now,
                    diposal = string.Empty,
                    data = ex.Message
                });
            }
        }

        /// <summary>
        /// 取得測驗列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("List")]
        public IActionResult GetList()
        {
            try
            {
                //mongoContext, 之後再統整注入方式
                MongoContext context = new MongoContext(Config.MongoConnectionString);

                var userUID = JUser.UID;
                var list = context.Quizzes.Find<Quiz>(quiz => quiz.UserUID == userUID);

                //學制科目轉換(很差的做法)
                var result = list.Select(quiz => quiz.EduSubjectFormat(Definitions));

                return Ok(new
                {
                    systemCode = "0200",
                    isSuccess = true,
                    systemNow = DateTime.Now,
                    diposal = string.Empty,
                    data = result
                });
            }
            catch (Exception ex)
            {
                //回傳格式還沒固定
                return Ok(new
                {
                    systemCode = "0400",
                    isSuccess = false,
                    systemNow = DateTime.Now,
                    diposal = string.Empty,
                    data = ex.Message
                });
            }
        }

        /// <summary>
        /// 取得目前測驗結果。與OneExam對接。
        /// https://dev.azure.com/oneclass-rd/OneExam/_wiki/wikis/OneExam.wiki/81/OneExam-API-Document
        /// </summary>
        /// <returns></returns>
        [HttpPost("Result")]
        public async Task<IActionResult> Result([FromBody] QuizResultBody quizResultbody)
        {
            try
            {
                //透過body可以取得:
                var quizUID = quizResultbody.QuizUID;

                //mongoContext, 之後再統整注入方式
                MongoContext context = new MongoContext(Config.MongoConnectionString);
                var quiz = context.Quizzes.FindOne<Quiz>(quiz => quiz.QuizUID == quizUID);


                //報錯: 找不到quiz
                if (quiz == null)
                {
                    //回傳格式還沒固定
                    return Ok(new
                    {
                        systemCode = "0402",
                        isSuccess = false,
                        systemNow = DateTime.Now,
                        data = "查無此測驗"
                    });
                }

                var quizName = quiz.QuizName;
                var schoolYear = quiz.SchoolYear;

                //進mongoDB取過往紀錄:
                var quizInstanceReport = context.Quizzes.FindOne<QuizInstantReport>(report => report.QuizUID == quizUID);

                //去OneExam取資料
                var oneExamQuizResult = await APISender.Create()
                                                       .AddUri(Configs["OneExamCloudFunction"] + @"/api/quiz/results/" + quizUID)
                                                       .AddAuthorization(quizResultbody.JwtToken)
                                                       .GetAsync<OneExamQuizResult>();
                //已交卷人數
                var completedNumber = oneExamQuizResult.Content.Count();

                //若資料未更新，則直接回傳歷史記錄
                if (quizInstanceReport != null && quizInstanceReport.CompletedNumber == completedNumber)
                {
                    //回傳格式還沒固定
                    return Ok(new
                    {
                        systemCode = "0200",
                        isSuccess = true,
                        systemNow = DateTime.Now,
                        diposal = string.Empty,
                        data = new
                        {
                            IsUpdate = false,
                            quizInstanceReport.CSVFileUrl,
                            quizInstanceReport.CompletedNumber
                        }
                    });
                }

                //若資料有更新，造本地csv並上傳firebase
                var students = oneExamQuizResult.Content
                                                .Select(student => new QuizStudent
                                                {
                                                    Seat = student.UserInfo.SeatNo,
                                                    Name = student.UserInfo.UserName,
                                                    Score = student.AnswerDatas.Sum(data => data.Score),
                                                    Url = Configs["oneExamDomainName"] +
                                                    $"{quiz.QuizCode}/{quiz.SchoolYear}/{student.UserInfo.SeatNo}/" +
                                                    $"{HttpUtility.UrlEncode(student.UserInfo.UserName)}"
                                                });


                var fileName = $"{quizName}_作答{completedNumber}人" + ".csv";
                var filePath = Path.Combine(CSV_ROOT_PATH, fileName);

                await CSVManager.WriteCSV<QuizStudent>(filePath, students);

                FirebaseManager firebaseManager = new FirebaseManager(key: Configs["firebaseKey"],
                                                                      domainName: Configs["firebaseDomainName"]);

                var url = await firebaseManager.UploadData(filePath);

                //將紀錄upsert進資料庫
                var newReport = new QuizInstantReport()
                {
                    QuizUID = quizUID,
                    CompletedNumber = completedNumber,
                    CSVFileUrl = url
                };
                context.Quizzes.UpsertOne<QuizInstantReport>(report => report.QuizUID == newReport.QuizUID,
                                                             newReport);

                //上傳成功後就刪除本地csv
                System.IO.File.Delete(filePath);

                //回傳格式還沒固定
                return Ok(new
                {
                    systemCode = "0200",
                    isSuccess = true,
                    systemNow = DateTime.Now,
                    diposal = string.Empty,
                    data = new
                    {
                        IsUpdate = true,
                        newReport.CSVFileUrl,
                        newReport.CompletedNumber
                    }
                });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("查無此試卷結果"))
                {
                    return Ok(new
                    {
                        systemCode = "0400",
                        isSuccess = false,
                        systemNow = DateTime.Now,
                        diposal = string.Empty,
                        data = new
                        {
                            IsUpdate = false,
                            CSVFileUrl = string.Empty,
                            CompletedNumber = 0
                        }
                    });
                }

                //回傳格式還沒固定
                return Ok(new
                {
                    systemCode = "0400",
                    isSuccess = false,
                    systemNow = DateTime.Now,
                    diposal = string.Empty,
                    data = ex.Message
                });
            }
        }
    }
}
