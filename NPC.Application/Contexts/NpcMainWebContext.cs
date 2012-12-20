using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using NPC.Domain.Models.Units;

namespace NPC.Application.Contexts
{
    public class NpcMainWebContext
    {
        public const string KeyOfUnit = "________keyOfUnit__________";
        public static Unit CurrentUnit
        {
            get
            {
                if (HttpContext.Current.Items[KeyOfUnit] != null)
                    return HttpContext.Current.Items[KeyOfUnit] as Unit;
                return null;
            }
        }
    }
}
