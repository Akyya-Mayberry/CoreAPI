using System;
using System.Security.Claims;

namespace Services.Helpers
{
    public class UserClaimsPrincipal : ClaimsPrincipal
    {
        public UserClaimsPrincipal(ClaimsPrincipal principal)
        : base(principal)
        {
        }

        public int UserID
        {
            get
            {
                return UtilityHelper.DefaultVal(Convert.ToInt32(FindFirst(ClaimTypes.NameIdentifier).Value), 0);
            }
        }

        public string Email
        {
            get
            {
                return UtilityHelper.DefaultVal(FindFirst(ClaimTypes.Email).Value, string.Empty);
            }
        }

        public string Name
        {
            get
            {
                return UtilityHelper.DefaultVal(FindFirst(ClaimTypes.Name).Value, string.Empty);
            }
        }

        public int CountyID
        {
            get
            {
                return UtilityHelper.DefaultVal(Convert.ToInt32(FindFirst("countyId").Value), 0);
            }
        }

        public string Role
        {
            get
            {
                return UtilityHelper.DefaultVal(FindFirst(ClaimTypes.Role).Value, string.Empty);
            }
        }

        public DateTime DateAdded
        {
            get
            {
                return UtilityHelper.DefaultVal(Convert.ToDateTime(FindFirst("dateAdded").Value), Convert.ToDateTime("1900-01-01"));
            }
        }
    }
}
