﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Web.HttpMoudles;
using NPC.Domain.Models.Common;
using NPC.Domain.Models.Units;
using UserInModel= NPC.Domain.Models.Users.User;
namespace NPC.Domain.Models.PhoneBooks
{
    public class PhoneBook 
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual UserInModel User { get; set; }
        public virtual RecordDescription RecordDescription { get; set; }
        public virtual PhoneBookType PhoneBookType { get; set; }
    }
}