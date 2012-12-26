using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using Fluent.Infrastructure.Log;
using OpenMas;
using log4net;

namespace NPC.OpenMasCallback.Host
{
    /// <summary>
    /// OpenMasCallbackService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class OpenMasCallbackService : System.Web.Services.WebService
    {
        private readonly ILog _logger;
        public OpenMasCallbackService()
        {
            _logger = new DefaultLoggerFactory().GetLogger();
        }
        [SoapDocumentMethod("urn:NotifySms", RequestNamespace = "http://openmas.chinamobile.com/pulgin", OneWay = true, Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        public void NotifySms(string messageId)
        {
            try
            {
                //调用上行短信获取接口获取短消息
                var sms = new OpenMas.Sms(OpenMasConfig.UrlOfSmsService);
                var message = sms.GetMessage(messageId);
                //业务逻辑，短信内容可以从message中获取
                //......
            }
            catch (Exception ex)
            {
                //处理异常信息
                //.......
            }
        }

        [WebMethod]
        [SoapDocumentMethod("urn:NotifySmsDeliveryReport", RequestNamespace = "http://openmas.chinamobile.com/pulgin", OneWay = true, Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        public void NotifySmsDeliveryReport(OpenMas.Proxy.DeliveryReport deliveryReport)
        {
            try
            {
                //内容提取
                var messageID = deliveryReport.messageId;        //短信ID
                var receivedAddress = deliveryReport.receivedAddress; //接收号码，通常为手机号码
                var statusCode = deliveryReport.statusCode;//返回的结果代码，0表示成功
                var messageDeliveryStatus = int.Parse(deliveryReport.messageDeliveryStatus);//结果状态

                //业务逻辑处理
                //.......
            }
            catch (Exception ex)
            {
                //处理异常信息
                //.......
            }
        }

    }
}
