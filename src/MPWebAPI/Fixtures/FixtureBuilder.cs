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
                    public DateTime? EndDate { get; set; }
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
                    public DateTime? EndDate { get; set; }
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

        private readonly DBContext _dbcontext;
        private readonly ILogger<FixtureBuilder> _logger;
        private readonly UserManager<MerlinPlanUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private FixtureData _fixtureData;

        public FixtureBuilder(
            DBContext dbcontext,
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
			 _logger.LogInformation("AddFixture {0} .", fixtureFile);

            var basePath = new DirectoryInfo(AppContext.BaseDirectory).Parent.Parent.Parent;

            var fixturePath = Path.Combine(
                basePath.FullName,
                Path.Combine("Fixtures", fixtureFile)
            );

            string fixtureJSON;

            try
            {
                using (var sr = new StreamReader(File.OpenRead(fixturePath)))
                {
                    fixtureJSON = sr.ReadToEnd();
                }
            }
            catch (Exception)
            {
                _logger.LogError(string.Format("The fixture file {0} could not be loaded, skipping fixture add.",
                    fixturePath));
                return;
            }
			
			_logger.LogInformation(string.Format("The fixture file {0} is loaded.",
                    fixturePath));
            try
            {
                _fixtureData = JsonConvert.DeserializeObject<FixtureData>(fixtureJSON);
            }
            catch (Exception e)
            {
                _logger.LogError($"The fixture data file was invalid: {e.Message}");
                return;
            }
			_logger.LogInformation($"Joson to data ok ");
           

            if(flushDb)
            {
                await _dbcontext.Database.EnsureDeletedAsync();
                await _dbcontext.Database.MigrateAsync();
            }

            _logger.LogInformation("Adding Organisations...");
            await AddOrganisations();

            _logger.LogInformation("Adding Groups...");
            await AddGroups();

            _logger.LogInformation("Adding Users...");
            await AddUsersAsync();

            _logger.LogInformation("Adding Business Units...");
            await AddBusinessUnits();

            _logger.LogInformation("Adding Risk Categories...");
            await AddRiskCategories();

            _logger.LogInformation("Adding Alignment Categories...");
            await AddAlignmentCategories();

            _logger.LogInformation("Adding Staff Resource Categories...");
            await AddStaffResourceCategories();

            _logger.LogInformation("Adding Financial Resource Categories...");
            await AddFinancialResourceCategories();

            _logger.LogInformation("Adding Benefit Categories...");
            await AddBenefitCategories();

            _logger.LogInformation("Adding Resource Scenarios...");
            await AddResourceScenarios();

            _logger.LogInformation("Adding Projects...");
            await AddProjects();

            _logger.LogInformation("Adding Portfolios...");
            await AddPortfolios();

            _logger.LogInformation("Fixture {0} added.", fixtureFile);
        }

        private static IEnumerable<FixtureData.ProjectFixture> DepSort(IEnumerable<FixtureData.ProjectFixture> insort)
        {
            var sorted = new List<FixtureData.ProjectFixture>(insort);
            // Ensures that for all px, py in ps:
            // if py in px.deps then py > px

            sorted.Sort((x, y) => {
                var xAllDeps = x.Options.SelectMany(ox => ox.Dependencies).ToList();
                var yAllDeps = y.Options.SelectMany(oy => oy.Dependencies).ToList();

                if (
                    xAllDeps.Count() == yAllDeps.Count() && xAllDeps.All(xd => yAllDeps.Any(yd => yd.Project == xd.Project && yd.Option == xd.Option)))
                {
                    return 0;
                }
                if(yAllDeps.Any(yd => yd.Project == x.Name))
                {
                    return -1;
                }
                return 1;
            });
            return sorted;
        }

        private async Task AddProjects()
        {
            if (!await _dbcontext.Project.AnyAsync())
            {
                foreach (var p in DepSort(_fixtureData.Projects))
                {
                    var newProject = new Project
                    {
                        Name = p.Name,
                        Summary = p.Summary,
                        Creator = await _dbcontext.Users.FirstAsync(u => u.UserName == p.Creator), 
                        Group = await _dbcontext.Group.FirstAsync(g => g.Name == p.Group),
                        
                        OwningBusinessUnit = await _dbcontext.BusinessUnit.FirstOrDefaultAsync(bu => bu.Name == p.OwningBusinessUnit),
                        ImpactedBusinessUnit = await _dbcontext.BusinessUnit.FirstOrDefaultAsync(ibu => ibu.Name == p.ImpactedBusinessUnit),
                    };
                    _dbcontext.Project.Add(newProject);
                    await _dbcontext.SaveChangesAsync();
                    
                    
                    foreach (var o in p.Options)
                    {
                        var option = new ProjectOption() {
                            Priority = o.Priority,
                            Complexity = o.Complexity,
                            Description = o.Description,
                            Project = newProject
                        };
                        _dbcontext.ProjectOption.Add(option);
                        await _dbcontext.SaveChangesAsync();

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
                            await _dbcontext.SaveChangesAsync();

                           foreach (var ft in phase.FinancialResources)
                           {
                                var newft = new FinancialTransaction() {
                                    Value = ft.Value,
                                    Additive = ft.Additive,
                                    Date = ft.Date,
                                    ProjectPhase = newPhase
                                };
                                _dbcontext.FinancialTransaction.Add(newft);
                                await _dbcontext.SaveChangesAsync();

                                foreach (var ftrc in ft.Categories)
                                {
                                    var newftrc = new FinancialTransactionResourceCategory() {
                                        FinancialResourceCategory = await _dbcontext.FinancialResourceCategory.FirstOrDefaultAsync(frc => frc.Name == ftrc),
                                        FinancialTransaction = newft 
                                    };
                                    _dbcontext.Add(newftrc);
                                    await _dbcontext.SaveChangesAsync();
                                }                               
                           }

                           foreach (var st in phase.StaffResources)
                           {
                               var newStaffTransaction = new StaffTransaction() {
                                   Value = st.Value,
                                   Additive = st.Additive,
                                   Date = st.Date,
                                   Category = await _dbcontext.StaffResourceCategory.FirstOrDefaultAsync(src => src.Name == st.Category),
                                   StaffResource = await _dbcontext.StaffResource.FirstOrDefaultAsync(sr => sr.Name == st.StaffResource),
                                   ProjectPhase = newPhase
                               };
                               _dbcontext.StaffTransaction.Add(newStaffTransaction);
                               await _dbcontext.SaveChangesAsync();
                           }
                        }

                        foreach (var rp in o.RiskProfile)
                        {
                            var newRiskProfile = new RiskProfile()
                            {
                                RiskCategory = await _dbcontext.RiskCategory.FirstAsync(rc => rc.Name == rp.Category),
                                Probability = rp.Probability,
                                Impact = rp.Impact,
                                Mitigation = rp.Mitigation ? 1.0f : 0.0f,
                                Residual = rp.Residual,
                                Actual = rp.Actual,
                                ProjectOption = option
                            };
                            _dbcontext.RiskProfile.Add(newRiskProfile);
                        }
                        await _dbcontext.SaveChangesAsync();

                        foreach (var b in o.Benefits)
                        {
                            var newBenefit = new ProjectBenefit
                            {
                                Name = b.Name,
                                Description = b.Description,
                                Achieved = b.Achieved,
                                ProjectOption = option
                            };
                            _dbcontext.ProjectBenefit.Add(newBenefit);
                            await _dbcontext.SaveChangesAsync();

                            foreach (var a in b.Alignments)
                            {
                                var newAlignment = new Alignment()
                                {
                                    Value = a.Value,
                                    Date = a.Date,
                                    AlignmentCategory = await _dbcontext.AlignmentCategory.FirstOrDefaultAsync(ac => ac.Name == a.AlignmentCategory),
                                    ProjectBenefit = newBenefit
                                };
                                _dbcontext.Alignment.Add(newAlignment);
                            }
                            await _dbcontext.SaveChangesAsync();

                            foreach (var bc in b.Categories)
                            {
                                var pbbc = new ProjectBenefitBenefitCategory() {
                                    ProjectBenefit = newBenefit,
                                    BenefitCategory = await _dbcontext.BenefitCategory.FirstAsync(c => c.Name == bc)
                                };
                                _dbcontext.Add(pbbc);
                            }
                            await _dbcontext.SaveChangesAsync();
                        }

                        foreach (var dep in o.Dependencies)
                        {
                            var newDep = new ProjectDependency() {
                                DependsOn = await _dbcontext.ProjectOption.FirstAsync(po => po.Project.Name == dep.Project && po.Description == dep.Option),
                                RequiredBy = option 
                            };
                            _dbcontext.Add(newDep);
                        }
                        await _dbcontext.SaveChangesAsync();
                    }
                }
            }
        }

        private async Task AddFinancialResourceCategories()
        {
            if (await _dbcontext.FinancialResourceCategory.AnyAsync()) return;
            foreach (var frc in _fixtureData.FinancialResourceCategories)
            {
                _dbcontext.FinancialResourceCategory.Add(new FinancialResourceCategory {
                    Name = frc.Name,
                    Group = await _dbcontext.Group.FirstAsync(g => g.Name == frc.Group)
                });
            }
            await _dbcontext.SaveChangesAsync();
        }

        private async Task AddStaffResourceCategories()
        {
            if (await _dbcontext.StaffResourceCategory.AnyAsync()) return;
            foreach (var src in _fixtureData.StaffResourceCategories)
            {
                _dbcontext.Add(new StaffResourceCategory() {
                    Name = src.Name,
                    Group = await _dbcontext.Group.FirstAsync(g => g.Name == src.Group)
                });                    
            }
            await _dbcontext.SaveChangesAsync();
        }

        private async Task AddPortfolios()
        {
            if (await _dbcontext.Portfolio.AnyAsync()) return;
            foreach (var p in _fixtureData.Portfolios)
            {
                var newPortfolio = new Portfolio() {
                    Creator = await _dbcontext.Users.FirstAsync(u => u.UserName == p.Creator),
                    Group = await _dbcontext.Group.FirstAsync(g => g.Name == p.Group),
                    Name = p.Name,
                    StartYear = p.StartYear,
                    EndYear = p.EndYear,
                    Approved = p.Approved,
                    ApprovedBy = await _dbcontext.Users.FirstOrDefaultAsync(u => u.UserName == p.ApprovedBy),
                };
                _dbcontext.Portfolio.Add(newPortfolio);
                await _dbcontext.SaveChangesAsync();

                foreach (var pc in p.Projects)
                {
                    var newProjectConfig = new ProjectConfig() {
                        ProjectOption = await _dbcontext.ProjectOption.FirstAsync(po => po.Description == pc.Option && po.Project.Name == pc.Project),
                        Portfolio = newPortfolio,
                        Owner = await _dbcontext.StaffResource.FirstOrDefaultAsync(sr => sr.Name == pc.Owner),
                            
                    };
                    _dbcontext.ProjectConfig.Add(newProjectConfig);
                    await _dbcontext.SaveChangesAsync();
                    
                    _dbcontext.AddRange(pc.Managers.Select(m => new StaffResourceProjectConfig {
                        StaffResource = _dbcontext.StaffResource.FirstOrDefault(sr => sr.Name == m),
                        ProjectConfig = newProjectConfig       
                    }));
                    await _dbcontext.SaveChangesAsync();

                    foreach (var ppc in pc.Phases)
                    {
                        _dbcontext.PhaseConfig.Add(new PhaseConfig() {
                            StartDate = ppc.StartDate,
                            EndDate = ppc.EndDate,
                            ProjectPhase = await _dbcontext.ProjectPhase.FirstAsync(pp => pp.Name == ppc.ProjectPhase && pp.ProjectOption == newProjectConfig.ProjectOption && pp.ProjectOption.Project.Name == newProjectConfig.ProjectOption.Project.Name),
                            ProjectConfig = newProjectConfig
                        });
                    }
                    await _dbcontext.SaveChangesAsync();                     
                }
            }
        }

        private async Task AddResourceScenarios()
        {
            if (await _dbcontext.ResourceScenario.AnyAsync()) return;
            foreach (var rs in _fixtureData.ResourceScenarios)
            {
                var resourceScenario = new ResourceScenario {
                    Name = rs.Name,
                    Creator = await _dbcontext.Users.FirstAsync(u => u.UserName == rs.Creator),
                    Group = await _dbcontext.Group.FirstAsync(g => g.Name == rs.Group),
                };

                _dbcontext.ResourceScenario.Add(resourceScenario);
                await _dbcontext.SaveChangesAsync();

                // Staff Resources
                foreach (var sr in rs.StaffResources)
                {
                    var staffResource = new StaffResource {
                        Name = sr.Name,
                        StartDate = sr.StartDate,
                        EndDate = sr.EndDate,
                        ResourceScenario = resourceScenario,
                    };
                    _dbcontext.Add(staffResource);
                    await _dbcontext.SaveChangesAsync();

                    foreach (var c in sr.Categories)
                    {
                        _dbcontext.Add(new StaffResourceStaffResourceCategory {
                            StaffResource = staffResource,
                            StaffResourceCategory = await _dbcontext.StaffResourceCategory.FirstAsync(src => src.Name == c) 
                        });
                    }
                    await _dbcontext.SaveChangesAsync();
                        
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
                    await _dbcontext.SaveChangesAsync();
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
                    await _dbcontext.SaveChangesAsync();
                        
                    // Partitions
                    foreach (var p in fr.Partitions)
                    {
                        var partition = new FinancialResourcePartition()
                        {
                            FinancialResource = financialResource
                        };
                        _dbcontext.FinancialResourcePartition.Add(partition);
                        await _dbcontext.SaveChangesAsync();

                        // Categories
                        foreach (var c in p.Categories)
                        {
                            _dbcontext.Add(new PartitionResourceCategory() {
                                FinancialResourcePartition = partition,
                                FinancialResourceCategory = await _dbcontext.FinancialResourceCategory.FirstAsync(frc => frc.Name == c)
                            });
                        }
                        await _dbcontext.SaveChangesAsync();

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
                        await _dbcontext.SaveChangesAsync();
                    }
                }
            }
        }

        private async Task AddUsersAsync()
        {
            if (await _dbcontext.Users.AnyAsync()) return;
            foreach (var u in _fixtureData.Users)
            {
                // Add Users
                var newUser = new MerlinPlanUser() {
                    UserName = u.UserName,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    EmployeeId = u.EmployeeId,
                    Email = u.Email,
                    Organisation = await _dbcontext.Organisation.FirstAsync(),
                    EmailConfirmed = true
                };

                await _userManager.CreateAsync(newUser, u.Password);

                // Add user to groups
                var userGroups = new List<UserGroup>();
                foreach (var g in u.Groups)
                {
                    var newUserGroup = new UserGroup() {
                        User = newUser,
                        Group = await _dbcontext.Group.FirstAsync(gr => gr.Name == g)
                    };
                    userGroups.Add(newUserGroup);
                }
                _dbcontext.UserGroup.AddRange(userGroups);
                await _dbcontext.SaveChangesAsync();
                    
                // Add Roles
                foreach (var r in u.Roles)
                {
                    var roleExistsResult = await _roleManager.RoleExistsAsync(r);
                    if (!roleExistsResult)
                    {
                        var newRole = new IdentityRole {Name = r};
                        await _roleManager.CreateAsync(newRole);
                    }
                    await _userManager.AddToRoleAsync(newUser, r);
                }
            }
        }

        private async Task AddAlignmentCategories()
        {
            if (await _dbcontext.AlignmentCategory.AnyAsync()) return;
            foreach (var ac in _fixtureData.AlignmentCategories)
            {
                _dbcontext.AlignmentCategory.Add(new AlignmentCategory() {
                    Name = ac.Name,
                    Description = ac.Description,
                    Area = ac.Area,
                    Group = await _dbcontext.Group.FirstAsync(g => g.Name == ac.Group)
                });
            }
            await _dbcontext.SaveChangesAsync();
        }

        private async Task AddBusinessUnits()
        {
            if (await _dbcontext.BusinessUnit.AnyAsync()) return;
            foreach (var bu in _fixtureData.BusinessUnits)
            {
                _dbcontext.BusinessUnit.Add(new BusinessUnit() {
                    Name = bu.Name,
                    Description = bu.Description,
                    Organisation = await _dbcontext.Organisation.FirstAsync(o => o.Name == bu.Organisation)
                });
            }
            await _dbcontext.SaveChangesAsync();
        }

        private async Task AddRiskCategories()
        {
            if (await _dbcontext.RiskCategory.AnyAsync()) return;
            foreach (var rc in _fixtureData.RiskCategories)
            {
                _dbcontext.RiskCategory.Add(new RiskCategory() {
                    Name = rc.Name,
                    Bias = rc.Bias,
                    Group = await _dbcontext.Group.FirstAsync(g => g.Name == rc.Group)
                });                   
            }
            await _dbcontext.SaveChangesAsync();
        }

        private async Task AddBenefitCategories()
        {
            if (await _dbcontext.BenefitCategory.AnyAsync()) return;
            foreach (var bc in _fixtureData.BenefitCategories)
            {
                _dbcontext.BenefitCategory.Add(new BenefitCategory() {
                    Name = bc.Name,
                    Description = bc.Description,
                    Group = await _dbcontext.Group.FirstAsync(g => g.Name == bc.Group)
                });
            }
            await _dbcontext.SaveChangesAsync();
        }

        private async Task AddGroups()
        {
            if (await _dbcontext.Group.AnyAsync()) return;
            foreach (var g in _fixtureData.Groups)
            {
                _dbcontext.Group.Add(new Group() {
                    Name = g.Name,
                    Description = g.Description,
                    Organisation = await _dbcontext.Organisation.FirstAsync(o => o.Name == g.Organisation)
                });
            }
            await _dbcontext.SaveChangesAsync();
        }

        private async Task AddOrganisations()
        {
            if (await _dbcontext.Organisation.AnyAsync()) return;
            _dbcontext.Organisation.AddRange(_fixtureData.Organisations);
            await _dbcontext.SaveChangesAsync();
        }
    }
}