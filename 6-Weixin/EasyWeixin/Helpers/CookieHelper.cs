using System;
using System.Web;

namespace EasyWeixin.Helpers
{
    public static class CookieHelper
    {
        public static void ChangeCookie(HttpRequest Request, HttpResponse Response, string name, string value, int day, int hours, int minutes, int seconds)
        {
            HttpCookie cookie = Request.Cookies[name];
            TimeSpan ts = new TimeSpan(day, hours, minutes, seconds);
            cookie.Expires = DateTime.Now.Add(ts);
            cookie.Value = value;
            Response.AppendCookie(cookie);
        }

        public static void DeleteCookie(HttpRequest Request, HttpResponse Response, string name, string value)
        {
            HttpCookie cookie = new HttpCookie(name);
            cookie.Expires = DateTime.Now.AddDays(-1D);
            cookie.Value = value;
            Response.AppendCookie(cookie);
        }

        public static string GetCookie(HttpRequestBase Request, string name)
        {
            if (Request.Cookies[name] != null)
            {
                return Request.Cookies[name].Value;
            }
            else
            {
                return "";
            }
        }

        public static void AddCookie(HttpResponseBase Response, string name, string value, int day, int hours, int minutes, int seconds)
        {
            HttpCookie cookie = new HttpCookie(name);
            TimeSpan ts = new TimeSpan(day, hours, minutes, seconds);
            cookie.Expires = DateTime.Now.Add(ts);
            cookie.Value = value;
            Response.AppendCookie(cookie);
        }
    }
}