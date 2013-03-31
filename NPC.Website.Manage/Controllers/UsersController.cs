using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fluent.Infrastructure.Mvc;
using NPC.Application;
using NPC.Application.Contexts;
using NPC.Application.ManageModels.Users;
using Newtonsoft.Json;

namespace NPC.Website.Manage.Controllers
{
    public class UsersController : CommonController
    {
        private readonly UserAction _userAction;

        public UsersController()
        {
            _userAction = new UserAction();
        }

        #region 选择用户

        public ActionResult SelectUser()
        {
            return View();
        }

      
        [HttpPost]
        public JsonResult ParseSelectedToUsers(string selectedJson)
        {
            return new NewtonsoftJsonResult() { Data = _userAction.ConvertToUserList(selectedJson) };
        }

        #endregion

        #region 选择用户(SelectUserByDepartments)
        public ActionResult SelectUserByDepartments(UserSearchModel userSearchModel)
        {
            userSearchModel.UserQueryItem.Pagination.PageIndex = PageIndex;
            userSearchModel.UserQueryItem.UnitId = new NpcContext().CurrentUser.Unit.Id;
            var model = _userAction.InitializeSelectUserByDepartmentsModel(userSearchModel.UserQueryItem);
            return View(model);
        }

        public JsonResult ParseSelectedToUsersByDepartments(string selectedJson)
        {
            var selectedUsersModel = JsonConvert.DeserializeObject<SelectedUsersModel>(selectedJson);
            selectedUsersModel.UnitId = new NpcContext().CurrentUser.Unit.Id;
            var model = _userAction.InitializeSelectedUsersResponse(selectedUsersModel);
            return new NewtonsoftJsonResult() { Data = model };
        }
        #endregion

        #region 修改密码

        public ActionResult EidtPassword()
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

        #endregion

        #region 添加或编辑用户

        public ActionResult EditUser(Guid? id)
        {
            var model = _userAction.InitializeEditUserModel(id);
            model.Id = id;
            return View(model);
        }

        [HttpPost, ActionName("EditUser")]
        public ActionResult EditUserPost(EditUserModel viewModel)
        {
            try
            {
                if (viewModel.Id.HasValue)
                {
                    _userAction.UpdateUser(viewModel);
                }
                else
                {
                    _userAction.NewUser(viewModel);
                }
                return RedirectToMessage("用户信息保存成功！");
            }
            catch (Exception exception)
            {
                return RedirectToMessage("保存用户时发生错误：" + exception.Message);
            }
        }

        #endregion

        #region 用户列表
        public ActionResult UserList(UserSearchModel userSearchModel)
        {
            userSearchModel.UserQueryItem.Pagination.PageIndex = PageIndex;
            userSearchModel.UserQueryItem.UnitId = new NpcContext().CurrentUser.Unit.Id;
            var model = _userAction.InitializeUserListModel(userSearchModel.UserQueryItem);
            return View(model);
        }
        #endregion

        #region Interactive
        public ActionResult Interactive()
        {
            var untiId = new NpcContext().CurrentUser.Unit.Id;
            var model = _userAction.InitializeInteractiveModel(untiId);
            return View(model);
        }
        #endregion

        #region 删除记录
        [HttpPost, ActionName("Delete")]
        public JsonResult Delete()
        {
            IList<Guid> ids = Request["ids"].Split(',').Select(o => new Guid(o)).ToList();
            _userAction.Delete(ids.ToArray());
            return new NewtonsoftJsonResult() { Data = new { Status = "success", Message = "删除成功!" } };
        }
        #endregion
    }
}
