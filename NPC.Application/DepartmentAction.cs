using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Application.ManageModels.Departments;
using NPC.Domain.Models.Departments;
using NPC.Domain.Repository;

namespace NPC.Application
{
    public class DepartmentAction : BaseAction
    {
        private readonly DepartmentRepository _departmentRepository;
        public DepartmentAction()
        {
            _departmentRepository = new DepartmentRepository();
        }
        #region 获取树对象 
        public DepartmentTreeModel InitializeDepartmentTreeModel(Guid? id)
        {
            var model = new DepartmentTreeModel();
            var unitId = NpcContext.CurrentUser.Unit.Id;
            var subs = id.HasValue ? _departmentRepository.GetSubDepartment(unitId, id.Value) : _departmentRepository.GetRootDepartment(unitId);
            subs.ToList().ForEach(o => model.Components.Add(ConvertDepartmentToModel(o, true)));
            return model;
        }
        #endregion
        #region 转换Department到Model
        private DepartmentTreeModelComponent ConvertDepartmentToModel(Department department, bool isNeedSub)
        {
            var model = new DepartmentTreeModelComponent()
            {
                Id = department.Id,
                Name = department.Name,
                IconCls = ApplicationConst.TreeLeafCls,
            };
            var childrens = _departmentRepository.GetSubDepartment(NpcContext.CurrentUser.Unit.Id, department.Id).ToList();
            if (childrens.Any())
            {
                if (isNeedSub)
                {
    
                    childrens.ForEach(o => model.Childrens.Add(ConvertDepartmentToModel(o, false)));
                }
                model.IconCls = ApplicationConst.TreeParentNode;
                model.State = isNeedSub ? "open" : "closed";
            }
         
            return model;
        }
        #endregion

        public void DeleteDepartment(Guid id)
        {
            throw new NotImplementedException();
        }

        public void CreateNewDepartment(EditDepartmentModel model)
        {
            throw new NotImplementedException();
        }
    }
}
