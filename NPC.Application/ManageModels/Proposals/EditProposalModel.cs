using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPC.Domain.Models.Proposals;
using NPC.Domain.Models.Users;

namespace NPC.Application.ManageModels.Proposals
{
    public class EditProposalModel
    {
        public EditProposalModel()
        {
            FormData = new EditProposalModelForm();
            ProposalTypeOptions=new Dictionary<string, string>();
        }

        public EditProposalModelForm FormData { get; set; }
        public Dictionary<string, string> ProposalTypeOptions { get; set; }
        public User CurrentUser { get; set; }
    }

    public class EditProposalModelForm
    {
        public EditProposalModelForm()
        {
            SelectedOriginatorIds = new List<Guid>();
        }
        public string Title { get; set; }
        public ProposalType? ProposalType { get; set; }
        public string Content { get; set; }
        public IList<Guid> SelectedOriginatorIds { get; set; }
        public string Attachment { get; set; }
    }
}
