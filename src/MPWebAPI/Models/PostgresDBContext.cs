using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using OpenIddict;

namespace MPWebAPI.Models
{
    public class PostgresDBContext : OpenIddictDbContext<MerlinPlanUser>
    {
        public PostgresDBContext(DbContextOptions<PostgresDBContext> options) :base(options)
        {
        }

        public DbSet<Alignment> Alignments;
        
        public DbSet<BusinessUnit> BusinessUnits;
        
        public DbSet<BenefitCategory> BenefitCategories;
        
        public DbSet<StaffResourceCategory> StaffResourceCategories;
        
        public DbSet<FinancialResourceCategory> FinancialResourceCategories;
        
        public DbSet<RiskCategory> RiskCategories;
        
        public DbSet<AlignmentCategory> AlignmentCategories;
        
        public DbSet<FinancialAdjustment> FinancialAdjustments;
        
        public DbSet<FinancialResource> FinancialResources;
        
        public DbSet<FinancialResourcePartition> FinancialResourcePartitions;
        public DbSet<PartitionResourceCategory> PartitionResourceCategories;
        
        public DbSet<FinancialTransactionResourceCategory> FinancialTransactionResourceCategories;
        public DbSet<FinancialTransaction> FinancialTransactions;
        
        public DbSet<Group> Groups;
        public DbSet<UserGroup> UserGroups;
        
        public DbSet<Organisation> Organisations;

        public DbSet<PhaseConfig> PhaseConfigs;
        
        public DbSet<Plan> Plans;
        public DbSet<PlanUser> PlanUsers;

        public DbSet<Project> Projects;
        public DbSet<StaffResourceProject> StaffResourceProjects;
        public DbSet<ProjectFinancialResourceCategory> ProjectFinancialResourceCategories;
        public DbSet<ProjectUser> ProjectUsers;

        public DbSet<ProjectBenefit> ProjectBenefits;
        public DbSet<ProjectBenefitBenefitCategory> ProjectBenefitBenefitCategories;

        public DbSet<ProjectConfig> ProjectConfigs;

        public DbSet<ProjectOption> ProjectOptions;
        public DbSet<ProjectDependency> ProjectDependencies;

        public DbSet<ProjectPhase> ProjectPhases;

        public DbSet<ResourceScenario> ResourceScenarios;
        public DbSet<ResourceScenarioUser> ResourceScenarioUsers;

        public DbSet<RiskProfile> RiskProfiles;

        public DbSet<StaffResource> StaffResources;
        public DbSet<StaffResourceStaffResourceCategory> StaffResourceStaffResourceCategories;

        public DbSet<StaffTransaction> StaffTransactions;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // relations

            // Alignment
            builder.Entity<Alignment>()
                .HasOne(a => a.AlignmentCategory)
                .WithMany(ac => ac.Alignments)
                .HasForeignKey(a => a.AlignmentCategoryId);
            
            builder.Entity<Alignment>()
                .HasOne(a => a.ProjectBenefit)
                .WithMany(pb => pb.Alignments)
                .HasForeignKey(a => a.ProjectBenefitId);

            // BusinessUnit
            builder.Entity<BusinessUnit>()
                .HasOne(bu => bu.Organisation)
                .WithMany(o => o.BusinessUnits)
                .HasForeignKey(bu => bu.OrganisationId);
            
            // BenefitCategory
            builder.Entity<BenefitCategory>()
                .HasOne(bc => bc.Group)
                .WithMany(g => BenefitCategories)
                .HasForeignKey(bc => bc.GroupId);

            // StaffResourceCategory
            builder.Entity<StaffResourceCategory>()
                .HasOne(src => src.Group)
                .WithMany(g => g.StaffResourceCategories)
                .HasForeignKey(src => src.GroupId);
            
            // FinancialResourceCategory
            builder.Entity<FinancialResourceCategory>()
                .HasOne(frc => frc.Group)
                .WithMany(g => g.FinancialResourceCategories)
                .HasForeignKey(frc => frc.GroupId);
            
            // RiskCategory
            builder.Entity<RiskCategory>()
                .HasOne(rc => rc.Group)
                .WithMany(g => g.RiskCategories)
                .HasForeignKey(rc => rc.GroupId);
            
            // AlignmentCategory
            builder.Entity<AlignmentCategory>()
                .HasOne(ac => ac.Group)
                .WithMany(g => g.AlignmentCategories)
                .HasForeignKey(ac => ac.GroupId);
            
            // FinancialAdjustment
            builder.Entity<FinancialAdjustment>()
                .HasOne(fa => fa.FinancialResourcePartition)
                .WithMany(fp => fp.Adjustments)
                .HasForeignKey(fa => fa.FinancialResourcePartitionId);
            
            // FinancialResource
            builder.Entity<FinancialResource>()
                .HasOne(fr => fr.ResourceScenario)
                .WithMany(rs => rs.FinancialResources)
                .HasForeignKey(fr => fr.ResourceScenarioId);
            
            // FinancialResourcePartition
            builder.Entity<FinancialResourcePartition>()
                .HasOne(frp => frp.FinancialResource)
                .WithMany(fr => fr.Partitions)
                .HasForeignKey(frp => frp.FinancialResourceId);
            
            builder.Entity<PartitionResourceCategory>()
                .HasOne(prc => prc.FinancialResourcePartition)
                .WithMany(frp => frp.Categories)
                .HasForeignKey(prc => prc.FinancialResourcePartitionId);
            
            builder.Entity<PartitionResourceCategory>()
                .HasOne(prc => prc.FinancialResourceCategory)
                .WithMany(frc => frc.FinancialPartitions)
                .HasForeignKey(prc => prc.FinancialResourceCategoryId);
            
            // FinancialTransaction
            builder.Entity<FinancialTransaction>()
                .HasOne(ft => ft.ProjectPhase)
                .WithMany(pp => pp.FinancialResources)
                .HasForeignKey(ft => ft.ProjectPhaseId);
            
            builder.Entity<FinancialTransactionResourceCategory>()
                .HasOne(ftrc => ftrc.FinancialResourceCategory)
                .WithMany(frc => frc.Transactions)
                .HasForeignKey(ftrc => ftrc.FinancialResourceCategoryId);
            
            builder.Entity<FinancialTransactionResourceCategory>()
                .HasOne(ftrc => ftrc.FinancialTransaction)
                .WithMany(ft => ft.Categories)
                .HasForeignKey(ftrc => ftrc.FinancialTransactionId);
            
            // Group
            builder.Entity<Group>()
                .HasOne(g => g.Parent)
                .WithMany(g => g.Children)
                .HasForeignKey(g => g.ParentId);
            
            builder.Entity<Group>()
                .HasOne(g => g.Organisation)
                .WithMany(o => o.Groups)
                .HasForeignKey(g => g.OrganisationId);
            
            // MerlinPlanUser
            builder.Entity<MerlinPlanUser>()
                .HasOne(u => u.Organisation)
                .WithMany(o => o.Users)
                .HasForeignKey(u => u.OrganisationId);
            
            builder.Entity<MerlinPlanUser>()
                .HasOne(u => u.StaffResource)
                .WithOne(sr => sr.UserData)
                .HasForeignKey<MerlinPlanUser>(u => u.StaffResourceId);
            
            builder.Entity<UserGroup>()
                .HasOne(ug => ug.User)
                .WithMany(u => u.Groups)
                .HasForeignKey(ug => ug.UserId);
            
            builder.Entity<UserGroup>()
                .HasOne(ug => ug.Group)
                .WithMany(g => g.Members)
                .HasForeignKey(ug => ug.GroupId);
            
            // PhaseConfig
            builder.Entity<PhaseConfig>()
                .HasOne(pc => pc.ProjectConfig)
                .WithMany(pjc => pjc.Phases)
                .HasForeignKey(pc => pc.ProjectConfigId);

            // Plan
            builder.Entity<Plan>()
                .HasOne(p => p.Creator)
                .WithMany(u => u.Plans)
                .HasForeignKey(p => p.CreatorId);
            
            builder.Entity<Plan>()
                .HasOne(p => p.Group)
                .WithMany(g => g.Plans)
                .HasForeignKey(p => p.GroupId);
            
            builder.Entity<PlanUser>()
                .HasOne(pu => pu.Plan)
                .WithMany(p => p.ShareUser)
                .HasForeignKey(pu => pu.PlanId);
            
            builder.Entity<PlanUser>()
                .HasOne(pu => pu.User)
                .WithMany(u => u.SharedPlans)
                .HasForeignKey(pu => pu.UserId);
            
            // Project
            builder.Entity<Project>()
                .HasOne(p => p.Creator)
                .WithMany(u => u.Projects)
                .HasForeignKey(p => p.CreatorId);
            
            builder.Entity<Project>()
                .HasOne(p => p.Group)
                .WithMany(g => g.Projects)
                .HasForeignKey(p => p.GroupId);
            
            builder.Entity<Project>()
                .HasOne(p => p.Owner)
                .WithMany(o => o.ProjectsOwned)
                .HasForeignKey(p => p.OwnerId);
            
            builder.Entity<Project>()
                .HasOne(p => p.OwningBusinessUnit)
                .WithMany(bu => bu.ProjectsOwned)
                .HasForeignKey(p => p.OwningBusinessUnitId);
            
            builder.Entity<Project>()
                .HasOne(p => p.ImpactedBusinessUnit)
                .WithMany(ibu => ibu.ProjectsImpacting)
                .HasForeignKey(p => p.ImpactedBusinessUnitId);
            
            builder.Entity<StaffResourceProject>()
                .HasOne(srp => srp.StaffResource)
                .WithMany(sr => sr.ProjectsManaged)
                .HasForeignKey(srp => srp.StaffResourceId);
            
            builder.Entity<StaffResourceProject>()
                .HasOne(srp => srp.Project)
                .WithMany(p => p.Managers)
                .HasForeignKey(srp => srp.ProjectId);
            
            builder.Entity<ProjectFinancialResourceCategory>()
                .HasOne(pfrc => pfrc.Project)
                .WithMany(p => p.FinancialResourceCategories)
                .HasForeignKey(pfrc => pfrc.ProjectId);
            
            builder.Entity<ProjectFinancialResourceCategory>()
                .HasOne(pfrc => pfrc.FinancialResourceCategory)
                .WithMany(frc => frc.Projects)
                .HasForeignKey(pfrc => pfrc.FinancialResourceCategoryId);

            builder.Entity<ProjectUser>()
                .HasOne(pu => pu.Project)
                .WithMany(p => p.ShareUser)
                .HasForeignKey(pu => pu.ProjectId);
            
            builder.Entity<ProjectUser>()
                .HasOne(pu => pu.User)
                .WithMany(u => u.SharedProjects)
                .HasForeignKey(pu => pu.UserId);
            
            // ProjectBenefit
            builder.Entity<ProjectBenefit>()
                .HasOne(pb => pb.ProjectOption)
                .WithMany(po => po.Benefits)
                .HasForeignKey(pb => pb.ProjectOptionId);
            
            builder.Entity<ProjectBenefitBenefitCategory>()
                .HasOne(pbbc => pbbc.ProjectBenefit)
                .WithMany(pb => pb.Categories)
                .HasForeignKey(pbbc => pbbc.ProjectBenefitId);
            
            builder.Entity<ProjectBenefitBenefitCategory>()
                .HasOne(pbbc => pbbc.BenefitCategory)
                .WithMany(bc => bc.ProjectBenefits)
                .HasForeignKey(pbbc => pbbc.BenefitCategoryId);
            
            // ProjectConfig
            builder.Entity<ProjectConfig>()
                .HasOne(pc => pc.Plan)
                .WithMany(p => p.Projects)
                .HasForeignKey(pc => pc.PlanId);
            
            // ProjectOption
            builder.Entity<ProjectOption>()
                .HasOne(po => po.Project)
                .WithMany(p => p.Options)
                .HasForeignKey(po => po.ProjectId);
            
            builder.Entity<ProjectDependency>()
                .HasOne(pd => pd.DependsOn)
                .WithMany(po => po.Dependencies)
                .HasForeignKey(pd => pd.DependsOnId);
            
            builder.Entity<ProjectDependency>()
                .HasOne(pd => pd.RequiredBy)
                .WithMany(po => po.RequiredBy)
                .HasForeignKey(pd => pd.RequiredById);
            
            // ProjectPhase
            builder.Entity<ProjectPhase>()
                .HasOne(pp => pp.ProjectOption)
                .WithMany(po => po.Phases)
                .HasForeignKey(pp => pp.ProjectOptionId);

            // ResourceScenario
             builder.Entity<ResourceScenario>()
                .HasOne(p => p.Creator)
                .WithMany(u => u.ResourceScenarios)
                .HasForeignKey(p => p.CreatorId);
            
            builder.Entity<ResourceScenario>()
                .HasOne(p => p.Group)
                .WithMany(g => g.ResourceScenarios)
                .HasForeignKey(p => p.GroupId);
            
            builder.Entity<ResourceScenarioUser>()
                .HasOne(pu => pu.ResourceScenario)
                .WithMany(p => p.ShareUser)
                .HasForeignKey(pu => pu.ResourceScenarioId);
            
            builder.Entity<ResourceScenarioUser>()
                .HasOne(pu => pu.User)
                .WithMany(u => u.SharedResourceScenarios)
                .HasForeignKey(pu => pu.UserId);
            
            // RiskProfile
            builder.Entity<RiskProfile>()
                .HasOne(rp => rp.ProjectOption)
                .WithMany(po => po.RiskProfile)
                .HasForeignKey(rp => rp.ProjectOptionId);
            
            builder.Entity<RiskProfile>()
                .HasOne(rp => rp.RiskCategory)
                .WithMany(rc => rc.RiskProfiles)
                .HasForeignKey(rp => rp.RiskCategoryId);
            
            // StaffResource
            builder.Entity<StaffResource>()
                .HasOne(sr => sr.ResourceScenario)
                .WithMany(rs => rs.StaffResources)
                .HasForeignKey(sr => sr.ResourceScenarioId);
            
            builder.Entity<StaffResourceStaffResourceCategory>()
                .HasOne(srsrc => srsrc.StaffResource)
                .WithMany(sr => sr.Categories)
                .HasForeignKey(srsrc => srsrc.StaffResourceId);
            
            builder.Entity<StaffResourceStaffResourceCategory>()
                .HasOne(srsrc => srsrc.StaffResourceCategory)
                .WithMany(src => src.StaffResources)
                .HasForeignKey(srsrc => srsrc.StaffResourceCategoryId);
            
            // StaffTransaction
            builder.Entity<StaffTransaction>()
                .HasOne(st => st.ProjectPhase)
                .WithMany(pp => pp.StaffResources)
                .HasForeignKey(st => st.ProjectPhaseId);                                                                                             
            base.OnModelCreating(builder);
        }
    }
}
