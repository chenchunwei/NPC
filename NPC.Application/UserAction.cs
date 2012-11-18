using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Application.ManageModels.Users;
using NPC.Domain.Models.Departments;
using NPC.Domain.Models.Units;
using NPC.Domain.Models.Users;
using NPC.Domain.Repository;

namespace NPC.Application
{
    public class UserAction : BaseAction
    {
        private readonly UnitRepository _unitRepository;
        private readonly DepartmentRepository _departmentRepository;
        private readonly UserRepository _userRepository;
        public UserAction()
        {
            _unitRepository = new UnitRepository();
            _departmentRepository = new DepartmentRepository();
            _userRepository = new UserRepository();
        }

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
    }
}
