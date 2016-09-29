using System;
using Xunit;
using Moq;
using MPWebAPI.Models;
using Microsoft.AspNetCore.Identity;
using MPWebAPI.Controllers;
using System.Collections.Generic;
using MPWebAPI.ViewModels;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Builder;

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
            
            var userStore = new Mock<IUserStore<MerlinPlanUser>>();
            var quserStore = userStore.As<IQueryableUserStore<MerlinPlanUser>>();
           
            // var userManager = new UserManager<MerlinPlanUser>( 
            // _controller = new GroupController(
            //     repository.Object, 
            //     businessLogic.Object, 
            //     userManager.Object
            // );
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