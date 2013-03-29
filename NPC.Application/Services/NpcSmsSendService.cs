using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using Fluent.Infrastructure.Log;
using NPC.Domain.Models.NpcSmsSends;
using NPC.Domain.Models.OpenMasConfigs;
using NPC.Domain.Repository;
using OpenMas;
using log4net;

namespace NPC.Application.Services
{

    public class NpcSmsSendService
    {
        private readonly ILog _logger;
        private readonly NpcSmsSendRepository _npcSmsSendRepository;
        private static readonly Object Locker = new Object();

        public NpcSmsSendService()
        {
            _logger = new DefaultLoggerFactory().GetLogger();
            _npcSmsSendRepository = new NpcSmsSendRepository();
        }

        public void Execute()
        {
            lock (Locker)
            {
                var npcMmsSends = _npcSmsSendRepository.GetNpcMmsSendsWaitingSend();
                npcMmsSends.ToList().ForEach(SingleSend);
            }
        }

        public void SingleSend(NpcSmsSend npcMmsSend)
        {
            var trans = TransactionManager.BeginTransaction();
            trans.Begin();

            try
            {
                trans.Commit();
            }
            catch (Exception exception)
            {
                trans.Rollback();
            }
        }


        /// <summary>
        /// 发送短信
        /// </summary>
        public static string SendSms(OpenMasConfig openMasConfig, string message, string[] destinationAddresses, DateTime? expectDateTime = null)
        {
            //短信客户端初始化
            var client = new Sms(openMasConfig.SmsMasService);
            if (expectDateTime == null)
                return client.SendMessage(destinationAddresses, message, openMasConfig.SmsExtensionNo, openMasConfig.SmsAppAccount, openMasConfig.SmsAppPwd);
            return client.SendMessage(destinationAddresses, message, openMasConfig.SmsExtensionNo, openMasConfig.SmsAppAccount, openMasConfig.SmsAppPwd, expectDateTime.Value);
        }
    }
}
