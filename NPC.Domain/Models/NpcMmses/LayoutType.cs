using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NPC.Domain.Models.NpcMmses
{
    public enum LayoutType
    {
        [Description("图片在上")]
        PicTop = 0,
        [Description("图片在下")]
        PicBottom = 1
    }
}
