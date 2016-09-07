using System.Linq;
using System.IO;
using MPWebAPI.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace MPWebAPI.Fixtures
{
    /// <summary>
    /// Seeds the MP database with some example data. This is
    /// used for testing and demonstration purposes. It could alos 
    /// be used to populate the db for a client to save them using the 
    /// UI. This class should be requested via DI and added to your DI
    /// container as a service.
    /// </summary>
    public class FixtureBuilder : IFixtureBuilder
    {
        public class FixtureData
        {
            public class ProjectFixture
            {

            }

            public class PortfolioFixture
            {

            }
            
            public List<Organisation> Organisations { get; set; }
            public List<Group> Groups { get; set; }
            
            // Categories
            public List<BusinessUnit> BusinessUnits { get; set; }
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
        private readonly ILogger<FixtureBuilder> _logger;
        private FixtureData _fixtureData;

        public FixtureBuilder(PostgresDBContext dbcontext, ILoggerFactory loggerFactory)
        {
            _dbcontext = dbcontext;
            _logger = loggerFactory.CreateLogger<FixtureBuilder>();
        }
        
        public void AddFixture(string fixtureFile, bool flushDb = false)
        {
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
                _logger.LogError(string.Format("The fixture file {0} could not be loaded, skipping fixture add.", fixturePath));
                return;
            }

            // Parse migration json
            _fixtureData = JsonConvert.DeserializeObject<FixtureData>(fixtureJSON);

            // Create db objects
            // Create Organisations
            if(flushDb)
            {
                _dbcontext.Database.EnsureDeleted();
                _dbcontext.Database.Migrate();
            }

            AddOrganisations();
            AddGroups();
            //AddUsers();
            AddBusinessUnits();
            //AddRiskCategories();
            //AddAlignmentCategories();
            //AddResourceScenarios();
            //AddPortfolios();
        }

        private void AddPortfolios()
        {
        }

        private void AddResourceScenarios()
        {
        }

        private void AddUsers()
        {
        }

        private void AddAlignmentCategories()
        {
        }

        private void AddBusinessUnits()
        {
            if (!_dbcontext.BusinessUnit.Any())
            {
                foreach (var bu in _fixtureData.BusinessUnits)
                {
                    bu.Organisation = _dbcontext.Organisation.First();
                    
                }
                _dbcontext.BusinessUnit.AddRange(_fixtureData.BusinessUnits);
                _dbcontext.SaveChanges();
            }
        }

        private void AddRiskCategories()
        {
        }

        private void AddGroups()
        {
            if (!_dbcontext.Group.Any())
            {
                foreach (var g in _fixtureData.Groups)
                {
                     g.Organisation = _dbcontext.Organisation.First();
                }
                _dbcontext.Group.AddRange(_fixtureData.Groups);
                _dbcontext.SaveChanges();
            }
        }

        private void AddOrganisations()
        {
            if (!_dbcontext.Organisation.Any())
            {
                _dbcontext.Organisation.AddRange(_fixtureData.Organisations);
                _dbcontext.SaveChanges();
            }
        }
    }
}