using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MPWebAPI.Controllers;
using MPWebAPI.Test.Fixtures;
using Xunit;

namespace MPWebAPI.Test.IntegrationTests
{
    [Collection("Database collection")]
    public class ProjectControllerTests
    {
        private readonly IntegrationFixture _fixture;

        public ProjectControllerTests(IntegrationFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task ShareWithUserTwice()
        {
            var users = new ProjectController.UserList
            {
                Users = new List<string> { "foo@lemonadelabs.io" }
            };

            var result = await _fixture.Client.PutAsJsonAsync($"api/project/{1}/user/share", users);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            result = await _fixture.Client.PutAsJsonAsync($"api/project/{1}/user/share", users);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }
    }
}
