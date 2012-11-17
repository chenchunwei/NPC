using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NPC.Application;
using NPC.Application.ManageModels.Proposals;

namespace NPC.Website.Manage.Controllers
{
    public class ProposalsController : BaseController
    {
        private readonly ProposalAction _proposalAction;
        public ProposalsController()
        {
            _proposalAction=new ProposalAction();
        }
        public ActionResult EditProposal(Guid? id)
        {
            var model = _proposalAction.InitializeEditProposalModel(id);
            return View(model);
        }

        public ActionResult List()
        {
            return View();
        }
    }
}
