using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Utilities;
using Fluent.Permission.RoleUsers;
using Fluent.Permission.Roles;
using NPC.Application.Contexts;
using NPC.Application.ManageModels.Users;
using NPC.Domain.Models.Departments;
using NPC.Domain.Models.PhoneBooks;
using NPC.Domain.Models.Units;
using NPC.Domain.Models.Users;
using NPC.Domain.Repository;
using NPC.Service;
using Newtonsoft.Json;

namespace NPC.Application
{
    public class UserAction : BaseAction
    {
        #region 构造函数及私有成员
        private readonly UnitRepository _unitRepository;
        private readonly DepartmentRepository _departmentRepository;
        private readonly UserRepository _userRepository;
        private readonly PhoneBookRecordRepository _phoneBookRecordRepository;
        public UserAction()
        {
            _unitRepository = new UnitRepository();
            _departmentRepository = new DepartmentRepository();
            _userRepository = new UserRepository();
            _phoneBookRecordRepository = new PhoneBookRecordRepository();
        }
        #endregion

        #region unit gridtree Model
        public SelectUserOptionsModel InitializeSelectUserOptionsModelWithUnit(Guid? unitId)
        {
            var model = new SelectUserOptionsModel();

            if (unitId.HasValue)
            {
                _unitRepository.GetSubUnit(unitId.Value).ToList().ForEach(o => model.SelectUserOptionsRows.Add(ConvertUnit2SelectUserOptionsRow(o)));
                _departmentRepository.GetRootDepartment(unitId.Value)
                    .ToList().ForEach(o => model.SelectUserOptionsRows.Add(ConvertDepartment2SelectUserOptionsRow(o)));
            }
            else
            {
                _unitRepository.GetRootUnit().ToList().ForEach(o => model.SelectUserOptionsRows.Add(ConvertUnit2SelectUserOptionsRow(o)));
            }
            return model;
        }

        private static SelectUserOptionsComponent ConvertUnit2SelectUserOptionsRow(Unit unit)
        {
            return new SelectUserOptionsComponent()
            {
                Id = unit.Id,
                Name = unit.Name,
                NodeType = 0,
                IconCls = ApplicationConst.TreeParentNode
            };
        }
        #endregion

        #region department gridtree Model
        public SelectUserOptionsModel InitializeSelectUserOptionsModelWithDepartment(Guid? departmentId)
        {
            var model = new SelectUserOptionsModel();
            if (departmentId.HasValue)
            {
                _departmentRepository.GetSubDepartment(NpcContext.CurrentUser.Unit.Id, departmentId.Value)
                    .ToList().ForEach(o => model.SelectUserOptionsRows.Add(ConvertDepartment2SelectUserOptionsRow(o)));
                _userRepository.GetUserByDeparment(departmentId.Value)
                    .ToList().ForEach(o => model.SelectUserOptionsRows.Add(ConvertUser2SelectUserOptionsRow(o)));
            }
            else
            {
                _departmentRepository.GetRootDepartment(NpcContext.CurrentUser.Unit.Id)
                    .ToList().ForEach(o => model.SelectUserOptionsRows.Add(ConvertDepartment2SelectUserOptionsRow(o)));

            }
            return model;
        }

        private static SelectUserOptionsComponent ConvertDepartment2SelectUserOptionsRow(Department department)
        {
            return new SelectUserOptionsComponent()
            {
                Id = department.Id,
                Name = department.Name,
                NodeType = 1,
                IconCls = ApplicationConst.TreeParentNode
            };
        }
        #endregion

        #region user gridtree Model
        public SelectUserOptionsModel InitializeSelectUserOptionsModelWithUser(Guid departmentId)
        {
            var users = _userRepository.GetUserByDeparment(departmentId);
            var model = new SelectUserOptionsModel();
            users.ToList().ForEach(o => model.SelectUserOptionsRows.Add(ConvertUser2SelectUserOptionsRow(o)));
            return model;
        }

        private static SelectUserOptionsComponent ConvertUser2SelectUserOptionsRow(User user)
        {
            return new SelectUserOptionsComponent()
            {
                Id = user.Id,
                Name = user.Name,
                NodeType = 2,
                State = "open",
                IconCls = ApplicationConst.TreeParentNode
            };
        }
        #endregion

        #region 转换用户数据成用户列表
        public IList<UserViewModel> ConvertToUserList(string selectedJson)
        {
            var selectedComponents = JsonConvert.DeserializeObject<IList<SelectUserOptionsComponent>>(selectedJson);
            var groups = selectedComponents.ToLookup(o => o.NodeType);
            var users = new List<User>();

            foreach (var @group in groups.Where(@group => @group.Key == 2))
            {
                users = _userRepository.GetUsers(@group.Select(o => o.Id).ToArray()).ToList();
            }
            var returns = new List<UserViewModel>();
            users.ForEach(o => returns.Add(new UserViewModel() { Id = o.Id, Name = o.Name }));
            return returns;
        }
        #endregion

        #region 初始化修改密码视图
        public EditPasswordModel InitializeEditPasswordModel()
        {
            var model = new EditPasswordModel();
            model.User = new NpcContext().CurrentUser;
            return model;
        }
        #endregion

        #region 修改密码
        public void EditPassword(EditPasswordModel model)
        {
            var user = new NpcContext().CurrentUser;
            if (user.Pwd != Md5Utility.GetMd5HashCode(model.OldPwd.Trim()))
                throw new ArgumentException("旧密码不正确");
            if (model.NewPwd.Trim() != model.ReNewPwd.Trim())
                throw new ArgumentException("两次密码输入不正确");
            user.Pwd = Md5Utility.GetMd5HashCode(model.ReNewPwd);
            user.RecordDescription.UpdateBy(user);
            _userRepository.Save(user);
        }
        #endregion

        #region InitializeEditUserModel
        public EditUserModel InitializeEditUserModel(Guid? id)
        {
            var model = new EditUserModel();
            if (id.HasValue)
            {
                var user = _userRepository.Find(id.Value);
                model.FormData.Account = user.Account;
                model.FormData.DepartmentId = user.Department != null ? user.Department.Id : default(Guid?);
                model.FormData.Mobile = user.PhoneBookRecord != null ? user.PhoneBookRecord.Mobile : string.Empty;
                model.FormData.Name = user.Name;
                model.FormData.QQ = user.QQ;
                model.FormData.OrderSort = user.OrderSort;
                var roleUserRepository = new RoleUserRepository();
                var roleUser = roleUserRepository.GetRoleUserByUserId(id.Value);
                if (roleUser != null)
                    model.FormData.RoleNames = string.Join(",", roleUser.Roles.Select(o => o.Code));
            }
            var roleRepository = new RoleRepository();
            model.Roles = roleRepository.GetAllRoleByUnitId(NpcContext.CurrentUser.Unit.Id);
            return model;
        }
        #endregion

        #region UpdateUser
        public void UpdateUser(EditUserModel viewModel)
        {
            var userInContext = NpcContext.CurrentUser;
            if (viewModel.Id == null)
                throw new ArgumentException("id不能为空");
            var user = _userRepository.Find(viewModel.Id.Value);
            FillUser(user, viewModel);
            if (user.PhoneBookRecord == null)
            {
                var phoneBookService = new PhoneBookService();
                var phoneBook = phoneBookService.CreateOrGetDefaultPhoneBook(userInContext.Unit);
                var phoneBookRecord = new PhoneBookRecord();
                phoneBookRecord.Mobile = viewModel.FormData.Mobile;
                phoneBookRecord.Name = viewModel.FormData.Name;
                phoneBookRecord.User = user;
                phoneBookRecord.PhoneBook = phoneBook;
                phoneBookRecord.RecordDescription.CreateBy(userInContext);
                _phoneBookRecordRepository.Save(phoneBookRecord);
                user.PhoneBookRecord = phoneBookRecord;
            }
            else
            {
                user.PhoneBookRecord.Name = viewModel.FormData.Name;
                user.PhoneBookRecord.RecordDescription.UpdateBy(userInContext);
                _phoneBookRecordRepository.Save(user.PhoneBookRecord);
            }
            _userRepository.Save(user);
            if (!string.IsNullOrEmpty(viewModel.FormData.RoleNames))
                SaveOrUpdateRoleUser(user.Id, viewModel.FormData.RoleNames.Split(new[] { ',' }));
        }
        #endregion

        #region NewUser
        public void NewUser(EditUserModel viewModel)
        {
            var userInContext = NpcContext.CurrentUser;
            if (_userRepository.IsRepeatAccount(viewModel.FormData.Mobile.Trim(), userInContext.Unit.Id))
            {
                throw new ArgumentException(string.Format("手机{0}已被使用,请选择其它的手机号码", viewModel.FormData.Mobile));
            }
            var user = new User();
            user.Account = viewModel.FormData.Mobile;
            FillUser(user, viewModel);
            user.Unit = userInContext.Unit;
            user.RecordDescription.CreateBy(userInContext);
            _userRepository.Save(user);
            var phoneBookService = new PhoneBookService();
            var phoneBook = phoneBookService.CreateOrGetDefaultPhoneBook(userInContext.Unit);
            var phoneBookRecord = new PhoneBookRecord();
            phoneBookRecord.Mobile = viewModel.FormData.Mobile;
            phoneBookRecord.Name = viewModel.FormData.Name;
            phoneBookRecord.User = user;
            phoneBookRecord.PhoneBook = phoneBook;
            phoneBookRecord.RecordDescription.CreateBy(userInContext);
            _phoneBookRecordRepository.Save(phoneBookRecord);
            user.PhoneBookRecord = phoneBookRecord;
            _userRepository.Save(user);
            if (!string.IsNullOrEmpty(viewModel.FormData.RoleNames))
                SaveOrUpdateRoleUser(user.Id, viewModel.FormData.RoleNames.Split(new[] { ',' }));
        }
        #endregion

        #region new RoleUser
        private void SaveOrUpdateRoleUser(Guid userId, IList<string> roleCodes)
        {
            if (roleCodes == null || !roleCodes.Any())
                return;
            var roleUserRepository = new RoleUserRepository();
            var roleRepository = new RoleRepository();
            var roleUser = roleUserRepository.GetRoleUserByUserId(userId) ?? new RoleUser();
            roleUser.Roles.Clear();
            roleUser.UserId = userId;
            var roles = roleRepository.GetRolesByCodes(roleCodes);
            roles.ToList().ForEach(role => roleUser.Roles.Add(role));
            roleUserRepository.Save(roleUser);
        }
        #endregion

        #region FillUser
        private void FillUser(User user, EditUserModel viewModel)
        {
            if (viewModel.FormData.DepartmentId == null)
                throw new ArgumentException("所属部门不能为空");
            user.Name = viewModel.FormData.Name;
            if (!(string.IsNullOrEmpty(viewModel.FormData.Pwd) || string.IsNullOrEmpty(viewModel.FormData.RePwd)) && viewModel.FormData.Pwd == viewModel.FormData.RePwd)
            {
                user.Pwd = Md5Utility.GetMd5HashCode(viewModel.FormData.Pwd);
            }
            user.QQ = viewModel.FormData.QQ;
            user.OrderSort = viewModel.FormData.OrderSort;
            user.Department = _departmentRepository.Find(viewModel.FormData.DepartmentId.Value);
        }
        #endregion

        #region InitializeUserListModel
        public UserListModel InitializeUserListModel(UserQueryItem userQueryItem)
        {
            var model = new UserListModel();
            model.Users = _userRepository.Query(userQueryItem);
            model.UserSearchModel.UserQueryItem = userQueryItem;
            var roleUserRepository = new RoleUserRepository();
            model.RoleUsers = roleUserRepository.GetRoleUsersByUserIds(model.Users.Select(o => o.Id));
            return model;
        }
        #endregion

        #region 实始化选择视图InitializeSelectUserByDepartmentsModel
        public UserListModel InitializeSelectUserByDepartmentsModel(UserQueryItem queryItem)
        {
            var model = new UserListModel();
            var users = _userRepository.Query(queryItem);
            model.Users = users;
            model.UserSearchModel.UserQueryItem = queryItem;
            return model;
        }
        #endregion

        #region select users response  4 ajax
        public SelectedUsersResponse InitializeSelectedUsersResponse(SelectedUsersModel selectedUsersModel)
        {
            var model = new SelectedUsersResponse();
            var queryItem = new UserQueryItem();
            queryItem.Pagination.PageSize = 100;
            if (selectedUsersModel.CheckedAllPage)
            {
                queryItem.DepartmentLikeId = selectedUsersModel.WhereOptions.DepartmentLikeId;
                queryItem.Name = selectedUsersModel.WhereOptions.Name;
            }
            else
            {
                if (!selectedUsersModel.Ids.Any())
                    return model;
                queryItem.Ids = selectedUsersModel.Ids;
            }
            queryItem.UnitId = selectedUsersModel.UnitId;
            _userRepository.Query(queryItem).ToList().ForEach(user =>
                model.UserViewModels.Add(new UserViewModel() { Id = user.Id, Name = user.Name }));
            return model;
        }
        #endregion

        #region InitializeInteractiveModel
        public InteractiveModel InitializeInteractiveModel(Guid untiId)
        {
            var model = new InteractiveModel();
            model.Departments = _departmentRepository.GetRootDepartment(untiId).ToList();
            model.Departments.ToList().ForEach(dept =>
            {
                var users = _userRepository.GetUserByDeparment(dept.Id);
                model.DepartmentUsers.Add(dept, users);
            });
            return model;
        }
        #endregion


        #region 删除用户
        public void Delete(params Guid[] ids)
        {
            if (ids != null && ids.Length > 0)
                ids.ToList().ForEach(SingleDelete);
        }
        private void SingleDelete(Guid id)
        {
            var target = _userRepository.Find(id);
            target.RecordDescription.Delete();
            _userRepository.SaveOrUpdate(target);
        }
        #endregion
    }
}
