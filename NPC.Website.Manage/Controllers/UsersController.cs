using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fluent.Infrastructure.Mvc;
using NPC.Application;

namespace NPC.Website.Manage.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserAction _userAction;
        public UsersController()
        {
            _userAction = new UserAction();
        }
        public ActionResult SelectUser()
        {
            return View();
        }
        public JsonResult GetSelectUserOptionsWithUnit(Guid? id)
        {
            var model = _userAction.InitializeSelectUserOptionsModelWithUnit(id);
            return new NewtonsoftJsonResult() { Data = model.SelectUserOptionsRows };
        }
        public JsonResult GetSelectUserOptionsWithDepartment(Guid? id)
        {
            var model = _userAction.InitializeSelectUserOptionsModelWithDepartment(id);
            return new NewtonsoftJsonResult() { Data = model.SelectUserOptionsRows };
        }
        public JsonResult GetSelectUserOptionsWithUser(Guid id)
        {
            var model = _userAction.InitializeSelectUserOptionsModelWithUser(id);
            return new NewtonsoftJsonResult() { Data = model.SelectUserOptionsRows };
        }
    }
}
