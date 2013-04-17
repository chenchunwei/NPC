using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Application.Common;
using NPC.Domain.Models.Units;
using NPC.Domain.Models.Users;
using NPC.Domain.Repository;

namespace NPC.Application.Services
{
    public class ProposalRoleService
    {
        public static User GetNpcAuditJieKouRen(Unit unit)
        {
            if (unit.UnitFlowSettings == null)
                throw new ArgumentException(unit.Name + "未设置审批单位相关信息，无法发起议案建议！请联系管理员进行设置");
            var targetUnit = unit.UnitFlowSettings.NpcUnit;
            if (targetUnit.JieKouRen == null)
                throw new ArgumentException(targetUnit.Name + "未设置审批议案建议的接口人，请联系该单位或管理员进行设置");
            return targetUnit.JieKouRen;
        }

        public static User GetGovAuditJieKouRen(Unit unit)
        {
            if (unit.UnitFlowSettings == null)
                throw new ArgumentException(unit.Name + "未设置审批单位相关信息，无法发起议案建议！请联系管理员进行设置");
            var targetUnit = unit.UnitFlowSettings.GovUnit;
            if (targetUnit.JieKouRen == null)
                throw new ArgumentException(targetUnit.Name + "未设置审批议案建议的接口人，请联系该单位或管理员进行设置");
            return targetUnit.JieKouRen;
        }
    }
}
