using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NPC.Application.Contexts;
using NPC.Application.ManageModels.Configs;
using NPC.Domain.Repository;

namespace NPC.Website.Manage.Controllers
{
    public class ConfigsController : CommonController
    {
        private readonly UnitRepository _unitRepository;
        private readonly UserRepository _userRepository;
        public ConfigsController()
        {
            _unitRepository = new UnitRepository();
            _userRepository = new UserRepository();
        }
        public ActionResult JieKouRenSettings()
        {
            var model = new JieKouRenSettingsModel();
            model.Unit = new NpcContext().CurrentUser.Unit;
            model.JieKouRenId = model.Unit.JieKouRen != null ? model.Unit.JieKouRen.Id : default(Guid?);
            model.AliasName = model.Unit.AliasName;
            return View(model);
        }
        [HttpPost, ActionName("JieKouRenSettings")]
        public ActionResult JieKouRenSettingsPost(JieKouRenSettingsModel model)
        {
            try
            {
                if (model.JieKouRenId == null)
                    throw new ArgumentException("必须设置接口人");
                var unit = new NpcContext().CurrentUser.Unit;
                unit.JieKouRen = _userRepository.Find(model.JieKouRenId.Value);
                unit.AliasName = model.AliasName;
                _unitRepository.Save(unit);
            }
            catch (Exception exception)
            {
                RedirectToMessage(exception.Message);
            }
            return RedirectToMessage("设置完成！");

        }
    }
}
