using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using MPWebAPI.Fixtures;
using MPWebAPI.Models;
using Newtonsoft.Json;
using Xunit;

namespace MPWebAPI.Test.Fixtures
{
    public class IntegrationFixture : IAsyncLifetime 
    {
        public readonly string FixtureFile = "fixture.json";

        public HttpClient Client { get; set; }
        public TestServer Server { get; set; }
        public PostgresDBContext DBContext { get; set; }
        
        public struct JSONResult<T>
        {
            public HttpResponseMessage response;
            public T JSONData;
        }

        public async Task<JSONResult<T>> GetJSONResult<T>(string emdpoint)
        {
             var response = await Client.GetAsync(emdpoint);
             T data = default(T);
             try
             {
                 data = JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
             }
             catch (Newtonsoft.Json.JsonSerializationException) {}
             return new JSONResult<T> {response = response, JSONData = data};
        }

        public async Task InitializeAsync()
        {
            Server = new TestServer(
                new WebHostBuilder().UseStartup<MPWebAPI.Startup>());
            Client = Server.CreateClient();

            // Add fixures
            var fb = Server.Host.Services.GetService(typeof(IFixtureBuilder)) as IFixtureBuilder;
            await fb.AddFixture(FixtureFile, true);
            DBContext = Server.Host.Services.GetService(typeof(PostgresDBContext)) as PostgresDBContext;
        }

        public Task DisposeAsync()
        {
            return null;
        }
    }    
}


