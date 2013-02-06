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

        public static string SmtpServer
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["SmtpServer"];
            }
        }

        public static string SmtpUserName
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["SmtpUserName"];
            }
        }

        public static string SmtpPassword
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["SmtpPassword"];
            }
        }
        public static string SmtpDomain
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["SmtpDomain"];
            }
        }
        public static string SmtpPort
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["SmtpPort"];
            }
        }
        public static string ContributeSendTo
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["ContributeSendTo"];
            }
        }
    }
}
