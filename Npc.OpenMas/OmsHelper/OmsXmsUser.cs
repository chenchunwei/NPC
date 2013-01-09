using System.Xml;

namespace Npc.OpenMas.OmsHelper
{

    /// <summary>
    ///OMSXmsUser 的摘要说明
    /// </summary>
    public class OmsXmsUser
    {
        ///<summary>
        ///</summary>
        public string UserId
        { get; private set; }

        ///<summary>
        ///</summary>
        public string Password
        { get; private set; }

        ///<summary>
        ///</summary>
        public string CustomData
        { get; private set; }

        ///<summary>
        ///</summary>
        public string Client
        { get; private set; }

        ///<summary>
        ///</summary>
        ///<param name="xmlUser"></param>
        public OmsXmsUser(string xmlUser)
        {
            var xd = new XmlDocument();
            xd.LoadXml(xmlUser);

            var nmManager = new XmlNamespaceManager(xd.NameTable);
            nmManager.AddNamespace("o", "http://schemas.microsoft.com/office/Outlook/2006/OMS");

            this.UserId = xd.SelectSingleNode("/o:xmsUser/o:userId", nmManager).InnerText;
            this.Password = xd.SelectSingleNode("/o:xmsUser/o:password", nmManager).InnerText;

            XmlNode node;
            node = xd.SelectSingleNode("/o:xmsUser/o:customData", nmManager);
            if (node != null)
                this.CustomData = node.InnerText;

            node = xd.SelectSingleNode("/o:xmsUser", nmManager).Attributes.GetNamedItem("client");
            if (node != null)
                this.Client = node.Value;
        }
    }
}