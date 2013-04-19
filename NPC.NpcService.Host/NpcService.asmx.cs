using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using Fluent.Infrastructure.Log;
using NPC.Domain.Repository;
using NPC.Website.Common;
using OpenMas;
using log4net;

namespace NPC.NpcService.Host
{
    /// <summary>
    /// NpcService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://rdzx.pinghu.gov.cn/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class NpcService : System.Web.Services.WebService
    {
        private readonly ILog _logger;
        public NpcService()
        {
            _logger = new DefaultLoggerFactory().GetLogger();
        }

        #region NotifySms
        [WebMethod]
        [SoapDocumentMethod("urn:NotifySms", RequestNamespace = "http://openmas.chinamobile.com/pulgin", OneWay = true, Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        public void NotifySms(string messageId)
        {
            try
            {
                _logger.DebugFormat("收到的messageId={0}的短信上行操作", messageId);
                var unitId = UnitMapping.UnitId;
                if (!unitId.HasValue)
                    return;
                _logger.DebugFormat("收到的UnitId={0}的短信上行操作", unitId.Value);
                var openMasConfig = new OpenMasConfigRepository().GetOpenMasConfigByUnit(unitId.Value);
                //调用上行短信获取接口获取短消息
                var sms = new OpenMas.Sms(openMasConfig.SmsMasService);
                var message = sms.GetMessage(messageId);

                //message.ApplicationId
                //message.Content
                //message.ExtendCode
                //message.From
                //message.ReceivedTime
                //message.To

                _logger.DebugFormat("收到的messageId={0}的短信内容为：{1}", messageId, message.Content);
                _logger.DebugFormat("全文序列化内容为：{0}", Newtonsoft.Json.JsonConvert.SerializeObject(message));
                //业务逻辑，短信内容可以从message中获取
            }
            catch (Exception ex)
            {
                _logger.ErrorFormat("接收到OpenMas发来的短信messageId={0},但处理时发生异常{1}", messageId, ex);
            }
        }
        #endregion

        #region NotifySmsDeliveryReport
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
            }
            catch (Exception ex)
            {
                _logger.ErrorFormat("回写messageId={0};receviedTel={1}短信状态时出错{2}", deliveryReport.messageId, deliveryReport.receivedAddress);
            }
        }
        #endregion

        #region NotifyMms
        [WebMethod]
        [SoapDocumentMethodAttribute("urn:NotifyMms", RequestNamespace = "http://openmas.chinamobile.com/pulgin", OneWay = true, Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        public void NotifyMms(string messageId)
        {
            try
            {
                _logger.DebugFormat("收到的messageId={0}的彩信上行操作", messageId);
                var unitId = UnitMapping.UnitId;
                if (!unitId.HasValue)
                    return;
                _logger.DebugFormat("收到的UnitId={0}的彩信上行操作", unitId.Value);

                var openMasConfig = new OpenMasConfigRepository().GetOpenMasConfigByUnit(unitId.Value);
                //调用上行彩信获取接口获取短消息
                var mms = new Mms(openMasConfig.MmsMasService);
                var message = mms.GetMessage(messageId);


                //message.ApplicationId
                //message.Content
                //message.ExtendCode
                //message.From
                //message.Priority
                //message.ReceivedTime
                //message.Subject
                
                _logger.DebugFormat("收到的messageId={0}的彩信内容为：{1}", messageId, message.Content);
                _logger.DebugFormat("全文序列化内容为：{0}", Newtonsoft.Json.JsonConvert.SerializeObject(message));

                //业务逻辑，彩信内容可以从message中获取
                _logger.InfoFormat("接收到OpenMas发来的彩信messageId={0}", messageId);
            }
            catch (Exception ex)
            {
                _logger.ErrorFormat("接收到OpenMas发来的彩信messageId={0},但处理时发生异常{1}", messageId, ex);
            }
        }
        #endregion

        #region NotifyMmsDeliveryReport
        [WebMethod]
        [SoapDocumentMethodAttribute("urn:NotifyMmsDeliveryReport", RequestNamespace = "http://openmas.chinamobile.com/pulgin", OneWay = true, Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        public void NotifyMmsDeliveryReport(OpenMas.Proxy.DeliveryReport deliveryReport)
        {
            var tans = TransactionManager.BeginTransaction();
            try
            {
                var npcMmsSendRepository = new NpcMmsSendRepository();
                //deliveryReport.receivedAddress; 接收号码，通常为手机号码
                // deliveryReport.statusCode;返回的结果代码，0表示成功
                // = deliveryReport.messageDeliveryStatus;结果状态
                var mmsSend = npcMmsSendRepository.GetByMessageId(deliveryReport.messageId);
                mmsSend.NpcMmsReceivers.Where(o => o.TelNum == deliveryReport.receivedAddress).ToList().ForEach(o =>
                {
                    o.DealStatus = deliveryReport.statusCode;
                    o.DealResult = deliveryReport.messageDeliveryStatus;
                });
                npcMmsSendRepository.Save(mmsSend);
                tans.Commit();
            }
            catch (Exception ex)
            {
                tans.Rollback();
                //处理异常信息
                _logger.ErrorFormat("回写messageId={0};receviedTel={1}彩信状态时出错{2}", deliveryReport.messageId, deliveryReport.receivedAddress);
            }
        }
        #endregion
    }
}
