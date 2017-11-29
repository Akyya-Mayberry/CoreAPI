using System;
using DataAccess;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Models.Base;
using Models.Data;
using Models.View;
using Services.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Services
{
    public interface IUserService
    {
        IEnumerable<County> GetCounties();
        User GetUser(int id);
        SignInResponse CheckSignIn(SignIn signin, string url);
        Task<string> SetSignInAuth(HttpContext context, User user, bool rememberMe);
    }
    public class UserService: IUserService
    {

        #region Constructors

        private UserContext _context;

        public UserService(UserContext context)
        {
            _context = context;
        }

        #endregion

        public IEnumerable<County> GetCounties()
        {
            return _context.lkpCounties
                .OrderBy(u => u.name);
        }

        public User GetUser(int id)
        {
            return _context.tblUsers.SingleOrDefault(u => u.userId == id);
        }

        public SignInResponse CheckSignIn(SignIn signin, string url)
        {
            var response = new SignInResponse
            {
                userId = 0,
                message = string.Empty,
                url = signin.returnUrl
            };
            try
            {
                //check username/password
                var user = _context.tblUsers.SingleOrDefault(
                    u => u.email.ToLower() == signin.email.ToLower()
                         && u.password == signin.password);
                if (user == null)
                {
                    throw new Exception("Invalid sign-in email/password");
                }
                response.userId = user.userId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return response;
        }

        public async Task<string> SetSignInAuth(HttpContext context, User user, bool rememberMe)
        {
            try
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.firstName + " " + user.lastName),
                    new Claim(ClaimTypes.Email, user.email),
                    new Claim(ClaimTypes.NameIdentifier, user.userId.ToString()),
                    new Claim(ClaimTypes.Role, user.role),
                    new Claim("countyId", user.countyId.ToString()),
                    new Claim("dateAdded", UtilityHelper.FormatDbDate(user.dateAdded))
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config.JwtKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(Config.AppName,
                  Config.AppName,
                  claims,
                  expires: rememberMe ? DateTime.UtcNow.AddDays(30) : DateTime.UtcNow.AddHours(4),
                  signingCredentials: creds);

                var identity = new ClaimsIdentity(claims, Config.AppName);
                var principal = new ClaimsPrincipal(identity);
                var properties = new AuthenticationProperties
                {
                    ExpiresUtc = rememberMe ? DateTime.UtcNow.AddDays(30) : DateTime.UtcNow.AddHours(4),
                    IsPersistent = true,
                    AllowRefresh = true
                };

                await context.SignInAsync(Config.AppName, principal, properties);
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
