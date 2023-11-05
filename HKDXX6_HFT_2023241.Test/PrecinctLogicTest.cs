using HKDXX6_HFT_2023241.Logic;
using HKDXX6_HFT_2023241.Models;
using HKDXX6_HFT_2023241.Repository;
using Moq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDXX6_HFT_2023241.Test
{
    [TestFixture]
    public class PrecinctLogicTest
    {
        PrecinctLogic logic;
        Mock<IRepository<Precinct>> mockRepo;

        [SetUp]
        public void Initialize()
        {
            List<Precinct> list = new List<Precinct>()
            {
                new Precinct(93,"100 Meserole Avenue"),
                new Precinct(99,"211 Union Avenue")
            };

            mockRepo = new Mock<IRepository<Precinct>>();
            mockRepo.Setup(r => r.ReadAll()).Returns(list.AsQueryable());
            mockRepo.Setup(r => r.Read(It.IsAny<int>())).Returns((int x) => list.AsQueryable().First(t => t.ID == x));

            logic = new PrecinctLogic(mockRepo.Object);
        }

        [Test]
        [TestCase(1, null)]
        [TestCase(1, "tooshort")]
        [TestCase(1, "WayTooLong,See?:" +
            "OPnnJQVVNcedtSoG2iCvORNX8439gPHXGDhFnUmkdDUt4PiR1oB4tj0SL5hK5iKA7tviJGeRA9nMSAl07Rm5Bgw2tmNflQcF")]
        [TestCase(null, "tooshort")]
        [TestCase(null, "ThisIsLongEnough")]
        [TestCase(0, "ThisIsLongEnough")]
        [TestCase(140, "ThisIsLongEnough")]
        public void CreateTest_WithIncorrectAddressValues_ThrowsArgumentException(int ID, string addr)
        {
            //Arrange
            var p = new Precinct() { ID = ID, Address = addr };

            //Act + Assert
            var ex = Assert.Throws<ArgumentException>(() => logic.Create(p));
            mockRepo.Verify(r => r.Create(p), Times.Never);
        }

        [Test]
        public void CreateTest_WithOfficersAdded_ThrowsArgumentException()
        {

            //Arrange

            List<Officer> olist = new()
            {
                //int BadgeNo, string FirstName, string LastName, Ranks Rank, int? DirectCO_BadgeNo, int PrecintID, DateTime hireDate
                new Officer(1234,"Marco","Polo",Ranks.Sergeant,null,99,DateTime.Now)
            };

            var p = new Precinct() {ID = 1, Address = "ThisIsALongAddressSoThatItPasses", Officers = olist };

            //Act + Assert
            var ex = Assert.Throws<ArgumentException>(() => logic.Create(p));
            Assert.That(ex.Message == "Officers must be empty when creating.");
            mockRepo.Verify(r => r.Create(p), Times.Never);
        }

        [Test]
        [TestCase(1,"PerfectLen")]
        [TestCase(139,"PerfectLen")]
        [TestCase(1,"BarelyRightLen:" +
            "aLFkXCuOl8S1zV20CgHJ6TzBasxwYe2D29VfHT4veftkuk0kXZLEYOLzbYN86c6jjzH5XU6KwsRV6Z2TzLA3g")]
        [TestCase(139, "BarelyRightLen:" +
            "aLFkXCuOl8S1zV20CgHJ6TzBasxwYe2D29VfHT4veftkuk0kXZLEYOLzbYN86c6jjzH5XU6KwsRV6Z2TzLA3g")]
        public void CreateTest_Correct(int id, string address)
        {
            //Arrange
            var p = new Precinct() { ID = id, Address = address };

            //Act
            logic.Create(p);

            //Assert
            mockRepo.Verify(r => r.Create(p), Times.Once);
        }

        [Test]
        public void UpdateTest_ChangeOfficerList_ThrowsArgumentException()
        {
            //Arrange
            List<Officer> olist = new()
            {
                //int BadgeNo, string FirstName, string LastName, Ranks Rank, int? DirectCO_BadgeNo, int PrecintID, DateTime hireDate
                new Officer(1234,"Marco","Polo",Ranks.Sergeant,null,99,DateTime.Now)
            };
            var p = new Precinct() { ID = 99, Address = "211 Union Avenue", Officers = olist };

            //Act+Assert
            var ex = Assert.Throws<ArgumentException>(() => logic.Update(p));
            Assert.That(ex.Message == "Officers cannot be changed from this side of the relationship.");
            mockRepo.Verify(r => r.Update(p), Times.Never);
        }

        [Test]
        [TestCase("tooshort")]
        [TestCase("WayTooLong,See?:" +
            "OPnnJQVVNcedtSoG2iCvORNX8439gPHXGDhFnUmkdDUt4PiR1oB4tj0SL5hK5iKA7tviJGeRA9nMSAl07Rm5Bgw2tmNflQcF")]
        public void UpdateTest_WithIncorrectAddressValues_ThrowsArgumentException(string addr)
        {
            //Arrange
            var p = new Precinct() { ID = 99, Address = addr};

            //Act+Assert
            var ex = Assert.Throws<ArgumentException>(() => logic.Update(p));
            Assert.That(ex.Message == "Length of address must be between 10 and 100 characters.");
            mockRepo.Verify(r => r.Update(p), Times.Never);

        }

        [Test]
        [TestCase("PerfectLen")]
        [TestCase("BarelyRightLen:" +
            "aLFkXCuOl8S1zV20CgHJ6TzBasxwYe2D29VfHT4veftkuk0kXZLEYOLzbYN86c6jjzH5XU6KwsRV6Z2TzLA3g")]
        public void UpdateTest_Correct(string addr)
        {
            //Arrange
            var p = new Precinct() { ID = 99, Address = addr };

            //Act
            logic.Update(p);

            //Assert
            mockRepo.Verify(r => r.Update(p), Times.Once);
        }
    }
}
