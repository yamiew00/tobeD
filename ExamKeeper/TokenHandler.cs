using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using ExamKeeper.JerryH.JResponse;
using ExamKeeper.JerryH.JUsers;
using ExamKeeper.JerryH.Jutils;
using ExamKeeperClassLibrary;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace ExamKeeper
{
    public class MemberAuthentication : AuthenticationSchemeOptions
    {

    }

    public class TokenHandler : AuthenticationHandler<MemberAuthentication>
    {
        /// <summary>
        /// 設定檔
        /// </summary>
        private readonly IConfiguration Configuration;

        public TokenHandler(
            IConfiguration configuration, 
            IOptionsMonitor<MemberAuthentication> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock) 
        {
            Configuration = configuration;
        }

        /// <summary>
        /// CMS的會員驗證，會將jwt轉換結果存在Items裡面
        /// </summary>
        /// <returns></returns>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            //檢查標頭是否有Authorization
            if (!Context.Request.Headers.TryGetValue("Authorization", out StringValues token))
            {
                return AuthenticateResult.NoResult();
            }
            
            try
            {
                //要打CMS API拿成員資料
                var url = Configuration.GetConnectionString("JAPIs:CMS:Members") + "Member/UserProfile";
                var cmsResponse = await APISender.Create()
                                                 .AddUri(url)
                                                 .AddAuthorization(token)
                                                 .GetAsync<CMSResponseBody<JUserProfile>>();

                if (cmsResponse.IsSuccess)
                {
                    //將驗證結果存放至Items。
                    Request.HttpContext.Items["JUser"] = cmsResponse.Data;
                    //Configuration帶進Items
                    Request.HttpContext.Items["Config"] = new ExamKeeperConfig(Configuration);

                    var claim2 = new ClaimsPrincipal(new ClaimsIdentity[] {
                            new ClaimsIdentity(
                                new Claim[]
                                {
                                    new Claim(ClaimsIdentity.DefaultNameClaimType, "everything is ok.")
                                },
                                "ExamKeeperAuthenticationType" // 必須要加入authenticationType，否則會被作為未登入
                            )
                        });

                    return AuthenticateResult.Success(new AuthenticationTicket(claim2, "ExamKeeperAuthenticationType"));
                }
            }
            catch(Exception ex)
            {
                //註: AuthenticateResult.Fail 不會正確的回傳message，只有給401的作用
                return AuthenticateResult.Fail(string.Empty);
            }

            //註: AuthenticateResult.Fail 不會正確的回傳message，只有給401的作用
            return AuthenticateResult.Fail(string.Empty);
        }
    }
}