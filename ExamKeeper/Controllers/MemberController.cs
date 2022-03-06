using System;
using System.Net;
using System.Threading.Tasks;
using ExamKeeper.Presenters;
using ExamKeeper.Views;
using ExamKeeperClassLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamKeeper.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class MemberController : ControllerUtil
    {
        private readonly DefinitionLibrary DefinitionLibrary;

        public MemberController(DefinitionLibrary definitionLibrary)
        {
            DefinitionLibrary = definitionLibrary;
        }

        /// <summary>
        /// 去CMS登錄並拿CMS回傳的token再回傳出去
        /// </summary>
        /// <param name="authorization"></param>
        /// <returns></returns>
        [HttpGet("Login")]
        public IActionResult Login([FromHeader] string authorization)
        {
            try
            {
                Member model = new Member();
                model.login(authorization);
                
                return Ok(model.response);
            }
            catch (Exception ex)
            {
                return setStatusCode(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [Authorize]
        [HttpGet("UserProfile")]
        public IActionResult getUser()
        {
            try
            {
                checkAuth();
                Member model = new Member();
                model.get(user);
                return Ok(model.response);
            }
            catch (Exception ex)
            {
                return setStatusCode(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [Authorize]
        [HttpGet("EduSubject")]
        public async Task<IActionResult> getMainMenu()
        {
            try
            {
                
                Member model = new Member();
                model.getMainMenu(user, DefinitionLibrary);
                return Ok(model.response);
            }
            catch (Exception ex)
            {
                return setStatusCode(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [Authorize]
        [HttpPost("Preference")]
        public IActionResult Preference(Preference preference)
        {
            try
            {
                checkAuth();
                EditMember model = new EditMember();
                model.savePreference(user, preference);
                return Ok(model.response);
            }
            catch (Exception ex)
            {
                return setStatusCode(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [Authorize]
        [HttpGet("Typesetting/Related")]
        public IActionResult ExamTypesetting()
        {
            try
            {
                checkAuth();
                Member model = new Member();
                model.getTypesetting(user);
                return Ok(model.response);
            }
            catch (Exception ex)
            {
                return setStatusCode(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [Authorize]
        [HttpPost("Typesetting")]
        public IActionResult Typesetting(Typesetting setting)
        {
            try
            {
                checkAuth();
                EditMember model = new EditMember();
                model.saveTypesetting(user, setting);
                return Ok(model.response);
            }
            catch (Exception ex)
            {
                return setStatusCode(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }
    }
}