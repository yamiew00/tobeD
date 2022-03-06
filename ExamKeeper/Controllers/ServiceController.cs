using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using ExamKeeper.Models;
using ExamKeeper.Presenters;
using ExamKeeper.Views;
using Microsoft.AspNetCore.Mvc;
using Utils;

namespace ExamKeeper.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceController : ControllerUtil {
        public ServiceLogger logger = ServiceLogger.Instance();

        #region -對接教學系統-
        /// <summary> 使用試題ID查詢試題明細 </summary>
        [HttpPost("CacheQuery")]
        public IActionResult cacheQuery([FromHeader] string authorization, QuestionIDPayload request) {
            try {
                if (!isService(authorization)) {
                    return setStatusCode(HttpStatusCode.Unauthorized);
                }
                ServiceQueryQuestion model = new ServiceQueryQuestion(service);
                model.queryQuestion(request);
                return Ok(model.response);
            } catch (Exception ex) {
                return setStatusCode(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        /// <summary> 複查試題明細結果 </summary>
        [HttpGet("CacheQuery")]
        public IActionResult getQuestionCache([FromHeader] string authorization, [Required] string searchKey) {
            try {
                if (!isService(authorization)) {
                    return setStatusCode(HttpStatusCode.Unauthorized);
                }
                ServiceQueryQuestion model = new ServiceQueryQuestion(service);
                model.getQuestionCache(searchKey);
                return Ok(model.response);
            } catch (Exception ex) {
                return setStatusCode(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        /// <summary> 查詢智能命題試題 </summary>
        [HttpPost("AutoPattern/Query")]
        public IActionResult AutoPatternQuery([FromHeader] string authorization, [Required] ServicePayload request) {
            try {
                if (!isService(authorization)) {
                    return setStatusCode(HttpStatusCode.Unauthorized);
                }
                ServiceQueryQuestion model = new ServiceQueryQuestion(service, request.identity);
                model.autoPatternQuery(request);
                return Ok(model.response);
            } catch (Exception ex) {
                return setStatusCode(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        /// <summary> 複查智能命題查詢結果 </summary>
        [HttpGet("AutoPattern/Cache")]
        public IActionResult AutoPatternCache([FromHeader] string authorization, [Required] string searchKey) {
            try {
                if (!isService(authorization)) {
                    return setStatusCode(HttpStatusCode.Unauthorized);
                }
                ServiceQueryQuestion model = new ServiceQueryQuestion(service);
                model.autoPatternCache(searchKey);
                return Ok(model.response);
            } catch (Exception ex) {
                return setStatusCode(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        /// <summary> 查詢系統預設卷 </summary>
        [HttpGet("ExamPaper")]
        public IActionResult getExamPaperList([FromHeader] string authorization, string year = "", string tag = "") {
            try {
                if (!isService(authorization)) {
                    return setStatusCode(HttpStatusCode.Unauthorized);
                }
                ServiceExam model = new ServiceExam(service);
                model.queryList(year, tag);
                return Ok(model.response);
            } catch (Exception ex) {
                return setStatusCode(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        /// <summary> 寫入試卷授權OTP </summary>
        [HttpPost("ExamPaper/OTP")]
        public IActionResult ExamPaperOTP([FromHeader] string authorization, [Required] ServiceOTP request) {
            try {
                if (!isService(authorization)) {
                    return setStatusCode(HttpStatusCode.Unauthorized);
                }
                ServiceExam model = new ServiceExam(service);
                model.insertOTP(request);
                return Ok(model.response);
            } catch (Exception ex) {
                return setStatusCode(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        /// <summary> 建立自主練習 </summary>
        [HttpPost("Practice/Create")]
        public IActionResult Create([FromHeader] string authorization, ServicePractice request) {
            try {
                if (!isService(authorization)) {
                    return setStatusCode(HttpStatusCode.Unauthorized);
                }
                PracticePresenter presenter = new PracticePresenter();
                presenter.create(service, request);
                return Ok(presenter.response);
            } catch (Exception ex) {
                return setStatusCode(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        /// <summary> 查詢自主練習列表 </summary>
        [HttpGet("Practice/Query")]
        public IActionResult Query([FromHeader] string authorization) {
            try {
                if (!isService(authorization)) {
                    return setStatusCode(HttpStatusCode.Unauthorized);
                }
                PracticePresenter presenter = new PracticePresenter();
                presenter.query(service);
                return Ok(presenter.response);
            } catch (Exception ex) {
                return setStatusCode(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        #endregion

        #region -線上測驗-
        /// <summary> 查詢試卷明細 </summary>
        /// <param name="UID">試卷UID</param>
        /// <param name="otp">授權碼</param>
        /// <param name="user">授權使用者</param>
        /// <param name="authorUID">出卷者UID</param>
        [HttpGet("ExamPaper/{UID}")]
        public IActionResult getExamPaper([FromHeader] string authorization, string UID, string otp = "", string user = "", Guid? authorUID = null) {
            try {
                if (!isService(authorization)) {
                    return setStatusCode(HttpStatusCode.Unauthorized);
                }
                ServiceExam model = new ServiceExam(service);
                model.queryExamPaper(UID, otp, user, authorUID);
                return Ok(model.response);
            } catch (Exception ex) {
                return setStatusCode(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        /// <summary> 查詢自主練習明細 </summary>
        [HttpGet("Practice/Info")]
        public IActionResult QueryInfos([FromHeader] string authorization, string UID) {
            try {
                if (!isService(authorization)) {
                    return setStatusCode(HttpStatusCode.Unauthorized);
                }
                PracticePresenter presenter = new PracticePresenter();
                presenter.query(service, UID);
                return Ok(presenter.response);
            } catch (Exception ex) {
                return setStatusCode(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        /// <summary> 封存試卷 </summary>
        [HttpPut("ExamPaper/Lock/{examUID}")]
        public IActionResult setLock([FromHeader] string authorization, string examUID) {
            try {
                if (!isService(authorization)) {
                    return setStatusCode(HttpStatusCode.Unauthorized);
                }
                ServiceExam model = new ServiceExam(service);
                model.setLock(examUID);
                return Ok(model.response);
            } catch (Exception ex) {
                return setStatusCode(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        /// <summary> 回寫測驗完成紀錄 </summary>
        [HttpPost("ExamResult")]
        public IActionResult setExamResult([Required] string otp, ExamNotice request) {
            try {
                PracticeExam model = new PracticeExam(null, string.Empty);
                return Ok(model.setExamNotice(otp, request));
            } catch (Exception ex) {
                return setStatusCode(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        #endregion

        #region -組卷程式-
        /// <summary> 取得一筆待組試卷 </summary>
        [HttpGet("ExamExport")]
        public IActionResult getExamExport([FromHeader] string authorization) {
            Response<object> response = null;
            try {
                if (!isService(authorization)) {
                    return setStatusCode(HttpStatusCode.Unauthorized);
                }
                ServiceExam model = new ServiceExam(service);
                model.getWaiting();
                response = model.response;
                return Ok(response);
            } catch (Exception ex) {
                return setStatusCode(HttpStatusCode.InternalServerError, ex.ToString());
            } finally {
                logger.insertPayloadLog(authorization, "GetExamExport", service, response);
            }
        }

        /// <summary> 寫入組卷進度 </summary>
        [HttpPost("ExportStatus")]
        public IActionResult setExportStatus([FromHeader] string authorization, ExportStatusPayload request) {
            Response<object> response = null;
            try {
                if (!isService(authorization)) {
                    return setStatusCode(HttpStatusCode.Unauthorized);
                }
                ServiceExam model = new ServiceExam(service);
                model.setStatus(request);
                response = model.response;
                return Ok(response);
            } catch (Exception ex) {
                return setStatusCode(HttpStatusCode.InternalServerError, ex.ToString());
            } finally {
                logger.insertPayloadLog(authorization, "UpdateExport", request, response);
            }
        }
        #endregion
    }
}