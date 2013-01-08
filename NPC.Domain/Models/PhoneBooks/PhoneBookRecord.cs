using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain;
using NPC.Domain.Models.Common;

namespace NPC.Domain.Models.PhoneBooks
{
    public class PhoneBookRecord : IAggregateRoot
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Mobile { get; set; }
        public virtual PhoneBook PhoneBook { get; set; }
        public virtual RecordDescription RecordDescription { get; set; }
    }
}