using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NPC.Domain.Models.PhoneBooks
{
    public enum PhoneBookType
    {
        [Description("个人通讯录")]
        Personal,
        [Description("企业通讯录W")]
        Unit
    }
}
