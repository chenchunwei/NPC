using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NPC.Domain.Models.Units;

namespace NPC.NpcService.Host
{
    public interface IUnitLoader
    {
        Unit GetUnit();
    }
}