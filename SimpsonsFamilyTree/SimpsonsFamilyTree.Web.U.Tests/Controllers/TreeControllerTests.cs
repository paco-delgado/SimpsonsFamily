using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SimpsonsFamilyTree.Domain.Model;
using SimpsonsFamilyTree.Domain.Repository;
using SimpsonsFamilyTree.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpsonsFamilyTree.Web.U.Tests.Controllers
{
    [TestClass]
    public class TreeControllerTests
    {
        private Mock<IPeopleRepository> _repositoryMock;
        private TreeController _controller;

        [TestInitialize]
        public void Initialize()
        {
            _repositoryMock = new Mock<IPeopleRepository>();
            _controller = new TreeController(_repositoryMock.Object);
        }

        [TestMethod]
        public void Get_PersonFound_ReturnsParentsTree()
        {
            // Arrange
            var foundParentsTree = new ParentsTree
            {
                Id = 87,
                Name = "Homer",
                LastName = "Simpson",
                Parents = new List<ParentsTree>
                {
                    new ParentsTree
                    {
                        Id = 34,
                        Name = "Abraham",
                        LastName = "Simpson"
                    }
                }
            };
            _repositoryMock.Setup(r => r.GetParentsTree(87)).Returns(foundParentsTree);

            // Act
            var result = _controller.Get(87) as OkObjectResult;

            // Assert
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(foundParentsTree, result.Value);
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
            _repositoryMock.Setup(r => r.GetParentsTree(65)).Throws(new Exception(errorMessage));

            // Act
            var result = _controller.Get(65) as ObjectResult;

            // Assert
            Assert.AreEqual(500, result.StatusCode);
            Assert.AreEqual(errorMessage, result.Value);
        }
    }
}
