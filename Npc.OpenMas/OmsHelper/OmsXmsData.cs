using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Text;
using System.Xml.XPath;
using System.Globalization;


namespace Npc.OpenMas.OmsHelper
{
    /// <summary>
    ///OMSXmsData 的摘要说明
    /// </summary>
    public class OmsXmsData
    {
        ///<summary>
        ///</summary>
        public UserInfo User { get; set; }
        ///<summary>
        ///</summary>
        public XmsHeadInfo XmsHead { get; set; }

        ///<summary>
        ///</summary>
        public XmsBodyInfo XmsBody
        {
            get;
            set;
        }

        ///<summary>
        ///</summary>
        ///<param name="xmsData"></param>
        public OmsXmsData(string xmsData)
        {
            try
            {
                XmlDocument xd = new XmlDocument();
                xd.LoadXml(xmsData);

                XmlNamespaceManager nmManager = new XmlNamespaceManager(xd.NameTable);
                nmManager.AddNamespace("o", "http://schemas.microsoft.com/office/Outlook/2006/OMS");

                XmlNode node = xd.SelectSingleNode("/o:xmsData/o:user", nmManager);
                this.User = new UserInfo(node, nmManager, "o");

                node = xd.SelectSingleNode("/o:xmsData/o:xmsHead", nmManager);
                this.XmsHead = new XmsHeadInfo(node, nmManager, "o");

                node = xd.SelectSingleNode("/o:xmsData/o:xmsBody", nmManager);
                this.XmsBody = new XmsBodyInfo(node, nmManager, "o");

            }
            catch (Exception)
            {
                throw;
            }
        }

        ///<summary>
        ///
        ///                    Returns a <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.
        ///                
        ///</summary>
        ///
        ///<returns>
        ///
        ///                    A <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.
        ///                
        ///</returns>
        ///<filterpriority>2</filterpriority>
        public override string ToString()
        {
            var s = new StringWriter(new StringBuilder(), CultureInfo.CurrentCulture);
            var xmlWrite = new XmlTextWriter(s);

            xmlWrite.WriteStartDocument();

            xmlWrite.WriteStartElement("xmsData");
            if(User != null)
                User.WriteXml(xmlWrite);
            
            if (XmsHead != null)
                XmsHead.WriteXml(xmlWrite);

            if (XmsBody != null)
                XmsBody.WriteXml(xmlWrite);

            xmlWrite.WriteEndElement();
            xmlWrite.WriteEndDocument();

            return s.GetStringBuilder().ToString();
        }

    }

    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// 用户 ID，可以是一个 Passport，也可以是由字符和数字组成的任何字符串。移动电话号码可用作非 Passport userId
        /// </summary>
        public string UserId;

        /// <summary>
        /// 向服务提供商注册服务时创建的用户密码
        /// </summary>
        public string Password;

        /// <summary>
        /// 回复号码或回拨号码，主要在韩国使用。不支持回拨号码的服务提供商可忽略此元素。可选
        /// </summary>
        public string ReplyPhone;

        /// <summary>
        /// 供以后扩展之用
        /// </summary>
        public string CustomData;

        /// <summary>
        /// 生成xml
        /// </summary>
        /// <param name="xmlWrite"></param>
        internal void WriteXml(XmlTextWriter xmlWrite)
        {
            xmlWrite.WriteStartElement("user");
            xmlWrite.WriteElementString("userId", this.UserId);
            xmlWrite.WriteElementString("password", this.Password);
            xmlWrite.WriteElementString("replyPhone", this.ReplyPhone);
            xmlWrite.WriteElementString("customData", this.CustomData);
            xmlWrite.WriteEndElement();
        }

        ///<summary>
        ///</summary>
        public UserInfo()
        { }

        ///<summary>
        ///</summary>
        ///<param name="node"></param>
        ///<param name="nmManager"></param>
        ///<param name="prefix"></param>
        internal UserInfo(XmlNode node, XmlNamespaceManager nmManager, string prefix)
        {
            XmlNode childNode = node.SelectSingleNode(string.Format(CultureInfo.CurrentCulture, "//{0}:userId", prefix), nmManager);
            if(childNode != null) 
                UserId = childNode.InnerText;

            childNode = node.SelectSingleNode(string.Format(CultureInfo.CurrentCulture, "//{0}:password", prefix), nmManager);
            if(childNode != null) 
                Password = childNode.InnerText;

            childNode = node.SelectSingleNode(string.Format(CultureInfo.CurrentCulture, "//{0}:replyPhone", prefix), nmManager);
            if (childNode != null)
                ReplyPhone = childNode.InnerText;

            childNode = node.SelectSingleNode(string.Format(CultureInfo.CurrentCulture, "//{0}:customData", prefix), nmManager);
            if (childNode != null)
                CustomData = childNode.InnerText;
        }
    }

    ///<summary>
    ///</summary>
    public class XmsHeadInfo
    {
        /// <summary>
        /// 客户端要求的服务。serviceInfo 字符串中定义的支持服务之一。有效值为 SMS_SENDER 或 MMS_SENDER
        /// </summary>
        public RequiredServiceType RequiredService;

        /// <summary>
        /// 向服务指出信息是从 Outlook Reminder、Calendar Summary 或 Rules 自动生成的，
        /// 还是使用 XMS 检查器或 Outlook 检查器（“other”）手动发送的。
        /// 有效值包括“reminder”、“calSummary”或“ruleBased”、“xmsInspector”或“other”。
        /// Web 服务可以为用户提供一种 Web UI，以便在特定时刻或特定情况下阻止部分此类信息。
        /// 如果 Web 服务不支持 sourceType 特定操作，则可以忽略此元素。可选。
        /// </summary>
        public string SourceType;

        /// <summary>
        /// 表示将在指定时间发送信息。
        /// 使用 YYYY-MM-DDThh:mm:ssZ 格式的 UTC 时间。精确度为“分”，因此“秒”部分（“ss”）永远为“00”。
        /// 服务端必须将计划的时间转换为当地时区的时间。如果指定时间为过去时间，则会立即发送此信息。可选。
        /// </summary>
        public string Scheduled;

        /// <summary>
        /// 信息主题。仅适用于 MMS 信息
        /// </summary>
        public string Subject;

        /// <summary>
        /// 收件人列表的父元素
        /// </summary>
        public ToInfo To;

        ///<summary>
        ///</summary>
        ///<param name="xmlWrite"></param>
        internal void WriteXml(XmlTextWriter xmlWrite)
        {
            xmlWrite.WriteStartElement("xmsHead");
            xmlWrite.WriteElementString("requiredService", this.RequiredService.ToString());

            if (!string.IsNullOrEmpty(this.SourceType))
            {
                xmlWrite.WriteElementString("sourceType", this.SourceType);
            }

            if (!string.IsNullOrEmpty(this.Scheduled))
            {
                xmlWrite.WriteElementString("scheduled", this.Scheduled);
            }

            if (!string.IsNullOrEmpty(this.Subject))
            {
                xmlWrite.WriteElementString("subject", this.Subject);
            }

            To.WriteXml(xmlWrite);

            xmlWrite.WriteEndElement();
        }

        ///<summary>
        ///</summary>
        public XmsHeadInfo() { }

        ///<summary>
        ///</summary>
        ///<param name="node"></param>
        ///<param name="nmManager"></param>
        ///<param name="prefix"></param>
        internal XmsHeadInfo(XmlNode node, XmlNamespaceManager nmManager, string prefix)
        {
            XmlNode childNode = node.SelectSingleNode(string.Format(CultureInfo.CurrentCulture, "//{0}:scheduled", prefix), nmManager);
            if (childNode != null)
                Scheduled = childNode.InnerText;

            childNode = node.SelectSingleNode(string.Format(CultureInfo.CurrentCulture, "//{0}:subject", prefix), nmManager);
            if (childNode != null)
                Subject = childNode.InnerText;

            childNode = node.SelectSingleNode(string.Format(CultureInfo.CurrentCulture, "//{0}:sourceType", prefix), nmManager);
            if (childNode != null)
                SourceType = childNode.InnerText;

            childNode = node.SelectSingleNode(string.Format(CultureInfo.CurrentCulture, "//{0}:requiredService", prefix), nmManager);
            if (childNode != null)
            {
                switch (childNode.InnerText.ToUpper(CultureInfo.CurrentCulture))
                {
                    case "MMS_SENDER":
                        RequiredService = RequiredServiceType.Mms_Sender;
                        break;
                    case "SMS_SENDER":
                        RequiredService = RequiredServiceType.Sms_Sender;
                        break;
                    default:
                        break;

                }
            }

            childNode = node.SelectSingleNode(string.Format(CultureInfo.CurrentCulture, "//{0}:to", prefix), nmManager);
            if (childNode != null)
                To = new ToInfo(childNode, nmManager, prefix);

        }
    }

    /// <summary>
    /// 收件人列表的父元素
    /// </summary>
    public class ToInfo
    {
        private readonly IList<RecipientInfo> _recipientList = new List<RecipientInfo>();

        /// <summary>
        /// 收件人列表
        /// </summary>
        public IList<RecipientInfo> RecipientList
        {
            get
            {
                return _recipientList;
            }
        }

        ///<summary>
        ///</summary>
        ///<param name="xmlWrite"></param>
        internal void WriteXml(XmlTextWriter xmlWrite)
        {
            xmlWrite.WriteStartElement("to");
            foreach (RecipientInfo item in this.RecipientList)
            {
                item.WriteXml(xmlWrite);
            }
            xmlWrite.WriteEndElement();
        }

        ///<summary>
        ///</summary>
        public ToInfo() { }

        ///<summary>
        ///</summary>
        ///<param name="node"></param>
        ///<param name="nmManager"></param>
        ///<param name="prefix"></param>
        internal ToInfo(XmlNode node, XmlNamespaceManager nmManager, string prefix)
        {
            foreach (XmlNode item in node.SelectNodes(string.Format(CultureInfo.CurrentCulture, "//{0}:recipient", prefix), nmManager))
            {
                var recipient = new RecipientInfo(item);
                this.RecipientList.Add(recipient);
            }
        }
    }

    ///<summary>
    /// 收件人
    ///</summary>
    public class RecipientInfo
    {
        /// <summary>
        /// 是指收件人的移动电话号码
        /// </summary>
        public string Value;

        ///<summary>
        ///</summary>
        ///<param name="xmlWrite"></param>
        internal void WriteXml(XmlTextWriter xmlWrite)
        {
            xmlWrite.WriteElementString("recipient", Value);
        }

        ///<summary>
        ///</summary>
        ///<param name="node"></param>
        internal RecipientInfo(XmlNode node)
        {
            Value = node.InnerText;
        }
    }

    /// <summary>
    /// XMS 信息正文元素的父元素。format 属性用于指定 xmsBody 的格式或类型。OMS 支持 SMS 和 MMS。以后可能会提供对其他信息格式的支持。
    /// </summary>
    public class XmsBodyInfo
    {
        ///<summary>
        ///</summary>
        public string Format
        {
            get;
            set;
        }

        private readonly IList<ContentInfo> contentList = new List<ContentInfo>();

        /// <summary>
        /// 如果 xmsBody 的 format 属性为“SMS”，则为拆分的 SMS 信息。
        /// SMS 可以存在多个内容元素，每个元素代表长信息的一部分。Web 服务应将每个元素作为单独的短信按顺序发送。
        /// 如果 xmsBody 的 format 属性为“MMS”并且 SMIL 部分可用（幻灯片模式），则为幻灯片文本、图像或音频对象。
        /// 如果 xmsBody 的 format 属性为“MMS”并且 SMIL 部分不可用（非幻灯片模式），则为 MMS 信息的文本页、图像或音频对象。
        /// </summary>
        public IList<ContentInfo> ContentList
        {
            get
            {
                return contentList;
            }
        }

        /// <summary>
        /// 演示说明部分的根元素。仅适用于 MMS 信息
        /// </summary>
        public MmsSlidesInfo MmsSlides { get; set; }

        ///<summary>
        ///</summary>
        ///<param name="xmlWrite"></param>
        internal void WriteXml(XmlTextWriter xmlWrite)
        {
            xmlWrite.WriteStartElement("xmsBody");
            xmlWrite.WriteAttributeString("format", this.Format);
            if (this.Format.ToUpper(CultureInfo.CurrentCulture) == "MMS")
            {
                MmsSlides.WriteXml(xmlWrite);
            }

            foreach (ContentInfo item in this.ContentList)
            {
                item.WriteXml(xmlWrite);
            }
            xmlWrite.WriteEndElement();
        }

        ///<summary>
        ///</summary>
        public XmsBodyInfo() { }

        ///<summary>
        ///</summary>
        ///<param name="node"></param>
        ///<param name="nmManager"></param>
        ///<param name="prefix"></param>
        internal XmsBodyInfo(XmlNode node, XmlNamespaceManager nmManager, string prefix)
        {
            Format = node.Attributes.GetNamedItem("format").Value;

            if (Format.Equals("MMS", StringComparison.OrdinalIgnoreCase))
            {
                XmlNode childNode = node.SelectSingleNode(string.Format(CultureInfo.CurrentCulture, "//{0}:mmsSlides", prefix), nmManager);

                this.MmsSlides = new MmsSlidesInfo(childNode, nmManager, prefix);
            }

            foreach (XmlNode item in node.SelectNodes(string.Format(CultureInfo.CurrentCulture, "//{0}:content", prefix), nmManager))
            {
                ContentInfo content = new ContentInfo(item);
                contentList.Add(content);
            }
        }

    }

    ///<summary>
    ///</summary>
    public class ContentInfo
    {
        /// <summary>
        /// SMIL 正文部分所指的内容 ID。SMS 和非幻灯片模式的 MMS 可忽略此属性
        /// </summary>
        public string ContentId;

        /// <summary>
        /// MIME（多用途 Internet 邮件扩展）内容类型。支持的内容类型在“内容类型表”中定义。
        /// </summary>
        public string ContentType;

        /// <summary>
        /// 指示媒体对象的文件名，在保存对象后可以将此文件名用作默认文件名
        /// </summary>
        public string ContentLocation;

        /// <summary>
        /// 正文
        /// </summary>
        public string Text;

        ///<summary>
        ///</summary>
        ///<param name="xmlWrite"></param>
        internal void WriteXml(XmlTextWriter xmlWrite)
        {
            xmlWrite.WriteStartElement("content");
            xmlWrite.WriteAttributeString("contentType", this.ContentType);
            xmlWrite.WriteAttributeString("contentId", this.ContentId);
            xmlWrite.WriteAttributeString("contentLocation", this.ContentLocation);
            xmlWrite.WriteString(this.Text);
            xmlWrite.WriteEndElement();
        }

        ///<summary>
        ///</summary>
        public ContentInfo() { }

        ///<summary>
        ///</summary>
        ///<param name="node"></param>
        internal ContentInfo(XmlNode node)
        {
            ContentId = node.Attributes.GetNamedItem("contentId").Value;
            ContentType = node.Attributes.GetNamedItem("contentType").Value;
            ContentLocation = node.Attributes.GetNamedItem("contentLocation").Value;

            Text = node.InnerText;
        }

    }

    /// <summary>
    /// SMIL
    /// </summary>
    public class MmsSlidesInfo
    {
        /// <summary>
        /// SMIL 标题部分的根标记
        /// </summary>
        public HeadInfo Head;

        /// <summary>
        /// SMIL 正文部分的根标记
        /// </summary>
        public BodyInfo Body;

        ///<summary>
        ///</summary>
        ///<param name="xmlWrite"></param>
        internal void WriteXml(XmlTextWriter xmlWrite)
        {
            xmlWrite.WriteStartElement("mmsSlides");

            Head.WriteXml(xmlWrite);

            Body.WriteXml(xmlWrite);

            xmlWrite.WriteEndElement();
        }

        ///<summary>
        ///</summary>
        public MmsSlidesInfo() { }

        ///<summary>
        ///</summary>
        ///<param name="node"></param>
        ///<param name="nmManager"></param>
        ///<param name="prefix"></param>
        internal MmsSlidesInfo(XmlNode node, XmlNamespaceManager nmManager, string prefix)
        {
            XmlNode childNode = node.SelectSingleNode(string.Format(CultureInfo.CurrentCulture, "//{0}:head", prefix), nmManager);
            if (childNode != null)
                Head = new HeadInfo(childNode, nmManager, prefix);

            childNode = node.SelectSingleNode(string.Format(CultureInfo.CurrentCulture, "//{0}:body", prefix), nmManager);
            if (childNode != null)
                Body = new BodyInfo(childNode, nmManager, prefix);
        }
    }

    ///<summary>
    ///</summary>
    public class HeadInfo
    {
        /// <summary>
        /// 指示 SMIL 部分作者的元数据，name = "author" 并且 content ="msOfficeOutlookOms"。服务端无需执行任何操作。
        /// </summary>
        public MetaInfo Meta;

        /// <summary>
        /// SMIL 布局部分的根标记
        /// </summary>
        public LayoutInfo Layout;
        ///<summary>
        ///</summary>
        ///<param name="xmlWrite"></param>
        internal void WriteXml(XmlTextWriter xmlWrite)
        {
            xmlWrite.WriteStartElement("head");

            if (Meta != null)
                Meta.WriteXml(xmlWrite);

            if (Layout != null)
                Layout.WriteXml(xmlWrite);

            xmlWrite.WriteEndElement();
        }

        ///<summary>
        ///</summary>
        public HeadInfo() { }

        ///<summary>
        ///</summary>
        ///<param name="node"></param>
        ///<param name="nmManager"></param>
        ///<param name="prefix"></param>
        internal HeadInfo(XmlNode node, XmlNamespaceManager nmManager, string prefix)
        {
            XmlNode childNode = node.SelectSingleNode(string.Format(CultureInfo.CurrentCulture, "//{0}:meta", prefix), nmManager);
            if (childNode != null)
                Meta = new MetaInfo(childNode);

            childNode = node.SelectSingleNode(string.Format(CultureInfo.CurrentCulture, "//{0}:layout", prefix), nmManager);
            if (childNode != null)
                Layout = new LayoutInfo(childNode, nmManager, prefix);
        }
    }

    /// <summary>
    /// 指示 SMIL 部分作者的元数据，name = "author" 并且 content ="msOfficeOutlookOms"。服务端无需执行任何操作。
    /// </summary>
    public class MetaInfo
    {
        ///<summary>
        ///</summary>
        public string Name { get; set; }
        ///<summary>
        ///</summary>
        public string Content { get; set; }
        ///<summary>
        ///</summary>
        ///<param name="_name"></param>
        public MetaInfo(string _name)
        {
            Name = _name;
            Content = "";
        }

        ///<summary>
        ///</summary>
        ///<param name="xmlWrite"></param>
        internal void WriteXml(XmlTextWriter xmlWrite)
        {
            xmlWrite.WriteStartElement("head");
            xmlWrite.WriteAttributeString("name", this.Name);
            if (!string.IsNullOrEmpty(this.Content))
            {
                xmlWrite.WriteAttributeString("content", this.Content);
            }
            xmlWrite.WriteEndElement();
        }

        ///<summary>
        ///</summary>
        public MetaInfo() { }

        ///<summary>
        ///</summary>
        ///<param name="node"></param>
        internal MetaInfo(XmlNode node)
        {
            XmlNode childNode = node.Attributes.GetNamedItem("name");
            if (childNode != null)
                Name = childNode.Value;

            childNode = node.Attributes.GetNamedItem("content");
            if (childNode != null)
                Content = childNode.Value;
        }
    }

    ///<summary>
    ///</summary>
    public class LayoutInfo
    {
        /// <summary>
        /// 指定电话屏幕的分辨率（以像素为单位）以及背景色
        /// </summary>
        public RootlayoutInfo Rootlayout;

        private readonly IList<RegionInfo> regionList = new List<RegionInfo>();
        ///<summary>
        ///</summary>
        public IList<RegionInfo> RegionList
        {
            get { return regionList; }
        }

        ///<summary>
        ///</summary>
        ///<param name="xmlWrite"></param>
        internal void WriteXml(XmlTextWriter xmlWrite)
        {
            xmlWrite.WriteStartElement("layout");

            Rootlayout.WriteXml(xmlWrite);

            foreach (RegionInfo item in regionList)
            {
                item.WriteXml(xmlWrite);
            }

            xmlWrite.WriteEndElement();
        }

        ///<summary>
        ///</summary>
        public LayoutInfo() { }

        ///<summary>
        ///</summary>
        ///<param name="node"></param>
        ///<param name="nmManager"></param>
        ///<param name="prefix"></param>
        internal LayoutInfo(XmlNode node, XmlNamespaceManager nmManager, string prefix)
        {
            XmlNode childNode = node.SelectSingleNode(string.Format(CultureInfo.CurrentCulture, "//{0}:root-layout", prefix), nmManager);
            if (childNode != null)
                Rootlayout = new RootlayoutInfo(childNode);

            foreach (XmlNode item in node.SelectNodes(string.Format(CultureInfo.CurrentCulture, "//{0}:region", prefix), nmManager))
            {
                RegionInfo region = new RegionInfo(item);
                this.regionList.Add(region);
            }
        }
    }

    /// <summary>
    /// 指定电话屏幕的分辨率（以像素为单位）以及背景色
    /// </summary>
    public class RootlayoutInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string Width;

        /// <summary>
        /// 
        /// </summary>
        public string Height;

        /// <summary>
        /// 
        /// </summary>
        public string BackgroundColor;

        /// <summary>
        /// 指定电话屏幕的分辨率（以像素为单位）以及背景色
        /// </summary>
        /// <param name="_width">宽度</param>
        /// <param name="_height">高度</param>
        /// <param name="_backgroundColor">背景色</param>
        public RootlayoutInfo(string _width, string _height, string _backgroundColor)
        {
            Width = _width;
            Height = _height;
            BackgroundColor = _backgroundColor;
        }
        ///<summary>
        ///</summary>
        public RootlayoutInfo()
        { }

        ///<summary>
        ///</summary>
        ///<param name="xmlWrite"></param>
        internal void WriteXml(XmlTextWriter xmlWrite)
        {
            xmlWrite.WriteStartElement("root-layout");

            xmlWrite.WriteAttributeString("width", this.Width);
            xmlWrite.WriteAttributeString("height", this.Height);
            xmlWrite.WriteAttributeString("background-color", this.BackgroundColor);

            xmlWrite.WriteEndElement();
        }

        ///<summary>
        ///</summary>
        ///<param name="node"></param>
        internal RootlayoutInfo(XmlNode node)
        {
            XmlNode childNode = node.Attributes.GetNamedItem("width");
            if (childNode != null)
                Width = childNode.Value;

            childNode = node.Attributes.GetNamedItem("height");
            if (childNode != null)
                Height = childNode.Value;

            childNode = node.Attributes.GetNamedItem("background-color");
            if (childNode != null)
                BackgroundColor = childNode.Value;
        }
    }

    /// <summary>
    /// 属性 id 可以是“image”或“text”，代表所定义的区域类型。其他 4 个属性指定区域的位置和尺寸（以像素为单位）。
    /// </summary>
    public class RegionInfo
    {
        /// <summary>
        /// 可以是“image”或“text”
        /// </summary>
        public string Id;
        /// <summary>
        /// 
        /// </summary>
        public string Left;
        /// <summary>
        /// 
        /// </summary>
        public string Top;
        /// <summary>
        /// 
        /// </summary>
        public string Width;
        /// <summary>
        /// 
        /// </summary>
        public string Height;
        /// <summary>
        /// 
        /// </summary>
        public FitType Fit;

        ///<summary>
        ///</summary>
        ///<param name="xmlWrite"></param>
        internal void WriteXml(XmlTextWriter xmlWrite)
        {
            xmlWrite.WriteStartElement("region");

            xmlWrite.WriteAttributeString("id", this.Id);
            xmlWrite.WriteAttributeString("left", this.Left);
            xmlWrite.WriteAttributeString("top", this.Top);
            xmlWrite.WriteAttributeString("width", this.Width);
            xmlWrite.WriteAttributeString("height", this.Height);

            if (this.Fit != FitType.Unknow)
            {
                xmlWrite.WriteAttributeString("fit", this.Fit.ToString().ToLower(CultureInfo.CurrentCulture));
            }

            xmlWrite.WriteEndElement();
        }

        ///<summary>
        ///</summary>
        public RegionInfo() { }

        ///<summary>
        ///</summary>
        ///<param name="node"></param>
        internal RegionInfo(XmlNode node)
        {
            XmlNode childNode = node.Attributes.GetNamedItem("id");
            if (childNode != null)
                Id = childNode.Value;

            childNode = node.Attributes.GetNamedItem("left");
            if (childNode != null)
                Left = childNode.Value;

            childNode = node.Attributes.GetNamedItem("top");
            if (childNode != null)
                Top = childNode.Value;

            childNode = node.Attributes.GetNamedItem("width");
            if (childNode != null)
                Width = childNode.Value;

            childNode = node.Attributes.GetNamedItem("height");
            if (childNode != null)
                Height = childNode.Value;

            childNode = node.Attributes.GetNamedItem("fit");
            if (childNode != null)
                Fit = (FitType)Enum.Parse(typeof(FitType), childNode.Value, true);

        }
    }

    /// <summary>
    /// SMIL 正文部分的根标记
    /// </summary>
    public class BodyInfo
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IList<ParInfo> _parList = new List<ParInfo>();

        /// <summary>
        /// MMS 幻灯片的根标记。属性 dur 指定幻灯片持续播放时间（以毫秒为单位）。
        /// 出现多次
        /// </summary>
        public IList<ParInfo> ParList
        {
            get { return _parList; }
        }

        ///<summary>
        ///</summary>
        ///<param name="xmlWrite"></param>
        internal void WriteXml(XmlTextWriter xmlWrite)
        {
            xmlWrite.WriteStartElement("body");

            foreach (ParInfo item in this.ParList)
            {
                item.WriteXml(xmlWrite);
            }

            xmlWrite.WriteEndElement();
        }

        ///<summary>
        ///</summary>
        public BodyInfo() { }

        ///<summary>
        ///</summary>
        ///<param name="node"></param>
        ///<param name="nmManager"></param>
        ///<param name="prefix"></param>
        internal BodyInfo(XmlNode node, XmlNamespaceManager nmManager, string prefix)
        {
            foreach (XmlNode item in node.SelectNodes(string.Format(CultureInfo.CurrentCulture, "//{0}:par", prefix), nmManager))
            {
                var par = new ParInfo(item);
                _parList.Add(par);
            }
        }
    }

    /// <summary>
    /// MMS 幻灯片的根标记。属性 dur 指定幻灯片持续播放时间（以毫秒为单位）。
    /// 出现多次
    /// </summary>
    public class ParInfo
    {
        /// <summary>
        /// 以毫秒(ms)为单位
        /// </summary>
        public string Dur;

        /// <summary>
        /// 
        /// </summary>
        public ImgInfo Img;
        /// <summary>
        /// 
        /// </summary>
        public TextInfo Text;
        /// <summary>
        /// 
        /// </summary>
        public AudioInfo Audio;

        ///<summary>
        ///</summary>
        ///<param name="xmlWrite"></param>
        internal void WriteXml(XmlTextWriter xmlWrite)
        {
            xmlWrite.WriteStartElement("par");
            xmlWrite.WriteAttributeString("dur", this.Dur);

            if(this.Img != null)
                this.Img.WriteXml(xmlWrite);

            if(this.Text != null)
                this.Text.WriteXml(xmlWrite);

            if (this.Audio != null)
                this.Audio.WriteXml(xmlWrite);

            xmlWrite.WriteEndElement();
        }

        ///<summary>
        ///</summary>
        public ParInfo() { }

        ///<summary>
        ///</summary>
        ///<param name="node"></param>
        internal ParInfo(XmlNode node)
        {
            XmlNode childNode = node.Attributes.GetNamedItem("dur");
            if (childNode != null)
                Dur = childNode.Value;


            foreach(XmlNode item in node.ChildNodes)
            {
                if (item.Name == "img")
                    Img = new ImgInfo(item);
                else if (item.Name == "text")
                    Text = new TextInfo(item);
                else if (item.Name == "audio")
                    Audio = new AudioInfo(item);
            }

            //childNode = node.SelectSingleNode(string.Format(CultureInfo.CurrentCulture, "//{0}:img", prefix), nmManager);
            //if (childNode != null)
            //    Img = new ImgInfo(childNode);

            //childNode = node.SelectSingleNode(string.Format(CultureInfo.CurrentCulture, "//{0}:text", prefix), nmManager);
            //if (childNode != null)
            //    Text = new TextInfo(childNode);

            //childNode = node.SelectSingleNode(string.Format(CultureInfo.CurrentCulture, "//{0}:audio", prefix), nmManager);
            //if (childNode != null)
            //    Audio = new AudioInfo(childNode);

        }
    }

    /// <summary>
    /// Base64 编码的图像对象。属性 src 指的是 content 元素的 contentId 属性。
    /// </summary>
    public class ImgInfo
    {
        /// <summary>
        /// src 指的是 content 元素的 contentID 属性
        /// </summary>
        public string Src;

        /// <summary>
        /// 可以是“image”或“text”
        /// 对应到RegionInfo的Id
        /// </summary>
        public string Region;

        ///<summary>
        ///</summary>
        ///<param name="xmlWrite"></param>
        internal void WriteXml(XmlTextWriter xmlWrite)
        {
            xmlWrite.WriteStartElement("img");
            xmlWrite.WriteAttributeString("src", this.Src);
            xmlWrite.WriteAttributeString("region", this.Region);
            xmlWrite.WriteEndElement();
        }

        ///<summary>
        ///</summary>
        public ImgInfo() { }

        ///<summary>
        ///</summary>
        ///<param name="node"></param>
        internal ImgInfo(XmlNode node)
        {
            XmlNode childNode = node.Attributes.GetNamedItem("src");
            if (childNode != null)
                Src = childNode.Value;

            childNode = node.Attributes.GetNamedItem("region");
            if (childNode != null)
                Region = childNode.Value;
        }
    }

    /// <summary>
    /// 纯文本对象。属性 src 指的是 content 元素的 contentID 属性。
    /// </summary>
    public class TextInfo
    {
        /// <summary>
        /// src 指的是 content 元素的 contentID 属性
        /// </summary>
        public string Src;

        /// <summary>
        /// 可以是“image”或“text”
        /// 对应到RegionInfo的Id
        /// </summary>
        public string Region;

        ///<summary>
        ///</summary>
        ///<param name="xmlWrite"></param>
        internal void WriteXml(XmlTextWriter xmlWrite)
        {
            xmlWrite.WriteStartElement("text");
            xmlWrite.WriteAttributeString("src", this.Src);
            xmlWrite.WriteAttributeString("region", this.Region);
            xmlWrite.WriteEndElement();
        }

        ///<summary>
        ///</summary>
        public TextInfo() { }
        ///<summary>
        ///</summary>
        ///<param name="node"></param>
        internal TextInfo(XmlNode node)
        {
            XmlNode childNode = node.Attributes.GetNamedItem("src");
            if (childNode != null)
                Src = childNode.Value;

            childNode = node.Attributes.GetNamedItem("region");
            if (childNode != null)
                Region = childNode.Value;
        }
    }

    /// <summary>
    /// Base64 编码的音频对象。属性src 指的是 content 元素的 contentId。
    /// </summary>
    public class AudioInfo
    {
        /// <summary>
        /// src 指的是 content 元素的 contentID 属性
        /// </summary>
        public string Src;

        /// <summary>
        /// 可以是“image”或“text”
        /// 对应到RegionInfo的Id
        /// </summary>
        public string Region;

        ///<summary>
        ///</summary>
        ///<param name="xmlWrite"></param>
        internal void WriteXml(XmlTextWriter xmlWrite)
        {
            xmlWrite.WriteStartElement("audio");
            xmlWrite.WriteAttributeString("src", this.Src);
            if (!string.IsNullOrEmpty(Region))
            {
                xmlWrite.WriteAttributeString("region", this.Region);
            }
            xmlWrite.WriteEndElement();
        }

        ///<summary>
        ///</summary>
        public AudioInfo() { }
        ///<summary>
        ///</summary>
        ///<param name="node"></param>
        internal AudioInfo(XmlNode node)
        {
            XmlNode childNode = node.Attributes.GetNamedItem("src");
            if (childNode != null)
                Src = childNode.Value;

            childNode = node.Attributes.GetNamedItem("region");
            if (childNode != null)
                Region = childNode.Value;
        }
    }
}