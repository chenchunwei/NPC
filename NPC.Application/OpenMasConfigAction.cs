using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Application.Contexts;
using NPC.Application.ManageModels.OpenMasConfigs;
using NPC.Domain.Models.OpenMasConfigs;
using NPC.Domain.Models.Units;
using NPC.Domain.Repository;

namespace NPC.Application
{
    public class OpenMasConfigAction : BaseAction
    {
        private readonly OpenMasConfigRepository _openMasConfigRepository;
        public OpenMasConfigAction()
        {
            _openMasConfigRepository = new OpenMasConfigRepository();
        }
        public EditOpenMasConfigModel InitializeEditOpenMasConfigModel(Unit unit)
        {
            var model = new EditOpenMasConfigModel();
            var config = _openMasConfigRepository.GetOpenMasConfigByUnit(unit.Id);
            if (config == null)
                return model;
            model.AppAccount = config.AppAccount;
            model.AppPwd = config.AppPwd;
            model.ExtensionNo = config.ExtensionNo;
            model.MasService = config.MasService;
            model.Signature = config.Signature;
            return model;
        }

        public void EditOpenMasConfig(EditOpenMasConfigModel model)
        {
            var config = _openMasConfigRepository.GetOpenMasConfigByUnit(model.Unit.Id) ?? new OpenMasConfig();
            config.AppAccount = model.AppAccount;
            config.AppPwd = model.AppPwd;
            config.ExtensionNo = model.ExtensionNo;
            config.MasService = model.MasService;
            config.Unit = model.Unit;
            config.Signature = model.Signature;
            config.RecordDescription.CreateBy(NpcContext.CurrentUser);
            _openMasConfigRepository.Save(config);
        }
    }
}
