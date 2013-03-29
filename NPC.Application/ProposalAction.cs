using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using Fluent.Infrastructure.Utilities;
using NPC.Application.Common;
using NPC.Application.ManageModels.Proposals;
using NPC.Application.Services;
using NPC.Domain.Models.FlowNodeInstances;
using NPC.Domain.Models.Flows;
using NPC.Domain.Models.Proposals;
using NPC.Domain.Models.Users;
using NPC.Domain.Repository;
using NPC.FlowEngine;
using NPC.Service;

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
                if (model.FormData.ProposalType == null)
                    throw new ArgumentException("model.FormData.ProposalType不能为空");
                var proposal = new Proposal();
                proposal.ProposalStatus = ProposalStatus.NpcAuditing;
                proposal.Title = model.FormData.Title;
                proposal.Content = model.FormData.Content;
                proposal.Attachment = model.FormData.Attachment;
                proposal.ProposalType = model.FormData.ProposalType.Value;
                proposal.RecordDescription.CreateBy(NpcContext.CurrentUser);
                if (model.FormData.SelectedOriginatorIds.Any())
                {
                    var users = _userRepository.GetUsers(model.FormData.SelectedOriginatorIds.ToArray());
                    users.ToList().ForEach(o => proposal.ProposalOriginators.Add(o));
                }
                _proposalRepository.Save(proposal);
                var args = new Dictionary<string, string>();
                var npcAuditor = ProposalRoleService.GetNpcAuditJieKouRen();
                args.Add("Originator", user.Id.ToString());
                args.Add("NpcAuditor", npcAuditor.Id.ToString());
                _flowService.CreateFlowWithAssignId(proposal.Id, "ProposalFlow", user,
                    string.Format("{0}发起[{1}]议案", user.Name, MyString.SubString(model.FormData.Title, 14, "…")),
                  args, "发起流程");
                trans.Commit();
                SendMessage(npcAuditor);
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
        /// <summary>
        /// 市人大常委审批
        /// </summary>
        /// <param name="scNpcAuditModel"></param>
        public void ScNpcAudit(ScNpcAuditModel scNpcAuditModel)
        {
            var trans = TransactionManager.BeginTransaction();
            try
            {
                var args = new Dictionary<string, string>();
                var proposal = _proposalRepository.Find(scNpcAuditModel.FlowId);
                var govAuditor = ProposalRoleService.GetGovAuditJieKouRen();
                if (scNpcAuditModel.Action == ScNpcAuditAction.Submit)
                {
                    proposal.ProposalStatus = ProposalStatus.GovAuditing;
                    args.Add("GovAuditor", govAuditor.Id.ToString());
                    SendMessage(govAuditor);
                }
                else
                {
                    proposal.ProposalStatus = ProposalStatus.NpcSendBack;
                }
                _proposalRepository.Save(proposal);
                _flowService.ExecuteTask(scNpcAuditModel.TaskId,
                  EnumHelper.GetDescription(scNpcAuditModel.Action),
                  NpcContext.CurrentUser, args, scNpcAuditModel.Comment);
                trans.Commit();
                if (scNpcAuditModel.Action == ScNpcAuditAction.Submit)
                {
                    SendMessage(govAuditor);
                }
                else
                {
                    //HACK:退回的时候没有发短信
                    SendMessage(null);
                }
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
        /// <summary>
        /// 市政办审核 
        /// </summary>
        /// <param name="govOfficeAuditModel"></param>
        public void GovOfficeAudit(GovOfficeAuditModel govOfficeAuditModel)
        {

            var trans = TransactionManager.BeginTransaction();
            try
            {
                var proposal = _proposalRepository.Find(govOfficeAuditModel.FlowId);
                if (govOfficeAuditModel.Action == GovOfficeAuditAction.Submit)
                {
                    proposal.ProposalStatus = ProposalStatus.SponsorAuditing;
                }
                else
                {
                    proposal.ProposalStatus = ProposalStatus.GovSendBack;
                }
                var unitRepository = new UnitRepository();
                var args = new Dictionary<string, string>();
                User govAuditor = null;
                if (govOfficeAuditModel.Action == GovOfficeAuditAction.Submit)
                {
                    if (govOfficeAuditModel.SponsorUnitId == null)
                        throw new ArgumentException("必须选择主办单位");
                    var sponsorUnit = unitRepository.Find(govOfficeAuditModel.SponsorUnitId.Value);
                    if (sponsorUnit.JieKouRen == null)
                        throw new ArgumentException("对不起！" + sponsorUnit.Name + "未配置流程处理接口人");
                    args.Add("Sponsor", sponsorUnit.JieKouRen.Id.ToString());
                }
                _flowService.ExecuteTask(govOfficeAuditModel.TaskId,
                      EnumHelper.GetDescription(govOfficeAuditModel.Action),
                      NpcContext.CurrentUser, args, govOfficeAuditModel.Comment);
                trans.Commit();
                SendMessage(govAuditor);
            }
            catch (Exception exception)
            {
                trans.Rollback();
                throw;
            }
        }

        /// <summary>
        /// 主办单位处理
        /// </summary>
        /// <param name="sponsorAuditModel"></param>
        public void SponsorAudit(SponsorAuditModel sponsorAuditModel)
        {

            var trans = TransactionManager.BeginTransaction();
            try
            {
                var proposal = _proposalRepository.Find(sponsorAuditModel.FlowId);
                proposal.ReplyAttachment = sponsorAuditModel.ReplyAttachment;
                if (sponsorAuditModel.Action == SponsorAuditAction.Finished)
                {
                    proposal.ProposalStatus = ProposalStatus.NpcAssessmenting;
                }
                else
                {
                    proposal.ProposalStatus = ProposalStatus.SponsorSendBack;
                }
                _proposalRepository.Save(proposal);
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
        /// <summary>
        /// 满意度反馈
        /// </summary>
        /// <param name="npcAssessmentModel"></param>
        public void NpcAssessment(NpcAssessmentModel npcAssessmentModel)
        {
            var trans = TransactionManager.BeginTransaction();
            try
            {
                var proposal = _proposalRepository.Find(npcAssessmentModel.FlowId);
                if (npcAssessmentModel.Action == NpcAssessmentAction.Satisfy)
                {
                    proposal.ProposalStatus = ProposalStatus.Finished;
                }
                else
                {
                    proposal.ProposalStatus = ProposalStatus.NpcAssessmentSendBack;
                }
                _proposalRepository.Save(proposal);
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

        private void SendMessage(User user)
        {
            if (user == null)
                return;
            try
            {
                var openConfig = new OpenMasConfigService().GetConfigOfUnit(Guid.Parse(AppConfig.CommonMessageSendUnitId));
                if (Helper.CheckRegex(user.Account, @"^(1[\d]{10},)*(1[\d]{10})$"))
                {
                    var returnMessage = NpcSmsSendService.SendSms(openConfig, "人大在线平台有新的议案建议需要您审批，请及时处理！", new string[] { user.Account });
                }
            }
            catch (Exception exception)
            {
                Logger.ErrorFormat("发送流程任务通知时出错");
            }
        }
    }
}
