using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using Fluent.Infrastructure.Utilities;
using NPC.Application.Common;
using NPC.Application.ManageModels.Proposals;
using NPC.Domain.Models.FlowNodeInstances;
using NPC.Domain.Models.Flows;
using NPC.Domain.Models.Proposals;
using NPC.Domain.Models.Users;
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
        private readonly FlowNodeInstanceTaskRepository _flowNodeInstanceTaskRepository;
        public ProposalAction()
        {
            _flowService = new FlowService();
            _userRepository = new UserRepository();
            _proposalRepository = new ProposalRepository();
            _flowRepository = new FlowRepository();
            _flowNodeInstanceTaskRepository = new FlowNodeInstanceTaskRepository();
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
                args.Add("Originator", user.Id.ToString());
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

        public ScNpcAuditModel InitializeScNpcAuditModel(Guid taskId)
        {
            var model = new ScNpcAuditModel();
            var task = _flowNodeInstanceTaskRepository.Find(taskId);
            if (task != null && task.UserId == NpcContext.CurrentUser.Id && !task.IsOpened)
            {
                task.IsOpened = true;
                _flowNodeInstanceTaskRepository.Save(task);
            }
            model.Flow = task.FlowNodeInstance.BelongsFlow;
            model.Proposal = _proposalRepository.Find(model.Flow.Id);
            model.TaskId = taskId;
            return model;
        }

        public void ScNpcAudit(ScNpcAuditModel scNpcAuditModel)
        {
            var trans = TransactionManager.BeginTransaction();
            try
            {
                var args = new Dictionary<string, string>();
                args.Add("NpcAuditor", NpcContext.CurrentUser.Id.ToString());
                _flowService.ExecuteTask(scNpcAuditModel.TaskId,
                  EnumHelper.GetDescription(scNpcAuditModel.Action),
                  NpcContext.CurrentUser, args, scNpcAuditModel.Comment);
                trans.Commit();
            }
            catch (Exception exception)
            {
                trans.Rollback();
                throw;
            }
        }

        public ProposalTasksModel InitializeProposalTasksModel()
        {
            var model = new ProposalTasksModel();
            var queryItem = new FlowNodeInstanceTaskQueryItem();
            queryItem.UserId = NpcContext.CurrentUser.Id;
            queryItem.TaskStatus = TaskStatus.Executing | TaskStatus.Created;
            model.FlowNodeInstanceTasks = _flowNodeInstanceTaskRepository.Query(queryItem);
            var ids = model.FlowNodeInstanceTasks.ToList().Select(o => o.FlowNodeInstance.BelongsFlow.Id).ToList();
            model.Proposals = _proposalRepository.Find(ids);
            return model;
        }

        public void AddComment(Flow flow, string comment, User user)
        {
            var history = new FlowHistory()
            {
                Action = "备注",
                Comment = HttpUtility.HtmlEncode(comment),
                Stage = "备注"
            };
            history.RecordDescription.CreateBy(user);
            flow.FlowHistories.Add(history);
            _flowRepository.Save(flow);
        }

        public GovOfficeAuditModel InitializeGovOfficeAuditModel(Guid taskId)
        {
            var unitRepository = new UnitRepository();
            var model = new GovOfficeAuditModel();
            var task = _flowNodeInstanceTaskRepository.Find(taskId);
            if (task != null && task.UserId == NpcContext.CurrentUser.Id && !task.IsOpened)
            {
                task.IsOpened = true;
                _flowNodeInstanceTaskRepository.Save(task);
            }
            model.Flow = task.FlowNodeInstance.BelongsFlow;
            model.Proposal = _proposalRepository.Find(model.Flow.Id);
            model.TaskId = taskId;
            unitRepository.GetFlowUnits().ToList().ForEach(unit => model.UnitOptions.Add(unit.Id.ToString(), unit.Name));
            return model;
        }

        public void GovOfficeAudit(GovOfficeAuditModel govOfficeAuditModel)
        {
            if (govOfficeAuditModel.SponsorUnitId == null)
                throw new ArgumentException("必须选择主办单位");
            var trans = TransactionManager.BeginTransaction();
            try
            {
                var unitRepository = new UnitRepository();
                var args = new Dictionary<string, string>();
                var sponsorUnit = unitRepository.Find(govOfficeAuditModel.SponsorUnitId.Value);
                if (sponsorUnit.JieKouRen == null)
                    throw new ArgumentException("对不起！" + sponsorUnit.Name + "未配置流程处理接口人");
                args.Add("SponsorId", sponsorUnit.JieKouRen.Id.ToString());
                _flowService.ExecuteTask(govOfficeAuditModel.TaskId,
                      EnumHelper.GetDescription(govOfficeAuditModel.Action),
                      NpcContext.CurrentUser, args, govOfficeAuditModel.Comment);
                trans.Commit();
            }
            catch (Exception exception)
            {
                trans.Rollback();
                throw;
            }
        }

        public void SponsorAudit(SponsorAuditModel sponsorAuditModel)
        {

            var trans = TransactionManager.BeginTransaction();
            try
            {
                var args = new Dictionary<string, string>();
                _flowService.ExecuteTask(sponsorAuditModel.TaskId,
                      EnumHelper.GetDescription(sponsorAuditModel.Action),
                      NpcContext.CurrentUser, args, sponsorAuditModel.Comment);
                trans.Commit();
            }
            catch (Exception exception)
            {
                trans.Rollback();
                throw;
            }
        }

        public SponsorAuditModel InitializeSponsorAuditModel(Guid taskId)
        {
            var model = new SponsorAuditModel();
            var task = _flowNodeInstanceTaskRepository.Find(taskId);
            if (task != null && task.UserId == NpcContext.CurrentUser.Id && !task.IsOpened)
            {
                task.IsOpened = true;
                _flowNodeInstanceTaskRepository.Save(task);
            }
            model.Flow = task.FlowNodeInstance.BelongsFlow;
            model.Proposal = _proposalRepository.Find(model.Flow.Id);
            model.TaskId = taskId;
            return model;
        }

        public void NpcAssessment(NpcAssessmentModel npcAssessmentModel)
        {
            var trans = TransactionManager.BeginTransaction();
            try
            {
                var args = new Dictionary<string, string>();
                _flowService.ExecuteTask(npcAssessmentModel.TaskId,
                      EnumHelper.GetDescription(npcAssessmentModel.Action),
                      NpcContext.CurrentUser, args, npcAssessmentModel.Comment);
                trans.Commit();
            }
            catch (Exception exception)
            {
                trans.Rollback();
                throw;
            }
        }

        public NpcAssessmentModel InitializeNpcAssessmentModel(Guid taskId)
        {
            var model = new NpcAssessmentModel();
            var task = _flowNodeInstanceTaskRepository.Find(taskId);
            if (task != null && task.UserId == NpcContext.CurrentUser.Id && !task.IsOpened)
            {
                task.IsOpened = true;
                _flowNodeInstanceTaskRepository.Save(task);
            }
            model.Flow = task.FlowNodeInstance.BelongsFlow;
            model.Proposal = _proposalRepository.Find(model.Flow.Id);
            model.TaskId = taskId;
            return model;
        }
    }
}
