using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using LeaveNotifierApplication.Api.Controllers;
using LeaveNotifierApplication.Api.Models;
using LeaveNotifierApplication.Api.Tests.UnitTests.Shared;
using LeaveNotifierApplication.Data;
using LeaveNotifierApplication.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace LeaveNotifierApplication.Api.Tests.UnitTests.Controllers
{
    public class LeavesControllerTests
    {
        [Fact]
        public void Get_ReturnsOkResult_WithTheListOfAllTheLeaves()
        {
            // Arrange
            var leaves = GetLeaves();
            // Mock the Repo
            var mockRepo = new Mock<ILeaveNotifierRepository>();
            mockRepo.Setup(repo => repo.GetAllLeaves()).Returns(leaves);

            // Mock the User
            var user = new LeaveNotifierUser()
            {
                UserName = "sudyok",
                FirstName = "Sudyok",
                LastName = "Mati"
            };
            var mockUserManager = CommonMocks.GetUserManagerMock();
            mockUserManager.Setup(userMgr => userMgr.FindByNameAsync(user.UserName)).ReturnsAsync(user);

            // Mock the Logger
            var mockLogger = new Mock<ILogger<LeavesController>>();

            // Use real Mapper
            var mapperConfig = new MapperConfiguration(cfg => {
                cfg.AddProfile(new LeaveMappingProfile());
            });
            var mapper = mapperConfig.CreateMapper();

            // Initialize the controller
            var controller = new LeavesController(mockRepo.Object, mockUserManager.Object, mockLogger.Object, mapper);

            // Act
            var query = new QueryModel<Leave>();
            var result = controller.Get(query);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<LeaveModel>>(okResult.Value);
            Assert.Equal(2, model.Count());
        }

        private IEnumerable<Leave> GetLeaves()
        {
            var leaves = new List<Leave>();
            leaves.Add(new Leave()
            {
                Means = Means.SMS,
                Justification = "Fever",
                DateCreated = DateTime.Now,
                From = DateTime.Now.AddDays(1)
            });

            leaves.Add(new Leave()
            {
                Means = Means.EMAIL,
                Justification = "Diarrhea",
                DateCreated = DateTime.Now,
                From = DateTime.Now.AddDays(2)
            });

            return leaves;
        }
    }
}