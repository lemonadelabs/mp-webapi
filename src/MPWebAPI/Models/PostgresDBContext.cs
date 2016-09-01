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
            
            // FinancialResourceCategory
            builder.Entity<FinancialResourceCategory>()
                .HasOne(frc => frc.Group)
                .WithMany(g => g.FinancialResourceCategories)
                .HasForeignKey(frc => frc.GroupId);
                

        
            base.OnModelCreating(builder);
        }
    }
}
