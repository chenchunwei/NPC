﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NPC.Domain.Models.Proposals;

namespace NPC.Domain.Model.Mappings.Proposals
{
    public class ProposalMap : ClassMap<Proposal>
    {
        public ProposalMap()
        {
            Id(o => o.Id).GeneratedBy.GuidComb();
            Map(o => o.Title);
            Map(o => o.ProposalType).CustomType<ProposalType>();
            Map(o => o.Content);
            Component(o => o.RecordDescription);
            HasManyToMany(o => o.ProposalOriginators).ParentKeyColumn("ProposalId").ChildKeyColumn("UserId");
            HasManyToMany(o => o.Tasks).ParentKeyColumn("ProposalId").ChildKeyColumn("TaskId");
        }
    }
}
