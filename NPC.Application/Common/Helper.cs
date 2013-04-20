using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using NPC.Domain.Models.Proposals;
using NPC.Domain.Models.Users;
using NPC.Domain.Repository;


namespace NPC.Application.Common
{
    public class Helper
    {
        public static Dictionary<string, string> GetProposalTypeOptions()
        {
            return new Dictionary<string, string>()
            {
              {ProposalType.NpcProposal.ToString(),"意见建议"},
              {ProposalType.NpcSuggest.ToString(),"议案建议"}
            };
        }

        public static User GetUser(Guid userId)
        {
            return new UserRepository().Find(userId);
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

        /// <summary>
        /// 使用正则表达式判断信息的格式是否正确
        /// </summary>
        public static bool CheckRegex(string text, string regex)
        {
            var reg = new Regex(regex);
            return reg.IsMatch(text);
        }
    }
}
