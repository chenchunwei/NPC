using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Permission.Privileges;
using Fluent.Permission.Roles;
using NPC.Application.ManageModels.Roles;
using Remotion.Linq.Utilities;

namespace NPC.Application
{
    public class RoleAction : BaseAction
    {
        private readonly RoleRepository _roleRepository;
        private readonly PrivilegeRepository _privilegeRepository;
        public RoleAction()
        {
            _roleRepository = new RoleRepository();
            _privilegeRepository = new PrivilegeRepository();
        }
        public RoleListModel InitializeRoleListModel()
        {
            var model = new RoleListModel();
            model.Roles = _roleRepository.GetAllRoleByUnitId(NpcContext.CurrentUser.Unit.Id);
            return model;
        }

        public void EditRole(EditRoleModel editRoleModel)
        {
            var role = editRoleModel.Id.HasValue
                                ? _roleRepository.Find(editRoleModel.Id.Value)
                                : new Role();
            //判断RoleCode是否重得
            if (_roleRepository.IsCodeRepeat(editRoleModel.RoleCode,editRoleModel.UnitId, editRoleModel.Id))
                throw new ApplicationException("角色编码已被使用，请更换其它编码");
            role.Code = editRoleModel.RoleCode;
            role.Name = editRoleModel.RoleName;
            role.UnitId = editRoleModel.UnitId;
            role.Description = editRoleModel.RoleDescription;
            _roleRepository.Save(role);
        }

        public object InitializeEditRoleModel(Guid? id)
        {
            var model = new EditRoleModel();
            model.Id = id;
            if (id.HasValue)
            {
                model.Role = _roleRepository.Find(id.Value);
                model.RoleName = model.Role.Name;
                model.RoleCode = model.Role.Code;
                model.RoleDescription = model.Role.Description;
            }
            return model;
        }

        public void Delete(Guid[] toArray)
        {
            return;
        }

        public RolePrivilegeSettingsModel InitializeRolePrivilegeSettingsModel(Guid roleId)
        {
            var model = new RolePrivilegeSettingsModel();
            model.Role = _roleRepository.Find(roleId);
            model.Privileges = _privilegeRepository.GetAllPrivileges();
            return model;
        }

        public void SaveRolePrivileges(RolePrivilegeSettingsModel model)
        {
            var role = _roleRepository.Find(model.Id);
            role.Privileges.Clear();
            model.SelectedPrivileges.ToList().ForEach(privilegeId =>
            {
                var privilege = _privilegeRepository.Find(privilegeId);
                role.Privileges.Add(privilege);
            });
            _roleRepository.Save(role);
        }
    }
}
