using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fluent.Infrastructure.Web.HttpMoudles;
using Fluent.Infrastructure.Web.Utilities;

namespace NPC.Website.Manage.Internals
{
    public class CustomAuthenticationHandler : IAuthenticationHandler
    {
        public void OnLoginStart()
        {
            return;
        }

        public void OnLoginEnd(LoginEntity loginEntity)
        {
            if (loginEntity.ReferrerUrl.IndexOf("manage", StringComparison.CurrentCultureIgnoreCase) < 0)
            {
                if (!loginEntity.IsLoginSuccess)
                {
                    var host = HttpContext.Current.Request.Url.Host;
                    HttpContext.Current.Response.Redirect("http://" + host + "/Home/Message?message=" + HttpUtility.UrlEncode("用户名或密码不正确!"));
                }
                else
                {
                    HttpContext.Current.Response.Redirect(WebUtilities.GetRelativePathWithApplicationHost("~/Users/Interactive"));
                }
            }

        }

        public void OnLogoutBegin()
        {
            return;
        }

        public void OnLogoutEnd()
        {
            return;
        }
    }
}