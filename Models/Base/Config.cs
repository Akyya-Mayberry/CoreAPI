namespace Models.Base
{
    public class Config
    {
        public const string BaseUrl = "https://www.somedomain.com";
        public const string SqlConnection = "Server=tcp:ttsc-prodsql-west.database.windows.net,1433;Initial Catalog=cetpa2017;Persist Security Info=False;User ID=cetpa;Password=Conference2017!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public const string AppName = "CetpaAPI";
        public static string JwtKey = "A2396491F8DD9E9787B7B9FC3EAEC";
    }
}

