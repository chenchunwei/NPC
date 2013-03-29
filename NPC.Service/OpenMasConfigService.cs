using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.OpenMasConfigs;
using NPC.Domain.Repository;

namespace NPC.Service
{
    public class OpenMasConfigService
    {
        private readonly OpenMasConfigRepository _openMasConfigRepository;
        private readonly IDictionary<Guid, OpenMasConfig> _openMasConfigs;

        public OpenMasConfigService()
        {
            _openMasConfigs=new Dictionary<Guid, OpenMasConfig>();
            _openMasConfigRepository=new OpenMasConfigRepository();
        }

        #region 获取彩信配置文件
        public OpenMasConfig GetConfigOfUnit(Guid unitId)
        {
            OpenMasConfig config;
            if (_openMasConfigs.ContainsKey(unitId))
            {
                config = _openMasConfigs[unitId];
            }
            else
            {
                config = _openMasConfigRepository.GetOpenMasConfigByUnit(unitId);
                _openMasConfigs.Add(unitId, config);
            }
            return config;
        }
        #endregion
    }
}
