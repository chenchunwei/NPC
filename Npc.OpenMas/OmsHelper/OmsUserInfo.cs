using System.Globalization;
using System.IO;
using System.Xml;
using System.Text;

namespace Npc.OpenMas.OmsHelper
{
    /// <summary>
    ///OMSUserInfo 的摘要说明
    /// </summary>
    public class OmsUserInfo
    {
        /// <summary>
        /// 返回值代码
        /// </summary>
        public ErrorCode Code
        {
            get;
            set;
        }

        ///<summary>
        ///</summary>
        public SeverityType Severity
        {
            get;
            set;
        }

        /// <summary>
        /// replyPhone 元素包含用户用来向服务提供商注册服务的移动电话号码
        /// replyPhone 值作为用户的默认移动电话号码显示在 Outlook 的帐户设置对话框中
        /// </summary>
        public string ReplyPhone
        {
            get;
            set;
        }

        /// <summary>
        /// smtpAddress 元素包含服务提供商为每个订户生成的唯一 SMTP 地址
        /// 服务提供商使用该 SMTP 地址将回复信息从移动电话发送回 Outlook 2007。
        /// 为防止恶意用户使用该 SMTP 地址发送垃圾电子邮件，OMS 服务提供商需要为每个订户创建唯一的 SMTP 地址。
        /// </summary>
        public string SmtpAddress
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var s = new StringWriter(new StringBuilder(), CultureInfo.CurrentCulture);
            var xmlWrite = new XmlTextWriter(s);

            xmlWrite.WriteStartDocument();
            xmlWrite.WriteStartElement("userInfo");
            xmlWrite.WriteAttributeString("xmlns", "http://schemas.microsoft.com/office/Outlook/2006/OMS/serviceInfo");

            xmlWrite.WriteStartElement("error");
            xmlWrite.WriteAttributeString("code", this.Code.ToString());

            if (this.Severity != SeverityType.Unknow)
            {
                xmlWrite.WriteAttributeString("severity", this.Severity.ToString());
            }
            xmlWrite.WriteEndElement();

            if (this.Code == ErrorCode.Ok)
            {
                if (!string.IsNullOrEmpty(this.ReplyPhone))
                {
                    xmlWrite.WriteElementString("replyPhone", this.ReplyPhone);
                }
                if (!string.IsNullOrEmpty(this.ReplyPhone))
                {
                    xmlWrite.WriteElementString("smtpAddress", this.SmtpAddress);
                }
            }

            xmlWrite.WriteEndElement();
            xmlWrite.WriteEndDocument();
            return s.GetStringBuilder().ToString();
        }
    }
}