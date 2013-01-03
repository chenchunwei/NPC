using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using NPC.Domain.Models.OpenMasConfigs;

namespace NPC.Domain.Repository
{
    public class OpenMasConfigRepository : AbstractNhibernateRepository<Guid, OpenMasConfig>
    {
        public OpenMasConfig GetOpenMasConfigByUnit(Guid unitId)
        {
            return Session.CreateSQLQuery("select * from OpenMasConfigs where UnitId=:unitId").AddEntity(typeof (OpenMasConfig))
                .SetGuid("unitId", unitId)
                .UniqueResult<OpenMasConfig>();
        }
    }
}
