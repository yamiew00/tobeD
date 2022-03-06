using ExamKeeper.Controllers.RequestObject.QueryObject;
using ExamKeeperClassLibrary;
using ExamKeeperClassLibrary.Models.QuestionBank.Mongos.Collections;
using ExamKeeperClassLibrary.Models.ResourceLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamKeeper.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class QueryController: ControllerUtil
    {
        private readonly BookLibrary BookLibrary;

        private readonly QuestionProvider QuestionProvider;

        private readonly DefinitionLibrary DefinitionLibrary;



        public QueryController(BookLibrary bookLibrary, QuestionProvider questionProvider, DefinitionLibrary definitionLibrary)
        {
            BookLibrary = bookLibrary;
            QuestionProvider = questionProvider;
            DefinitionLibrary = definitionLibrary;
        }

        [HttpPost("GetBook")]
        public async Task<IActionResult> GetBook([FromBody] QueryBookBody queryBookBody)
        {
            try
            {
                var eduSubject = queryBookBody.Edusubject;
                //出版社沒填的話就全找
                var publishers = queryBookBody.Publishers.Select(publisher => DefinitionLibrary.Publisher[publisher]);

                //篩選條件
                var targetBooks = BookLibrary.Books
                                         .Values
                                        .Where(book => book.EduSubject == eduSubject &&
                                                       ((publishers.Count() == 0) || publishers.Contains(book.Version)));

                var output = targetBooks.GroupBy(book => book.Version)
                                        .Select(item => new
                                        {
                                            publisher = item.Key,
                                            collections = item.GroupBy(book => book.Curriculum)
                                                              .Select(book => new
                                                              {
                                                                  curriculum = book.Key,
                                                                  content = book.GroupBy(subBook => subBook.Year)
                                                                             .Select(subBook => new 
                                                                             {
                                                                                 year = subBook.Key,
                                                                                 books = subBook.ToList()
                                                                             })
                                                              })
                                        });

                //冊次對照表
                var volumeTable= DefinitionLibrary.VolumeName[eduSubject]
                                         .Where(volume => targetBooks.Any(book => book.VolumeName == volume.Key))
                                         .ToDictionary(volume => volume.Key, volume => volume.Value);

                //出處對照表
                var availableSource = await QuestionProvider[eduSubject].GetSourcesKey();
                var sourceTable = availableSource.OrderBy(source => source)
                                                 .Select(source => DefinitionLibrary.Source[source]);

                return Ok(new
                {
                    systemCode = "0200",
                    isSuccess = true,
                    systemNow = DateTime.Now,
                    data = new 
                    {
                        sourceTable,
                        volumeTable,
                        result = output
                    }
                });
            }
            catch(Exception ex)
            {
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost("GetQuestionType")]
        public async Task<IActionResult> TestMongo([FromBody] QueryQuestionTypeBody queryQuestionTypeBody)
        {
            //汲取body
            var eduSubject = queryQuestionTypeBody.EduSubject;
            var knowledges = queryQuestionTypeBody.Knowledges;
            var bookIds = queryQuestionTypeBody.BookIDs;
            var sources = queryQuestionTypeBody.Sources;

            //篩選條件
            var knowsIds = await QuestionProvider[eduSubject].GetByKnowledges(knowledges);
            var booksIds = await QuestionProvider[eduSubject].GetByBooks(bookIds);
            var sourceIds = await QuestionProvider[eduSubject].GetBySources(sources);

            //篩選結果
            var resultIds = knowsIds.Intersect(booksIds).Intersect(sourceIds);

            var questions = await QuestionProvider[eduSubject].GetQuestionById(resultIds);

            //onePaper專用的難易度轉換表
            var onePaperDifficulty = new Dictionary<string, string>()
            {
                { "BEGIN", "BEGIN"},
                { "BASIC", "INTERMEDIATE"},
                { "INTERMEDIATE", "INTERMEDIATE"},
                { "ADVANCED","INTERMEDIATE"},
                { "EXPERT", "EXPERT"}
            };

            var output = questions.GroupBy(question => question.MetaData.QuestionType)
                                  .Select(group => new
                                  {
                                      questionType = group.Key,
                                      questionTypeName = DefinitionLibrary.QuestionType[group.Key],
                                      questions = group.Select(question => new 
                                      {
                                          uid = question.UID,
                                          difficulty = onePaperDifficulty[question.MetaData.Difficulty],
                                          answerAmount = question.AnswerInfos.Sum(info => info.AnswerAmount)
                                      }),
                                      diffcultyAggregate = group.GroupBy(question => onePaperDifficulty[question.MetaData.Difficulty])
                                                                .Select(subGroup => new 
                                                                {
                                                                    difficulty = subGroup.Key,
                                                                    question = subGroup.Sum(question => question.AnswerInfos.Count()),
                                                                    answer = subGroup.Sum(question => question.AnswerInfos.Sum(info => info.AnswerAmount))
                                                                }),
                                      sum = new 
                                      {
                                          question = group.Sum(question => question.AnswerInfos.Count()),
                                          answer = group.Sum(question => question.AnswerInfos.Sum(info => info.AnswerAmount))
                                      }
                                  });

            return Ok(new
            {
                systemCode = "0200",
                isSuccess = true,
                systemNow = DateTime.Now,
                data = output
            });
        }
    }
}
