﻿using System;
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

                    childrens.ForEach(o => model.Childrens.Add(ConvertDepartmentToModel(o, true)));
                }
                model.IconCls = ApplicationConst.TreeParentNode;
                model.State = isNeedSub ? "open" : "closed";
            }

            return model;
        }
        #endregion

        public void DeleteDepartment(Guid id)
        {
            var department = _departmentRepository.Find(id);
            department.RecordDescription.Delete();
            _departmentRepository.Save(department);
        }

        public void CreateNewDepartment(EditDepartmentModel model)
        {
            var department = new Department();
            department.Name = model.FormData.Name;
            department.Unit = NpcContext.CurrentUser.Unit;
            if (model.ParentId.HasValue)
                department.Parent = _departmentRepository.Find(model.ParentId.Value);
            department.RecordDescription.CreateBy(NpcContext.CurrentUser);
            _departmentRepository.Save(department);
        }

        public void UpdateDepartment(EditDepartmentModel model)
        {
            if (!model.Id.HasValue)
                throw new ArgumentException("model.id不能为null");
            var department = _departmentRepository.Find(model.Id.Value);
            department.Name = model.FormData.Name;
            department.Unit = NpcContext.CurrentUser.Unit;
            department.RecordDescription.UpdateBy(NpcContext.CurrentUser);
            _departmentRepository.Save(department);
        }
    }
}
