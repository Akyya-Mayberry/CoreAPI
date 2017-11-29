using Microsoft.AspNetCore.Http;
using System;

namespace cetpaApi.Helpers
{
    public class HttpHelper
    {
        public static Uri GetAbsoluteUri(HttpRequest request)
        {
            UriBuilder uriBuilder = new UriBuilder();
            uriBuilder.Scheme = request.Scheme;
            uriBuilder.Host = request.Host.ToString();
            uriBuilder.Path = request.Path.ToString();
            uriBuilder.Query = request.QueryString.ToString();
            return uriBuilder.Uri;
        }

        public static string GetAbsoluteUrl(HttpRequest request)
        {
            UriBuilder uriBuilder = new UriBuilder();
            return request.Host.ToString().ToLower();
        }

        public static string GetAbsolutePath(HttpRequest request)
        {
            UriBuilder uriBuilder = new UriBuilder();
            return request.Path.ToString();
        }

        public static string GetQueryString(HttpRequest request)
        {
            UriBuilder uriBuilder = new UriBuilder();
            return request.QueryString.ToString();
        }

        public static string GetIpAddress(HttpRequest request)
        {
            string ip = string.Empty;
            try
            {
                var _ip = request.HttpContext.Connection.RemoteIpAddress;
                if (_ip != null)
                {
                    ip = _ip.ToString();
                }
            }
            catch
            {
            }
            return ip;
        }

        public static string GetReferer(HttpRequest request)
        {
            string referer = string.Empty;
            try
            {
                referer = request.Headers["Referer"];
                if (!string.IsNullOrEmpty(referer))
                {
                    referer = referer.ToLower();
                }
            }
            catch
            {
            }
            return referer;
        }

        public static string GetBrowser(HttpRequest request)
        {
            string browser = string.Empty;
            try
            {
                browser = request.Headers["User-Agent"];
                if (!string.IsNullOrEmpty(browser))
                {
                    browser = browser.ToLower();
                }
            }
            catch
            {
            }
            return browser;
        }

        public static string GetPlatform(HttpRequest request)
        {
            var ua = request.Headers["User-Agent"].ToString().ToLower();

            if (ua.Contains("rim tablet") || (ua.Contains("bb") && ua.Contains("mobile")))
                return "Black Berry";

            if (ua.Contains("windows phone"))
                return "Windows Phone";

            if (ua.Contains("linux") && ua.Contains("kfapwi"))
                return "Kindle Fire";

            if (ua.Contains("android"))
                return "Android";

            if (ua.Contains("ipad"))
                return "iPad";

            if (ua.Contains("iphone"))
                return "iPhone";

            if (ua.Contains("mac os"))
                return "Mac OS";

            if (ua.Contains("windows nt 5.1") || ua.Contains("windows nt 5.2"))
                return "Windows XP";

            if (ua.Contains("windows nt 6.0"))
                return "Windows Vista";

            if (ua.Contains("windows nt 6.1"))
                return "Windows 7";

            if (ua.Contains("windows nt 6.2"))
                return "Windows 8";

            if (ua.Contains("windows nt 6.3"))
                return "Windows 8.1";

            if (ua.Contains("windows nt 10"))
                return "Windows 10";

            //fallback 
            return "Undetected";
        }
    }
}
