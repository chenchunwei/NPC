using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NPC.Application;
using NPC.Application.Contexts;
using NPC.Application.ManageModels.OpenMasConfigs;

namespace NPC.Website.Manage.Controllers
{
    public class OpenMasConfigsController : CommonController
    {
        private readonly OpenMasConfigAction _openMasConfigAction;
        public OpenMasConfigsController()
        {
            _openMasConfigAction = new OpenMasConfigAction();
        }
        public ActionResult EditOpenMasConfig()
        {
            var model = _openMasConfigAction.InitializeEditOpenMasConfigModel(new NpcContext().CurrentUser.Unit);
            return View(model);
        }
        [HttpPost, ActionName("EditOpenMasConfig")]
        public ActionResult EditOpenMasConfigPost(EditOpenMasConfigModel model)
        {
            try
            {
                model.Unit = new NpcContext().CurrentUser.Unit;
                _openMasConfigAction.EditOpenMasConfig(model);
                return RedirectToMessage("OpenMas服务配置完成！");
            }
            catch (Exception exception)
            {
                return RedirectToMessage(exception.Message);
            }
        }
    }
}
