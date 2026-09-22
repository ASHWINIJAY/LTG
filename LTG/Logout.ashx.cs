using System;
using System.Web;

namespace LTG
{
    public class Logout : IHttpHandler
    {
        public bool IsReusable { get { return false; } }

        public void ProcessRequest(HttpContext context)
        {
            int userId;
            HttpCookie loginCookie = context.Request.Cookies["LoginId"];
            if (loginCookie != null && int.TryParse(loginCookie.Value, out userId))
                KpiData.RecordLogout(userId);

            foreach (string name in new[] { "LoginId", "Username", "loginRole", "firstName" })
            {
                HttpCookie cookie = new HttpCookie(name, string.Empty);
                cookie.Expires = DateTime.Now.AddDays(-1);
                context.Response.Cookies.Add(cookie);
            }

            context.Response.Redirect("Login");
        }
    }
}