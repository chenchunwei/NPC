using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.NpcMmses;

namespace NPC.Application.ManageModels.NpcMmses
{
    public class EditNpcMmsContentModel
    {
        public EditNpcMmsContentModel()
        {
            NpcMmsContentJsons = new List<string>();
            NpcMmsContents=new List<NpcMmsContent>();
        }
        public IList<string> NpcMmsContentJsons { get; set; }

        public IList<NpcMmsContent> NpcMmsContents { get; set; }
    }

}
