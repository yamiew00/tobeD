using System;
using System.Net;
using ExamKeeper.Presenters;
using ExamKeeper.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamKeeper.Controllers {
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AnomalyController : ControllerUtil {
        [HttpGet("Related")]
        public IActionResult related() {
            try {
                checkAuth();
                AnomalyReport model = new AnomalyReport(user);
                model.related();
                return Ok(model.response);
            } catch (Exception ex) {
                return setStatusCode(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [HttpPost("Report")]
        public IActionResult sendAnomaly(AnomalyPayload request) {
            try {
                checkAuth();
                AnomalyReport model = new AnomalyReport(user);
                model.report(request);
                return Ok(model.response);
            } catch (Exception ex) {
                return setStatusCode(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }
    }
}