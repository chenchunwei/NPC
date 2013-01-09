using System.Globalization;
using System.IO;
using System.Xml;
using System.Text;

namespace Npc.OpenMas.OmsHelper
{
    /// <summary>
    ///OMSServiceInfo 的摘要说明
    /// </summary>
    public class OmsServiceInfo
    {
        /// <summary>
        /// 服务提供商的公司名称，会显示在 Outlook 2007 的 OMS 帐户设置用户界面 (UI) 中
        /// </summary>
        public string ServiceProvider { get; set; }

        /// <summary>
        /// Web 服务的统一资源标识符 (URI)，是该 Web 服务的唯一 ID。
        /// 如果 OMS Web 服务的 serviceUri 发生更改，则 OMS 客户端会将该服务视为新的 OMS Web 服务
        /// </summary>
        public string ServiceUri { get; set; }

        /// <summary>
        /// OMS Web 服务的注册或登录页面的 URI。
        /// </summary>
        public string SignUpPage { get; set; }

        /// <summary>
        /// 区域设置 ID (LCID)。用于说明 Web 服务的目标国家或区域（例如 2052 表示简体中文，1042 表示韩语）。
        /// </summary>
        public string TargetLocale { get; set; }

        /// <summary>
        /// 以本地语言显示的服务名称。
        /// 当 targetLocale 与 Office UI 语言相同时，OMS 帐户设置 UI 中会显示此名称。否则使用 englishName。
        /// 默认帐户名称由 localName或 englishName，然后加上 userInfo 字符串的 replyPhone 元素构成（参见本文中 userInfo 字符串引用），
        /// 格式为 replyPhone (localName)。
        /// </summary>
        public string LocalName { get; set; }

        /// <summary>
        /// 以英文显示的服务名称。当 targetLocale 与 Office UI 语言不同时，OMS 帐户设置 UI 中会显示此名称。
        /// </summary>
        public string EnglishName { get; set; }

        /// <summary>
        /// 用于指明 Web 服务支持的身份验证方法。
        /// 支持的值有“passport”和“other”。
        /// 如果不支持“passport”，则使用“other”，这说明用户只要使用普通的用户 ID 和密码即可通过验证。
        /// </summary>
        public AuthenticationType Authentication { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SupportedServiceInfo SupportedService;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var s = new StringWriter(new StringBuilder(), CultureInfo.CurrentCulture);
            var xmlWrite = new XmlTextWriter(s);
            xmlWrite.WriteStartDocument();
            xmlWrite.WriteStartElement("ServiceInfo");
            xmlWrite.WriteAttributeString("xmlns", "http://schemas.microsoft.com/office/Outlook/2006/OMS/serviceInfo");

            xmlWrite.WriteElementString("serviceProvider", this.ServiceProvider);
            xmlWrite.WriteElementString("serviceUri", this.ServiceUri);
            xmlWrite.WriteElementString("signUpPage", this.SignUpPage);
            xmlWrite.WriteElementString("targetLocale", this.TargetLocale);
            xmlWrite.WriteElementString("localName", this.LocalName);
            xmlWrite.WriteElementString("englishName", this.EnglishName);
            xmlWrite.WriteElementString("authenticationType", this.Authentication.ToString());

            SupportedService.WriteXml(xmlWrite);

            xmlWrite.WriteEndElement();
            xmlWrite.WriteEndDocument();

            return s.GetStringBuilder().ToString();
        }
    }

    /// <summary>
    /// 支持的服务元素的父标记
    /// </summary>
    public struct SupportedServiceInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public SmsSenderInfo SmsSender;
        /// <summary>
        /// 
        /// </summary>
        public MmsSenderInfo MmsSender;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlWrite"></param>
        public void WriteXml(XmlTextWriter xmlWrite)
        {
            xmlWrite.WriteStartElement("supportedService");

            SmsSender.WriteXml(xmlWrite);
            MmsSender.WriteXml(xmlWrite);

            xmlWrite.WriteEndElement();
        }
    }

    /// <summary>
    /// 支持的服务类型(短信)
    /// </summary>
    public struct SmsSenderInfo
    {
        /// <summary>
        /// 表示一条 SMS 信息所允许的收件人数
        /// 服务端负责检查收件人数是否超出了服务设定的限制。如果客户端超出该限制，Web 服务应使用“other”错误代码返回错误
        /// </summary>
        public int MaxRecipientsPerMessage;

        /// <summary>
        /// 表示在一个 xmsData 字符串中允许存在的不同 SMS 信息的数量
        /// 请注意，OMS 客户端支持的最大值为 20。但是有一个例外，即当 xmsData 字符串的 sourceType 元素为“calSummary”时，
        /// 一个 xmsData 字符串中最多可以包含 50 条不同的信息
        /// 建议的 maxMessagesPerSend 值为 20
        /// Web 服务负责检查一个 xmsData 字符串中包含的信息数量是否超出了 Web 服务设定的限制。
        /// 如果 OMS 客户端超出该限制，Web 服务应使用“other”错误代码返回错误。
        /// </summary>
        public int MaxMessagesPerSend;

        /// <summary>
        /// 表示一条只包含 US ASCII 字符的 SMS 信息所允许的字符数
        /// </summary>
        public int MaxSbcsPerMessage;

        /// <summary>
        /// 表示一条包含双字节字符的 SMS 信息所允许的字符数
        /// </summary>
        public int MaxDbcsPerMessage;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxRecipientsPerMessage"></param>
        /// <param name="maxMessagesPerSend"></param>
        /// <param name="maxSbcsPerMessage"></param>
        /// <param name="maxDbcsPerMessage"></param>
        public SmsSenderInfo(int maxRecipientsPerMessage, int maxMessagesPerSend, int maxSbcsPerMessage,
                             int maxDbcsPerMessage)
        {
            MaxRecipientsPerMessage = maxRecipientsPerMessage;
            MaxMessagesPerSend = maxMessagesPerSend;
            MaxSbcsPerMessage = maxSbcsPerMessage;
            MaxDbcsPerMessage = maxDbcsPerMessage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlWrite"></param>
        public void WriteXml(XmlTextWriter xmlWrite)
        {
            xmlWrite.WriteStartElement("SMS_SENDER");

            xmlWrite.WriteAttributeString("maxRecipientsPerMessage", this.MaxRecipientsPerMessage.ToString(CultureInfo.CurrentCulture));
            xmlWrite.WriteAttributeString("maxMessagesPerSend", this.MaxMessagesPerSend.ToString(CultureInfo.CurrentCulture));
            xmlWrite.WriteAttributeString("maxSbcsPerMessage", this.MaxSbcsPerMessage.ToString(CultureInfo.CurrentCulture));
            xmlWrite.WriteAttributeString("maxDbcsPerMessage", this.MaxDbcsPerMessage.ToString(CultureInfo.CurrentCulture));

            xmlWrite.WriteEndElement();
        }
    }

    /// <summary>
    /// 支持的服务类型(彩信)
    /// </summary>
    public struct MmsSenderInfo
    {
        /// <summary>
        /// 指出在描述 MMS 信息的显示时是否支持同步多媒体整合语言 (SMIL)。
        /// 客户端不会阻止用户向 Web 服务发送幻灯片模式的 MMS 信息（即使用 SMIL 的信息）。
        /// 如果 Web 服务收到使用 SMIL 的信息，但并不支持它，则 Web 服务一定会报告“other”类型的错误或者将此信息转换为非幻灯片模式。
        /// </summary>
        public bool SupportSlide;

        /// <summary>
        /// 表示一条 SMS 信息所允许的收件人数。
        /// 服务端负责检查收件人数是否超出了服务设定的限制。
        /// 如果客户端超出该限制，Web 服务应使用“other”错误代码返回错误（请参阅本文中 OMS 错误代码部分以了解错误代码的定义）。
        /// OMS 客户端允许用户将一条 SMS 信息同时发送给任何数量的收件人
        /// </summary>
        public int MaxRecipientsPerMessage;

        /// <summary>
        /// 表示 MMS 信息的最大字节长度
        /// </summary>
        public int MaxSizePerMessage;

        /// <summary>
        /// 表示一条 MMS 信息可以包含的幻灯片的最大数量
        /// </summary>
        public int MaxSlidesPerMessage;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="supportSlide"></param>
        /// <param name="maxRecipientsPerMessage"></param>
        /// <param name="maxSizePerMessage"></param>
        /// <param name="maxSlidesPerMessage"></param>
        public MmsSenderInfo(bool supportSlide, int maxRecipientsPerMessage, int maxSizePerMessage,
                             int maxSlidesPerMessage)
        {
            SupportSlide = supportSlide;
            MaxRecipientsPerMessage = maxRecipientsPerMessage;
            MaxSizePerMessage = maxSizePerMessage;
            MaxSlidesPerMessage = maxSlidesPerMessage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlWrite"></param>
        public void WriteXml(XmlTextWriter xmlWrite)
        {
            xmlWrite.WriteStartElement("supportedService");

            xmlWrite.WriteAttributeString("supportSlide", this.SupportSlide.ToString(CultureInfo.CurrentCulture));
            xmlWrite.WriteAttributeString("maxRecipientsPerMessage", this.MaxRecipientsPerMessage.ToString(CultureInfo.CurrentCulture));
            xmlWrite.WriteAttributeString("maxSizePerMessage", this.MaxSizePerMessage.ToString(CultureInfo.CurrentCulture));
            xmlWrite.WriteAttributeString("maxSlidesPerMessage", this.MaxSlidesPerMessage.ToString(CultureInfo.CurrentCulture));

            xmlWrite.WriteEndElement();
        }
    }
}