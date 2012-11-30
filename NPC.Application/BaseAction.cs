using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Log;
using NPC.Application.Contexts;
using log4net;

namespace NPC.Application
{
    public class BaseAction
    {
        protected readonly ILog Logger;

        public BaseAction()
        {
            Logger = new DefaultLoggerFactory().GetLogger();
        }

        public NpcContext NpcContext
        {
            get { return new NpcContext(); }
        }
    }
}
