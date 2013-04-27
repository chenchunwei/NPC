using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using Fluent.Infrastructure.Log;
using Fluent.Infrastructure.Utilities;
using NPC.Domain.Models.NotifyMessages;
using NPC.Domain.Models.Proposals;
using NPC.Domain.Models.Users;
using NPC.Domain.Repository;
using NPC.FlowEngine;
using NPC.Service;
using log4net;

namespace NPC.Application.Services
{
    public class ProposalService
    {
        private readonly ProposalRepository _proposalRepository;
        private readonly NotifyMessageRepository _notifyMessageRepository;
        private readonly UserRepository _userRepository;
        private readonly FlowService _flowService;
        private readonly ILog _log;
        public ProposalService()
        {
            _flowService = new FlowService();
            _userRepository = new UserRepository();
            _proposalRepository = new ProposalRepository();
            _notifyMessageRepository = new NotifyMessageRepository();
            _log = new DefaultLoggerFactory().GetLogger();
        }

        public void FetchProposalFromMessage()
        {
            var queryItem = new NotifyMessageQueryItem();
            queryItem.IsDealed = false;
            var notifyMessages = _notifyMessageRepository.Query(queryItem);
            notifyMessages.ToList().ForEach(notifyMessage =>
            {
                var trans = TransactionManager.BeginTransaction();
                trans.Begin();
                try
                {
                    var user = _userRepository.FindByMobile(notifyMessage.From);
                    if (user != null)
                        DealMessage(notifyMessage, user);
                    notifyMessage.IsDealed = true;
                    _notifyMessageRepository.Save(notifyMessage);
                    trans.Commit();
                }
                catch (Exception exception)
                {
                    _log.ErrorFormat("处理notifyMessageId={0}时发生异常：{1}", notifyMessage.Id, exception);
                    trans.Rollback();
                }
            });
        }

        private void DealMessage(NotifyMessage notifyMessage, User user)
        {
            var proposal = new Proposal();
            proposal.Content = notifyMessage.Content;
            proposal.IsFromMessage = true;
            proposal.ProposalType = ProposalType.NpcProposal;
            proposal.RecordDescription.CreateBy(user);
            proposal.Title = string.Format("{0}", MyString.SubString(notifyMessage.Content, 24, ""));
            _proposalRepository.Save(proposal);
            var args = new Dictionary<string, string>();
            var npcAuditor = ProposalRoleService.GetNpcAuditJieKouRen(user.Unit);
            args.Add("Originator", user.Id.ToString());
            args.Add("NpcAuditor", npcAuditor.Id.ToString());
            _flowService.CreateFlowWithAssignId(proposal.Id, "ProposalFlow", user,
                string.Format("{0}发起[{1}][短信]", user.Name, proposal.Title),
              args, "发起流程");
        }
    }
}
