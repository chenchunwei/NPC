using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace NPC.OpenMasCallback.Host
{
    public class OpenMasConfig
    {
        public static string UrlOfSmsService
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["urlOfSmsService"]; }
        }

        public static string UrlOfMmsService
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["urlOfMmsService"]; }
        }
    }
}