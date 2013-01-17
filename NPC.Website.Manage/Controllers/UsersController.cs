using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fluent.Infrastructure.Mvc;
using NPC.Application;
using NPC.Application.ManageModels.Users;

namespace NPC.Website.Manage.Controllers
{
    public class UsersController : CommonController
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
        [HttpPost]
        public JsonResult ParseSelectedToUsers(string selectedJson)
        {
            return new NewtonsoftJsonResult() { Data = _userAction.ConvertToUserList(selectedJson) };
        }

        public ActionResult EidtPassword( )
        {
            var model = _userAction.InitializeEditPasswordModel();
            return View(model);
        }
        [HttpPost, ActionName("EidtPassword")]
        public ActionResult EidtPasswordPost(EditPasswordModel model)
        {
            try
            {
                _userAction.EditPassword(model);
                return RedirectToMessage("密码修改成功！");
            }
            catch (Exception exception)
            {
                return RedirectToMessage("修改密码时发生错误：" + exception.Message);
            }
        }
    }
}
