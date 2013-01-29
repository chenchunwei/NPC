﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace NPC.Application.Common
{
    public class Helper
    {
        public static Dictionary<string, string> GetProposalTypeOptions()
        {
            return new Dictionary<string, string>()
            {
              {"0","代表议案"},
              {"1","代表建议"},
              {"2","群众意见"}
            };
        }

        public static Dictionary<string, string> GetProposalStatusOptions()
        {
            return new Dictionary<string, string>()
            {
              {"1","人大常委会审核中"},
              {"2","市政办处理中"},
              {"4","主办单位处理中"},
              {"8","代表满意度回馈"},
              {"16","完成"}
            };
        }
    }
}
