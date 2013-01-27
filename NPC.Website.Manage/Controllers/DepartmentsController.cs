using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fluent.Infrastructure.Mvc;
using NPC.Application;
using NPC.Application.ManageModels.Departments;

namespace NPC.Website.Manage.Controllers
{
    public class DepartmentsController : CommonController
    {
        private readonly  DepartmentAction _departmentAction;
        public DepartmentsController()
        {
            _departmentAction = new DepartmentAction();
        }

        public ActionResult List()
        {
            return View();
        }

        public JsonResult GetDepartments(Guid? id)
        {
            var model = _departmentAction.InitializeDepartmentTreeModel(id);
            return new NewtonsoftJsonResult() { Data = model.Components };
        }

        [HttpPost, ActionName("EditDepartment")]
        public JsonResult EditUnitPost(EditDepartmentModel model)
        {
            try
            {
               if (model.Id.HasValue)
               {
                   _departmentAction.UpdateDepartment(model);  
               }
               else
               {
                   _departmentAction.CreateNewDepartment(model);                   
               }
            }
            catch (Exception)
            {
                return new NewtonsoftJsonResult() { Data = new { status = "failure" } };
            }
            return new NewtonsoftJsonResult() { Data = new { status = "success" } };
        }
         [HttpPost, ActionName("DeleteDepartment")]
        public JsonResult DeleteUnitPost(Guid id)
        {
            try
            {
                _departmentAction.DeleteDepartment(id);
            }
            catch (Exception)
            {
                return new NewtonsoftJsonResult() { Data = new { status = "failure" } };
            }
            return new NewtonsoftJsonResult() { Data = new { status = "success" } };
        }
    }
}
