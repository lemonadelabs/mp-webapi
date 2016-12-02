using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
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
        public DBContext DBContext { 
            get
            {
                var serviceScope = Server.Host.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
                var context = serviceScope.ServiceProvider.GetService<DBContext>();
                return context;
            }
        }
        
        public struct JSONResult<T>
        {
            public HttpResponseMessage Response;
            public T JSONData;
        }

        public enum RequestMethod
        {
            Get,
            Put,
            Post,
            Delete
        }

        public async Task<JSONResult<T>> GetJSONResult<T>(string endpoint, RequestMethod method = RequestMethod.Get, object requestData = null)
        {
             HttpResponseMessage response;

            switch (method)
            {
               case RequestMethod.Get:
                    response = await Client.GetAsync(endpoint);
                    break;
               case RequestMethod.Post:
                    response = await Client.PostAsJsonAsync(endpoint, requestData);
                    break;
               case RequestMethod.Put:
                    response = await Client.PutAsJsonAsync(endpoint, requestData);
                    break;
                case RequestMethod.Delete:
                    response = await Client.DeleteAsync(endpoint);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(method), method, null);
            }

             var data = default(T);
             try
             {
                 if (response != null)
                 {
                     data = JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
                 }
             }
             catch (JsonSerializationException) {}
             return new JSONResult<T> {Response = response, JSONData = data};
        }

        public async Task InitializeAsync()
        {
            Server = new TestServer(
                new WebHostBuilder()
                .UseEnvironment("development")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>());
            Client = Server.CreateClient();

            // Add fixures
            var fb = Server.Host.Services.GetService(typeof(IFixtureBuilder)) as IFixtureBuilder;
            if (fb != null) await fb.AddFixture(FixtureFile, true);
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }

    [CollectionDefinition("Database collection")]
    public class DatabaseCollection : ICollectionFixture<IntegrationFixture>
    {
    }
}


