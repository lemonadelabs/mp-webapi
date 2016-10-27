using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using OpenIddict;


namespace MPWebAPI.Models
{
    public class DBContext : OpenIddictDbContext<MerlinPlanUser>
    {
        public DBContext(DbContextOptions<DBContext> options) :base(options)
        {
        }

        public DbSet<Alignment> Alignment { get; set; }
        public DbSet<BusinessUnit> BusinessUnit { get; set; }
        public DbSet<BenefitCategory> BenefitCategory { get; set; }
        public DbSet<StaffResourceCategory> StaffResourceCategory { get; set; }
        public DbSet<FinancialResourceCategory> FinancialResourceCategory { get; set; }
        public DbSet<RiskCategory> RiskCategory { get; set; }
        public DbSet<AlignmentCategory> AlignmentCategory { get; set; }
        public DbSet<FinancialAdjustment> FinancialAdjustment { get; set; }
        public DbSet<FinancialResource> FinancialResource { get; set; }
        public DbSet<FinancialResourcePartition> FinancialResourcePartition { get; set; }
        public DbSet<PartitionResourceCategory> PartitionResourceCategory { get; set; }
        public DbSet<FinancialTransaction> FinancialTransaction { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<UserGroup> UserGroup { get; set; }
        public DbSet<Organisation> Organisation { get; set; }
        public DbSet<PhaseConfig> PhaseConfig { get; set; }
        public DbSet<Portfolio> Portfolio { get; set; }
        public DbSet<PortfolioTag> PortfolioTag { get; set; }
        public DbSet<ProjectConfigPortfolioTag> ProjectConfigPortfolioTag { get; set; }
        public DbSet<PortfolioUser> PortfolioUser { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<ProjectBenefit> ProjectBenefit { get; set; }
        public DbSet<ProjectConfig> ProjectConfig { get; set; }
        public DbSet<ProjectOption> ProjectOption { get; set; }
        public DbSet<ProjectPhase> ProjectPhase { get; set; }
        public DbSet<ProjectUser> ProjectUser { get; set; }
        public DbSet<ResourceScenario> ResourceScenario { get; set; }
        public DbSet<ResourceScenarioUser> ResourceScenarioUser { get; set; }
        public DbSet<RiskProfile> RiskProfile { get; set; }
        public DbSet<StaffResource> StaffResource { get; set; }
        public DbSet<StaffAdjustment> StaffAdjustment { get; set; }
        public DbSet<StaffTransaction> StaffTransaction { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Alignment
            builder.Entity<Alignment>()
                .HasOne(a => a.AlignmentCategory)
                .WithMany(ac => ac.Alignments)
                .HasForeignKey(a => a.AlignmentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<Alignment>()
                .HasOne(a => a.ProjectBenefit)
                .WithMany(pb => pb.Alignments)
                .HasForeignKey(a => a.ProjectBenefitId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // BusinessUnit
            builder.Entity<BusinessUnit>()
                .HasOne(bu => bu.Organisation)
                .WithMany(o => o.BusinessUnits)
                .HasForeignKey(bu => bu.OrganisationId);
            
            builder.Entity<BusinessUnit>()
                .Property(c => c.Name)
                .IsRequired();
            
            // BenefitCategory
            builder.Entity<BenefitCategory>()
                .HasOne(bc => bc.Group)
                .WithMany(g => g.BenefitCategories)
                .HasForeignKey(bc => bc.GroupId);

             builder.Entity<BenefitCategory>()
                .Property(c => c.Name)
                .IsRequired();

            // StaffResourceCategory
            builder.Entity<StaffResourceCategory>()
                .HasOne(src => src.Group)
                .WithMany(g => g.StaffResourceCategories)
                .HasForeignKey(src => src.GroupId);
            
            builder.Entity<StaffResourceCategory>()
                .Property(c => c.Name)
                .IsRequired();
            
            // FinancialResourceCategory
            builder.Entity<FinancialResourceCategory>()
                .HasOne(frc => frc.Group)
                .WithMany(g => g.FinancialResourceCategories)
                .HasForeignKey(frc => frc.GroupId);
            
            builder.Entity<FinancialResourceCategory>()
                .Property(c => c.Name)
                .IsRequired();
            
            // RiskCategory
            builder.Entity<RiskCategory>()
                .HasOne(rc => rc.Group)
                .WithMany(g => g.RiskCategories)
                .HasForeignKey(rc => rc.GroupId);
            
            builder.Entity<RiskCategory>()
                .Property(c => c.Name)
                .IsRequired();
            
            // AlignmentCategory
            builder.Entity<AlignmentCategory>()
                .HasOne(ac => ac.Group)
                .WithMany(g => g.AlignmentCategories)
                .HasForeignKey(ac => ac.GroupId);
            
            builder.Entity<AlignmentCategory>()
                .Property(c => c.Name)
                .IsRequired();

            // FinancialAdjustment
            builder.Entity<FinancialAdjustment>()
                .HasOne(fa => fa.FinancialResourcePartition)
                .WithMany(fp => fp.Adjustments)
                .HasForeignKey(fa => fa.FinancialResourcePartitionId);
            
            builder.Entity<FinancialAdjustment>()
                .Property(fa => fa.Date)
                .IsRequired();
            
            // FinancialResource
            builder.Entity<FinancialResource>()
                .HasOne(fr => fr.ResourceScenario)
                .WithMany(rs => rs.FinancialResources)
                .HasForeignKey(fr => fr.ResourceScenarioId);
            
            builder.Entity<FinancialResource>()
                .Property(fr => fr.Name)
                .IsRequired();
            
            builder.Entity<FinancialResource>()
                .Property(fr => fr.StartDate)
                .IsRequired();
            
            // FinancialResourcePartition
            builder.Entity<FinancialResourcePartition>()
                .HasOne(frp => frp.FinancialResource)
                .WithMany(fr => fr.Partitions)
                .HasForeignKey(frp => frp.FinancialResourceId);

            builder.Entity<PartitionResourceCategory>()
                .HasKey(prc => new {prc.FinancialResourcePartitionId, prc.FinancialResourceCategoryId});
            
            builder.Entity<PartitionResourceCategory>()
                .HasOne(prc => prc.FinancialResourcePartition)
                .WithMany(frp => frp.Categories)
                .HasForeignKey(prc => prc.FinancialResourcePartitionId);
            
            builder.Entity<PartitionResourceCategory>()
                .HasOne(prc => prc.FinancialResourceCategory)
                .WithMany(frc => frc.FinancialPartitions)
                .HasForeignKey(prc => prc.FinancialResourceCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // FinancialTransaction
            builder.Entity<FinancialTransaction>()
                .HasOne(ft => ft.ProjectPhase)
                .WithMany(pp => pp.FinancialResources)
                .HasForeignKey(ft => ft.ProjectPhaseId);
            
            builder.Entity<FinancialTransaction>()
                .Property(ft => ft.Date)
                .IsRequired();
            
            builder.Entity<FinancialTransactionResourceCategory>()
                .HasKey(ftrc => new {ftrc.FinancialResourceCategoryId, ftrc.FinancialTransactionId});
            
            builder.Entity<FinancialTransactionResourceCategory>()
                .HasOne(ftrc => ftrc.FinancialResourceCategory)
                .WithMany(frc => frc.Transactions)
                .HasForeignKey(ftrc => ftrc.FinancialResourceCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<FinancialTransactionResourceCategory>()
                .HasOne(ftrc => ftrc.FinancialTransaction)
                .WithMany(ft => ft.Categories)
                .HasForeignKey(ftrc => ftrc.FinancialTransactionId);
            
            // Group
            builder.Entity<Group>()
                .HasOne(g => g.Parent)
                .WithMany(g => g.Children)
                .HasForeignKey(g => g.ParentId)
                .OnDelete(DeleteBehavior.SetNull);
            
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
                .WithOne(sr => sr.UserData);
            
            builder.Entity<UserGroup>()
                .HasKey(ug => new { ug.UserId, ug.GroupId });

            builder.Entity<UserGroup>()
                .HasOne(ug => ug.User)
                .WithMany(u => u.Groups)
                .HasForeignKey(ug => ug.UserId);
            
            builder.Entity<UserGroup>()
                .HasOne(ug => ug.Group)
                .WithMany(g => g.Members)
                .HasForeignKey(ug => ug.GroupId);
            
            builder.Entity<Organisation>()
                .Property(o => o.Name)
                .IsRequired();
            
            // PhaseConfig
            builder.Entity<PhaseConfig>()
                .HasOne(pc => pc.ProjectPhase)
                .WithMany()
                .HasForeignKey(pc => pc.ProjectPhaseId);

            builder.Entity<PhaseConfig>()
                .HasOne(pc => pc.ProjectConfig)
                .WithMany(pjc => pjc.Phases)
                .HasForeignKey(pc => pc.ProjectConfigId);
            
            // Portfolio
            builder.Entity<Portfolio>()
                .HasOne(p => p.Creator)
                .WithMany(u => u.Portfolios);
            
            builder.Entity<Portfolio>()
                .HasOne(p => p.Group)
                .WithMany(g => g.Portfolios);
            
            builder.Entity<Portfolio>()
                .HasOne(p => p.ApprovedBy)
                .WithMany(u => u.PortfoliosApproved);
            
            builder.Entity<Portfolio>()
                .Property(p => p.Created)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("now()");
            
            builder.Entity<Portfolio>()
                .Property(p => p.Modified)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("now()");
            
            builder.Entity<Portfolio>()
                .Property(p => p.Name)
                .IsRequired();
            
            builder.Entity<Portfolio>()
                .Property(p => p.StartYear)
                .IsRequired();

            builder.Entity<Portfolio>()
                .Property(p => p.EndYear)
                .IsRequired();
            
            builder.Entity<PortfolioUser>()
                .HasKey(pu => new {pu.PortfolioId, pu.UserId});
            
            builder.Entity<PortfolioUser>()
                .HasOne(pu => pu.Portfolio)
                .WithMany(p => p.ShareUser)
                .HasForeignKey(pu => pu.PortfolioId);
            
            builder.Entity<PortfolioUser>()
                .HasOne(pu => pu.User)
                .WithMany(u => u.SharedPortfolios)
                .HasForeignKey(pu => pu.UserId);
            
            builder.Entity<PortfolioTag>()
                .HasOne(pt => pt.Portfolio)
                .WithMany(p => p.PortfolioTags)
                .HasForeignKey(pt => pt.PortfolioId);
            
            builder.Entity<ProjectConfigPortfolioTag>()
                .HasOne(pcpt => pcpt.PortfolioTag)
                .WithMany(pt => pt.Projects)
                .HasForeignKey(pcpt => pcpt.PortfolioTagId);
            
            builder.Entity<ProjectConfigPortfolioTag>()
                .HasOne(pcpt => pcpt.ProjectConfig)
                .WithMany()
                .HasForeignKey(pcpt => pcpt.ProjectConfigId);
            
            builder.Entity<ProjectConfigPortfolioTag>()
                .HasKey(pcpt => new {pcpt.PortfolioTagId, pcpt.ProjectConfigId});
            
            // Project
            builder.Entity<Project>()
                .HasOne(p => p.Creator)
                .WithMany(u => u.Projects);
            
            builder.Entity<Project>()
                .HasOne(p => p.Group)
                .WithMany(g => g.Projects);
            
            builder.Entity<Project>()
                .HasOne(p => p.OwningBusinessUnit)
                .WithMany(bu => bu.ProjectsOwned)
                .HasForeignKey(p => p.OwningBusinessUnitId);
            
            builder.Entity<Project>()
                .HasOne(p => p.ImpactedBusinessUnit)
                .WithMany(ibu => ibu.ProjectsImpacting)
                .HasForeignKey(p => p.ImpactedBusinessUnitId);
            
            builder.Entity<Project>()
                .Property(p => p.Name)
                .IsRequired();
            
            builder.Entity<Project>()
                .Property(p => p.Created)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("now()");
            
            builder.Entity<Project>()
                .Property(p => p.Modified)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("now()");
            
            builder.Entity<StaffResourceProjectConfig>()
                .HasKey(srp => new {srp.StaffResourceId, srp.ProjectConfigId});
            
            builder.Entity<StaffResourceProjectConfig>()
                .HasOne(srp => srp.StaffResource)
                .WithMany(sr => sr.ProjectsManaged)
                .HasForeignKey(srp => srp.StaffResourceId);
            
            builder.Entity<StaffResourceProjectConfig>()
                .HasOne(srpc => srpc.ProjectConfig)
                .WithMany(p => p.Managers)
                .HasForeignKey(srp => srp.ProjectConfigId);
            
            builder.Entity<ProjectFinancialResourceCategory>()
                .HasKey(pfrc => new {pfrc.ProjectId, pfrc.FinancialResourceCategoryId});

            builder.Entity<ProjectFinancialResourceCategory>()
                .HasOne(pfrc => pfrc.Project)
                .WithMany(p => p.FinancialResourceCategories)
                .HasForeignKey(pfrc => pfrc.ProjectId);
            
            builder.Entity<ProjectFinancialResourceCategory>()
                .HasOne(pfrc => pfrc.FinancialResourceCategory)
                .WithMany(frc => frc.Projects)
                .HasForeignKey(pfrc => pfrc.FinancialResourceCategoryId);
            
            builder.Entity<ProjectUser>()
                .HasKey(pu => new {pu.ProjectId, pu.UserId});

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
                .HasKey(pbc => new {pbc.ProjectBenefitId, pbc.BenefitCategoryId});
            
            builder.Entity<ProjectBenefitBenefitCategory>()
                .HasOne(pbbc => pbbc.ProjectBenefit)
                .WithMany(pb => pb.Categories)
                .HasForeignKey(pbbc => pbbc.ProjectBenefitId);
            
            builder.Entity<ProjectBenefitBenefitCategory>()
                .HasOne(pbbc => pbbc.BenefitCategory)
                .WithMany(bc => bc.ProjectBenefits)
                .HasForeignKey(pbbc => pbbc.BenefitCategoryId);
            
            builder.Entity<ProjectBenefit>()
                .Property(pb => pb.Name)
                .IsRequired();
            
            // ProjectConfig
            builder.Entity<ProjectConfig>()
                .HasOne(pc => pc.Portfolio)
                .WithMany(p => p.Projects)
                .HasForeignKey(pc => pc.PortfolioId);
            
            builder.Entity<ProjectConfig>()
                .Property(pc => pc.StartDate)
                .IsRequired();
            
            builder.Entity<ProjectConfig>()
                .HasOne(p => p.Owner)
                .WithMany(o => o.ProjectsOwned)
                .HasForeignKey(p => p.OwnerId)
                .OnDelete(DeleteBehavior.SetNull);
            
            // ProjectOption
            builder.Entity<ProjectOption>()
                .HasOne(po => po.Project)
                .WithMany(p => p.Options)
                .HasForeignKey(po => po.ProjectId);
            
            builder.Entity<ProjectDependency>()
                .HasKey(pd => new {pd.DependsOnId, pd.RequiredById});
            
            builder.Entity<ProjectDependency>()
                .HasOne(pd => pd.DependsOn)
                .WithMany(po => po.Dependencies)
                .HasForeignKey(pd => pd.DependsOnId);
            
            builder.Entity<ProjectDependency>()
                .HasOne(pd => pd.RequiredBy)
                .WithMany(po => po.RequiredBy)
                .HasForeignKey(pd => pd.RequiredById);
            
            builder.Entity<ProjectOption>()
                .Property(po => po.Description)
                .IsRequired();
            
            // ProjectPhase
            builder.Entity<ProjectPhase>()
                .HasOne(pp => pp.ProjectOption)
                .WithMany(po => po.Phases)
                .HasForeignKey(pp => pp.ProjectOptionId);
            
            builder.Entity<ProjectPhase>()
                .Property(pp => pp.Name)
                .IsRequired();
            
            builder.Entity<ProjectPhase>()
                .Property(pp => pp.StartDate)
                .IsRequired();
            
            builder.Entity<ProjectPhase>()
                .Property(pp => pp.EndDate)
                .IsRequired();

            // ResourceScenario
             builder.Entity<ResourceScenario>()
                .HasOne(p => p.Creator)
                .WithMany(u => u.ResourceScenarios);
            
            builder.Entity<ResourceScenario>()
                .HasOne(p => p.Group)
                .WithMany(g => g.ResourceScenarios);
            
            builder.Entity<ResourceScenario>()
                .HasOne(rs => rs.ApprovedBy)
                .WithMany(u => u.ResourceScenariosApproved);
            
            builder.Entity<ResourceScenarioUser>()
                .HasKey(rsu => new {rsu.ResourceScenarioId, rsu.UserId});
            
            builder.Entity<ResourceScenarioUser>()
                .HasOne(pu => pu.ResourceScenario)
                .WithMany(p => p.ShareUser)
                .HasForeignKey(pu => pu.ResourceScenarioId);
            
            builder.Entity<ResourceScenarioUser>()
                .HasOne(pu => pu.User)
                .WithMany(u => u.SharedResourceScenarios)
                .HasForeignKey(pu => pu.UserId);
            
            builder.Entity<ResourceScenario>()
                .Property(rs => rs.Name)
                .IsRequired();
            
            builder.Entity<ResourceScenario>()
                .Property(rs => rs.Created)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("now()");
            
            builder.Entity<ResourceScenario>()
                .Property(rs => rs.Modified)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("now()");
            
            // RiskProfile
            builder.Entity<RiskProfile>()
                .HasOne(rp => rp.ProjectOption)
                .WithMany(po => po.RiskProfile)
                .HasForeignKey(rp => rp.ProjectOptionId);
            
            builder.Entity<RiskProfile>()
                .HasOne(rp => rp.RiskCategory)
                .WithMany(rc => rc.RiskProfiles)
                .HasForeignKey(rp => rp.RiskCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // StaffResource
            builder.Entity<StaffResource>()
                .HasOne(sr => sr.ResourceScenario)
                .WithMany(rs => rs.StaffResources)
                .HasForeignKey(sr => sr.ResourceScenarioId);
            
            builder.Entity<StaffResourceStaffResourceCategory>()
                .HasKey(srsrc => new {srsrc.StaffResourceId, srsrc.StaffResourceCategoryId});
            
            builder.Entity<StaffResourceStaffResourceCategory>()
                .HasOne(srsrc => srsrc.StaffResource)
                .WithMany(sr => sr.Categories)
                .HasForeignKey(srsrc => srsrc.StaffResourceId);
            
            builder.Entity<StaffResourceStaffResourceCategory>()
                .HasOne(srsrc => srsrc.StaffResourceCategory)
                .WithMany(src => src.StaffResources)
                .HasForeignKey(srsrc => srsrc.StaffResourceCategoryId);
            
            builder.Entity<StaffResource>()
                .Property(sr => sr.Name)
                .IsRequired();
            
            builder.Entity<StaffResource>()
                .Property(sr => sr.StartDate)
                .IsRequired();
            
            builder.Entity<StaffResource>()
                .HasOne(sr => sr.UserData)
                .WithOne(u => u.StaffResource)
                .HasForeignKey<MerlinPlanUser>(u => u.StaffResourceId);
            
            // StaffAdjustment
            builder.Entity<StaffAdjustment>()
                .HasOne(st => st.StaffResource)
                .WithMany(sr => sr.Adjustments)
                .HasForeignKey(st => st.StaffResourceId);
            
            builder.Entity<StaffAdjustment>()
                .Property(st => st.Date)
                .IsRequired();

            // StaffTransaction
            builder.Entity<StaffTransaction>()
                .HasOne(st => st.ProjectPhase)
                .WithMany(pp => pp.StaffResources)
                .HasForeignKey(st => st.ProjectPhaseId);
            
            builder.Entity<StaffTransaction>()
                .HasOne(st => st.StaffResource)
                .WithMany()
                .HasForeignKey(st => st.StaffResourceId);
            
            builder.Entity<StaffTransaction>()
                .HasOne(st => st.Category)
                .WithMany()
                .HasForeignKey(st => st.StaffResourceCategoryId);

            builder.Entity<StaffTransaction>()
                .Property(st => st.Date)
                .IsRequired();                                                                                 
            base.OnModelCreating(builder);
        }
    }
}
