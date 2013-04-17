using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.Units;

namespace NPC.Domain.Models.UnitDomains
{
    public class UnitDomain
    {
        public UnitDomain()
        {
            Units = new List<Unit>();
        }
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual IList<Unit> Units { get; set; }
    }
}
