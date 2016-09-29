using System;
using Xunit;
using Moq;
using MPWebAPI.Models;
using Microsoft.AspNetCore.Identity;
using MPWebAPI.Controllers;
using System.Collections.Generic;
using MPWebAPI.ViewModels;

namespace MPWebAPI.Test.UnitTests.ControllerTests
{
    public class GroupControllerTests
    {
        private readonly Mock<IMerlinPlanRepository> _repository;
        private readonly Mock<IMerlinPlanBL> _businessLogic;
        private readonly Mock<UserManager<MerlinPlanUser>> _userManager;
        private readonly GroupController _controller;
        
        public GroupControllerTests() 
        {
            _repository = new Mock<IMerlinPlanRepository>();
            _repository.Setup(repo => repo.Groups).Returns(GetTestGroups());
            
            _businessLogic = new Mock<IMerlinPlanBL>();
            _userManager = new Mock<UserManager<MerlinPlanUser>>();
            _controller = new GroupController(
                _repository.Object, 
                _businessLogic.Object, 
                _userManager.Object
            );
        }

        [Fact]
        public void Get_ReturnsListOfViewModels()
        {
            var result = _controller.Get();
            Assert.IsType(typeof(IEnumerable<GroupViewModel>), result);
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