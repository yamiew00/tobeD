using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ExamKeeper.Models;
using ExamKeeperClassLibrary.Models.QuestionBank.Mongos.Collections;
using Microsoft.AspNetCore.Mvc;
using Utils;

namespace ExamKeeper.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerUtil
    {

        private QuestionProvider QuestionProvider;
        public TestController(QuestionProvider questionProvider)
        {
            QuestionProvider = questionProvider;
        }

        public MongoLogger logger = new InitMongoLog("TestController");
        [HttpGet("CMS")]
        public IActionResult TestCMS(string token)
        {
            try
            {
                CMSSingleton CMS = CMSSingleton.Instance(logger);
                return Ok(CMS.getUserProfile(token));
            }
            catch (Exception ex)
            {
                return setStatusCode(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }


        [HttpGet("init")]
        public IActionResult Init()
        {
            Dictionary<string, List<string>> result = new Dictionary<string, List<string>>()
            {
                { "done", new List<string>() },
                { "undone", new List<string>() }
            };

            foreach (var questionCollection in QuestionProvider.QuestionCollections.Values)
            {
                if (questionCollection.IsCompleted())
                {
                    result["done"].Add(questionCollection.Subject);
                    continue;
                }
                result["undone"].Add(questionCollection.Subject);
            }

            return Ok(result);
        }
    }
}