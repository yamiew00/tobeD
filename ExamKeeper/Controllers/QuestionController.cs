using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using ExamKeeper.Presenters;
using ExamKeeper.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utils;

namespace ExamKeeper.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api")]
    public class QuestionController : ControllerUtil
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pattern">ÂÂªºpattern</param>
        /// <param name="request"></param>
        /// <param name="choosePattern">·sªºpattern</param>
        /// <returns></returns>
        [HttpGet("{pattern}/Selection")]
        public IActionResult Selection([Required] DrawUpPattern pattern, [FromQuery] PatternPayload request)
        {
            try
            {
                QueryQuestion model = new QueryQuestion(user, pattern.ToString());
                model.getSelection(request);
                return Ok(model.response);
            }
            catch (Exception ex)
            {
                return setStatusCode(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [HttpPost("{pattern}/CacheQuery")]
        public IActionResult cacheQuery([Required] string pattern, QuestionPayload request)
        {
            try
            {
                checkAuth();
                QueryQuestion model = new QueryQuestion(user, pattern);
                model.queryQuestion(request);
                return Ok(model.response);
            }
            catch (Exception ex)
            {
                return setStatusCode(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [HttpGet("{pattern}/CacheQuery")]
        public IActionResult getQuestionCache([Required] string pattern, [Required] string searchKey)
        {
            try
            {
                checkAuth();
                QueryQuestion model = new QueryQuestion(user, pattern);
                model.getQuestionCache(searchKey);
                return Ok(model.response);
            }
            catch (Exception ex)
            {
                return setStatusCode(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [HttpGet("{pattern}/QuestionInfo")]
        public IActionResult getQuestionInfo([Required] string pattern, [Required] string searchKey)
        {
            try
            {
                checkAuth();
                QueryQuestion model = new QueryQuestion(user, pattern);
                model.queryQuestionInfo(searchKey);
                return Ok(model.response);
            }
            catch (Exception ex)
            {
                return setStatusCode(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [HttpPost("Question/Chart")]
        public IActionResult QuestionChart([Required] ChartPayload request)
        {
            try
            {
                checkAuth();
                QuestionChart model = new QuestionChart(user);
                model.getChart(request);
                return Ok(model.response);
            }
            catch (Exception ex)
            {
                return setStatusCode(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [HttpGet("Exam/Question")]
        public IActionResult copyExamQuestion(string examUID, string action)
        {
            try
            {
                checkAuth();
                CopyQuestion model = new CopyQuestion(user);
                if (model.getExamPaper(examUID, action))
                {
                    model.getQuestions();
                }
                return Ok(model.response);
            }
            catch (Exception ex)
            {
                return setStatusCode(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }
    }
}