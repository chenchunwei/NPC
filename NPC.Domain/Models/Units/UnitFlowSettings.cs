using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC.Domain.Models.Units
{
   public class UnitFlowSettings
    {
       public UnitFlowSettings()
       {
           SubsidiaryUnits = new List<Unit>();
           SponsorUnits = new List<Unit>();
       }

       public virtual Unit Unit { get; set; }
       public virtual Guid Id { get; set; }
       public virtual Unit GovUnit { get; set; }
       public virtual Unit NpcUnit { get; set; }
       public virtual IList<Unit> SponsorUnits { get; set; }
       public virtual IList<Unit> SubsidiaryUnits { get; set; }
    }
}
