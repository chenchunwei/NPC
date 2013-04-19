using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NPC.Domain.Repository;
using NPC.Website.Common;

namespace NPC.NpcService.Host.UnitLoaders
{
    public class HttpContextUnitLoader : IUnitLoader
    {
        public Domain.Models.Units.Unit GetUnit()
        {
            if (!UnitMapping.UnitId.HasValue)
                return null;
            return new UnitRepository().Find(UnitMapping.UnitId.Value);
        }
    }
}