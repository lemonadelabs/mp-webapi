using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace MPWebAPI.Test.IntegrationTests
{
    public class GroupControllerTests
    {
        private readonly HttpClient _client;
        private readonly TestServer _server;
        
        public GroupControllerTests()
        {
            _server = new TestServer(
                new WebHostBuilder().UseStartup<MPWebAPI.Startup>()
                );
            _client = _server.CreateClient();
        }

        [Fact]
        public void Get_NonExistantGroup()
        {
            //_client.
        }
        


    }
}