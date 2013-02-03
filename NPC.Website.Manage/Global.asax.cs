using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using Fluent.Infrastructure.Log;
using NHibernate;
using log4net;

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
        protected void Application_Error(Object sender, EventArgs e)
        {
            var error = Server.GetLastError();
            if (error == null) return;
            var errorMessage = new StringBuilder();
            errorMessage.AppendLine(Request.Url.ToString());
            errorMessage.AppendLine(error.TargetSite.ToString());
            errorMessage.AppendLine(error.Message);
            errorMessage.AppendLine(error.ToString());
            errorMessage.AppendLine(error.StackTrace);
            var loggerFactory = new DefaultLoggerFactory();
            loggerFactory.GetLogger().InfoFormat(errorMessage.ToString());
            var url = UrlHelper
                .GenerateUrl("Default", "Message", "System", null,
                RouteTable.Routes, HttpContext.Current.Request.RequestContext, false);
            if (!string.Equals(Request.RawUrl, url, StringComparison.CurrentCultureIgnoreCase))
                HttpContext.Current.Response.Redirect(url + "?Message=" + error.Message, true);
        }

        protected void Application_Start()
        {
          var  log = new DefaultLoggerFactory().GetLogger();
          log.InfoFormat("日志已启动！");
            AreaRegistration.RegisterAllAreas();


            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            #region session bug fixed
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
            #endregion
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            var sessionItem = HttpContext.Current.Items[WebSessionManager.NhibernateSessionKey];
            var session = sessionItem as ISession;
            if (session != null)
            {
                session.Close();
                var log = new DefaultLoggerFactory().GetLogger();
                log.InfoFormat("Session已关闭！");
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