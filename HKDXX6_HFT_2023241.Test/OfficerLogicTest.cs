using HKDXX6_HFT_2023241.Logic;
using HKDXX6_HFT_2023241.Models;
using HKDXX6_HFT_2023241.Repository;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
            List<Officer> list = new List<Officer>()
            {
                new Officer(1973,"Jack","Joel", Ranks.Captain,null,93,new DateTime(1980,01,01)),
                new Officer(3711,"David","Majors", Ranks.Detective,1973,93, new DateTime(2001,01,02)),
                new Officer(6382,"Raymond","Holt", Ranks.Captain,null,99, new DateTime(1980, 01, 02)),
                new Officer(378,"Terrence","Jeffords", Ranks.Sergeant,6382,99, new DateTime(1999, 03, 12)),
                new Officer(3263,"Amy","Santiago", Ranks.Sergeant,6382,99, new DateTime(2005, 02, 12)),
                new Officer(9544,"Jake","Peralta", Ranks.Detective,378,99, new DateTime(2004, 03, 10)),
                new Officer(426,"Charles","Boyle", Ranks.Detective,378,99, new DateTime(2000, 05, 16)),
                new Officer(3118,"Rosa","Diaz", Ranks.Detective,378,99, new DateTime(2004, 06, 18)),
                new Officer(18324,"Teri","Haver", Ranks.PatrolOfficer,3263,99, new DateTime(2013, 08, 06)),
                new Officer(7529,"Lou","Vargas", Ranks.PatrolOfficer,3263,99, new DateTime(2015, 03, 18)),
                new Officer(94499,"Gary","Jennings", Ranks.PatrolOfficer,3263,99, new DateTime(2016, 11, 26))
            };

            mockRepo = new Mock<IRepository<Officer>>();
            logic = new OfficerLogic(mockRepo.Object);
            mockRepo.Setup(r => r.ReadAll()).Returns(list.AsQueryable());
            mockRepo.Setup(r => r.Read(It.IsAny<int>())).Returns((int x) => list.AsQueryable().First(t => t.BadgeNo == x));
        }

        [Test]
        [TestCase("", "")]
        [TestCase("", "NoMatterWhatName")]
        [TestCase("NoMatterWhatName", "")]
        public void CreateTest_WithIncorrectNameValues_ThrowsArgumentException(string fname, string lname) 
        {
            //Arrange
            var o = new Officer() { FirstName = fname, LastName = lname, PrecinctID = 99 };

            //Act + Assert
            var ex = Assert.Throws<ArgumentException>(() => logic.Create(o));
            Assert.That(ex.Message == "First and last name must be at least two characters long");
            mockRepo.Verify(r => r.Create(o), Times.Never);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(10000)]
        public void CreateTest_WithIncorrectIDValues_ThrowsArgumentException(int ID)
        {
            //Arrange
            var o = new Officer() { BadgeNo = ID, FirstName = "LongEnough", LastName = "NameValues", PrecinctID = 99 };

            //Act + Assert
            var ex = Assert.Throws<ArgumentException>(() => logic.Create(o));
            Assert.That(ex.Message == "ID is assigned by the system automatically.");
            mockRepo.Verify(r => r.Create(o), Times.Never);
        }

        [Test]
        [TestCase(0)]
        [TestCase(null)]
        [TestCase(140)]
        public void CreateTest_WithIncorrectPrecinctIDValues_ThrowsArgumentException(int pID)
        {
            //Arrange
            var o = new Officer() { FirstName = "LongEnough", LastName = "NameValues", PrecinctID = pID };

            //Act + Assert
            var ex = Assert.Throws<ArgumentException>(() => logic.Create(o));
            Assert.That(ex.Message == "PrecinctID must be between 1 and 139 inclusively.");
            mockRepo.Verify(r => r.Create(o), Times.Never);
        }

        [Test]
        public void CreateTest_WithFutureHireDate_ThrowsArgumentException()
        {
            //Arrange
            var d = DateTime.Now.AddDays(1);
            var o = new Officer() { FirstName = "LongEnough", LastName = "NameValues", PrecinctID = 99, HireDate = d };

            //Act + Assert
            var ex = Assert.Throws<ArgumentException>(() => logic.Create(o));
            Assert.That(ex.Message == "HireDate cannot be in the future.");
            mockRepo.Verify(r => r.Create(o), Times.Never);
        }

        [Test]
        public void CreateTest_WithCasesAdded_ThrowsArgumentException()
        {
            //Arrange
            var c = new List<Case>(){ new Case() { Name = "Test" } };

            var o = new Officer() { FirstName = "LongEnough", LastName = "NameValues", PrecinctID = 99, Cases = c };

            //Act + Assert
            var ex = Assert.Throws<ArgumentException>(() => logic.Create(o));
            Assert.That(ex.Message == "Cases and OfficersUnderCommand cannot be filled when creating officer.");
            mockRepo.Verify(r => r.Create(o), Times.Never);
        }

        [Test]
        public void CreateTest_WithOfficersAdded_ThrowsArgumentException()
        {
            //Arrange
            List<Officer> olist = new()
            {
                new Officer(1234,"Marco","Polo",Ranks.Sergeant,null,99,DateTime.Now)
            };

            var o = new Officer() { FirstName = "LongEnough", LastName = "NameValues", PrecinctID = 99, OfficersUnderCommand = olist };

            //Act + Assert
            var ex = Assert.Throws<ArgumentException>(() => logic.Create(o));
            Assert.That(ex.Message == "Cases and OfficersUnderCommand cannot be filled when creating officer.");
            mockRepo.Verify(r => r.Create(o), Times.Never);
        }

        [Test]
        public void UpdateTest_WithCasesAdded_ThrowsArgumentException()
        {
            //Arrange
            var c = new List<Case>() { new Case() { Name = "Test" } };
            var o = new Officer(9544, "Jake", "Peralta", Ranks.Detective, 378, 99, new DateTime(2004, 03, 10));
            o.Cases = c;

            var ex = Assert.Throws<ArgumentException>(() => logic.Update(o));
            Assert.That(ex.Message == "Cases and OfficersUnderCommand cannot be updated from this side of the relationship.");
            mockRepo.Verify(r => r.Update(o), Times.Never);
        }

        [Test]
        public void UpdateTest_WithOfficersAdded_ThrowsArgumentException()
        {
            //Arrange
            List<Officer> olist = new()
            {
                new Officer(1234,"Marco","Polo",Ranks.Sergeant,null,99,DateTime.Now)
            };
            var o = new Officer(9544, "Jake", "Peralta", Ranks.Detective, 378, 99, new DateTime(2004, 03, 10));
            o.OfficersUnderCommand = olist;

            var ex = Assert.Throws<ArgumentException>(() => logic.Update(o));
            Assert.That(ex.Message == "Cases and OfficersUnderCommand cannot be updated from this side of the relationship.");
            mockRepo.Verify(r => r.Update(o), Times.Never);
        }

        [Test]
        public void UpdateTest_Correct_ManyFieldsChanged()
        {
            //Arrange
            var o = new Officer(9544, "Jake", "Peralta", Ranks.Detective, 378, 99, new DateTime(2004, 03, 10));
            o.FirstName = "Test";
            o.LastName = "Test";
            o.Rank = Ranks.Sergeant;
            o.DirectCO_BadgeNo = 6382;
            o.HireDate = DateTime.Now;

            //Act
            logic.Update(o);

            //Assert
            mockRepo.Verify(r => r.Update(o), Times.Once);

        }
    }
}
