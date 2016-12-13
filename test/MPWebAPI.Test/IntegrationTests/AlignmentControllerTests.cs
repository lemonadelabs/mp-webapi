using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MPWebAPI.Models;
using MPWebAPI.Test.Fixtures;
using MPWebAPI.ViewModels;
using Xunit;

namespace MPWebAPI.Test.IntegrationTests
{
    [Collection("Database collection")]
    public class AlignmentControllerTests
    {
        private readonly IntegrationFixture _fixture;

        public AlignmentControllerTests(IntegrationFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task TestGetAllAlignments()
        {
            var response = await _fixture.GetJSONResult<IEnumerable<AlignmentViewModel>>("api/alignment");
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.Response.StatusCode);
        }

        [Fact]
        public async Task TestCreateAlignment()
        {
            var response = await _fixture.GetJSONResult<IEnumerable<AlignmentViewModel>>("api/alignment");
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.Response.StatusCode);
            var count = response.JSONData.Count();
            var newAlignment = new AlignmentViewModel
            {
                Date = DateTime.Now,
                Value = 1.0f,
                AlignmentCategory = new AlignmentViewModel.AlignmentCategoryData
                {
                    Id = 1
                },
                ProjectBenefit = new AlignmentViewModel.ProjectBenefitData
                {
                    Id = 1
                }
            };

            var postResponse = await _fixture.GetJSONResult<AlignmentViewModel>(
                "api/alignment",
                IntegrationFixture.RequestMethod.Post,
                newAlignment);

            Assert.NotNull(postResponse);
            Assert.Equal(HttpStatusCode.OK, postResponse.Response.StatusCode);

            response = await _fixture.GetJSONResult<IEnumerable<AlignmentViewModel>>("api/alignment");
            Assert.Equal(count + 1, response.JSONData.Count());
            Assert.NotNull(response.JSONData.SingleOrDefault(a => a.Id == postResponse.JSONData.Id));
        }

//        [Fact]
//        public async Task TestDeleteAlignment()
//        {
//
//        }
    }
}