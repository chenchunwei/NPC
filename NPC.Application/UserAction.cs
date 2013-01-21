using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Utilities;
using NPC.Application.Contexts;
using NPC.Application.ManageModels.Users;
using NPC.Domain.Models.Departments;
using NPC.Domain.Models.PhoneBooks;
using NPC.Domain.Models.Units;
using NPC.Domain.Models.Users;
using NPC.Domain.Repository;
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
            if (user.Pwd != MD5Utility.GetMD5HashCode(model.OldPwd.Trim()))
                throw new ArgumentException("旧密码不正确");
            if (model.NewPwd.Trim() != model.ReNewPwd.Trim())
                throw new ArgumentException("两次密码输入不正确");
            user.Pwd = MD5Utility.GetMD5HashCode(model.ReNewPwd);
            user.RecordDescription.UpdateBy(user);
            _userRepository.Save(user);
        }
        #endregion

        public EditUserModel InitializeEditUserModel(Guid? id)
        {
            var model = new EditUserModel();
            if (id.HasValue)
            {
                var user = _userRepository.Find(id.Value);
                model.FormData.Account = user.Account;
                model.FormData.DepartmentId = user.Department != null ? user.Department.Id : default(Guid?);
                model.FormData.Mobile = string.Empty;
                model.FormData.Name = user.Name;
                model.FormData.QQ = string.Empty;
            }
            return model;
        }

        public void UpdateUser(EditUserModel viewModel)
        {
            var userInContext = NpcContext.CurrentUser;
            if (viewModel.Id == null)
                throw new ArgumentException("id不能为空");
            var user = _userRepository.Find(viewModel.Id.Value);
            FillUser(user, viewModel);
            if (user.PhoneBookRecord == null)
            {
                var phoneBookRecord = new PhoneBookRecord();
                phoneBookRecord.Mobile = viewModel.FormData.Mobile;
                phoneBookRecord.Name = viewModel.FormData.Name;
                phoneBookRecord.User = user;
                phoneBookRecord.RecordDescription.CreateBy(userInContext);
                _phoneBookRecordRepository.Save(phoneBookRecord);
                user.PhoneBookRecord = phoneBookRecord;
            }
            else
            {
                user.PhoneBookRecord.Mobile = viewModel.FormData.Mobile;
                user.PhoneBookRecord.Name = viewModel.FormData.Name;
                user.PhoneBookRecord.RecordDescription.UpdateBy(userInContext);
                _phoneBookRecordRepository.Save(user.PhoneBookRecord);
            }
            _userRepository.Save(user);
        }

        public void NewUser(EditUserModel viewModel)
        {
            var userInContext = NpcContext.CurrentUser;
            if (_userRepository.IsRepeatAccount(viewModel.FormData.Account, userInContext.Unit.Id))
            {
                throw new ArgumentException(string.Format("帐号{0}已被使用,请选择其它的帐号名", viewModel.FormData.Account));
            }
            var user = new User();
            user.Account = viewModel.FormData.Account;
            FillUser(user, viewModel);
            user.Unit = userInContext.Unit;
            user.RecordDescription.CreateBy(userInContext);
            _userRepository.Save(user);
            var phoneBookRecord = new PhoneBookRecord();
            phoneBookRecord.Mobile = viewModel.FormData.Mobile;
            phoneBookRecord.Name = viewModel.FormData.Name;
            phoneBookRecord.User = user;
            phoneBookRecord.RecordDescription.CreateBy(userInContext);
            _phoneBookRecordRepository.Save(phoneBookRecord);
            user.PhoneBookRecord = phoneBookRecord;
            _userRepository.Save(user);
        }

        private void FillUser(User user, EditUserModel viewModel)
        {
            user.Name = viewModel.FormData.Name;
            if (!(string.IsNullOrEmpty(viewModel.FormData.Pwd) || string.IsNullOrEmpty(viewModel.FormData.RePwd)) && viewModel.FormData.Pwd == viewModel.FormData.RePwd)
            {
                user.Pwd = MD5Utility.GetMD5HashCode(viewModel.FormData.Pwd);
            }
            user.QQ = viewModel.FormData.QQ;
            user.Department = _departmentRepository.Find(viewModel.FormData.DepartmentId.Value);
        }

        public UserListModel InitializeUserListModel(UserQueryItem userQueryItem)
        {
            var model = new UserListModel();
            model.Users = _userRepository.Query(userQueryItem);
            return model;
        }
    }
}
