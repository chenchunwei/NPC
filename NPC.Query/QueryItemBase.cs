﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC.Query
{
    public abstract class QueryItemBase
    {
        protected QueryItemBase()
        {
            Pagination = new Pagination();
        }
        public Pagination Pagination { get; set; }
    }
}
