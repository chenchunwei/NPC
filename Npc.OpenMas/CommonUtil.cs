using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Data;
using System.Data.OleDb;
using Npc.OpenMas.OmsHelper;
using System.Xml;
using System.Data.Odbc;

namespace Npc.OpenMas
{
    public static class CommonUtil
    {
        /// <summary>
        /// 生成各表ID字段的值
        /// </summary>
        /// <returns></returns>
        public static string CreateSystemId()
        {
            return Guid.NewGuid().ToString();
        }

        /// <summary>
        /// 获取Url中的文件名
        /// </summary>
        /// <param name="url">操作页面地址</param>
        /// <returns></returns>
        public static string GetUrlFileName(string url)
        {
            if (string.IsNullOrEmpty(url))
                return string.Empty;
            var nodes = url.Split('/');
            var lastNode = nodes[nodes.Length - 1];
            var paramIndex = lastNode.IndexOf('?');
            return (paramIndex == -1) ? lastNode : lastNode.Substring(0, paramIndex);
        }


        /// <summary>
        /// 格式化XML
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static string FormatXml(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return "";
            }

            const string startXml = "<?";
            const string endXml = "?>";
            var startPos = xml.IndexOf(startXml, StringComparison.CurrentCultureIgnoreCase);
            var endPos = xml.IndexOf(endXml, StringComparison.CurrentCultureIgnoreCase);
            if (!(startPos == -1 || endPos == -1))
            {
                return xml.Remove(startPos, endPos - startPos + endXml.Length);
            }
            return xml;
        }

        /// <summary>
        /// 取得字符串从0开始指定长度的字符串
        /// </summary>
        /// <param name="source"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetFixedLengthString(string source, int length)
        {
            if (source.Length > length)
            {
                source = source.Substring(0, length - 1);
            }
            return source;
        }

        /// <summary>
        /// 生成smil文件
        /// </summary>
        /// <param name="layout">smil的布局信息</param>
        /// <param name="parInfoList">资料列表</param>
        /// <returns></returns>
        public static string BuilderSmil(LayoutInfo layout, IList<ParInfo> parInfoList)
        {
            var smilHead = new HeadInfo();
            smilHead.Layout = layout;

            var smilBody = new BodyInfo();
            foreach (var parInfo in parInfoList)
            {
                smilBody.ParList.Add(parInfo);
            }

            var s = new StringWriter(new StringBuilder(), CultureInfo.CurrentCulture);
            var x = new XmlTextWriter(s);

            x.WriteStartDocument();
            x.WriteStartElement("smil");

            smilHead.WriteXml(x);
            smilBody.WriteXml(x);

            x.WriteEndElement();
            x.WriteEndDocument();

            return FormatXml(s.GetStringBuilder().ToString());
        }
    }

}