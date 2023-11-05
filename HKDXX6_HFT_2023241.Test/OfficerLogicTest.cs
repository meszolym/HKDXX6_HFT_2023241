using HKDXX6_HFT_2023241.Logic;
using HKDXX6_HFT_2023241.Models;
using HKDXX6_HFT_2023241.Repository;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDXX6_HFT_2023241.Test
{
    [TestFixture]
    public class OfficerLogicTest
    {
        OfficerLogic logic;
        Mock<IRepository<Officer>> mockRepo;

        [SetUp]
        public void Initialize()
        {
            mockRepo = new Mock<IRepository<Officer>>();
            logic = new OfficerLogic(mockRepo.Object);
        }

        [Test]
        public void Test() { }
    }
}
