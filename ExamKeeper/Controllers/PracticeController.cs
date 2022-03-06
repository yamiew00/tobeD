using System;
using System.Net;
using ExamKeeper.Presenters;
using ExamKeeper.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamKeeper.Controllers {
    /// <summary>自主練習</summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PracticeController : ControllerUtil {
        [HttpPost("Create")]
        public IActionResult Create(CreatePractice request) {
            try {
                checkAuth();
                PracticePresenter presenter = new PracticePresenter();
                presenter.create(user, request);
                return Ok(presenter.response);
            } catch (Exception ex) {
                return setStatusCode(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [HttpGet("Query")]
        public IActionResult Query(string year = "") {
            try {
                checkAuth();
                PracticePresenter model = new PracticePresenter();
                model.query(user, year);
                return Ok(model.response);
            } catch (Exception ex) {
                return setStatusCode(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [HttpPost("Start")]
        public IActionResult setStart(PracticeExamPayload request) {
            try {
                checkAuth();
                PracticeExam model = new PracticeExam(user, request.UID);
                model.createExam(request.oneclubToken);
                return Ok(model.response);
            } catch (Exception ex) {
                return setStatusCode(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }
    }
}