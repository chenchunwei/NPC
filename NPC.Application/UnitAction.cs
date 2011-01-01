using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using Fluent.Infrastructure.Utilities;
using Fluent.Permission.Privileges;
using Fluent.Permission.RoleUsers;
using Fluent.Permission.Roles;
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
                IsFlowUnit = unit.IsFlowUint,
                IsWebUnit = unit.IsWebUint,
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

        #region 创建或编辑组织
        public void CreateNewUnit(EditUnitModel model)
        {
            var trans = TransactionManager.BeginTransaction();
            try
            {
                var unit = new Unit()
                {
                    Name = model.FormData.Name,
                    IsFlowUint = model.FormData.IsFlowUnit,
                    IsWebUint = model.FormData.IsWebUnit,
                    ParentUint = model.ParentId.HasValue ? _unitRepository.Find(model.ParentId.Value) : null
                };
                _unitRepository.Save(unit);
                var user = new User()
                {
                    Account = "admin",
                    Name = "管理员",
                    Pwd = Md5Utility.GetMd5HashCode("admin"),
                    Unit = unit
                };
                InitUnitPermissions(unit, user);
                _userRepository.Save(user);
                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
        }

        public void UpdateUnit(EditUnitModel model)
        {
            if (model.Id == null)
                throw new ArgumentException("id不能为null");
            var trans = TransactionManager.BeginTransaction();
            try
            {
                var unit = _unitRepository.Find(model.Id.Value);
                unit.Name = model.FormData.Name;
                unit.IsWebUint = model.FormData.IsWebUnit;
                unit.IsFlowUint = model.FormData.IsFlowUnit;
                var admin = _userRepository.FindByAccount("admin", unit.Id);
                InitUnitPermissions(unit, admin);
                _unitRepository.Save(unit);
                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
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

        #region 初始化组织权限
        private void InitUnitPermissions(Unit unit, User admin)
        {
            var privilegeRepository = new PrivilegeRepository();
            var privileges = privilegeRepository.GetAllPrivileges();
            var roleRepository = new RoleRepository();
            var roles = roleRepository.GetAllRoleByUnitId(unit.Id);
            var role = roles.FirstOrDefault(r => r.Code == "SuperAdmin");
            if (role == null)
            {
                role = new Role();
                role.Name = "超级管理员";
                role.Code = "SuperAdmin";
                role.UnitId = unit.Id;
                privileges.ToList().ForEach(privilege => role.Privileges.Add(privilege));
                roleRepository.Save(role);
            }
            var roleUserRepository = new RoleUserRepository();
            var roleUser = roleUserRepository.GetRoleUserByUserId(admin.Id);
            if (roleUser == null)
            {
                roleUser = new RoleUser();
                roleUser.UserId = admin.Id;
                roleUser.Roles.Add(role);
                roleUserRepository.Save(roleUser);
            }
        }
        #endregion

        #region FlowSettings
        public UnitFlowSettingsModel InitializeUnitFlowSettingsModel(Guid id)
        {
            var unit = _unitRepository.Find(id);
            var model = new UnitFlowSettingsModel();
            model.Id = id;
            model.Unit = unit;
            if (unit.UnitFlowSettings != null)
            {
                model.NpcUnitId = unit.UnitFlowSettings.NpcUnit.Id;
                model.GovUnitId = unit.UnitFlowSettings.GovUnit.Id;
                model.SponsorUnitIdString = string.Join(",", unit.UnitFlowSettings.SponsorUnits.Select(o => o.Id));
                model.SubsidiaryUnitString = string.Join(",", unit.UnitFlowSettings.SubsidiaryUnits.Select(o => o.Id));
            }
            return model;
        }

        public void FlowSettings(UnitFlowSettingsModel unitFlowSettingsModel)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
