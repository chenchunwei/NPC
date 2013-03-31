using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fluent.Infrastructure.Mvc;
using NPC.Application;
using NPC.Application.Contexts;
using NPC.Application.ManageModels.Roles;

namespace NPC.Website.Manage.Controllers
{
    public class RolesController : CommonController
    {
        private readonly RoleAction _roleAction;
        public RolesController()
        {
            _roleAction = new RoleAction();
        }
        public ActionResult RoleList()
        {
            var model = _roleAction.InitializeRoleListModel();
            return View(model);
        }

        public ActionResult EditRole(Guid? id)
        {
            var model = _roleAction.InitializeEditRoleModel(id);
            return View(model);
        }

        [HttpPost, ActionName("EditRole")]
        public ActionResult EditRolePost(EditRoleModel editRoleModel)
        {
            try
            {
                editRoleModel.UnitId = new NpcContext().CurrentUser.Unit.Id;
                _roleAction.EditRole(editRoleModel);
            }
            catch (Exception exception)
            {
                return RedirectToMessage("保存角色时出错：" + exception.Message);
            }
            return RedirectToMessage("角色保存成功！");
        }


        public ActionResult RolePrivilegeSettings(Guid id)
        {
            var model = _roleAction.InitializeRolePrivilegeSettingsModel(id);
            return View(model);
        }
        [HttpPost, ActionName("RolePrivilegeSettings")]
        public ActionResult RolePrivilegeSettingsPost(RolePrivilegeSettingsModel model)
        {
            try
            {
                _roleAction.SaveRolePrivileges(model);
            }
             catch (Exception exception)
            {
                return RedirectToMessage("保存角色权限时出错：" + exception.Message);
            }
            return RedirectToMessage("角色权权设置成功！");
        }


        [HttpPost, ActionName("DeleteRole")]
        public ActionResult DeleteRolePost()
        {
            IList<Guid> ids = Request["ids"].Split(',').Select(o => new Guid(o)).ToList();
            _roleAction.Delete(ids.ToArray());
            return new NewtonsoftJsonResult() { Data = new { Status = "success", Message = "删除成功!" } };
        }


    }
}
