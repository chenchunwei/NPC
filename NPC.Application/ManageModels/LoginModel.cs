using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC.Application.ManageModels
{
   public class LoginModel
    {
       public LoginModel()
       {
           UnitOptions=new Dictionary<string, string>();
       }
       public IDictionary<string, string> UnitOptions { get; set; }
    }
}
