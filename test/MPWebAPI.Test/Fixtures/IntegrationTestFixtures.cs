using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using MPWebAPI.Fixtures;
using Newtonsoft.Json;

namespace MPWebAPI.Test.Fixtures
{
    public class IntegrationFixture
    {
        public readonly string FixtureFile = "fixture.json";

        public HttpClient Client { get; }
        public TestServer Server { get; }
        
        public IntegrationFixture()
        {
            Server = new TestServer(
                new WebHostBuilder().UseStartup<MPWebAPI.Startup>());
            Client = Server.CreateClient();

            // Add fixures
            var fb = Server.Host.Services.GetService(typeof(IFixtureBuilder)) as IFixtureBuilder;
            fb.AddFixture(FixtureFile, true);
        }

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
    }    
}


