using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fluent.Infrastructure.Mvc;
using NPC.Application;
using NPC.Application.Contexts;
using NPC.Application.ManageModels.Privileges;

namespace NPC.Website.Manage.Controllers
{
    public class PrivilegesController : CommonController
    {
        private readonly PrivilegeAction _privilegeAction;
        public PrivilegesController()
        {
            _privilegeAction = new PrivilegeAction();
        }
        public ActionResult PrivilegeList()
        {
            var model = _privilegeAction.InitializePrivilegeListModel();
            return View(model);
        }

        [HttpPost, ActionName("DeletePrivilege")]
        public ActionResult DeletePrivilegePost()
        {
            IList<Guid> ids = Request["ids"].Split(',').Select(o => new Guid(o)).ToList();
            _privilegeAction.Delete(ids.ToArray());
            return new NewtonsoftJsonResult() { Data = new { Status = "success", Message = "删除成功!" } };
        }

        public ActionResult EditPrivilege(Guid? id)
        {
            var model = _privilegeAction.InitializeEditPrivilegeModel(id);
            return View(model);
        }

        [HttpPost, ActionName("EditPrivilege")]
        public ActionResult EditPrivilegePost(EditPrivilegeModel model)
        {
            try
            {
                model.UnitId = new NpcContext().CurrentUser.Unit.Id;
                _privilegeAction.EditPrivilege(model);
            }
            catch (Exception exception)
            {
                return RedirectToMessage("保存权时出错：" + exception.Message);
            }
            return RedirectToMessage("权限保存成功！");
        }
    }
}
