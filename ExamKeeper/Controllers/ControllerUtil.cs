using System;
using System.Linq;
using System.Net;
using System.Text.Json;
using ExamKeeper.JerryH.JUsers;
using ExamKeeper.Models;
using ExamKeeper.Views;
using ExamKeeperClassLibrary;
using ExamKeeperClassLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Utils;
using Utils.Setting;

namespace ExamKeeper.Controllers
{
    public class ControllerUtil : ControllerBase
    {
        protected ExamKeeperConfig Config => (ExamKeeperConfig)Request.HttpContext.Items["Config"];

        /// <summary>
        /// (新版本用戶資訊)來自CMS資料庫的使用者資料。
        /// pass value with Request.HttpContext.Items
        /// </summary>
        protected JUserProfile JUser => (JUserProfile)Request.HttpContext.Items["JUser"];

        /// <summary>
        /// (待棄用)使用者資料。
        /// </summary>
        public UserProfile user
        {
            get
            {
                var juser = (JUserProfile)Request.HttpContext.Items["JUser"];
                var jOrganization = juser.Organization;
                return new UserProfile()
                {
                    UID = juser.UID,
                    account = juser.Account,
                    email = juser.Email,
                    identity = juser.Identity,
                    lastLogin = juser.LastLogin,
                    name = juser.Name,
                    organization = new AttributeMap(code: jOrganization.Code, name: jOrganization.Name, attrType: jOrganization.Type),
                    status = juser.Status,
                    usetype = juser.UseType
                };
            }
        }


        private string authMessage { get; set; }
        //public UserProfile user { get; private set; }
        public ServiceProfile service { get; private set; }
        protected IActionResult setStatusCode(HttpStatusCode type, string message = "")
        {
            switch (type)
            {
                case HttpStatusCode.BadRequest:
                    if (string.IsNullOrWhiteSpace(message))
                    {
                        message = payloadMessage();
                    }
                    return BadRequest(message);
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.Forbidden:
                    return StatusCode((int)type, authMessage);
                case HttpStatusCode.InternalServerError:
                default:
                    return StatusCode((int)type, message);
            }
        }
        protected string payloadMessage()
        {
            return String.Join(Environment.NewLine,
                ModelState.Where(x => x.Value.Errors.Count > 0).ToDictionary(k => k.Key, k => k.Value.Errors.Select(e => e.ErrorMessage).ToArray()));
        }

        protected bool checkAuth()
        {
            authMessage = "Check User Auth";
            //getUserProfile();
            return user != null;
        }

        //protected void getUserProfile() {
        //    string profileString = User?.Identity?.Name ?? string.Empty;
        //    if (!string.IsNullOrWhiteSpace(profileString)) {
        //        user = JsonSerializer.Deserialize<UserProfile>(User.Identity.Name);
        //    }
        //}

        protected bool isService(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                authMessage = "Token Required.";
                return false;
            }
            CMSSingleton CMS = CMSSingleton.Instance(new InitMongoLog("ControllerUtil"));
            service = CMS.getServiceProfile(token);
            if (service == null)
            {
                authMessage = "not access service";
                return false;
            }
            return true;
        }
    }
}