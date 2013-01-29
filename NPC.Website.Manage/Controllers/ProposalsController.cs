using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NPC.Application;
using NPC.Application.ManageModels.Proposals;

namespace NPC.Website.Manage.Controllers
{
    public class ProposalsController : CommonController
    {
        private readonly ProposalAction _proposalAction;
        public ProposalsController()
        {
            _proposalAction = new ProposalAction();
        }
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
        public ActionResult List(ProposalSearchModel searchModel)
        {
            searchModel.ProposalQueryItem.Pagination.PageIndex = PageIndex;
            var model = _proposalAction.InitializeProposalListModel(searchModel.ProposalQueryItem);
            return View(model);
        }

        public ActionResult RequestView(Guid id)
        {
            var model = _proposalAction.InitializeRequestViewModel(id);
            return View(model);
        }

    }
}
