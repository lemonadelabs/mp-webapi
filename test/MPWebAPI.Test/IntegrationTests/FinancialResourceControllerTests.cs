using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MPWebAPI.Controllers;
using MPWebAPI.Test.Fixtures;
using MPWebAPI.ViewModels;
using Xunit;

namespace MPWebAPI.Test.IntegrationTests
{
    [Collection("Database collection")]
    public class FinancialResourceControllerTests
    {
        private readonly IntegrationFixture _fixture;

        public FinancialResourceControllerTests(IntegrationFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task MovingResourceProducesValidTransactions()
        {
            const decimal value = 1234567.33m;

            // Create new financial resource with value
            var financialResource = new ResourceScenarioController.NewFinancialResourceRequest
            {
                EndDate = new DateTime(2016, 4, 1),
                Name = "UnitTestResource",
                Recurring = true,
                ResourceScenarioId = 1,
                StartDate = new DateTime(2016, 1, 1),
                DefaultPartitionValue = value
            };

            var createResponse =
                await
                    _fixture.GetJSONResult<FinancialResourceViewModel>(
                        "/api/resourcescenario/1/financialresource",
                        IntegrationFixture.RequestMethod.Post,
                        financialResource
                    );

            Assert.Equal(createResponse.Response.StatusCode, HttpStatusCode.OK);

            // Move it around
            createResponse.JSONData.StartDate = financialResource.StartDate.AddMonths(1);
            createResponse.JSONData.EndDate = financialResource.EndDate?.AddMonths(0);

            var moveResponse =
                await
                    _fixture.GetJSONResult<FinancialResourceViewModel>("/api/financialresource",
                        IntegrationFixture.RequestMethod.Put, createResponse.JSONData);

            Assert.True(moveResponse.Response.IsSuccessStatusCode);
            Assert.NotNull(moveResponse.JSONData);

            // Check partiton value in db
            var partition = await
                _fixture.DBContext.FinancialResourcePartition
                    .Include(frp => frp.Adjustments)
                    .SingleOrDefaultAsync(frp => frp.FinancialResourceId == moveResponse.JSONData.Id);

            Assert.NotNull(partition);
            var adj = partition.Adjustments.OrderBy(a => a.Date).FirstOrDefault();

            Assert.Equal(value, adj.Value);
        }
    }
}