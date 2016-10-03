using Xunit;
using Moq;
using MPWebAPI.Models;
using MPWebAPI.Controllers;
using System.Collections.Generic;
using MPWebAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MPWebAPI.Test.UnitTests.ControllerTests
{
    public class GroupControllerTests
    {
        private readonly GroupController _controller;
        
        public GroupControllerTests() 
        {
            var repository = new Mock<IMerlinPlanRepository>();
            repository.Setup(repo => repo.Groups).Returns(GetTestGroups());
            var businessLogic = new Mock<IMerlinPlanBL>();
            _controller = new GroupController(repository.Object, businessLogic.Object);
        }

        [Fact]
        public void Get_ReturnsListOfViewModels()
        {
            var result = _controller.Get();
            Assert.IsType<List<GroupViewModel>>(result);
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void Get_ReturnsModelInstance()
        {
            var result = _controller.Get(1);
            Assert.IsType<JsonResult>(result);
        }

        [Fact]
        public async Task GetGroupMembers_ReturnsJSONResult()
        {
            var result = await _controller.GroupUser(1);
            Assert.IsType<JsonResult>(result);
        }

        private List<Group> GetTestGroups()
        {
            var groups = new List<Group> 
            {
                new Group 
                {
                    Id = 1,
                    Name = "Group 1",
                    OrganisationId = 1
                },

                new Group
                {
                    Id = 2,
                    Name = "Group 2",
                    OrganisationId = 1
                },

                new  Group
                {
                    Id = 3,
                    Name = "Group 3",
                    OrganisationId = 1
                }
            };

            return groups;
        }
    }
}