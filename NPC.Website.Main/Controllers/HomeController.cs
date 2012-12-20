using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NPC.Application;

namespace NPC.Website.Main.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IndexAction _indexAction;
        public HomeController()
        {
            _indexAction=new IndexAction();
        }
        public ActionResult Index()
        {
            var model = _indexAction.InitializeIndexModel();
            return View(model);
        }

    }
}
