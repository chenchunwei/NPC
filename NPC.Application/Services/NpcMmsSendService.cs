using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using Fluent.Infrastructure.Log;
using NPC.Domain.Models.NpcMmsSends;
using NPC.Domain.Repository;
using OpenMas;
using log4net;

namespace NPC.Application.Services
{
    public class NpcMmsSendService
    {
        private readonly ILog _logger;
        private readonly NpcMmsSendRepository _npcMmsSendRepository;
        public NpcMmsSendService()
        {
            _logger = new DefaultLoggerFactory().GetLogger();
            _npcMmsSendRepository = new NpcMmsSendRepository();
        }
        public void Execute()
        {
            var npcMmsSends = _npcMmsSendRepository.GetNpcMmsSendsWaitingSend(DateTime.Now);
            npcMmsSends.ToList().ForEach(SingleSend);
        }

        private void SingleSend(NpcMmsSend npcMmsSend)
        {
            var trans = TransactionManager.BeginTransaction();
            trans.Begin();
            try
            {
                var mms=new Mms("");
                npcMmsSend.SendStatus = SendStatus.Done;
                trans.Commit();
            }
            catch (Exception exception)
            {
                _logger.ErrorFormat("id={0}发送时出错:{1}", npcMmsSend.Id, exception);
                trans.Rollback();
                throw;
            }
        }
    }
}
