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
            if (id.HasValue)
            {
                var proposal = _proposalRepository.Find(id.Value);
                model.FormData.Attachment = proposal.Attachment;
                model.FormData.Content = proposal.Content;
                model.FormData.ProposalType = proposal.ProposalType;
                model.FormData.Title = proposal.Title;

            }
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
                proposal.ProposalType = model.FormData.ProposalType.Value;
                proposal.Attachment = model.FormData.Attachment;
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
                var npcAuditor = ProposalRoleService.GetNpcAuditJieKouRen(user.Unit);
                args.Add("Originator", user.Id.ToString());
                args.Add("NpcAuditor", npcAuditor.Id.ToString());
                _flowService.CreateFlowWithAssignId(proposal.Id, "ProposalFlow", user,
                    string.Format("{0}发起[{1}]议案", user.Name, MyString.SubString(model.FormData.Title, 14, "…")),
                  args, "发起流程");
                trans.Commit();
                SendMessage(NpcContext.CurrentUser.Unit.Id, npcAuditor, "人大在线平台有新的议案建议需要您审批，请登录人大互动在线及时处理！");
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
                var govAuditor = ProposalRoleService.GetGovAuditJieKouRen(proposal.RecordDescription.UserOfCreate.Unit);
                if (scNpcAuditModel.Action == ScNpcAuditAction.Submit)
                {
                    proposal.ProposalStatus = ProposalStatus.GovAuditing;
                    args.Add("GovAuditor", govAuditor.Id.ToString());
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
                    SendMessage(proposal.RecordDescription.UserOfCreate.Unit.Id, govAuditor, "人大在线平台有新的议案建议需要您审批，请登录人大互动在线及时处理！！");
                }
                else
                {
                    SendMessage(proposal.RecordDescription.UserOfCreate.Unit.Id, proposal.RecordDescription.UserOfCreate, string.Format("意见或建议[{0}]被否决退回!", proposal.Title));
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
            model.Proposal.RecordDescription.UserOfCreate.Unit.UnitFlowSettings.SponsorUnits.ToList().ForEach(unit => model.UnitOptions.Add(unit.Id.ToString(), unit.Name));
            model.Proposal.RecordDescription.UserOfCreate.Unit.UnitFlowSettings.SubsidiaryUnits.ToList().ForEach(unit => model.SubsidiaryOptions.Add(unit.Id.ToString(), unit.Name));
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

                User sponsorAuditor = null;
                if (govOfficeAuditModel.Action == GovOfficeAuditAction.Submit)
                {
                    if (govOfficeAuditModel.SponsorUnitId == null)
                        throw new ArgumentException("必须选择主办单位");
                    var sponsorUnit = unitRepository.Find(govOfficeAuditModel.SponsorUnitId.Value);
                    if (sponsorUnit.JieKouRen == null)
                        throw new ArgumentException("对不起！" + sponsorUnit.Name + "未配置流程处理接口人");
                    sponsorAuditor = sponsorUnit.JieKouRen;
                    args.Add("Sponsor", sponsorAuditor.Id.ToString());

                }

                _flowService.ExecuteTask(govOfficeAuditModel.TaskId,
                      EnumHelper.GetDescription(govOfficeAuditModel.Action),
                      NpcContext.CurrentUser, args, govOfficeAuditModel.Comment);
                trans.Commit();
                if (govOfficeAuditModel.Action == GovOfficeAuditAction.Submit)
                {
                    var govAuditor = ProposalRoleService.GetGovAuditJieKouRen(proposal.RecordDescription.UserOfCreate.Unit);
                    SendMessage(proposal.RecordDescription.UserOfCreate.Unit.Id, govAuditor, "人大在线平台有新的议案建议需要您审批，请登录人大互动在线及时处理！");
                }
                else
                {
                    SendMessage(proposal.RecordDescription.UserOfCreate.Unit.Id, sponsorAuditor, "意见或建议被否决退回,请登录人大互动在线进行处理");
                }
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

                if (sponsorAuditModel.Action == SponsorAuditAction.Finished)
                {
                    var originator = proposal.RecordDescription.UserOfCreate;
                    SendMessage(proposal.RecordDescription.UserOfCreate.Unit.Id, originator, "议案建议已处理完成，请登录人大互动在线及时处理反馈！");
                }
                else
                {
                    var govAuditor = ProposalRoleService.GetGovAuditJieKouRen(proposal.RecordDescription.UserOfCreate.Unit);
                    SendMessage(proposal.RecordDescription.UserOfCreate.Unit.Id, govAuditor, "意见或建议被否决退回,请登录人大互动在线进行处理");
                }

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

        private void SendMessage(Guid openMasUnitId, User user, string message)
        {
            if (user == null)
                return;
            try
            {
                var openConfig = new OpenMasConfigService().GetConfigOfUnit(openMasUnitId);
                if (openConfig == null)
                    return;
                var telNum = user.PhoneBookRecord != null
                                 ? user.PhoneBookRecord.Mobile
                                 : Helper.CheckRegex(user.Account, @"^(1[\d]{10},)*(1[\d]{10})$")
                                       ? user.Account
                                       : string.Empty;
                if (!string.IsNullOrEmpty(telNum))
                {
                    var returnMessage = NpcSmsSendService.SendSms(openConfig, message, new string[] { user.Account });
                    Logger.DebugFormat("发送出去的短信id={0}", returnMessage);
                }
            }
            catch (Exception exception)
            {
                Logger.ErrorFormat("发送流程任务通知时出错,ex:{0}", exception);
            }
        }
    }
}
