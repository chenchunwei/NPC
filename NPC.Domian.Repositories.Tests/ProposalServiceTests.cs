using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NPC.Service;

namespace NPC.Domian.Repositories.Tests
{
    [TestClass]
    public class ProposalServiceTests
    {
        [TestMethod]
        public void TestFetchProposalFromMessage()
        {
            var service = new ProposalService();
            service.FetchProposalFromMessage();
        }
    }
}
