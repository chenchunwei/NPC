using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Fluent.Infrastructure.Log;

namespace NPC.Website.Manage
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            new DefaultLoggerFactory().GetLogger().Info("日志已启动");
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        void Application_BeginRequest(object sender, EventArgs e)
        {
            /* Fix for the Flash Player Cookie bug in Non-IE browsers.
             * Since Flash Player always sends the IE cookies even in FireFox
             * we have to bypass the cookies by sending the values as part of the POST or GET
             * and overwrite the cookies with the passed in values.
             * 
             * The theory is that at this point (BeginRequest) the cookies have not been read by
             * the Session and Authentication logic and if we update the cookies here we'll get our
             * Session and Authentication restored correctly
             */
            try
            {
                const string sessionParamName = "ASPSESSID";
                const string sessionCookieName = "ASP.NET_SESSIONID";

                if (HttpContext.Current.Request.Form[sessionParamName] != null)
                {
                    UpdateCookie(sessionCookieName, HttpContext.Current.Request.Form[sessionParamName]);
                }
                else if (HttpContext.Current.Request.QueryString[sessionParamName] != null)
                {
                    UpdateCookie(sessionCookieName, HttpContext.Current.Request.QueryString[sessionParamName]);
                }
            }
            catch (Exception)
            {
                Response.StatusCode = 500;
                Response.Write("Error Initializing Session");
            }

            try
            {
                const string authParamName = "AUTHID";
                var authCookieName = FormsAuthentication.FormsCookieName;
                if (HttpContext.Current.Request.Form[authParamName] != null)
                {
                    UpdateCookie(authCookieName, HttpContext.Current.Request.Form[authParamName]);
                }
                else if (HttpContext.Current.Request.QueryString[authParamName] != null)
                {
                    UpdateCookie(authCookieName, HttpContext.Current.Request.QueryString[authParamName]);
                }
            }
            catch (Exception)
            {
                Response.StatusCode = 500;
                Response.Write("Error Initializing Forms Authentication");
            }
        }

        void UpdateCookie(string cookieName, string cookieValue)
        {
            var cookie = HttpContext.Current.Request.Cookies.Get(cookieName);
            if (cookie == null)
            {
                cookie = new HttpCookie(cookieName);
                HttpContext.Current.Request.Cookies.Add(cookie);
            }
            cookie.Value = cookieValue;
            HttpContext.Current.Request.Cookies.Set(cookie);
        }
    }
}