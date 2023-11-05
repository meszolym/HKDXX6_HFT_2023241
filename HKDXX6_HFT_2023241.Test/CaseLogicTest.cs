using HKDXX6_HFT_2023241.Logic;
using HKDXX6_HFT_2023241.Models;
using HKDXX6_HFT_2023241.Repository;
using Moq;
using NUnit.Framework;
using System;

namespace HKDXX6_HFT_2023241.Test
{
    [TestFixture]
    public class CaseLogicTest
    {
        CaseLogic logic;
        Mock<IRepository<Case>> mockCaseRepo;
        Mock<IRepository<Precinct>> mockPrecinctRepo;

        [SetUp]
        public void Initialize()
        {
            mockCaseRepo = new Mock<IRepository<Case>>();
            mockPrecinctRepo = new Mock<IRepository<Precinct>>();
            logic = new CaseLogic(mockCaseRepo.Object, mockPrecinctRepo.Object);
        }

        [Test]
        public void Test() { }
    }
}
