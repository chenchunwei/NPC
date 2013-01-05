using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NPC.Domain.Models.NpcMmsSends;
using NPC.Domain.Repository;

namespace NPC.Domian.Repositories.Tests
{
    [TestClass]
    public class NpcMmsSendRepositoryTests
    {
        [TestMethod]
        public void TestMmsSendSave()
        {
            var npcMmsRepository = new NpcMmsRepository();
            var unitRepository = new UnitRepository();
            var npcMmsSendRepository = new NpcMmsSendRepository();
            var npcMmsSend = new NpcMmsSend();
            var mms = npcMmsRepository.Find(Guid.Parse("fc224427-5730-4438-8494-a13b00dba61c"));
            npcMmsSend.NpcMms = mms;
            npcMmsSend.Title = "新年快乐";
            npcMmsSend.Unit = mms.Unit;
            npcMmsSend.RecordDescription.CreateBy(null);
            npcMmsSendRepository.Save(npcMmsSend);
        }
    }
}
