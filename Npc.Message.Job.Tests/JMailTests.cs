using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NPC.Application;
using NPC.Application.MianModels.Homes;

namespace Npc.Message.Job.Tests
{
    [TestClass]
    public class JMailTests
    {
        [TestMethod]
        public void TestSmtp()
        {
            var indexAction=new IndexAction();
            indexAction.Contribute(new ContributeModel());
            
        }
    }
}
