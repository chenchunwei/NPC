using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.NodeRecords;
using NPC.Domain.Models.Units;

namespace NPC.Application.MainTownModels
{
    public class HeaderModel
    {
        public HeaderModel()
        {
            Menus = new List<NodeRecord>();
        }

        public Unit Unit { get; set; }
        public NodeRecord TopBanner { get; set; }
        public IList<NodeRecord> Menus { get; set; }
    }
}
