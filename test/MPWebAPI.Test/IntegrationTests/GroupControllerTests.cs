using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using MPWebAPI.ViewModels;
using System.Net.Http;
using MPWebAPI.Test.Fixtures;
using MPWebAPI.Controllers;
using Microsoft.EntityFrameworkCore;

namespace MPWebAPI.Test.IntegrationTests
{
    [Collection("Database collection")]
    public class GroupControllerTests
    {
        private readonly IntegrationFixture _fixture;

        public GroupControllerTests(IntegrationFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetGroups()
        {
            var response = await _fixture.GetJSONResult<List<GroupViewModel>>("/api/group");
            var gr = response.JSONData.FirstOrDefault();
            Assert.NotNull(gr);
            Assert.Equal("EPMO", gr.Name);
            Assert.Equal(HttpStatusCode.OK, response.Response.StatusCode);
        }

        [Fact]
        public async Task GetExistingGroup()
        {
             var response = await _fixture.GetJSONResult<GroupViewModel>("/api/group/1");
             Assert.NotNull(response.JSONData);
             Assert.Equal("EPMO", response.JSONData.Name);
             Assert.Equal(HttpStatusCode.OK, response.Response.StatusCode);
        }

        [Theory]
        [InlineData("100")]
        [InlineData("-100")]
        [InlineData("a")]
        [InlineData("foo")]
        public async Task GetInvalidGroup(string id)
        {
            var response = await _fixture.GetJSONResult<GroupViewModel>($"/api/group/{id}");
            Assert.Null(response.JSONData);
            Assert.Equal(HttpStatusCode.NotFound, response.Response.StatusCode);
        }

        [Fact]
        public async Task GetGroupUsers()
        {
            var response = await _fixture.GetJSONResult<List<UserViewModel>>("/api/group/1/user"); 
            Assert.NotNull(response.JSONData);
            var u = response.JSONData.FirstOrDefault(us => us.UserName == "friedrich@don.govt.nz");
            Assert.Equal("EPMO", u.Groups.First().Name);
            Assert.Equal(HttpStatusCode.OK, response.Response.StatusCode);
        }

        [Theory]
        [InlineData("100")]
        [InlineData("-100")]
        [InlineData("a")]
        [InlineData("foo")]
        public async Task GetUsersOfInvalidGroup(string id)
        {
            var response = await _fixture.GetJSONResult<List<UserViewModel>>($"/api/group/{id}/user");
            Assert.Null(response.JSONData);
            Assert.Equal(HttpStatusCode.NotFound, response.Response.StatusCode);
        }

        [Fact]
        public async Task CreateValidGroup()
        {
            var newGroup = new GroupViewModel {
                Name = "New Group",
                OrganisationId = 1
            };
            var postResponse = await _fixture.Client.PostAsJsonAsync("/api/group", newGroup);
            Assert.Equal(HttpStatusCode.OK, postResponse.StatusCode);
            var getResponse = await _fixture.GetJSONResult<GroupViewModel>("/api/group/3");
            Assert.NotNull(getResponse.JSONData);
            Assert.Equal(HttpStatusCode.OK, getResponse.Response.StatusCode);
            Assert.Equal("New Group", getResponse.JSONData.Name);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task CreateInvalidGroupOrgReference(int index)
        {
            var groups = new List<GroupViewModel> {
                new GroupViewModel { Name = "New Group", OrganisationId = 0},
                new GroupViewModel { Name = "New Group"},
                new GroupViewModel {OrganisationId = 1},
                new GroupViewModel()
            };
            
            var g = groups[index];

            var postResponse = await _fixture.Client.PostAsJsonAsync("/api/group", g);
            Assert.Equal(HttpStatusCode.BadRequest, postResponse.StatusCode);
        }

        [Fact]
        public async Task AddUserToGroup()
        {
            var user = await _fixture.DBContext.Users.FirstOrDefaultAsync(u => u.UserName == "sam@lemonadelabs.io");
            Assert.NotNull(user);
            Assert.False(await _fixture.DBContext.UserGroup.AnyAsync(ug => ug.GroupId == 2 && ug.UserId == user.Id));
            var ur = new GroupController.UserRequest {Users = new List<string> {user.Id}};
            var postResponse = await _fixture.Client.PutAsJsonAsync("/api/group/2/adduser", ur);
            Assert.Equal(HttpStatusCode.OK, postResponse.StatusCode);
            Assert.True(await _fixture.DBContext.UserGroup.AnyAsync(ug => ug.UserId == user.Id && ug.GroupId == 2));
        }

        [Fact]
        public async Task RemoveUserFromGroup()
        {
            var user = await _fixture.DBContext.Users.FirstOrDefaultAsync(u => u.UserName == "sam@lemonadelabs.io");
            Assert.NotNull(user);
            Assert.True(await _fixture.DBContext.UserGroup.AnyAsync(ug => ug.GroupId == 1 && ug.UserId == user.Id));
            var ur = new GroupController.UserRequest {Users = new List<string> {user.Id}};
            var postResponse = await _fixture.Client.PutAsJsonAsync("/api/group/1/removeuser", ur);
            Assert.Equal(HttpStatusCode.OK, postResponse.StatusCode);
            Assert.False(await _fixture.DBContext.UserGroup.AnyAsync(ug => ug.UserId == user.Id && ug.GroupId == 1));
        }

        [Fact]
        public async Task ParentGroup()
        {
            
            var dbcontext = _fixture.DBContext;
            
            var childGroup = dbcontext.Group
                .Include(g => g.Parent)
                .FirstOrDefault(g => g.Id == 1);
            
            var parentGroup = dbcontext.Group
                .Include(g => g.Children)
                .FirstOrDefault(g => g.Id == 2);
            
            Assert.NotNull(childGroup);
            Assert.NotNull(parentGroup);
            Assert.Null(childGroup.Parent);
            Assert.Equal(0, parentGroup.Children.Count);
            
            var putResponse = await _fixture.Client.PutAsJsonAsync("/api/group/1/group/2", "");

            dbcontext = _fixture.DBContext;

            childGroup = dbcontext.Group
                .Include(g => g.Parent)
                .FirstOrDefault(g => g.Id == 1);
            
            parentGroup = dbcontext.Group
                .Include(g => g.Children)
                .FirstOrDefault(g => g.Id == 2);
            
            Assert.Equal(HttpStatusCode.OK, putResponse.StatusCode);
            Assert.Equal(parentGroup, childGroup.Parent);
            Assert.True(parentGroup.Children.Contains(childGroup));
        }

        [Fact]
        public async Task UnparentGroup()
        {
            var putResponse = await _fixture.Client.PutAsJsonAsync("/api/group/1/group/2", "");

            var dbcontext = _fixture.DBContext;
            
            var childGroup = dbcontext.Group
                .Include(g => g.Parent)
                .FirstOrDefault(g => g.Id == 1);
            
            var parentGroup = dbcontext.Group
                .Include(g => g.Children)
                .FirstOrDefault(g => g.Id == 2);
            
            Assert.Equal(HttpStatusCode.OK, putResponse.StatusCode);
            Assert.NotNull(childGroup);
            Assert.NotNull(parentGroup);
            Assert.Equal(parentGroup, childGroup.Parent);
            Assert.True(parentGroup.Children.Contains(childGroup));

            putResponse = await _fixture.Client.PutAsJsonAsync("/api/group/1/group", "");
            
            childGroup = _fixture.DBContext.Group
                .Include(g => g.Parent)
                .FirstOrDefault(g => g.Id == 1);
            
            Assert.Equal(HttpStatusCode.OK, putResponse.StatusCode);
            Assert.NotNull(childGroup);
            Assert.Null(childGroup.Parent);
        }
    }
}