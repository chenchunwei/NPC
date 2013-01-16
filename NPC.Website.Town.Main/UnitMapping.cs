using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NPC.Website.Town.Main
{
    public class UnitMapping
    {
        private static Guid? GetUintId()
        {
            var hostName = HttpContext.Current.Request.Url.Host.ToLower();
            if (System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains(hostName))
            {
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