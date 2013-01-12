using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC.Application.Common
{
    public class AppConfig
    {
        public static string AttachmentsPath
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["AttachmentsPath"];
            }
        }
    }
}
