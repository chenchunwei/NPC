using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC.Application.ManageModels
{
    public class RedirectMessageModel
    {
        public string Message { get; set; }
        public string ReturnUrl { get; set; }
        public string TextOfReturnUrl { get; set; }
    }
}
