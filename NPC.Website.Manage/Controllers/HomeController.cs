using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NPC.Application;
using NPC.Application.Contexts;

namespace NPC.Website.Manage.Controllers
{
    public class HomeController : CommonController
    {
        private readonly ManageHomeAction _manageHomeAction;
        public HomeController()
        {
            _manageHomeAction = new ManageHomeAction();
        }
        public ActionResult Login()
        {
            var model = _manageHomeAction.InitializeLoginModel();
            return View(model);
        }
        public ActionResult Index()
        {
            ViewBag.SessionId = Session.SessionID;
            return View();
        }

        public ActionResult ManageMenus()
        {
            if (System.Configuration.ConfigurationManager.AppSettings["ManageUnitId"] != new NpcContext().CurrentUser.Unit.Id.ToString())
                return new EmptyResult();
            return PartialView("_ManageMenus");
        }
    }
}
