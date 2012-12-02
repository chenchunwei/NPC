using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain;
using NPC.Domain.Models.FlowNodeInstances;
using NPC.Domain.Models.Common;
using NPC.Domain.Models.FlowTypes;
using NPC.Domain.Models.Users;

namespace NPC.Domain.Models.Flows
{
    public class Flow : IAggregateRoot
    {
        public Flow()
        {
            RecordDescription = new RecordDescription();
            FlowDataFields = new List<FlowDataField>();
            FlowNodeInstances = new List<FlowNodeInstance>();
        }
        public virtual Guid Id { get; set; }
        public virtual string Title { get; set; }
        public virtual User UserOfFlowAdmin { get; set; }
        public virtual FlowType FlowType { get; set; }
        public virtual FlowStatus FlowStatus { get; set; }
        public virtual DateTime? DateTimeofFinished { get; set; }
        public virtual RecordDescription RecordDescription { get; set; }
        public virtual IList<FlowDataField> FlowDataFields { get; set; }
        public virtual IList<FlowNodeInstance> FlowNodeInstances { get; set; }

        public virtual void Finished()
        {
            FlowStatus = FlowStatus.Finished;
            RecordDescription.DateOfLastestModify = DateTime.Now;
            DateTimeofFinished = DateTime.Now;
        }

        public virtual void WriteDataFields(Dictionary<string, string> args)
        {
            if (args == null)
                return;
            args.ToList().ForEach(pair =>
            {
                var dataField = FlowDataFields.SingleOrDefault(o => o.Name == pair.Key);
                if (dataField != null)
                {
                    dataField.Value = pair.Value;
                    return;
                }
                FlowDataFields.Add(new FlowDataField()
               {
                   Value = pair.Value,
                   Name = pair.Key
               });
            });
        }
    }
}
