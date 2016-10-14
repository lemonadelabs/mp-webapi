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
                public class ProjectConfigFixture
                {
                    public class PhaseConfigFixture
                    {
                        public DateTime StartDate { get; set; }
                        public DateTime EndDate { get; set; }
                        public string ProjectPhase { get; set; }
                    }
                    
                    public DateTime StartDate { get; set; }
                    public string Project { get; set; }
                    public string Option { get; set; }
                    public string Owner { get; set; }
                    public List<string> Managers { get; set; }
                    public List<PhaseConfigFixture> Phases { get; set; }                   
                }
                
                public string Creator { get; set; }
                public string Group { get; set; }
                public string Name { get; set; }
                public DateTime StartYear { get; set; }
                public DateTime EndYear { get; set; }
                public bool Approved { get; set; }
                public string ApprovedBy { get; set; }
                public int TimeScale { get; set; }
                public List<ProjectConfigFixture> Projects { get; set; }
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

            public class ProjectFixture
            {
                public class OptionsFixture
                {
                    
                    public class RiskProfileFixture
                    {
                        public string Category { get; set; }
                        public float Probability { get; set; }
                        public float Impact { get; set; }
                        public bool Mitigation { get; set; }
                        public float Residual { get; set; }
                        public DateTime Date { get; set; }
                        public bool Actual { get; set; }
                    }

                    public class ProjectBenefitFixture
                    {
                        public class AlignmentFixture
                        {
                            public float Value { get; set; }
                            public DateTime Date { get; set; }
                            public string AlignmentCategory { get; set; }
                        }
                        
                        public string Name { get; set; }
                        public string Description { get; set; }
                        public bool Achieved { get; set; }
                        public List<AlignmentFixture> Alignments { get; set; }
                        public List<string> Categories { get; set; }
                    }
                    
                    public class PhaseFixture
                    {
                        public class FinancialTransactionFixture
                        {
                            public decimal Value { get; set; }
                            public bool Additive { get; set; }
                            public DateTime Date { get; set; }
                            public List<string> Categories { get; set; }
                        }

                        public class StaffTransactionFixture
                        {
                            public int Value { get; set; }
                            public bool Additive { get; set; }
                            public DateTime Date { get; set; }
                            public string Category { get; set; }
                            public string StaffResource { get; set; }
                        }
                        
                        public string Name { get; set; }
                        public string BusinessCase { get; set; }
                        public DateTime StartDate { get; set; }
                        public DateTime EndDate { get; set; }
                        public List<FinancialTransactionFixture> FinancialResources { get; set; }
                        public List<StaffTransactionFixture> StaffResources { get; set; }
                       
                    }

                    public class DependencyFixture
                    {
                        public string Project { get; set; }
                        public string Option { get; set; }
                    }
                    
                    public string Description { get; set; }
                    public float Priority { get; set; }
                    public float Complexity { get; set; }
                    public List<PhaseFixture> Phases { get; set; }
                    public List<RiskProfileFixture> RiskProfile { get; set; }
                    public List<ProjectBenefitFixture> Benefits { get; set; }
                    public List<DependencyFixture> Dependencies { get; set; }
                }
                
                public string Name { get; set; }
                public string Summary { get; set; }
                public string Creator { get; set; }
                public string Group { get; set; }
               
                public string OwningBusinessUnit { get; set; }
                public string ImpactedBusinessUnit { get; set; }
                public List<OptionsFixture> Options { get; set; }
            }

            public class BenefitCategoryFixture
            {
                public string Name { get; set; }
                public string Description { get; set; }
                public string Group { get; set; }
            }
            
            public List<GroupFixture> Groups { get; set; }
            public List<BusinessUnitFixture> BusinessUnits { get; set; }
            public List<RiskCategoryFixture> RiskCategories { get; set; }
            public List<AlignmentCategoryFixture> AlignmentCategories { get; set; }
            public List<BenefitCategoryFixture> BenefitCategories { get; set; }
            public List<StaffResourceCategoryFixture> StaffResourceCategories { get; set; }
            public List<FinancialResourceCategoryFixture> FinancialResourceCategories { get; set; }
            public List<UserFixture> Users { get; set; }
            public List<ProjectFixture> Projects { get; set; }
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
            _logger = loggerFactory.AddDebug().CreateLogger<FixtureBuilder>();
            _userManager = userManager;
            _roleManager = roleManager;
        }
        
        public async Task AddFixture(string fixtureFile, bool flushDb = false)
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
                await _dbcontext.Database.EnsureDeletedAsync();
                await _dbcontext.Database.MigrateAsync();
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

            _logger.LogInformation("Adding Financial Resource Categories...");
            AddFinancialResourceCategories();

            _logger.LogInformation("Adding Benefit Categories...");
            AddBenefitCategories();

            _logger.LogInformation("Adding Resource Scenarios...");
            AddResourceScenarios();

            _logger.LogInformation("Adding Projects...");
            AddProjects();

            _logger.LogInformation("Adding Portfolios...");
            AddPortfolios();

            _logger.LogInformation("Fixture {0} added.", fixtureFile);
        }

        private List<FixtureData.ProjectFixture> DepSort(List<FixtureData.ProjectFixture> insort)
        {
            var sorted = new List<FixtureData.ProjectFixture>(insort);
            // Ensures that for all px, py in ps:
            // if py in px.deps then py > px

            sorted.Sort((x, y) => {
                var xAllDeps = x.Options.SelectMany(ox => ox.Dependencies);
                var yAllDeps = y.Options.SelectMany(oy => oy.Dependencies);

                if (
                    xAllDeps.Count() == yAllDeps.Count() && xAllDeps.All(xd => yAllDeps.Any(yd => yd.Project == xd.Project && yd.Option == xd.Option)))
                {
                    return 0;
                }
                else if(yAllDeps.Any(yd => yd.Project == x.Name))
                {
                    return -1;
                }
                else {
                    return 1;
                }
            });
            return sorted;
        }

        private void AddProjects()
        {
            if (!_dbcontext.Project.Any())
            {
                foreach (var p in DepSort(_fixtureData.Projects))
                {
                    var newProject = new Project()
                    {
                        Name = p.Name,
                        Summary = p.Summary,
                        Creator = _dbcontext.Users.First(u => u.UserName == p.Creator), 
                        Group = _dbcontext.Group.First(g => g.Name == p.Group),
                        
                        OwningBusinessUnit = _dbcontext.BusinessUnit.FirstOrDefault(bu => bu.Name == p.OwningBusinessUnit),
                        ImpactedBusinessUnit = _dbcontext.BusinessUnit.FirstOrDefault(ibu => ibu.Name == p.ImpactedBusinessUnit),
                    };
                    _dbcontext.Project.Add(newProject);
                    _dbcontext.SaveChanges();
                    
                    
                    foreach (var o in p.Options)
                    {
                        var option = new ProjectOption() {
                            Priority = o.Priority,
                            Complexity = o.Complexity,
                            Description = o.Description,
                            Project = newProject
                        };
                        _dbcontext.ProjectOption.Add(option);
                        _dbcontext.SaveChanges();

                        foreach (var phase in o.Phases)
                        {
                            var newPhase = new ProjectPhase()
                            {
                                Name = phase.Name,
                                StartDate = phase.StartDate,
                                EndDate = phase.EndDate,
                                ProjectOption = option
                            };
                            _dbcontext.ProjectPhase.Add(newPhase);
                            _dbcontext.SaveChanges();

                           foreach (var ft in phase.FinancialResources)
                           {
                                var newft = new FinancialTransaction() {
                                    Value = ft.Value,
                                    Additive = ft.Additive,
                                    Date = ft.Date,
                                    ProjectPhase = newPhase
                                };
                                _dbcontext.FinancialTransaction.Add(newft);
                                _dbcontext.SaveChanges();

                                foreach (var ftrc in ft.Categories)
                                {
                                    var newftrc = new FinancialTransactionResourceCategory() {
                                        FinancialResourceCategory = _dbcontext.FinancialResourceCategory.FirstOrDefault(frc => frc.Name == ftrc),
                                        FinancialTransaction = newft 
                                    };
                                    _dbcontext.Add(newftrc);
                                    _dbcontext.SaveChanges();
                                }                               
                           }

                           foreach (var st in phase.StaffResources)
                           {
                               var newStaffTransaction = new StaffTransaction() {
                                   Value = st.Value,
                                   Additive = st.Additive,
                                   Date = st.Date,
                                   Category = _dbcontext.StaffResourceCategory.FirstOrDefault(src => src.Name == st.Category),
                                   StaffResource = _dbcontext.StaffResource.FirstOrDefault(sr => sr.Name == st.StaffResource),
                                   ProjectPhase = newPhase
                               };
                           }
                        }

                        foreach (var rp in o.RiskProfile)
                        {
                            var newRiskProfile = new RiskProfile()
                            {
                                RiskCategory = _dbcontext.RiskCategory.First(rc => rc.Name == rp.Category),
                                Probability = rp.Probability,
                                Impact = rp.Impact,
                                Mitigation = rp.Mitigation ? 1.0f : 0.0f,
                                Residual = rp.Residual,
                                Actual = rp.Actual,
                                ProjectOption = option
                            };
                            _dbcontext.RiskProfile.Add(newRiskProfile);
                        }
                        _dbcontext.SaveChanges();

                        foreach (var b in o.Benefits)
                        {
                            var newBenefit = new ProjectBenefit()
                            {
                                Name = b.Name,
                                Description = b.Description,
                                Achieved = b.Achieved,
                                ProjectOption = option
                            };
                            _dbcontext.ProjectBenefit.Add(newBenefit);
                            _dbcontext.SaveChanges();

                            foreach (var a in b.Alignments)
                            {
                                var newAlignment = new Alignment()
                                {
                                    Value = a.Value,
                                    Date = a.Date,
                                    AlignmentCategory = _dbcontext.AlignmentCategory.FirstOrDefault(ac => ac.Name == a.AlignmentCategory),
                                    ProjectBenefit = newBenefit
                                };
                                _dbcontext.Alignment.Add(newAlignment);
                            }
                            _dbcontext.SaveChanges();

                            foreach (var bc in b.Categories)
                            {
                                var pbbc = new ProjectBenefitBenefitCategory() {
                                    ProjectBenefit = newBenefit,
                                    BenefitCategory = _dbcontext.BenefitCategory.First(c => c.Name == bc)
                                };
                                _dbcontext.Add(pbbc);
                            }
                            _dbcontext.SaveChanges();
                        }

                        foreach (var dep in o.Dependencies)
                        {
                            var newDep = new ProjectDependency() {
                                DependsOn = _dbcontext.ProjectOption.First(po => po.Project.Name == dep.Project && po.Description == dep.Option),
                                RequiredBy = option 
                            };
                            _dbcontext.Add(newDep);
                        }
                        _dbcontext.SaveChanges();
                    }
                }
            }
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
                foreach (var p in _fixtureData.Portfolios)
                {
                    var newPortfolio = new Portfolio() {
                        Creator = _dbcontext.Users.First(u => u.UserName == p.Creator),
                        Group = _dbcontext.Group.First(g => g.Name == p.Group),
                        Name = p.Name,
                        StartYear = p.StartYear,
                        EndYear = p.EndYear,
                        Approved = p.Approved,
                        ApprovedBy = _dbcontext.Users.FirstOrDefault(u => u.UserName == p.ApprovedBy),
                    };
                    _dbcontext.Portfolio.Add(newPortfolio);
                    _dbcontext.SaveChanges();

                    foreach (var pc in p.Projects)
                    {
                        var newProjectConfig = new ProjectConfig() {
                            StartDate = pc.StartDate,
                            ProjectOption = _dbcontext.ProjectOption.First(po => po.Description == pc.Option && po.Project.Name == pc.Project),
                            Portfolio = newPortfolio,
                            Owner = _dbcontext.StaffResource.FirstOrDefault(sr => sr.Name == pc.Owner),
                            
                        };
                        _dbcontext.ProjectConfig.Add(newProjectConfig);
                        _dbcontext.SaveChanges();
                        _dbcontext.AddRange(pc.Managers.Select(m => new StaffResourceProjectConfig() {
                                StaffResource = _dbcontext.StaffResource.FirstOrDefault(sr => sr.Name == m),
                                ProjectConfig = newProjectConfig         
                          }));
                        _dbcontext.SaveChanges();

                        foreach (var ppc in pc.Phases)
                        {
                            _dbcontext.PhaseConfig.Add(new PhaseConfig() {
                                StartDate = ppc.StartDate,
                                EndDate = ppc.EndDate,
                                ProjectPhase = _dbcontext.ProjectPhase.First(pp => pp.Name == ppc.ProjectPhase && pp.ProjectOption == newProjectConfig.ProjectOption && pp.ProjectOption.Project.Name == newProjectConfig.ProjectOption.Project.Name),
                                ProjectConfig = newProjectConfig
                            });
                        }
                        _dbcontext.SaveChanges();                     
                    }
                }
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

                    // Financial Resources
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
                        EmailConfirmed = true
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

        private void AddBenefitCategories()
        {
            if (!_dbcontext.BenefitCategory.Any())
            {
                foreach (var bc in _fixtureData.BenefitCategories)
                {
                    _dbcontext.BenefitCategory.Add(new BenefitCategory() {
                        Name = bc.Name,
                        Description = bc.Description,
                        Group = _dbcontext.Group.First(g => g.Name == bc.Group)
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