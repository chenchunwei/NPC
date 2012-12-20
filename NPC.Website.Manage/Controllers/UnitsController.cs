using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fluent.Infrastructure.Mvc;
using NPC.Application;
using NPC.Application.ManageModels.Units;

namespace NPC.Website.Manage.Controllers
{
    public class UnitsController : CommonController
    {
        private readonly UnitAction _unitAction;
        public UnitsController()
        {
            _unitAction = new UnitAction();
        }

        public ActionResult List()
        {
            return View();
        }

        public JsonResult GetUnits(Guid? id)
        {
            var model = _unitAction.InitializeUnitTreeModel(id);
            return new NewtonsoftJsonResult() { Data = model.Components };
        }

        [HttpPost, ActionName("EditUnit")]
        public JsonResult EditUnitPost(EditUnitModel model)
        {
            try
            {
                _unitAction.CreateNewUnit(model);
            }
            catch (Exception)
            {
                return new NewtonsoftJsonResult() { Data = new { status = "failure" } };
            }
            return new NewtonsoftJsonResult() { Data = new { status = "success" } };
        }
        [HttpPost, ActionName("DeleteUnit")]
        public JsonResult DeleteUnitPost(Guid id)
        {
            try
            {
                _unitAction.DeleteUnit(id);
            }
            catch (Exception)
            {
                return new NewtonsoftJsonResult() { Data = new { status = "failure" } };
            }
            return new NewtonsoftJsonResult() { Data = new { status = "success" } };
        }
    }
}
