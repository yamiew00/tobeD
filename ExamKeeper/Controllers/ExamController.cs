using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using ExamKeeper.Presenters;
using ExamKeeper.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utils;

namespace ExamKeeper.Controllers {
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    /// <summary>試卷</summary>
    public class ExamController : ControllerUtil {
        [HttpPost("Create")]
        public IActionResult Create(CreateExam request) {
            try {
                checkAuth();
                EditExamPaper model = new EditExamPaper(user);
                model.create(request);
                return Ok(model.response);
            } catch (Exception ex) {
                return setStatusCode(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [HttpPut("Edit/{examUID}")]
        public IActionResult Edit(string examUID, CreateExam request) {
            try {
                checkAuth();
                EditExamPaper model = new EditExamPaper(user);
                model.edit(examUID, request);
                return Ok(model.response);
            } catch (Exception ex) {
                return setStatusCode(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [HttpPut("Edit")]
        public IActionResult Edit(EditExam request) {
            try {
                checkAuth();
                EditExamPaper model = new EditExamPaper(user);
                model.update(request);
                return Ok(model.response);
            } catch (Exception ex) {
                return setStatusCode(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [HttpGet("Query")]
        public IActionResult Query(string year = "") {
            try {
                checkAuth();
                QueryExamPaper model = new QueryExamPaper(user);
                model.getPersonal(year);
                return Ok(model.response);
            } catch (Exception ex) {
                return setStatusCode(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [HttpPut("Public")]
        public IActionResult setPublic(PublicExam request) {
            try {
                checkAuth();
                EditExamPaper model = new EditExamPaper(user);
                model.setPublic(request);
                return Ok(model.response);
            } catch (Exception ex) {
                return setStatusCode(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [HttpGet("Public")]
        public IActionResult getPublic([Required] string field, string content) {
            try {
                checkAuth();
                QueryExamPaper model = new QueryExamPaper(user);
                model.getPublic(field, content);
                return Ok(model.response);
            } catch (Exception ex) {
                return setStatusCode(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [HttpPut("Favorite")]
        public IActionResult setFavorite([Required] FavoriteExam request) {
            try {
                checkAuth();
                EditExamPaper model = new EditExamPaper(user);
                model.setFavorite(request);
                return Ok(model.response);
            } catch (Exception ex) {
                return setStatusCode(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [HttpGet("Preview")]
        public IActionResult getPreview([Required] string examUID) {
            try {
                checkAuth();
                QueryExamPaper model = new QueryExamPaper(user);
                model.getPreview(examUID);
                return Ok(model.response);
            } catch (Exception ex) {
                return setStatusCode(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [HttpGet("Export/Related")]
        public IActionResult getTypeSetting(string examUID = "") {
            try {
                checkAuth();
                ExportExamPaper model = new ExportExamPaper(user);
                model.related(examUID);
                return Ok(model.response);
            } catch (Exception ex) {
                return setStatusCode(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [HttpPost("Export")]
        public IActionResult startExport([Required] ExportPayload request) {
            try {
                checkAuth();
                ExportExamPaper model = new ExportExamPaper(user);
                model.start(request);
                return Ok(model.response);
            } catch (Exception ex) {
                return setStatusCode(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [HttpGet("Export")]
        public IActionResult getExport() {
            try {
                checkAuth();
                ExportExamPaper model = new ExportExamPaper(user);
                model.getExportList();
                return Ok(model.response);
            } catch (Exception ex) {
                return setStatusCode(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        /// <summary> 查詢組卷進度 </summary>
        [HttpGet("Export/{UID}")]
        public IActionResult getExport(string UID) {
            try {
                checkAuth();
                ExportExamPaper model = new ExportExamPaper(user);
                model.getExport(UID);
                return Ok(model.response);
            } catch (Exception ex) {
                return setStatusCode(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }
    }
}