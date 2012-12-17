using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.NodeRecords;

namespace NPC.Application.MianModels
{
    public class HeaderModel
    {
        public HeaderModel()
        {
            Menus = new List<NodeRecord>();
        }
        public NodeRecord TopBanner { get; set; }
        public IList<NodeRecord> Menus { get; set; }
    }
}
