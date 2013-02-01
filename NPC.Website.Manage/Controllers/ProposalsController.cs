using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NPC.Application;
using NPC.Application.Contexts;
using NPC.Application.ManageModels.Proposals;
using NPC.Domain.Models.Flows;

namespace NPC.Website.Manage.Controllers
{
    public class ProposalsController : CommonController
    {
        private readonly ProposalAction _proposalAction;
        public ProposalsController()
        {
            _proposalAction = new ProposalAction();
        }

        #region 发起议案
        public ActionResult EditProposal(Guid? id)
        {
            var model = _proposalAction.InitializeEditProposalModel(id);
            return View(model);
        }
        [HttpPost, ActionName("EditProposal")]
        public ActionResult EditProposalPost(EditProposalModel model, Guid? id)
        {
            try
            {
                if (id.HasValue)
                    _proposalAction.Update(model, id.Value);
                else
                    _proposalAction.Create(model);
            }
            catch (Exception exception)
            {
                return RedirectToMessage("议案提交出错!" + exception.Message);
            }
            return RedirectToMessage("议案提交成功!");
        }
        #endregion

        #region 议案列表
        public ActionResult List(ProposalSearchModel searchModel)
        {
            searchModel.ProposalQueryItem.Pagination.PageIndex = PageIndex;
            var model = _proposalAction.InitializeProposalListModel(searchModel.ProposalQueryItem);
            return View(model);
        }
        #endregion

        #region RequestView
        public ActionResult RequestView(Guid id)
        {
            var model = _proposalAction.InitializeRequestViewModel(id);
            return View(model);
        }
        #endregion

        #region RequestView
        [HttpPost, ActionName("RequestView")]
        public ActionResult RequestViewPost(Guid id)
        {
            var user = new NpcContext().CurrentUser;
            var comment = Request["comment"];
            var model = _proposalAction.InitializeRequestViewModel(id);
            _proposalAction.AddComment(model.Flow, comment, user);
            return View(model);
        }
        #endregion

        #region ScNpcAudit
        public ActionResult ScNpcAudit(Guid taskId)
        {
            var model = _proposalAction.InitializeScNpcAuditModel(taskId);
            return View(model);
        }
        [HttpPost, ActionName("ScNpcAudit")]
        public ActionResult ScNpcAuditPost(ScNpcAuditModel scNpcAuditModel)
        {
            try
            {
                _proposalAction.ScNpcAudit(scNpcAuditModel);
            }
            catch (Exception exception)
            {
                return RedirectToMessage(HttpUtility.UrlEncode(exception.Message));
            }
            return RedirectToMessage("任务处理完成");
        }
        #endregion

        #region GovOfficeAudit
        public ActionResult GovOfficeAudit(Guid taskId)
        {
            var model = _proposalAction.InitializeGovOfficeAuditModel(taskId);
            return View(model);
        }
        [HttpPost, ActionName("GovOfficeAudit")]
        public ActionResult GovOfficeAuditPost(GovOfficeAuditModel govOfficeAuditModel)
        {
            try
            {
                _proposalAction.GovOfficeAudit(govOfficeAuditModel);
            }
            catch (Exception exception)
            {
                return RedirectToMessage(HttpUtility.UrlEncode(exception.Message));
            }
            return RedirectToMessage("任务处理完成");
        }
        #endregion

        #region 待办任务列表
        public ActionResult ProposalTasks()
        {
            var model = _proposalAction.InitializeProposalTasksModel();
            return View(model);
        }
        #endregion

        #region SponsorAudit
        public ActionResult SponsorAudit(Guid taskId)
        {
            var model = _proposalAction.InitializeSponsorAuditModel(taskId);
            return View(model);
        }
        [HttpPost, ActionName("SponsorAudit")]
        public ActionResult SponsorAuditPost(SponsorAuditModel sponsorAuditModel)
        {
            try
            {
                _proposalAction.SponsorAudit(sponsorAuditModel);
            }
            catch (Exception exception)
            {
                return RedirectToMessage(HttpUtility.UrlEncode(exception.Message));
            }
            return RedirectToMessage("任务处理完成");
        }
        #endregion
    }
}
