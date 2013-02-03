using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Application.Common;
using NPC.Domain.Models.Users;
using NPC.Domain.Repository;

namespace NPC.Application.Services
{
    public class ProposalRoleService
    {
        public static User GetNpcAuditJieKouRen()
        {
            var unitRepository = new UnitRepository();
            var unit = unitRepository.Find(AppConfig.NpcAuditJieKouRenUnitId);
            if(unit==null)
                throw new ArgumentException("未找到配置的NpcAuditJieKouRenUnit，请核实WebConfig的配置项NpcAuditJieKouRenUnitId");
            return unit.JieKouRen;
        }

        public static User GetGovAuditJieKouRen()
        {
            var unitRepository = new UnitRepository();
            var unit = unitRepository.Find(AppConfig.GovAuditJieKouRenUnitId);
            if (unit == null)
                throw new ArgumentException("未找到配置的GovAuditJieKouRenUnit，请核实WebConfig的配置项GovAuditJieKouRenUnitId");
            return unit.JieKouRen;
        }
    }
}
