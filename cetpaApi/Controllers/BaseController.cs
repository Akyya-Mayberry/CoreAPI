using Microsoft.AspNetCore.Mvc;
using Services.Helpers;
using System.Security.Claims;

namespace cetpaApi.Controllers
{
    public class BaseController : Controller
    {
        public UserClaimsPrincipal CurrentUser
        {
            get
            {
                return new UserClaimsPrincipal(User as ClaimsPrincipal);
            }
        }
    }
}
