using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NPC.Domain.Models.Units;
using NPC.Domain.Repository;

namespace NPC.Domian.Repositories.Tests
{
    [TestClass]
    public class UnitRepositoryTests
    {
        [TestMethod]
        public void TestAdd()
        {
            var unitRepository = new UnitRepository();
            Unit unit = new Unit();
            unit.Name = "平湖市人大";
            unit.BannerImgUrl = "dd";
            unitRepository.Save(unit);

        }

        [TestMethod]
        public void TestGet()
        {
            var unitRepository = new UnitRepository();
            var root = unitRepository.GetRootUnit();
            Assert.IsNotNull(root);
        }
    }
}
