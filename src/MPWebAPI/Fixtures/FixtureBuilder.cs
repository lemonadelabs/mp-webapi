using System;
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
            
            public class PortfolioFixture
            {
                
            }

            public class ResourceScenarioFixture
            {
                public class StaffResourceFixture
                {
                    public class StaffAdjustmentFixture
                    {
                        public float Value { get; set; }
                        public bool Additive { get; set; }
                        public DateTime Date { get; set; }
                        public bool Actual { get; set; }
                    }
                    
                    public string Name { get; set; }
                    public DateTime StartDate { get; set; }
                    public DateTime EndDate { get; set; }
                    public List<string> Categories { get; set; }
                    public List<StaffAdjustmentFixture> Adjustments { get; set; }
                }

                public class FinancialResourceFixture
                {
                    public class FinancialResourcePartitionFixture
                    {
                        public class FinancialResourceAdjustmentFixture
                        {
                            public decimal Value { get; set; }
                            public bool Additive { get; set; }
                            public DateTime Date { get; set; }
                            public bool Actual { get; set; }
                        }
                        
                        public List<string> Categories { get; set; }
                        public List<FinancialResourceAdjustmentFixture> Adjustments { get; set; }
                    }
                    
                    public string Name { get; set; }
                    public List<FinancialResourcePartitionFixture> Partitions { get; set; }
                    public DateTime StartDate { get; set; }
                    public DateTime EndDate { get; set; }
                }
                
                public string Name { get; set; }
                public string Creator { get; set; }
                public string Group { get; set; }
                public List<StaffResourceFixture> StaffResources { get; set; }
                public List<FinancialResourceFixture> FinancialResources { get; set; }
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

            _logger.LogInformation("Adding Organisations...");
            AddOrganisations();

            _logger.LogInformation("Adding Groups...");
            AddGroups();

            _logger.LogInformation("Adding Users...");
            await AddUsersAsync();

            _logger.LogInformation("Adding Business Units...");
            AddBusinessUnits();

            _logger.LogInformation("Adding Risk Categories...");
            AddRiskCategories();

            _logger.LogInformation("Adding Alignment Categories...");
            AddAlignmentCategories();

            _logger.LogInformation("Adding Staff Resource Categories...");
            AddStaffResourceCategories();

            _logger.LogInformation("Adding Financial Resource Categories");
            AddFinancialResourceCategories();

            _logger.LogInformation("Adding Resource Scenarios...");
            AddResourceScenarios();

            //_logger.LogInformation("Adding Groups...");
            //AddPortfolios();

            _logger.LogInformation("Fixture {0} added.", fixtureFile);
        }

        private void AddFinancialResourceCategories()
        {
            if (!_dbcontext.FinancialResourceCategory.Any())
            {
                foreach (var frc in _fixtureData.FinancialResourceCategories)
                {
                    _dbcontext.FinancialResourceCategory.Add(new FinancialResourceCategory() {
                        Name = frc.Name,
                        Group = _dbcontext.Group.First(g => g.Name == frc.Group)
                    });
                }
                _dbcontext.SaveChanges();
            }
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
                foreach (var rs in _fixtureData.ResourceScenarios)
                {
                    ResourceScenario resourceScenario = new ResourceScenario() {
                        Name = rs.Name,
                        Creator = _dbcontext.Users.First(u => u.UserName == rs.Creator),
                        Group = _dbcontext.Group.First(g => g.Name == rs.Group),
                    };

                    _dbcontext.ResourceScenario.Add(resourceScenario);
                    _dbcontext.SaveChanges();

                    // Staff Resources
                    foreach (var sr in rs.StaffResources)
                    {
                        var staffResource = new StaffResource() {
                            Name = sr.Name,
                            StartDate = sr.StartDate,
                            EndDate = sr.EndDate,
                            ResourceScenario = resourceScenario,
                        };
                        _dbcontext.Add(staffResource);
                        _dbcontext.SaveChanges();

                        foreach (var c in sr.Categories)
                        {
                            _dbcontext.Add(new StaffResourceStaffResourceCategory() {
                                StaffResource = staffResource,
                                StaffResourceCategory = _dbcontext.StaffResourceCategory.First(src => src.Name == c) 
                            });
                        }
                        _dbcontext.SaveChanges();
                        
                        foreach (var a in sr.Adjustments)
                        {
                            _dbcontext.Add(new StaffAdjustment() {
                                Value = a.Value,
                                Additive = a.Additive,
                                Date = a.Date,
                                Actual = a.Actual,
                                StaffResource = staffResource
                            });
                        }
                        _dbcontext.SaveChanges();
                    }

                    // FinancialResources
                    foreach (var fr in rs.FinancialResources)
                    {
                        var financialResource = new FinancialResource() {
                            Name = fr.Name,
                            StartDate = fr.StartDate,
                            EndDate = fr.EndDate,
                            ResourceScenario = resourceScenario
                        };
                        _dbcontext.FinancialResource.Add(financialResource);
                        _dbcontext.SaveChanges();
                        
                        // Partitions
                        foreach (var p in fr.Partitions)
                        {
                            var partition = new FinancialResourcePartition()
                            {
                                FinancialResource = financialResource
                            };
                            _dbcontext.FinancialResourcePartition.Add(partition);
                            _dbcontext.SaveChanges();

                            // Categories
                            foreach (var c in p.Categories)
                            {
                                _dbcontext.Add(new PartitionResourceCategory() {
                                    FinancialResourcePartition = partition,
                                    FinancialResourceCategory = _dbcontext.FinancialResourceCategory.First(frc => frc.Name == c)
                                });
                            }
                            _dbcontext.SaveChanges();

                            // Adjustments
                            foreach (var a in p.Adjustments)
                            {
                                _dbcontext.FinancialAdjustment.Add(new FinancialAdjustment() {
                                    Value = a.Value,
                                    Additive = a.Additive,
                                    Date = a.Date,
                                    Actual = a.Actual,
                                    FinancialResourcePartition = partition
                                });
                            }
                            _dbcontext.SaveChanges();
                        }
                    }
                }
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
                        Group = _dbcontext.Group.First(g => g.Name == rc.Group)
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