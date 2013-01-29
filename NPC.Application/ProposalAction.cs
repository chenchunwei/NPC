using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using Fluent.Infrastructure.Utilities;
using NPC.Application.Common;
using NPC.Application.ManageModels.Proposals;
using NPC.Domain.Models.Proposals;
using NPC.Domain.Repository;
using NPC.FlowEngine;

namespace NPC.Application
{
    public class ProposalAction : BaseAction
    {
        private readonly ProposalRepository _proposalRepository;
        private readonly UserRepository _userRepository;
        private readonly FlowService _flowService;
        private readonly FlowRepository _flowRepository;
        public ProposalAction()
        {
            _flowService = new FlowService();
            _userRepository = new UserRepository();
            _proposalRepository = new ProposalRepository();
            _flowRepository = new FlowRepository();
        }

        #region 初始化发起视图InitializeEditProposalModel
        public EditProposalModel InitializeEditProposalModel(Guid? id)
        {
            var model = new EditProposalModel();
            model.ProposalTypeOptions = Helper.GetProposalTypeOptions();
            model.CurrentUser = NpcContext.CurrentUser;
            return model;
        }
        #endregion

        #region 更新议案
        public void Update(EditProposalModel model, Guid id)
        {
            var trans = TransactionManager.BeginTransaction();
            try
            {
                if (model.FormData.ProposalType == null)
                    throw new ArgumentException("model.FormData.ProposalType不能为空");
                var proposal = _proposalRepository.Find(id);
                proposal.Title = model.FormData.Title;
                proposal.Content = model.FormData.Content;
                // proposal.ProposalType = model.FormData.ProposalType.Value;
                proposal.RecordDescription.UpdateBy(NpcContext.CurrentUser);
                var users = _userRepository.GetUsers(model.FormData.SelectedOriginatorIds.ToArray());
                users.ToList().ForEach(o => proposal.ProposalOriginators.Add(o));
                _proposalRepository.Save(proposal);
                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
        }
        #endregion

        #region 创建新议案
        public void Create(EditProposalModel model)
        {
            var trans = TransactionManager.BeginTransaction();
            try
            {
                var user = NpcContext.CurrentUser;
                //if (model.FormData.ProposalType == null)
                //    throw new ArgumentException("model.FormData.ProposalType不能为空");
                var proposal = new Proposal();
                proposal.Title = model.FormData.Title;
                proposal.Content = model.FormData.Content;
                //proposal.ProposalType = model.FormData.ProposalType.Value;
                proposal.RecordDescription.CreateBy(NpcContext.CurrentUser);
                var users = _userRepository.GetUsers(model.FormData.SelectedOriginatorIds.ToArray());
                users.ToList().ForEach(o => proposal.ProposalOriginators.Add(o));
                _proposalRepository.Save(proposal);
                var args = new Dictionary<string, string>();
                args.Add("Auditor", user.Id.ToString());
                _flowService.CreateFlowWithAssignId(proposal.Id, "ProposalFlow", user,
                    string.Format("{0}发起[{1}]议案", user.Name, MyString.SubString(model.FormData.Title, 14, "…")),
                  args, "发起流程");
                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
        }
        #endregion

        #region InitializeProposalListModel
        public ProposalListModel InitializeProposalListModel(ProposalQueryItem proposalQueryItem)
        {
            var model = new ProposalListModel();
            model.Proposals = _proposalRepository.Query(proposalQueryItem);
            model.ProposalSearchModel.ProposalQueryItem = proposalQueryItem;
            model.ProposalSearchModel.ProposalStatusesOptions = Helper.GetProposalStatusOptions();
            return model;
        }
        #endregion

        #region InitializeRequestViewModel
        public RequestViewModel InitializeRequestViewModel(Guid id)
        {
            var model = new RequestViewModel();
            model.Flow = _flowRepository.Find(id);
            model.Proposal = _proposalRepository.Find(id);
            model.Id = id;
            return model;
        }
        #endregion
    }
}
