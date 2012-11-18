using System;
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
    }
}
