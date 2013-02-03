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

        public static Guid NpcAuditJieKouRenUnitId
        {
            get
            {
                return Guid.Parse(System.Configuration.ConfigurationManager.AppSettings["NpcAuditJieKouRenUnitId"]);
            }
        }

        public static Guid GovAuditJieKouRenUnitId
        {
            get
            {
                return Guid.Parse(System.Configuration.ConfigurationManager.AppSettings["GovAuditJieKouRenUnitId"]);
            }
        }
    }
}
