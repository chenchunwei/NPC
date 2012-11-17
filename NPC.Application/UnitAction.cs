using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Utilities;
using NPC.Application.ManageModels.Units;
using NPC.Domain.Models.Departments;
using NPC.Domain.Models.Units;
using NPC.Domain.Models.Users;
using NPC.Domain.Repository;

namespace NPC.Application
{
    public class UnitAction : BaseAction
    {
        #region 构造函数
        private readonly UnitRepository _unitRepository;
        private readonly UserRepository _userRepository;
        private readonly DepartmentRepository _departmentRepository;
        public UnitAction()
        {
            _unitRepository = new UnitRepository();
            _userRepository = new UserRepository();
            _departmentRepository = new DepartmentRepository();
        }
        #endregion

        #region 初始化树模型
        public UnitTreeModel InitializeUnitTreeModel(Guid? parentId)
        {
            var model = new UnitTreeModel();
            var subs = parentId.HasValue ? _unitRepository.GetSubUnit(parentId.Value) : _unitRepository.GetRootUnit();

            subs.ToList().ForEach(o => model.Components.Add(ConvertUnitToModel(o, true)));

            return model;
        }
        #endregion
        #region 转换unit到Model
        private UnitTreeModelComponent ConvertUnitToModel(Unit unit, bool isNeedSub)
        {
            var model = new UnitTreeModelComponent()
            {
                Id = unit.Id,
                Name = unit.Name,
                IconCls = ApplicationConst.TreeLeafCls,
            };
            var childrens = _unitRepository.GetSubUnit(unit.Id).ToList();
            if (childrens.Any())
            {
                if (isNeedSub)
                {
                    childrens.ForEach(o => model.Childrens.Add(ConvertUnitToModel(o, true)));
                }
                model.IconCls = ApplicationConst.TreeParentNode;
                model.State = isNeedSub ? "open" : "closed";
            }
            return model;
        }
        #endregion

        #region 创建新组织
        public void CreateNewUnit(EditUnitModel model)
        {
            var unit = new Unit()
            {
                Name = model.FormData.Name,
                ParentUint = model.Id.HasValue ? _unitRepository.Find(model.Id.Value) : null
            };
            _unitRepository.Save(unit);
            var department = new Department()
            {
                Name = "人民代表大会",
                Unit = unit
            };
            _departmentRepository.Save(department);
            var user = new User()
            {
                Account = "admin",
                Name = "管理员",
                Pwd = MD5Utility.GetMD5HashCode("admin"),
                Unit = unit,
                Department = department
            };
            _userRepository.Save(user);
        }
        #endregion

        #region 删除组织单位
        public void DeleteUnit(Guid id)
        {
            var unit = _unitRepository.Find(id);
            unit.RecordDescription.IsDelete = true;
            _unitRepository.Save(unit);
        }
        #endregion
    }
}
