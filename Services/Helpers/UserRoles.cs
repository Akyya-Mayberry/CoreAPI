using Models.Base;

namespace Services.Helpers
{
    public class UserRoles
    {
        public static string GetRole(string userType)
        {
            var role = string.Empty;
            switch (userType.ToLower())
            {
                case "admin":
                    role = IdentityRoles.Administrator;
                    break;
                case "county":
                    role = IdentityRoles.County;
                    break;
                case "district":
                    role = IdentityRoles.District;
                    break;
            }
            return role;
        }

    }
}
