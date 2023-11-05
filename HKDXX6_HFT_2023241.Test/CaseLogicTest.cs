using HKDXX6_HFT_2023241.Logic;
using HKDXX6_HFT_2023241.Models;
using HKDXX6_HFT_2023241.Repository;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

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

            List<Precinct> precincts = new List<Precinct>()
            {
                new Precinct(93,"100 Meserole Avenue"),
                new Precinct(99,"211 Union Avenue")
            };

            mockPrecinctRepo.Setup(r => r.ReadAll()).Returns(precincts.AsQueryable());
            mockPrecinctRepo.Setup(r => r.Read(It.IsAny<int>())).Returns((int x) => precincts.AsQueryable().First(t => t.ID == x));

            List<Case> cases = new List<Case>()
            {
                new Case(1,"Missing ham","A Jamón Iberico ham was stolen valued at $6000. According to Charles it is an amazing cured ham from Spain.", 9544,new DateTime(2013,09,17,19,00,00)),
                new Case(2,"Blackmail of Parlov","Famous writer D.C. Parlov's manuscript of his upcoming book was stolen, and some of it was leaked online. The culprit wants a ransom or they will release the rest of the manuscript.",378, new DateTime(2013,09,17,19,00,00)),
                new Case(3,"Kidnapping of Cheddar the dog","Someone kidnapped the captain's dog, Cheddar (the fluffy boy), and demands ransom.",6382, new DateTime(2013, 09, 17, 19, 00, 00)),
                new Case()
                {
                    ID = 4,
                    Name = "TEST THING",
                    Description = "THIS IS A TEST DESC, NOTHING TO SEE HERE",
                    OpenedAt = DateTime.Now.AddDays(-1),
                    ClosedAt = DateTime.Now
                }
            };

            

            mockCaseRepo.Setup(r => r.ReadAll()).Returns(cases.AsQueryable());
            mockCaseRepo.Setup(r => r.Read(It.IsAny<int>())).Returns((int x) => cases.AsQueryable().First(t => t.ID == x));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(1000)]
        public void CreateTest_WithIncorrectCaseID_ThrowsArgumentException(int id) 
        {
            //Arrange
            var c = new Case() { ID = id, Name = "TenChars..", Description = "FifteenChars...", OpenedAt = DateTime.Now };

            //Act+Assert
            var ex = Assert.Throws<ArgumentException>(() => logic.Create(c));
            Assert.That(ex.Message == "ID is assigned by the system automatically.");
            mockCaseRepo.Verify(r => r.Create(c), Times.Never);

        }

        [Test]
        public void CreateTest_WithIncorrectName_ThrowsArgumentException()
        {
            //Arrange
            var c = new Case() { Name = "tooshort", Description = "FifteenChars...", OpenedAt = DateTime.Now };


            //Act+Assert
            var ex = Assert.Throws<ArgumentException>(() => logic.Create(c));
            Assert.That(ex.Message == "Name of case must be at least 10 characters.");
            mockCaseRepo.Verify(r => r.Create(c), Times.Never);
        }

        [Test]
        public void CreateTest_WithIncorrectDesc_ThrowsArgumentException()
        {
            //Arrange
            var c = new Case() { Name = "TenChars..", Description = "tooshort", OpenedAt = DateTime.Now };


            //Act+Assert
            var ex = Assert.Throws<ArgumentException>(() => logic.Create(c));
            Assert.That(ex.Message == "Description of case must be at least 15 characters.");
            mockCaseRepo.Verify(r => r.Create(c), Times.Never);
        }

        [Test]
        public void CreateTest_WithFutureDate_ThrowsArgumentException()
        {
            //Arrange
            var d = DateTime.Now.AddDays(1);
            var c = new Case() { Name = "TenChars..", Description = "FifteenChars...", OpenedAt = d };


            //Act+Assert
            var ex = Assert.Throws<ArgumentException>(() => logic.Create(c));
            Assert.That(ex.Message == "Cases in the future cannot be recorded.");
            mockCaseRepo.Verify(r => r.Create(c), Times.Never);
        }

        [Test]
        public void CreateTest_WithIncorrectCloseDate_ThrowsArgumentException() 
        {
            //Arrange
            var d = DateTime.Now.AddDays(-1);
            var c = new Case() { Name = "TenChars..", Description = "FifteenChars...", OpenedAt = DateTime.Now, ClosedAt = d };

            //Act+Assert
            var ex = Assert.Throws<ArgumentException>(() => logic.Create(c));
            Assert.That(ex.Message == "Case cannot be closed before it is opened.");
            mockCaseRepo.Verify(r => r.Create(c), Times.Never);
        }

        [Test]
        public void CreateTest_WithFutureCloseDate_ThrowsArgumentException()
        {
            //Arrange
            var d = DateTime.Now.AddDays(1);
            var c = new Case() { Name = "TenChars..", Description = "FifteenChars...", OpenedAt = DateTime.Now, ClosedAt = d };

            //Act+Assert
            var ex = Assert.Throws<ArgumentException>(() => logic.Create(c));
            Assert.That(ex.Message == "Case closure cannot be in the future.");
            mockCaseRepo.Verify(r => r.Create(c), Times.Never);
        }

        [Test]
        public void UpdateTest_WithClosedCaseWithoutReopening_ThrowsArgumentException()
        {
            //Arrange
            var c = new Case(4, "TEST THING", "THIS IS A TEST DESC, NOTHING TO SEE HERE", DateTime.Now.AddDays(-1));
            c.ClosedAt = DateTime.Now;


            //Act+Assert
            var ex = Assert.Throws<ArgumentException>(() => logic.Update(c));
            Assert.That(ex.Message == "Case has to be open to be updated.");
            mockCaseRepo.Verify(r => r.Update(c), Times.Never);
        }

        [Test]
        public void UpdateTest_WithIncorrectCloseDate_ThrowsArgumentException()
        {
            //Arrange
            var c = new Case(1, "Missing ham", "A Jamón Iberico ham was stolen valued at $6000. According to Charles it is an amazing cured ham from Spain.", 9544, new DateTime(2013, 09, 17, 19, 00, 00));
            c.ClosedAt = new DateTime(2000, 09, 17, 19, 00, 00);

            //Act+Assert
            var ex = Assert.Throws<ArgumentException>(() => logic.Update(c));
            Assert.That(ex.Message == "Case cannot be closed before it is opened.");
            mockCaseRepo.Verify(r => r.Update(c), Times.Never);
        }

        [Test]
        public void UpdateTest_WithFutureDate_ThrowsArgumentException()
        {
            //Arrange
            var d = DateTime.Now.AddDays(1);
            var c = new Case(1, "Missing ham", "A Jamón Iberico ham was stolen valued at $6000. According to Charles it is an amazing cured ham from Spain.", 9544, new DateTime(2013, 09, 17, 19, 00, 00));
            c.OpenedAt = d;


            //Act+Assert
            var ex = Assert.Throws<ArgumentException>(() => logic.Update(c));
            Assert.That(ex.Message == "Cases in the future cannot be recorded.");
            mockCaseRepo.Verify(r => r.Update(c), Times.Never);
        }


        [Test]
        public void UpdateTest_WithIncorrectName_ThrowsArgumentException()
        {
            //Arrange
            var c = new Case(1, "Missing ham", "A Jamón Iberico ham was stolen valued at $6000. According to Charles it is an amazing cured ham from Spain.", 9544, new DateTime(2013, 09, 17, 19, 00, 00));
            c.Name = "tooshort";

            //Act+Assert
            var ex = Assert.Throws<ArgumentException>(() => logic.Update(c));
            Assert.That(ex.Message == "Name of case must be at least 10 characters.");
            mockCaseRepo.Verify(r => r.Update(c), Times.Never);
        }

        [Test]
        public void UpdateTest_WithIncorrectDesc_ThrowsArgumentException()
        {
            //Arrange
            var c = new Case(1, "Missing ham", "A Jamón Iberico ham was stolen valued at $6000. According to Charles it is an amazing cured ham from Spain.", 9544, new DateTime(2013, 09, 17, 19, 00, 00));
            c.Description = "tooshort";

            //Act+Assert
            var ex = Assert.Throws<ArgumentException>(() => logic.Update(c));
            Assert.That(ex.Message == "Description of case must be at least 15 characters.");
            mockCaseRepo.Verify(r => r.Update(c), Times.Never);
        }

        [Test]
        public void UpdateTest_WithFutureCloseDate_ThrowsArgumentException()
        {
            //Arrange
            var c = new Case(1, "Missing ham", "A Jamón Iberico ham was stolen valued at $6000. According to Charles it is an amazing cured ham from Spain.", 9544, new DateTime(2013, 09, 17, 19, 00, 00));
            var d = DateTime.Now.AddDays(1);

            c.ClosedAt = d;

            //Act+Assert
            var ex = Assert.Throws<ArgumentException>(() => logic.Update(c));
            Assert.That(ex.Message == "Case closure cannot be in the future.");
            mockCaseRepo.Verify(r => r.Update(c), Times.Never);
        }
    }
}
