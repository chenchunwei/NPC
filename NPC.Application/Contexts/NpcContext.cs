using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Fluent.Infrastructure.Web.HttpMoudles;
using NPC.Domain.Models.Units;
using NPC.Domain.Repository;
using User = NPC.Domain.Models.Users.User;

namespace NPC.Application.Contexts
{
    public class NpcContext
    {
        private readonly string _keyOfNpcUser = "__________keyOfNpcUser___________";
        protected UserRepository UserRepository { get; set; }
        public NpcContext()
        {
            UserRepository = new UserRepository();
        }

        public User CurrentUser
        {
            get
            {
                if (HttpContext.Current.Items[_keyOfNpcUser] != null)
                    return HttpContext.Current.Items[_keyOfNpcUser] as User;
                var userId = AuthenticationClinet.CurrentUser.Id;
                var user = UserRepository.Find(userId);
                HttpContext.Current.Items[_keyOfNpcUser] = user;
                return user;
            }
        }

    }
}
