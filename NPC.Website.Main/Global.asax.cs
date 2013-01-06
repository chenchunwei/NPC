using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Fluent.Infrastructure.Log;

namespace NPC.Website.Main
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
                .GenerateUrl("Default", "Message", "Home", null,
                RouteTable.Routes, HttpContext.Current.Request.RequestContext, false);
            if (!string.Equals(Request.Url.AbsolutePath, url, StringComparison.CurrentCultureIgnoreCase))
                HttpContext.Current.Response.Redirect(url + "?Message=" + error.Message, true);
        }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}