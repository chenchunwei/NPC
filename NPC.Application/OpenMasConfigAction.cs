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
            model.MmsAppAccount = config.MmsAppAccount;
            model.MmsAppPwd = config.MmsAppPwd;
            model.MmsExtensionNo = config.MmsExtensionNo;
            model.MmsMasService = config.MmsMasService;

            model.SmsAppAccount = config.SmsAppAccount;
            model.SmsAppPwd = config.SmsAppPwd;
            model.SmsExtensionNo = config.SmsExtensionNo;
            model.SmsMasService = config.SmsMasService;

            model.Signature = config.Signature;
            return model;
        }

        public void EditOpenMasConfig(EditOpenMasConfigModel model)
        {
            var config = _openMasConfigRepository.GetOpenMasConfigByUnit(model.Unit.Id) ?? new OpenMasConfig();
            config.MmsAppAccount = model.MmsAppAccount;
            config.MmsAppPwd = model.MmsAppPwd;
            config.MmsExtensionNo = model.MmsExtensionNo;
            config.MmsMasService = model.MmsMasService;
            config.SmsAppAccount = model.SmsAppAccount;
            config.SmsAppPwd = model.SmsAppPwd;
            config.SmsExtensionNo = model.SmsExtensionNo;
            config.SmsMasService = model.SmsMasService;
            config.Unit = model.Unit;
            config.Signature = model.Signature;
            config.RecordDescription.CreateBy(NpcContext.CurrentUser);
            _openMasConfigRepository.Save(config);
        }
    }
}
