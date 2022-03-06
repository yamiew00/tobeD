using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using ExamKeeper.Presenters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamKeeper.Controllers {

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FixController : ControllerUtil {

        [HttpGet("Selection")]
        public IActionResult getSelection() {
            try {
                checkAuth();
                FixRecord model = new FixRecord(user);
                model.get();
                return Ok(model.response);
            } catch (Exception ex) {
                return setStatusCode(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [HttpGet("{eduSubject}")]
        public IActionResult getList([StringLength(3)] string eduSubject, string year) {
            try {
                checkAuth();
                FixRecord model = new FixRecord(user);
                model.get(eduSubject, year);
                return Ok(model.response);
            } catch (Exception ex) {
                return setStatusCode(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [HttpPut("{eduSubject}/Understand")]
        public IActionResult setUnderstand([StringLength(3)] string eduSubject, string ID) {
            try {
                checkAuth();
                FixRecord model = new FixRecord(user);
                model.setUnderstand(eduSubject, ID);
                return Ok(model.response);
            } catch (Exception ex) {
                return setStatusCode(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [HttpPost("Practice")]
        public IActionResult create(FixPractice request) {
            try {
                checkAuth();
                FixRecord model = new FixRecord(user);
                model.createPractice(request);
                return Ok(model.response);
            } catch (Exception ex) {
                return setStatusCode(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }
    }
}