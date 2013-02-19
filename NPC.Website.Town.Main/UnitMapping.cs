using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fluent.Infrastructure.Log;

namespace NPC.Website.Town.Main
{
    public class UnitMapping
    {
        private static Guid? GetUintId()
        {
            var hostName = HttpContext.Current.Request.Url.Host.ToLower();
            var log = new DefaultLoggerFactory().GetLogger();
            log.DebugFormat("访问的host:{0}", hostName);
            if (System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains(hostName))
            {
                log.DebugFormat("对应的id:{0}", System.Configuration.ConfigurationManager.AppSettings[hostName]);
                return Guid.Parse(System.Configuration.ConfigurationManager.AppSettings[hostName]);
            }
            return default(Guid?);
        }
        public static Guid? UnitId
        {
            get { return GetUintId(); }
        }
    }
}