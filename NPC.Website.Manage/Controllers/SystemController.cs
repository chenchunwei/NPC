using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NPC.Application.ManageModels;

namespace NPC.Website.Manage.Controllers
{
    public class SystemController : BaseController
    {
        public ActionResult Message(RedirectMessageModel model)
        {
            return View(model);
        }
    }
}
