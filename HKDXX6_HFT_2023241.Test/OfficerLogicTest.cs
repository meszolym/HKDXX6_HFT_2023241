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
        List<Precinct> precincts;
        List<Officer> officers;

        [SetUp]
        public void Initialize()
        {
            InitializePrecincts();
            InitializeOfficers();

            mockRepo = new Mock<IRepository<Officer>>();
            logic = new OfficerLogic(mockRepo.Object);
            mockRepo.Setup(r => r.ReadAll()).Returns(officers.AsQueryable());
            mockRepo.Setup(r => r.Read(It.IsAny<int>())).Returns((int x) => officers.AsQueryable().First(t => t.BadgeNo == x));
        }

        private void InitializePrecincts()
        {
            var ninethree = new Precinct(93, "100 Meserole Avenue");
            var ninenine = new Precinct(99, "211 Union Avenue");

            precincts = new List<Precinct> { ninethree, ninenine };
        }

        private void InitializeOfficers()
        {
            var Joel = new Officer(1973, "Jack", "Joel", Ranks.Captain, null, 93, new DateTime(1980, 01, 01));
            Joel.DirectCO = null;
            Joel.Precinct = precincts[0];
            precincts[0].Officers.Add(Joel);

            var Vulture = new Officer(3711, "David", "Majors", Ranks.Detective, 1973, 93, new DateTime(2001, 01, 02));
            Vulture.DirectCO = Joel;
            Vulture.Precinct = precincts[0];
            precincts[0].Officers.Add(Vulture);
            Joel.OfficersUnderCommand.Add(Vulture);

            var Holt = new Officer(6382, "Raymond", "Holt", Ranks.Captain, null, 99, new DateTime(1980, 01, 02));
            Holt.DirectCO = null;
            Holt.Precinct = precincts[1];
            precincts[1].Officers.Add(Holt);

            var Terry = new Officer(378, "Terrence", "Jeffords", Ranks.Sergeant, 6382, 99, new DateTime(1999, 03, 12));
            Terry.DirectCO = Holt;
            Terry.Precinct = precincts[1];
            precincts[1].Officers.Add(Terry);
            Holt.OfficersUnderCommand.Add(Terry);

            var Amy = new Officer(3263, "Amy", "Santiago", Ranks.Sergeant, 6382, 99, new DateTime(2005, 02, 12));
            Amy.DirectCO = Holt;
            Amy.Precinct = precincts[1];
            precincts[1].Officers.Add(Amy);
            Holt.OfficersUnderCommand.Add(Amy);

            var Jake = new Officer(9544, "Jake", "Peralta", Ranks.Detective, 378, 99, new DateTime(2004, 03, 10));
            Jake.DirectCO = Terry;
            Jake.Precinct = precincts[1];
            precincts[1].Officers.Add(Jake);
            Terry.OfficersUnderCommand.Add(Jake);

            var Charles = new Officer(426, "Charles", "Boyle", Ranks.Detective, 378, 99, new DateTime(2000, 05, 16));
            Charles.DirectCO = Terry;
            Charles.Precinct = precincts[1];
            precincts[1].Officers.Add(Charles);
            Terry.OfficersUnderCommand.Add(Charles);

            var Rosa = new Officer(3118, "Rosa", "Diaz", Ranks.Detective, 378, 99, new DateTime(2004, 06, 18));
            Rosa.DirectCO = Terry;
            Rosa.Precinct = precincts[1];
            precincts[1].Officers.Add(Rosa);
            Terry.OfficersUnderCommand.Add(Rosa);

            var Teri = new Officer(18324, "Teri", "Haver", Ranks.PatrolOfficer, 3263, 99, new DateTime(2013, 08, 06));
            Teri.DirectCO = Amy;
            Teri.Precinct = precincts[1];
            precincts[1].Officers.Add(Teri);
            Amy.OfficersUnderCommand.Add(Teri);

            var Lou = new Officer(7529, "Lou", "Vargas", Ranks.PatrolOfficer, 3263, 99, new DateTime(2015, 03, 18));
            Lou.DirectCO = Amy;
            Lou.Precinct = precincts[1];
            precincts[1].Officers.Add(Lou);
            Amy.OfficersUnderCommand.Add(Lou);

            var Gary = new Officer(94499, "Gary", "Jennings", Ranks.PatrolOfficer, 3263, 99, new DateTime(2016, 11, 26));
            Gary.DirectCO = Amy;
            Gary.Precinct = precincts[1];
            precincts[1].Officers.Add(Gary);
            Amy.OfficersUnderCommand.Add(Gary);

            officers = new List<Officer>()
            {
                Joel, Vulture, Holt, Terry, Amy, Jake, Charles, Rosa, Teri, Lou, Gary
            };
            
        }

        [Test]
        [TestCase("", "")]
        [TestCase("", "NoMatterWhatName")]
        [TestCase("NoMatterWhatName", "")]
        public void CreateTest_WithIncorrectNameValues_ThrowsArgumentException(string fname, string lname) 
        {
            //Arrange
            var o = new Officer() {BadgeNo = 1111, FirstName = fname, LastName = lname, PrecinctID = 99 };

            //Act + Assert
            var ex = Assert.Throws<ArgumentException>(() => logic.Create(o));
            Assert.That(ex.Message == "First and last name must be at least two characters long");
            mockRepo.Verify(r => r.Create(o), Times.Never);
        }

        [Test]
        [TestCase(-1)]
        public void CreateTest_WithIncorrectIDValues_ThrowsArgumentException(int ID)
        {
            //Arrange
            var o = new Officer() { BadgeNo = ID, FirstName = "LongEnough", LastName = "NameValues", PrecinctID = 99 };

            //Act + Assert
            var ex = Assert.Throws<ArgumentException>(() => logic.Create(o));
            Assert.That(ex.Message == "ID has to be positive.");
            mockRepo.Verify(r => r.Create(o), Times.Never);
        }

        [Test]
        [TestCase(0)]
        [TestCase(null)]
        [TestCase(140)]
        public void CreateTest_WithIncorrectPrecinctIDValues_ThrowsArgumentException(int pID)
        {
            //Arrange
            var o = new Officer() {BadgeNo = 1111, FirstName = "LongEnough", LastName = "NameValues", PrecinctID = pID };

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
            var o = new Officer() { BadgeNo = 1111, FirstName = "LongEnough", LastName = "NameValues", PrecinctID = 99, HireDate = d };

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

            var o = new Officer() { BadgeNo = 1111, FirstName = "LongEnough", LastName = "NameValues", PrecinctID = 99, Cases = c };

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

            var o = new Officer() { BadgeNo = 1111, FirstName = "LongEnough", LastName = "NameValues", PrecinctID = 99, OfficersUnderCommand = olist };

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
        [TestCase(0)]
        [TestCase(null)]
        [TestCase(140)]
        public void UpdateTest_WithIncorrectPrecinctID_ThrowsArgumentException(int pID)
        {
            //Arrange
            var o = new Officer(9544, "Jake", "Peralta", Ranks.Detective, 378, 99, new DateTime(2004, 03, 10));
            //old
            o.Cases = officers[5].Cases;
            o.OfficersUnderCommand = officers[5].OfficersUnderCommand;
            //new
            o.PrecinctID = pID;

            //Act + Assert
            var ex = Assert.Throws<ArgumentException>(() => logic.Update(o));
            Assert.That(ex.Message == "PrecinctID must be between 1 and 139 inclusively.");
            mockRepo.Verify(r => r.Update(o), Times.Never);
        }

        [Test]
        [TestCase("", "")]
        [TestCase("", "NoMatterWhatName")]
        [TestCase("NoMatterWhatName", "")]
        public void UpdateTest_WithIncorrectNameValues_ThrowsArgumentException(string fname, string lname)
        {
            //Arrange
            var o = new Officer(9544, "Jake", "Peralta", Ranks.Detective, 378, 99, new DateTime(2004, 03, 10));
            //old
            o.Cases = officers[5].Cases;
            o.OfficersUnderCommand = officers[5].OfficersUnderCommand;
            //new
            o.FirstName = fname;
            o.LastName = lname;

            //Act + Assert
            var ex = Assert.Throws<ArgumentException>(() => logic.Update(o));
            Assert.That(ex.Message == "First and last name must be at least two characters long");
            mockRepo.Verify(r => r.Update(o), Times.Never);
        }

        [Test]
        public void UpdateTest_WithFutureHireDate_ThrowsArgumentException()
        {
            //Arrange
            var d = DateTime.Now.AddDays(1);
            var o = new Officer(9544, "Jake", "Peralta", Ranks.Detective, 378, 99, new DateTime(2004, 03, 10));
            //old
            o.Cases = officers[5].Cases;
            o.OfficersUnderCommand = officers[5].OfficersUnderCommand;
            //new
            o.HireDate = d;

            //Act + Assert
            var ex = Assert.Throws<ArgumentException>(() => logic.Update(o));
            Assert.That(ex.Message == "HireDate cannot be in the future.");
            mockRepo.Verify(r => r.Update(o), Times.Never);
        }


        [Test]
        public void UpdateTest_Correct_ManyFieldsChanged()
        {
            //Arrange
            var o = new Officer(9544, "Jake", "Peralta", Ranks.Detective, 378, 99, new DateTime(2004, 03, 10));
            //old
            o.Cases = officers[5].Cases;
            o.OfficersUnderCommand = officers[5].OfficersUnderCommand;
            //new
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

        [Test]
        public void CreateTest_TwoCaptainsProblem_ThrowsArgumentException()
        {
            //Officer
            var o = new Officer(1111, "Marco", "Polo", Ranks.Captain, null, 99, DateTime.Now);
            o.DirectCO = null;
            o.Precinct = precincts[1];
            

            //Act + Assert
            var ex = Assert.Throws<ArgumentException>(() => logic.Create(o));
            Assert.That(ex.Message == "Cannot have two captains at one precinct.");
            mockRepo.Verify(r => r.Create(o), Times.Never);
        }

        [Test]
        public void UpdateTest_TwoCaptainsProblem_ThrowsArgumentException()
        {
            //Arrange
            var o = new Officer(9544, "Jake", "Peralta", Ranks.Detective, 378, 99, new DateTime(2004, 03, 10));
            o.Cases = officers[5].Cases;
            o.OfficersUnderCommand = officers[5].OfficersUnderCommand;
            //new
            o.Rank = Ranks.Captain;
            o.DirectCO_BadgeNo = null;
            o.DirectCO = null;
            o.Precinct = precincts[1];


            //Act + Assert
            var ex = Assert.Throws<ArgumentException>(() => logic.Update(o));
            Assert.That(ex.Message == "Cannot have two captains at one precinct.");
            mockRepo.Verify(r => r.Update(o), Times.Never);
        }

        [Test]
        public void UpdateTest_OfficersUnderCommandRedirection()
        {
            //Arrange
            var o = new Officer(3263, "Amy", "Santiago", Ranks.Sergeant, 6382, 99, new DateTime(2005, 02, 12));
            //old
            o.OfficersUnderCommand = officers[4].OfficersUnderCommand;
            o.Cases = officers[4].Cases;
            //new
            o.DirectCO_BadgeNo = 1973;
            o.DirectCO = officers[0];
            officers[0].OfficersUnderCommand.Add(o);
            o.PrecinctID = 93;
            o.Precinct = precincts[0];
            precincts[0].Officers.Add(o);

            //Act
            logic.Update(o);

            //Assert
            Assert.That(officers[8].DirectCO_BadgeNo, Is.EqualTo(6382));
            Assert.That(officers[9].DirectCO_BadgeNo, Is.EqualTo(6382));
            Assert.That(officers[10].DirectCO_BadgeNo, Is.EqualTo(6382));
            mockRepo.Verify(r => r.Update(o), Times.Once);
        }
    }
}
