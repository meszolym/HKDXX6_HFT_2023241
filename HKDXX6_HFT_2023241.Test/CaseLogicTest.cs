using HKDXX6_HFT_2023241.Logic;
using HKDXX6_HFT_2023241.Models.DBModels;
using HKDXX6_HFT_2023241.Repository;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using static HKDXX6_HFT_2023241.Logic.CaseLogic;

namespace HKDXX6_HFT_2023241.Test
{
    [TestFixture]
    public class CaseLogicTest
    {
        CaseLogic logic;
        Mock<IRepository<Case>> mockCaseRepo;
        Mock<IRepository<Precinct>> mockPrecinctRepo;
        List<Officer> officers;
        List<Precinct> precincts;
        List<Case> cases;

        [SetUp]
        public void Initialize()
        {
            mockCaseRepo = new Mock<IRepository<Case>>();
            mockPrecinctRepo = new Mock<IRepository<Precinct>>();
            logic = new CaseLogic(mockCaseRepo.Object, mockPrecinctRepo.Object);

            precincts = new List<Precinct>()
            {
                new Precinct(93,"100 Meserole Avenue"),
                new Precinct(99,"211 Union Avenue")
            };

            mockPrecinctRepo.Setup(r => r.ReadAll()).Returns(precincts.AsQueryable());
            mockPrecinctRepo.Setup(r => r.Read(It.IsAny<int>())).Returns((int x) => precincts.AsQueryable().First(t => t.ID == x));

            InitializeOfficers();

            InitializeCases();

            mockCaseRepo.Setup(r => r.ReadAll()).Returns(cases.AsQueryable());
            mockCaseRepo.Setup(r => r.Read(It.IsAny<int>())).Returns((int x) => cases.AsQueryable().First(t => t.ID == x));
        }

        private void InitializeCases()
        {
            cases = new List<Case>()
            {
                new Case()
                {
                    ID = 1,
                    Name = "Missing ham",
                    Description = "A Jamón Iberico ham was stolen valued at $6000. According to Charles it is an amazing cured ham from Spain.",
                    OfficerOnCaseID = 9544,
                    OfficerOnCase = officers[0],
                    OpenedAt = new DateTime(2013,09,17,19,00,00),
                    ClosedAt = new DateTime(2013,09,17,19,30,00)
                },
                new Case()
                {
                    ID = 2,
                    Name = "Blackmail of Parlov",
                    Description = "Famous writer D.C. Parlov's manuscript of his upcoming book was stolen, and some of it was leaked online. The culprit wants a ransom or they will release the rest of the manuscript.",
                    OfficerOnCaseID = 378,
                    OfficerOnCase = officers[1],
                    OpenedAt = new DateTime(2013,09,17,19,00,00)
                },
                new Case() {
                    ID = 3,
                    Name = "Kidnapping of Cheddar the dog",
                    Description = "Someone kidnapped the captain's dog, Cheddar (the fluffy boy), and demands ransom.",
                    OfficerOnCaseID = 6382,
                    OfficerOnCase = officers[2],
                    OpenedAt = new DateTime(2013, 09, 17, 19, 00, 00)
                },
                new Case()
                {
                    ID = 4,
                    Name = "TEST THING",
                    Description = "THIS IS A TEST DESC, NOTHING TO SEE HERE",
                    OfficerOnCaseID = 6382,
                    OfficerOnCase = officers[4],
                    OpenedAt = new DateTime(2023,01,01,0,0,0),
                    ClosedAt = new DateTime(2023,01,11,0,0,0)
                },
                new Case() {
                    ID = 5,
                    Name = "TEST THING NO.2",
                    Description = "THIS IS A TEST DESC, NOTHING TO SEE HERE",
                    OpenedAt = new DateTime(2023,01,01,0,0,0),
                }
            };
        }

        private void InitializeOfficers()
        {
            Officer Holt = new Officer()
            {
                BadgeNo = 6382,
                FirstName = "Raymond",
                LastName = "Holt",
                Rank = Ranks.Captain,
                DirectCO_BadgeNo = null,
                DirectCO = null,
                PrecinctID = 99,
                Precinct = precincts[1],
                HireDate = new DateTime(1980, 01, 02)
            };

            Officer Terry = new Officer()
            {
                BadgeNo = 378,
                FirstName = "Terrence",
                LastName = "Jeffords",
                Rank = Ranks.Sergeant,
                DirectCO_BadgeNo = 6382,
                DirectCO = Holt,
                PrecinctID = 99,
                Precinct = precincts[1],
                HireDate = new DateTime(1999, 03, 12)
            };

            Officer Jake = new Officer()
            {
                BadgeNo = 9544,
                FirstName = "Jake",
                LastName = "Peralta",
                Rank = Ranks.Detective,
                DirectCO_BadgeNo = 378,
                DirectCO = Terry,
                PrecinctID = 99,
                Precinct = precincts[1],
                HireDate = new DateTime(2004, 03, 10)
            };

            Officer Joel = new Officer()
            {
                BadgeNo = 1973,
                FirstName = "Jack",
                LastName = "Joel",
                Rank = Ranks.Captain,
                DirectCO_BadgeNo = null,
                DirectCO = null,
                PrecinctID = 93,
                HireDate = new DateTime(1980, 01, 01)
            };

            Officer Vulture = new Officer()
            {
                BadgeNo = 3711,
                FirstName = "David",
                LastName = "Majors",
                Rank = Ranks.Detective,
                DirectCO_BadgeNo = 1973,
                DirectCO = Joel,
                PrecinctID = 93,
                Precinct = precincts[0],
                HireDate = new DateTime(2001, 01, 02)
            };

            officers = new List<Officer>() { Jake, Terry, Holt, Joel, Vulture };
        }

        [Test]
        [TestCase(-1)]
        public void CreateTest_WithIncorrectCaseID_ThrowsArgumentException(int id) 
        {
            //Arrange
            var c = new Case() { ID = id, Name = "TenChars..", Description = "FifteenChars...", OpenedAt = DateTime.Now };

            //Act+Assert
            var ex = Assert.Throws<ArgumentException>(() => logic.Create(c));
            Assert.That(ex.Message == "ID has to be positive or zero.");
            mockCaseRepo.Verify(r => r.Create(c), Times.Never);

        }

        [Test]
        [TestCase("tooshort")]
        [TestCase("waaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaytoolong")]
        public void CreateTest_WithIncorrectName_ThrowsArgumentException(string name)
        {
            //Arrange
            var c = new Case() {ID = 1000, Name = name, Description = "FifteenChars...", OpenedAt = DateTime.Now };


            //Act+Assert
            var ex = Assert.Throws<ArgumentException>(() => logic.Create(c));
            Assert.That(ex.Message == "Name of case must be at least 10, at most 240 characters.");
            mockCaseRepo.Verify(r => r.Create(c), Times.Never);
        }

        [Test]
        public void CreateTest_WithIncorrectDesc_ThrowsArgumentException()
        {
            //Arrange
            var c = new Case() { ID = 1000, Name = "TenChars..", Description = "tooshort", OpenedAt = DateTime.Now };


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
            var c = new Case() { ID = 1000, Name = "TenChars..", Description = "FifteenChars...", OpenedAt = d };


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
            var c = new Case() { ID = 1000, Name = "TenChars..", Description = "FifteenChars...", OpenedAt = DateTime.Now, ClosedAt = d };

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
            var c = new Case() { ID = 1000, Name = "TenChars..", Description = "FifteenChars...", OpenedAt = DateTime.Now, ClosedAt = d };

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
            var c = new Case()
            {
                ID = 2,
                Name = "Blackmail of Parlov",
                Description = "Famous writer D.C. Parlov's manuscript of his upcoming book was stolen, and some of it was leaked online. The culprit wants a ransom or they will release the rest of the manuscript.",
                OfficerOnCaseID = 378,
                OfficerOnCase = officers[1],
                OpenedAt = new DateTime(2013, 09, 17, 19, 00, 00)
            };
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
        [TestCase("tooshort")]
        [TestCase("waaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaytoolong")]

        public void UpdateTest_WithIncorrectName_ThrowsArgumentException(string name)
        {
            //Arrange
            var c = new Case(1, "Missing ham", "A Jamón Iberico ham was stolen valued at $6000. According to Charles it is an amazing cured ham from Spain.", 9544, new DateTime(2013, 09, 17, 19, 00, 00));
            c.Name = name;

            //Act+Assert
            var ex = Assert.Throws<ArgumentException>(() => logic.Update(c));
            Assert.That(ex.Message == "Name of case must be at least 10, at most 240 characters.");
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
            var c = new Case()
            {
                ID = 2,
                Name = "Blackmail of Parlov",
                Description = "Famous writer D.C. Parlov's manuscript of his upcoming book was stolen, and some of it was leaked online. The culprit wants a ransom or they will release the rest of the manuscript.",
                OfficerOnCaseID = 378,
                OfficerOnCase = officers[1],
                OpenedAt = new DateTime(2013, 09, 17, 19, 00, 00)
            };
            var d = DateTime.Now.AddDays(1);

            c.ClosedAt = d;

            //Act+Assert
            var ex = Assert.Throws<ArgumentException>(() => logic.Update(c));
            Assert.That(ex.Message == "Case closure cannot be in the future.");
            mockCaseRepo.Verify(r => r.Update(c), Times.Never);
        }

        [Test]
        public void OfficerCaseStatsTest()
        {
            //Arrange:
            var exp = new List<OfficerCaseStatistic>()
            {
                new OfficerCaseStatistic
                {
                    Officer = officers[0],
                    ClosedCases = 1,
                    OpenCases = 0
                },
                new OfficerCaseStatistic
                {
                    Officer = officers[1],
                    ClosedCases = 0,
                    OpenCases = 1
                },
                new OfficerCaseStatistic
                {
                    Officer = officers[2],
                    ClosedCases = 0,
                    OpenCases = 1
                },
                new OfficerCaseStatistic
                {
                    Officer = officers[4],
                    ClosedCases = 1,
                    OpenCases = 0
                }
            };

            //Act
            var res = logic.officerCaseStatistics().ToList();

            //Assert
            CollectionAssert.AreEquivalent(exp, res);
        }

        [Test]
        public void PrecinctCaseStatsTest()
        {
            //Arrange
            var exp = new List<PrecinctCaseStatistic>()
            {
                new PrecinctCaseStatistic
                {
                    Precinct = precincts[0],
                    OpenCases = 0,
                    ClosedCases = 1
                },
                new PrecinctCaseStatistic
                {
                    Precinct = precincts[1],
                    OpenCases = 2,
                    ClosedCases = 1
                }
            };

            //Act
            var res = logic.precinctCaseStatistics().ToList();

            //Assert
            CollectionAssert.AreEquivalent(exp, res);
        }

        [Test]
        [TestCase(1,99)]
        [TestCase(1,93)]
        [TestCase(2,99)]
        [TestCase(2,93)]
        [TestCase(3,99)]
        [TestCase(3,93)]
        [TestCase(4,99)]
        [TestCase(4,93)]
        public void AutoAssignCaseTest_WithAlreadyAssignedOrClosedCase_ThrowsInvalidOperationException(int id, int precid)
        {
            //Arrange: N/A

            //Act+Assert
            var ex = Assert.Throws<InvalidOperationException>(() => logic.AutoAssignCase(id,precid));
            Assert.That(ex.Message == "Cannot auto-assign already assigned/closed case.");
            mockCaseRepo.Verify(r => r.Update(It.IsAny<Case>()), Times.Never);
        }

        [Test]
        public void AutoAssignCaseTest93()
        {
            //Arrange:N/A

            //Act
            logic.AutoAssignCase(5, 93);

            //Assert
            Assert.That(cases[4].OfficerOnCaseID, Is.AnyOf(1973,3711));
        }

        [Test]
        public void AutoAssignCaseTest99()
        {
            //Arrange:N/A

            //Act
            logic.AutoAssignCase(5, 99);

            //Assert
            Assert.That(cases[4].OfficerOnCaseID, Is.EqualTo(9544));
        }

        [Test]
        public void OfficerCaseAverageOpenTimeTest()
        {
            //Arrange
            var exp = new List<KeyValuePair<Officer, TimeSpan>>()
            {
                new KeyValuePair<Officer, TimeSpan>(officers[4],TimeSpan.FromDays(10)),
                new KeyValuePair<Officer, TimeSpan>(officers[0],TimeSpan.FromMinutes(30))

            };

            //Act
            var res = logic.OfficerCaseAverageOpenTime().ToList();

            //Assert
            CollectionAssert.AreEquivalent(exp, res);
        }

        [Test]
        public void PrecinctCaseAverageOpenTimeTest()
        {
            //Arrange
            var exp = new List<KeyValuePair<Precinct, TimeSpan>>()
            {
                new KeyValuePair<Precinct, TimeSpan>(precincts[0], TimeSpan.FromDays(10)),
                new KeyValuePair<Precinct, TimeSpan>(precincts[1], TimeSpan.FromMinutes(30))

            };

            //Act
            var res = logic.PrecinctCaseAverageOpenTime().ToList();

            //Assert
            CollectionAssert.AreEquivalent(exp, res);
        }

        [Test]
        public void CasesOfPrecinctTest99()
        {
            //Arrange
            var exp = new List<Case>()
            {
                cases[0],
                cases[1],
                cases[2],
            };

            //Act
            var res = logic.CasesOfPrecint(99).ToList();

            //Assert
            CollectionAssert.AreEquivalent(exp, res);
        }

        [Test]
        public void CasesOfPrecinctTest93()
        {
            //Arrange
            var exp = new List<Case>()
            {
                cases[3]
            };

            //Act
            var res = logic.CasesOfPrecint(93).ToList();

            //Assert
            CollectionAssert.AreEquivalent(exp, res);
        }

        [Test]
        public void CasesOfPrecinctsTest()
        {
            //Arrange
            var exp = new List<KeyValuePair<Precinct, List<Case>>>()
            {
                new KeyValuePair<Precinct, List<Case>>(precincts[0],new List<Case>()
                {
                    cases[3]
                }),
                new KeyValuePair<Precinct, List<Case>>(precincts[1],new List<Case>()
                {
                    cases[0],
                    cases[1],
                    cases[2],
                }),
            };

            //Act
            var res = logic.CasesOfPrecincts().ToList();

            //Assert
            CollectionAssert.AreEquivalent(exp, res);
        }

    }
}
