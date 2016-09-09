using System.Linq;
using System.IO;
using MPWebAPI.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Threading.Tasks;

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
        protected class FixtureData
        {
            public List<Organisation> Organisations { get; set; }
            
           
            public class ProjectFixture
            {

            }

            public class PortfolioFixture
            {

            }

            public class ResourceScenarioFixture
            {

            }

            public class GroupFixture
            {
                public string Name { get; set; }
                public string Description { get; set; }
                public string Organisation { get; set; }
            }

            public class UserFixture
            {
                public string UserName { get; set; }
                public string FirstName { get; set; }
                public string LastName { get; set; }
                public string EmployeeId { get; set; }
                public string Password { get; set; }
                public string Email { get; set; }
                public List<string> Roles { get; set; }
                public List<string> Groups { get; set; }
            }
            
            public class BusinessUnitFixture
            {
                public string Name { get; set; }
                public string Description { get; set; }
                public string Organisation { get; set; }
            }

            public class RiskCategoryFixture
            {
                public string Name { get; set; }
                public float Bias { get; set; }
                public string Group { get; set; }
            }

            public class AlignmentCategoryFixture
            {
                public string Name { get; set; }
                public string Description { get; set; }
                public string Area { get; set; }
                public string Group { get; set; }
            }

            public class StaffResourceCategoryFixture
            {
                public string Name { get; set; }
                public string Group { get; set; }
            }

            public class FinancialResourceCategoryFixture
            {
                public string Name { get; set; }
                public string Group { get; set; }
            }

            public class BenefitCategoryFixture
            {
                
            }
            
            public List<GroupFixture> Groups { get; set; }
            public List<BusinessUnitFixture> BusinessUnits { get; set; }
            public List<RiskCategoryFixture> RiskCategories { get; set; }
            public List<AlignmentCategoryFixture> AlignmentCategories { get; set; }
            public List<BenefitCategoryFixture> BenefitCategories { get; set; }
            public List<StaffResourceCategoryFixture> StaffResourceCategories { get; set; }
            public List<FinancialResourceCategoryFixture> FinancialResourceCategories { get; set; }
            public List<UserFixture> Users { get; set; }
            public List<PortfolioFixture> Portfolios { get; set; }
            public List<ResourceScenarioFixture> ResourceScenarios { get; set; }
        }

        private readonly PostgresDBContext _dbcontext;
        private readonly ILogger<FixtureBuilder> _logger;
        private readonly UserManager<MerlinPlanUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private FixtureData _fixtureData;

        public FixtureBuilder(
            PostgresDBContext dbcontext, 
            ILoggerFactory loggerFactory,
            UserManager<MerlinPlanUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            _dbcontext = dbcontext;
            _logger = loggerFactory.CreateLogger<FixtureBuilder>();
            _userManager = userManager;
            _roleManager = roleManager;
        }
        
        public async void AddFixture(string fixtureFile, bool flushDb = false)
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

            _fixtureData = JsonConvert.DeserializeObject<FixtureData>(fixtureJSON);

            if(flushDb)
            {
                _dbcontext.Database.EnsureDeleted();
                _dbcontext.Database.Migrate();
            }

            AddOrganisations();
            AddGroups();
            await AddUsersAsync();
            AddBusinessUnits();
            AddRiskCategories();
            AddAlignmentCategories();
            AddStaffResourceCategories();
            AddResourceScenarios();
            AddPortfolios();
        }

        private void AddStaffResourceCategories()
        {
            if (!_dbcontext.StaffResourceCategory.Any())
            {
                foreach (var src in _fixtureData.StaffResourceCategories)
                {
                    _dbcontext.Add(new StaffResourceCategory() {
                        Name = src.Name,
                        Group = _dbcontext.Group.First(g => g.Name == src.Group)
                    });                    
                }
                _dbcontext.SaveChanges();
            }
        }

        private void AddPortfolios()
        {
            if (!_dbcontext.Portfolio.Any())
            {
                
            }
        }

        private void AddResourceScenarios()
        {
            if (!_dbcontext.ResourceScenario.Any())
            {
                  
            }
        }

        private async Task AddUsersAsync()
        {
            if (!_dbcontext.Users.Any())
            {
                foreach (var u in _fixtureData.Users)
                {
                    // Add Users
                    var newUser = new MerlinPlanUser() {
                        UserName = u.UserName,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        EmployeeId = u.EmployeeId,
                        Email = u.Email,
                        Organisation = _dbcontext.Organisation.First(),
                    };

                    await _userManager.CreateAsync(newUser, u.Password);

                      // Add user to groups
                    var userGroups = new List<UserGroup>();
                    foreach (var g in u.Groups)
                    {
                        var newUserGroup = new UserGroup() {
                            User = newUser,
                            Group = _dbcontext.Group.First(gr => gr.Name == g)
                        };
                        userGroups.Add(newUserGroup);
                    }
                    _dbcontext.UserGroup.AddRange(userGroups);
                    _dbcontext.SaveChanges();
                    
                    // Add Roles
                    foreach (var r in u.Roles)
                    {
                        var roleExistsResult = await _roleManager.RoleExistsAsync(r);
                        if (!roleExistsResult)
                        {
                            var newRole = new IdentityRole();
                            newRole.Name = r;
                            await _roleManager.CreateAsync(newRole);
                        }
                        await _userManager.AddToRoleAsync(newUser, r);
                    }
                }
            }
        }

        private void AddAlignmentCategories()
        {
            if (!_dbcontext.AlignmentCategory.Any())
            {
                foreach (var ac in _fixtureData.AlignmentCategories)
                {
                    _dbcontext.AlignmentCategory.Add(new AlignmentCategory() {
                        Name = ac.Name,
                        Description = ac.Description,
                        Area = ac.Area,
                        Group = _dbcontext.Group.First(g => g.Name == ac.Group)
                    });
                }
                _dbcontext.SaveChanges();
            }
        }

        private void AddBusinessUnits()
        {
            if (!_dbcontext.BusinessUnit.Any())
            {
                foreach (var bu in _fixtureData.BusinessUnits)
                {
                    _dbcontext.BusinessUnit.Add(new BusinessUnit() {
                        Name = bu.Name,
                        Description = bu.Description,
                        Organisation = _dbcontext.Organisation.First(o => o.Name == bu.Organisation)
                    });
                }
                _dbcontext.SaveChanges();
            }
        }

        private void AddRiskCategories()
        {
            if (!_dbcontext.RiskCategory.Any())
            {
                foreach (var rc in _fixtureData.RiskCategories)
                {
                    _dbcontext.RiskCategory.Add(new RiskCategory() {
                        Name = rc.Name,
                        Bias = rc.Bias,
                        Group = _dbcontext.Group.First(g => g.Name == rc.Name)
                    });                    
                }
                _dbcontext.SaveChanges();
            }
        }

        private void AddGroups()
        {
            if (!_dbcontext.Group.Any())
            {
                foreach (var g in _fixtureData.Groups)
                {
                     _dbcontext.Group.Add(new Group() {
                         Name = g.Name,
                         Description = g.Description,
                         Organisation = _dbcontext.Organisation.First(o => o.Name == g.Organisation)
                     });
                }
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