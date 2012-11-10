using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NPC.Domain.Models.Units
{
    public enum UnitStatus
    {
        [Description("启用")]
        Enable = 1,
        [Description("禁用")]
        Disable = 2
    }
}
