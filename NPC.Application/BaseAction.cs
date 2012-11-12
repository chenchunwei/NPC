using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Application.Contexts;

namespace NPC.Application
{
    public class BaseAction
    {
        public NpcContext NpcContext
        {
            get { return new NpcContext(); }
        }
    }
}
