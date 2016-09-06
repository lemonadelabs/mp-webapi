using System.IO;
using MPWebAPI.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace MPWebAPI.Fixtures
{
    public class FixtureBuilder
    {
        public class FixtureData
        {
            public class ProjectFixture
            {

            }

            public class PortfolioFixture
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
            public List<PortfolioFixture> Portfolios { get; set; }
        }

        private readonly PostgresDBContext _dbcontext;

        public FixtureBuilder(PostgresDBContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        
        public void AddFixture(string fixtureFile, bool flushDb = false)
        {
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();
            
            var logger = loggerFactory.CreateLogger("FixtureBuilder");
            // var db = app.ApplicationServices.GetService<PostgresDBContext>();
            
            if (flushDb)
            {
                         
            }
            
            var fixturePath = Path.Combine(
                Directory.GetCurrentDirectory(), 
                Path.Combine("Fixtures", fixtureFile));
            
            string fixtureJSON = null;

            try
            {
                using(StreamReader sr = new StreamReader(File.OpenRead(fixturePath)))
                {
                    fixtureJSON = sr.ReadToEnd();
                }    
            }
            catch (System.Exception)
            {
                logger.LogError(string.Format("The fixture file {0} could not be loaded, skipping fixture add.", fixturePath));
                return;
            }

            // Parse migration json
            var fixtureData = JsonConvert.DeserializeObject<FixtureData>(fixtureJSON);
            var org = fixtureData.Organisations[0];
          
            // Create db objects
            // Create Organisations
            // db.Organisations.Add(new Organisation());
            // db.SaveChanges();
            // logger.LogInformation(fixtureData.Organisations[0].Name);
            

            // Create Groups

            // Create RiskCategories

            // Create AlignmentCategories

            // Create Alignments

            // Create Users

            // CreatePortfolios

            // Save the objects
        }
    }
}