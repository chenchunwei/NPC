using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.NpcMmses;

namespace NPC.Application.ManageModels.NpcMmsSends
{
    public class EditNpcMmsSendModel
    {
        public Guid NpcMmsId { get; set; }
        public NpcMms NpcMms { get; set; }
        public string SendTitle { get; set; }
        public DateTime? TimeOfExpectSend { get; set; }
        public IList<string> Receivers { get; set; }
        public  string ReceiversStr { get; set; }
    }
}
