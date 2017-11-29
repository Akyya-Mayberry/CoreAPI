using System;

namespace Models.View
{
    public class UserView
    {
        public int userId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public int countyId { get; set; }
        public DateTime dateAdded { get; set; }
        public string role { get; set; }
    }

    public class SignIn
    {
        public string email { get; set; }
        public string password { get; set; }
        public bool rememberMe { get; set; }
        public string returnUrl { get; set; }
    }

    public class SignInResponse
    {
        public int userId { get; set; }
        public string message { get; set; }
        public string url { get; set; }
    }
}
