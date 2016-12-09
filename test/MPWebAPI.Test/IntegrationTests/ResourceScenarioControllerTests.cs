using System.Collections.Generic;
using System.Threading.Tasks;
using MPWebAPI.Controllers;
using MPWebAPI.Test.Fixtures;
using Xunit;

namespace MPWebAPI.Test.IntegrationTests
{
    [Collection("Database collection")]
    public class ResourceScenarioControllerTests
    {
        private readonly IntegrationFixture _fixture;

        public ResourceScenarioControllerTests(IntegrationFixture fixture)
        {
            _fixture = fixture;
        }

//        [Fact]
//        public async Task ShareWithUser()
//        {
//            var shareRequest = new ResourceScenarioController.UserList
//            {
//                Users = new List<string> {"hicks@don.govt.nz"}
//            };
//
//
//        }
    }
}