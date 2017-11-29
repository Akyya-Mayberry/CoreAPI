using System.Threading.Tasks;
using AutoMapper;
using cetpaApi.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Services;
using Microsoft.Extensions.Logging;
using Models.Base;
using Microsoft.AspNetCore.Authorization;
using Models.Data;
using Models.View;
using Services.Helpers;

namespace cetpaApi.Controllers
{
    [Route("api/[controller]/[action]")]
    public class UserController : BaseController
    {
        #region Contructors

        private readonly IUserService _userService;
        private readonly IConfiguration _config;
        private readonly ILogger _logger;

        public UserController(IConfiguration config,
            IUserService userService,
            ILogger<UserController> logger)
        {
            _config = config;
            _userService = userService;
            _logger = logger;
        }

        #endregion


        #region Get Lists

        [HttpGet]
        public JsonResponse GetCounties()
        {
            var resp = new JsonResponse
            {
                id = 0,
                message = string.Empty,
                error = false,
                token = string.Empty,
                data = _userService.GetCounties()
            };
            return resp;
        }

        #endregion


        #region Get Singles

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Role(IdentityRoles.Administrator, IdentityRoles.County)]
        public User GetUser()
        {
            int userId = CurrentUser.UserID;
            return _userService.GetUser(userId);
        }

        #endregion


        #region Login/Logout

        [HttpPost, AllowAnonymous]
        public async Task<JsonResponse> SignIn([FromBody] SignIn signin)
        {
            signin.returnUrl = UtilityHelper.DefaultVal(signin.returnUrl, "/home");
            var check = _userService.CheckSignIn(signin, HttpHelper.GetAbsoluteUrl(Request));
            var error = check.userId == 0;
            var message = check.message;
            var token = string.Empty;
            var userSummary = new UserView();

            if (check.userId > 0)
            {
                var user = _userService.GetUser(check.userId);
                token = await _userService.SetSignInAuth(HttpContext, user, signin.rememberMe);
                signin.returnUrl = check.url;

                var config = new MapperConfiguration(cfg => cfg.CreateMap<User, UserView>());
                var mapper = config.CreateMapper();
                userSummary = mapper.Map<UserView>(user);
            }
            var resp = new JsonResponse
            {
                id = check.userId,
                message = message,
                error = error,
                returnUrl = signin.returnUrl,
                token = token,
                data = userSummary
            };
            return resp;
        }

        [HttpPost]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(Config.AppName);
            return Redirect(Config.BaseUrl);
        }

        #endregion
    }
}