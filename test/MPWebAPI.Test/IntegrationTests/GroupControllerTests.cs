using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using MPWebAPI.ViewModels;
using System.Net.Http;
using MPWebAPI.Test.Fixtures;

namespace MPWebAPI.Test.IntegrationTests
{
    public class GroupControllerTests : IClassFixture<IntegrationFixture>
    {
        IntegrationFixture fixture;

        public GroupControllerTests(IntegrationFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task GetGroups()
        {
            var response = await fixture.GetJSONResult<List<GroupViewModel>>("/api/group");
            var gr = response.JSONData.FirstOrDefault();
            Assert.NotNull(gr);
            Assert.Equal("EPMO", gr.Name);
            Assert.Equal(HttpStatusCode.OK, response.response.StatusCode);
        }

        [Fact]
        public async Task GetExistingGroup()
        {
             var response = await fixture.GetJSONResult<GroupViewModel>("/api/group/1");
             Assert.NotNull(response.JSONData);
             Assert.Equal("EPMO", response.JSONData.Name);
             Assert.Equal(HttpStatusCode.OK, response.response.StatusCode);
        }

        [Theory]
        [InlineData("100")]
        [InlineData("-100")]
        [InlineData("a")]
        [InlineData("foo")]
        public async Task GetInvalidGroup(string id)
        {
            var response = await fixture.GetJSONResult<GroupViewModel>($"/api/group/{id}");
            Assert.Null(response.JSONData);
            Assert.Equal(HttpStatusCode.NotFound, response.response.StatusCode);
        }

        [Fact]
        public async Task GetGroupUsers()
        {
            var response = await fixture.GetJSONResult<List<UserViewModel>>("/api/group/1/user");
            Assert.NotNull(response.JSONData);
            var u = response.JSONData.FirstOrDefault(us => us.UserName == "friedrich@don.govt.nz");
            Assert.Equal("EPMO", u.Groups.First().Name);
            Assert.Equal(HttpStatusCode.OK, response.response.StatusCode);
        }

        [Theory]
        [InlineData("100")]
        [InlineData("-100")]
        [InlineData("a")]
        [InlineData("foo")]
        public async Task GetUsersOfInvalidGroup(string id)
        {
            var response = await fixture.GetJSONResult<List<UserViewModel>>($"/api/group/{id}/user");
            Assert.Null(response.JSONData);
            Assert.Equal(HttpStatusCode.NotFound, response.response.StatusCode);
        }

        [Fact]
        public async Task CreateValidGroup()
        {
            var newGroup = new GroupViewModel {
                Name = "New Group",
                OrganisationId = 1
            };
            var postResponse = await fixture.Client.PostAsJsonAsync("/api/group", newGroup);
            Assert.Equal(HttpStatusCode.OK, postResponse.StatusCode);
            var getResponse = await fixture.GetJSONResult<GroupViewModel>("/api/group/2");
            Assert.NotNull(getResponse.JSONData);
            Assert.Equal(HttpStatusCode.OK, getResponse.response.StatusCode);
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

            var postResponse = await fixture.Client.PostAsJsonAsync("/api/group", g);
            Assert.Equal(HttpStatusCode.BadRequest, postResponse.StatusCode);
        }


    }
}