using Microsoft.AspNetCore.Authorization;

namespace cetpaApi.Helpers
{
    public class RoleAttribute : AuthorizeAttribute
    {
        public RoleAttribute(params string[] roles) : base()
        {
            Roles = string.Join(",", roles);
        }
    }
}
