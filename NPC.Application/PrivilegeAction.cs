using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Permission.Privileges;
using NPC.Application.ManageModels.Privileges;
using Remotion.Linq.Utilities;

namespace NPC.Application
{
    public class PrivilegeAction
    {
        private readonly PrivilegeRepository _privilegeRepository;
        public PrivilegeAction()
        {
            _privilegeRepository = new PrivilegeRepository();
        }
        public PrivilegeListModel InitializePrivilegeListModel()
        {
            var model = new PrivilegeListModel();
            model.Privileges = _privilegeRepository.GetAllPrivileges();
            return model;
        }

        public void Delete(params Guid[] ids)
        {
            if (ids != null && ids.Length > 0)
                ids.ToList().ForEach(SingleDelete);
        }

        private void SingleDelete(Guid id)
        {
            var target = _privilegeRepository.Find(id);
            //HACK:权限的删除未完成
            //target.RecordDescription.Delete();
            _privilegeRepository.SaveOrUpdate(target);
        }
        public EditPrivilegeModel InitializeEditPrivilegeModel(Guid? id)
        {
            var model = new EditPrivilegeModel();
            model.Id = id;
            if (id.HasValue)
            {
                model.Privilege = _privilegeRepository.Find(id.Value);
                model.PrivilegeName = model.Privilege.Name;
                model.PrivilegeCode = model.Privilege.Code;
                model.PrivilegeDescription = model.Privilege.Description;
            }
            return model;
        }

        public void EditPrivilege(EditPrivilegeModel editPrivilegeModel)
        {
            var privilege = editPrivilegeModel.Id.HasValue
                                ? _privilegeRepository.Find(editPrivilegeModel.Id.Value)
                                : new Privilege();
            //判断PrivilegeCode是否重得
            if (_privilegeRepository.IsCodeRepeat(editPrivilegeModel.PrivilegeCode, editPrivilegeModel.Id))
                throw new ApplicationException("权限编码已被使用，请更换其它编码");
            privilege.Code = editPrivilegeModel.PrivilegeCode;
            privilege.Name = editPrivilegeModel.PrivilegeName;
            privilege.UnitId = editPrivilegeModel.UnitId;
            privilege.Description = editPrivilegeModel.PrivilegeDescription;
            _privilegeRepository.Save(privilege);
        }
    }
}
