using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SimpsonsFamilyTree.Domain.Model;
using SimpsonsFamilyTree.Domain.Repository;
using SimpsonsFamilyTree.Web.Controllers;
using System;

// NOTE: This test class is created just as an example on how to create unit tests for the controllers.
//       Just tests for Get method are created. Tests for the rest of the actions are pending.
namespace SimpsonsFamilyTree.Web.U.Tests.Controllers
{
    [TestClass]
    public class PeopleControllerTests
    {
        private Mock<IPeopleRepository> _repositoryMock;
        private PeopleController _controller;

        [TestInitialize]
        public void Initialize()
        {
            _repositoryMock = new Mock<IPeopleRepository>();
            _controller = new PeopleController(_repositoryMock.Object);
        }

        [TestMethod]
        public void Get_PersonFound_ReturnsPerson()
        {
            // Arrange
            var foundPerson = new Person
            {
                Id = 65,
                Name = "Homer",
                LastName = "Simpson",
                BirthDate = new DateTime(1976, 12, 24)
            };
            _repositoryMock.Setup(r => r.GetPerson(65)).Returns(foundPerson);

            // Act
            var result = _controller.Get(65) as OkObjectResult;

            // Assert
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(foundPerson, result.Value);
        }

        [TestMethod]
        public void Get_PersonNotFound_ReturnsNoContent()
        {
            // Act
            var result = _controller.Get(65) as NoContentResult;

            // Assert
            Assert.AreEqual(204, result.StatusCode);
        }

        [TestMethod]
        public void Get_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            string errorMessage = "Some error occurred!";
            _repositoryMock.Setup(r => r.GetPerson(65)).Throws(new Exception(errorMessage));

            // Act
            var result = _controller.Get(65) as ObjectResult;

            // Assert
            Assert.AreEqual(500, result.StatusCode);
            Assert.AreEqual(errorMessage, result.Value);
        }
    }
}
