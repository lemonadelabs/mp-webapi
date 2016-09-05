using System.IO;
using System;
using MPWebAPI.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MPWebAPI.Fixtures
{
    public static class FixtureBuilder
    {
        public class FixtureData
        {
            public class ProjectFixture
            {

            }

            public class PlanFixture
            {

            }
            
            public string SchemaSupport { get; set; }
            public List<Organisation> Organisations { get; set; }
            public List<Group> Groups { get; set; }
            
            // Categories
            public List<RiskCategory> RiskCategories { get; set; }
            public List<AlignmentCategory> AlignmentCategories { get; set; }
            public List<BenefitCategory> BenefitCategories { get; set; }
            public List<StaffResourceCategory> StaffResourceCategories { get; set; }
            public List<FinancialResourceCategory> FinancialResourceCategories { get; set; }


            public List<Alignment> Alignments { get; set; }
            public List<MerlinPlanUser> Users { get; set; }
        }

        
        public static void AddFixture(this IApplicationBuilder app, string fixtureFile)
        {
            //var db = app.ApplicationServices.GetService<PostgresDBContext>();
            //db.Database.EnsureDeleted();
            //db.Database.Migrate();
            var fixturePath = Path.Combine(
                Directory.GetCurrentDirectory(), 
                Path.Combine("Fixtures", fixtureFile));

            using(StreamReader sr = new StreamReader(File.OpenRead(fixturePath)))
            {
                var fixtureJSON = sr.ReadToEnd();
            }

            // Parse migration json
            //var fixtureData = JsonConvert.DeserializeObject<FixtureData>(fixtureJSON);

            // Create db objects

            // Save the objects
        }
    }
}