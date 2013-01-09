using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace Npc.OpenMas.OmsHelper
{
    /// <summary>
    ///OMSXmsResponse 的摘要说明
    /// </summary>
    public class OmsXmsResponse
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IList<ErrorInfo> errorList = new List<ErrorInfo>();
        ///<summary>
        ///</summary>
        public IList<ErrorInfo> ErrorList
        {
            get
            {
                return errorList;
            }
        }

        ///<summary>
        ///</summary>
        public OmsXmsResponse()
        {

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
            xmlWrite.WriteStartElement("xmsResponse");

            foreach (ErrorInfo item in this.ErrorList)
            {
                xmlWrite.WriteStartElement("error");

                xmlWrite.WriteAttributeString("code", item.Code.ToString());

                if (item.Severity != SeverityType.Unknow)
                {
                    xmlWrite.WriteAttributeString("severity", item.Severity.ToString());
                }

                if (item.Code != ErrorCode.Ok)
                {
                    if (!string.IsNullOrEmpty(item.Content))
                    {
                        xmlWrite.WriteElementString("content", item.Content);
                    }

                    if (!string.IsNullOrEmpty(item.RecipientList))
                    {
                        xmlWrite.WriteElementString("recipientList", item.RecipientList);
                    }
                }

                xmlWrite.WriteEndElement();

                //如果成功，则直接退出循环
                if (item.Code == ErrorCode.Ok)
                    break;
            }

            xmlWrite.WriteEndElement();
            xmlWrite.WriteEndDocument();

            return s.GetStringBuilder().ToString();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public struct ErrorInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public ErrorCode Code;
        /// <summary>
        /// 
        /// </summary>
        public SeverityType Severity;
        /// <summary>
        /// 
        /// </summary>
        public string Content;
        /// <summary>
        /// 
        /// </summary>
        public string RecipientList;
    }
}