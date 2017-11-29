namespace Models.Base
{
    public class JsonResponse
    {
        public int id { get; set; }
        public string message { get; set; }
        public bool error { get; set; }
        public string returnUrl { get; set; }
        public object data { get; set; }
        public string token { get; set; }
    }
}
