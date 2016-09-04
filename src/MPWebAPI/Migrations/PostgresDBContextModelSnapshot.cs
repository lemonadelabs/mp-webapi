﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using MPWebAPI.Models;

namespace MPWebAPI.Migrations
{
    [DbContext(typeof(PostgresDBContext))]
    partial class PostgresDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431");

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("MPWebAPI.Models.Alignment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Actual");

                    b.Property<int>("AlignmentCategoryId");

                    b.Property<DateTime>("Date");

                    b.Property<int>("ProjectBenefitId");

                    b.Property<float>("Value");

                    b.Property<float>("Weight");

                    b.HasKey("Id");

                    b.HasIndex("AlignmentCategoryId");

                    b.HasIndex("ProjectBenefitId");

                    b.ToTable("Alignment");
                });

            modelBuilder.Entity("MPWebAPI.Models.AlignmentCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Area");

                    b.Property<string>("Description");

                    b.Property<int>("GroupId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("AlignmentCategory");
                });

            modelBuilder.Entity("MPWebAPI.Models.BenefitCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<int>("GroupId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("BenefitCategory");
                });

            modelBuilder.Entity("MPWebAPI.Models.BusinessUnit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("OrganisationId");

                    b.HasKey("Id");

                    b.HasIndex("OrganisationId");

                    b.ToTable("BusinessUnit");
                });

            modelBuilder.Entity("MPWebAPI.Models.FinancialAdjustment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Actual");

                    b.Property<bool>("Additive");

                    b.Property<DateTime>("Date");

                    b.Property<int>("FinancialResourcePartitionId");

                    b.Property<int?>("StaffResourceId");

                    b.Property<decimal>("Value");

                    b.HasKey("Id");

                    b.HasIndex("FinancialResourcePartitionId");

                    b.HasIndex("StaffResourceId");

                    b.ToTable("FinancialAdjustment");
                });

            modelBuilder.Entity("MPWebAPI.Models.FinancialResource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("EndDate");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("ResourceScenarioId");

                    b.Property<DateTime>("StartDate");

                    b.HasKey("Id");

                    b.HasIndex("ResourceScenarioId");

                    b.ToTable("FinancialResource");
                });

            modelBuilder.Entity("MPWebAPI.Models.FinancialResourceCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<int>("GroupId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("FinancialResourceCategory");
                });

            modelBuilder.Entity("MPWebAPI.Models.FinancialResourcePartition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("FinancialResourceId");

                    b.HasKey("Id");

                    b.HasIndex("FinancialResourceId");

                    b.ToTable("FinancialResourcePartition");
                });

            modelBuilder.Entity("MPWebAPI.Models.FinancialTransaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Actual");

                    b.Property<bool>("Additive");

                    b.Property<DateTime>("Date");

                    b.Property<int>("ProjectPhaseId");

                    b.Property<string>("Reference");

                    b.Property<decimal>("Value");

                    b.HasKey("Id");

                    b.HasIndex("ProjectPhaseId");

                    b.ToTable("FinancialTransaction");
                });

            modelBuilder.Entity("MPWebAPI.Models.FinancialTransactionResourceCategory", b =>
                {
                    b.Property<int>("FinancialResourceCategoryId");

                    b.Property<int>("FinancialTransactionId");

                    b.HasKey("FinancialResourceCategoryId", "FinancialTransactionId");

                    b.HasIndex("FinancialResourceCategoryId");

                    b.HasIndex("FinancialTransactionId");

                    b.ToTable("FinancialTransactionResourceCategory");
                });

            modelBuilder.Entity("MPWebAPI.Models.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<int>("OrganisationId");

                    b.Property<int>("ParentId");

                    b.HasKey("Id");

                    b.HasIndex("OrganisationId");

                    b.HasIndex("ParentId");

                    b.ToTable("Group");
                });

            modelBuilder.Entity("MPWebAPI.Models.MerlinPlanUser", b =>
                {
                    b.Property<string>("Id");

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("EmployeeId");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedUserName")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<int>("OrganisationId");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<int>("StaffResourceId");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.HasIndex("OrganisationId");

                    b.HasIndex("StaffResourceId")
                        .IsUnique();

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("MPWebAPI.Models.Organisation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address1");

                    b.Property<string>("Address2");

                    b.Property<string>("Address3");

                    b.Property<string>("Contact");

                    b.Property<string>("Country");

                    b.Property<int>("FinancialYearStart");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("PostCode");

                    b.Property<string>("WebDomain");

                    b.HasKey("Id");

                    b.ToTable("Organisation");
                });

            modelBuilder.Entity("MPWebAPI.Models.PartitionResourceCategory", b =>
                {
                    b.Property<int>("FinancialResourcePartitionId");

                    b.Property<int>("FinancialResourceCategoryId");

                    b.HasKey("FinancialResourcePartitionId", "FinancialResourceCategoryId");

                    b.HasIndex("FinancialResourceCategoryId");

                    b.HasIndex("FinancialResourcePartitionId");

                    b.ToTable("PartitionResourceCategory");
                });

            modelBuilder.Entity("MPWebAPI.Models.PhaseConfig", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("EndDate");

                    b.Property<int>("ProjectConfigId");

                    b.Property<int>("ProjectPhaseId");

                    b.Property<DateTime>("StartDate");

                    b.HasKey("Id");

                    b.HasIndex("ProjectConfigId");

                    b.HasIndex("ProjectPhaseId");

                    b.ToTable("PhaseConfig");
                });

            modelBuilder.Entity("MPWebAPI.Models.Plan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Approved");

                    b.Property<string>("ApprovedById");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("now()");

                    b.Property<string>("CreatorId");

                    b.Property<DateTime>("EndYear");

                    b.Property<int?>("GroupId");

                    b.Property<DateTime>("Modified")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasDefaultValueSql("now()");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<bool>("ShareAll");

                    b.Property<bool>("ShareGroup");

                    b.Property<DateTime>("StartYear");

                    b.Property<int>("TimeScale");

                    b.HasKey("Id");

                    b.HasIndex("ApprovedById");

                    b.HasIndex("CreatorId");

                    b.HasIndex("GroupId");

                    b.ToTable("Plan");
                });

            modelBuilder.Entity("MPWebAPI.Models.PlanUser", b =>
                {
                    b.Property<int>("PlanId");

                    b.Property<string>("UserId");

                    b.HasKey("PlanId", "UserId");

                    b.HasIndex("PlanId");

                    b.HasIndex("UserId");

                    b.ToTable("PlanUser");
                });

            modelBuilder.Entity("MPWebAPI.Models.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("now()");

                    b.Property<string>("CreatorId");

                    b.Property<int?>("GroupId");

                    b.Property<int?>("ImpactedBusinessUnitId");

                    b.Property<DateTime>("Modified")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasDefaultValueSql("now()");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int?>("OwnerId");

                    b.Property<int?>("OwningBusinessUnitId");

                    b.Property<string>("Reference");

                    b.Property<bool>("ShareAll");

                    b.Property<bool>("ShareGroup");

                    b.Property<string>("Summary");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("GroupId");

                    b.HasIndex("ImpactedBusinessUnitId");

                    b.HasIndex("OwnerId");

                    b.HasIndex("OwningBusinessUnitId");

                    b.ToTable("Project");
                });

            modelBuilder.Entity("MPWebAPI.Models.ProjectBenefit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Achieved");

                    b.Property<float>("AchievedValue");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("ProjectOptionId");

                    b.HasKey("Id");

                    b.HasIndex("ProjectOptionId");

                    b.ToTable("ProjectBenefit");
                });

            modelBuilder.Entity("MPWebAPI.Models.ProjectBenefitBenefitCategory", b =>
                {
                    b.Property<int>("ProjectBenefitId");

                    b.Property<int>("BenefitCategoryId");

                    b.HasKey("ProjectBenefitId", "BenefitCategoryId");

                    b.HasIndex("BenefitCategoryId");

                    b.HasIndex("ProjectBenefitId");

                    b.ToTable("ProjectBenefitBenefitCategory");
                });

            modelBuilder.Entity("MPWebAPI.Models.ProjectConfig", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("PlanId");

                    b.Property<int>("ProjectOptionId");

                    b.Property<DateTime>("StartDate");

                    b.HasKey("Id");

                    b.HasIndex("PlanId");

                    b.HasIndex("ProjectOptionId");

                    b.ToTable("ProjectConfig");
                });

            modelBuilder.Entity("MPWebAPI.Models.ProjectDependency", b =>
                {
                    b.Property<int>("DependsOnId");

                    b.Property<int>("RequiredById");

                    b.HasKey("DependsOnId", "RequiredById");

                    b.HasIndex("DependsOnId");

                    b.HasIndex("RequiredById");

                    b.ToTable("ProjectDependency");
                });

            modelBuilder.Entity("MPWebAPI.Models.ProjectFinancialResourceCategory", b =>
                {
                    b.Property<int>("ProjectId");

                    b.Property<int>("FinancialResourceCategoryId");

                    b.HasKey("ProjectId", "FinancialResourceCategoryId");

                    b.HasIndex("FinancialResourceCategoryId");

                    b.HasIndex("ProjectId");

                    b.ToTable("ProjectFinancialResourceCategory");
                });

            modelBuilder.Entity("MPWebAPI.Models.ProjectOption", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<float>("Complexity");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<float>("Priority");

                    b.Property<int>("ProjectId");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("ProjectOption");
                });

            modelBuilder.Entity("MPWebAPI.Models.ProjectPhase", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<DateTime>("EndDate");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("ProjectOptionId");

                    b.Property<DateTime>("StartDate");

                    b.HasKey("Id");

                    b.HasIndex("ProjectOptionId");

                    b.ToTable("ProjectPhase");
                });

            modelBuilder.Entity("MPWebAPI.Models.ProjectUser", b =>
                {
                    b.Property<int>("ProjectId");

                    b.Property<string>("UserId");

                    b.HasKey("ProjectId", "UserId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("UserId");

                    b.ToTable("ProjectUser");
                });

            modelBuilder.Entity("MPWebAPI.Models.ResourceScenario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("now()");

                    b.Property<string>("CreatorId");

                    b.Property<int?>("GroupId");

                    b.Property<DateTime>("Modified")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasDefaultValueSql("now()");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<bool>("ShareAll");

                    b.Property<bool>("ShareGroup");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("GroupId");

                    b.ToTable("ResourceScenario");
                });

            modelBuilder.Entity("MPWebAPI.Models.ResourceScenarioUser", b =>
                {
                    b.Property<int>("ResourceScenarioId");

                    b.Property<string>("UserId");

                    b.HasKey("ResourceScenarioId", "UserId");

                    b.HasIndex("ResourceScenarioId");

                    b.HasIndex("UserId");

                    b.ToTable("ResourceScenarioUser");
                });

            modelBuilder.Entity("MPWebAPI.Models.RiskCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<int>("GroupId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("RiskCategory");
                });

            modelBuilder.Entity("MPWebAPI.Models.RiskProfile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Actual");

                    b.Property<DateTime>("Date");

                    b.Property<float>("Impact");

                    b.Property<float>("Mitigation");

                    b.Property<float>("Probability");

                    b.Property<int>("ProjectOptionId");

                    b.Property<float>("Residual");

                    b.Property<int>("RiskCategoryId");

                    b.HasKey("Id");

                    b.HasIndex("ProjectOptionId");

                    b.HasIndex("RiskCategoryId");

                    b.ToTable("RiskProfile");
                });

            modelBuilder.Entity("MPWebAPI.Models.StaffResource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("EndDate");

                    b.Property<float?>("FteOutput");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("ResourceScenarioId");

                    b.Property<DateTime>("StartDate");

                    b.HasKey("Id");

                    b.HasIndex("ResourceScenarioId");

                    b.ToTable("StaffResource");
                });

            modelBuilder.Entity("MPWebAPI.Models.StaffResourceCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<int>("GroupId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("StaffResourceCategory");
                });

            modelBuilder.Entity("MPWebAPI.Models.StaffResourceProject", b =>
                {
                    b.Property<int>("StaffResourceId");

                    b.Property<int>("ProjectId");

                    b.HasKey("StaffResourceId", "ProjectId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("StaffResourceId");

                    b.ToTable("StaffResourceProject");
                });

            modelBuilder.Entity("MPWebAPI.Models.StaffResourceStaffResourceCategory", b =>
                {
                    b.Property<int>("StaffResourceId");

                    b.Property<int>("StaffResourceCategoryId");

                    b.HasKey("StaffResourceId", "StaffResourceCategoryId");

                    b.HasIndex("StaffResourceCategoryId");

                    b.HasIndex("StaffResourceId");

                    b.ToTable("StaffResourceStaffResourceCategory");
                });

            modelBuilder.Entity("MPWebAPI.Models.StaffTransaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Actual");

                    b.Property<bool>("Additive");

                    b.Property<DateTime>("Date");

                    b.Property<int>("ProjectPhaseId");

                    b.Property<int>("Value");

                    b.HasKey("Id");

                    b.HasIndex("ProjectPhaseId");

                    b.ToTable("StaffTransaction");
                });

            modelBuilder.Entity("MPWebAPI.Models.UserGroup", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<int>("GroupId");

                    b.HasKey("UserId", "GroupId");

                    b.HasIndex("GroupId");

                    b.HasIndex("UserId");

                    b.ToTable("UserGroup");
                });

            modelBuilder.Entity("OpenIddict.OpenIddictApplication", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("ClientId");

                    b.Property<string>("ClientSecret");

                    b.Property<string>("DisplayName");

                    b.Property<string>("LogoutRedirectUri");

                    b.Property<string>("RedirectUri");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.HasIndex("ClientId")
                        .IsUnique();

                    b.ToTable("OpenIddictApplications");
                });

            modelBuilder.Entity("OpenIddict.OpenIddictAuthorization", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("Scope");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("OpenIddictAuthorizations");
                });

            modelBuilder.Entity("OpenIddict.OpenIddictScope", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("Description");

                    b.HasKey("Id");

                    b.ToTable("OpenIddictScopes");
                });

            modelBuilder.Entity("OpenIddict.OpenIddictToken", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("ApplicationId");

                    b.Property<string>("AuthorizationId");

                    b.Property<string>("Type");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId");

                    b.HasIndex("AuthorizationId");

                    b.HasIndex("UserId");

                    b.ToTable("OpenIddictTokens");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("MPWebAPI.Models.MerlinPlanUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("MPWebAPI.Models.MerlinPlanUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MPWebAPI.Models.MerlinPlanUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MPWebAPI.Models.Alignment", b =>
                {
                    b.HasOne("MPWebAPI.Models.AlignmentCategory", "AlignmentCategory")
                        .WithMany("Alignments")
                        .HasForeignKey("AlignmentCategoryId");

                    b.HasOne("MPWebAPI.Models.ProjectBenefit", "ProjectBenefit")
                        .WithMany("Alignments")
                        .HasForeignKey("ProjectBenefitId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MPWebAPI.Models.AlignmentCategory", b =>
                {
                    b.HasOne("MPWebAPI.Models.Group", "Group")
                        .WithMany("AlignmentCategories")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MPWebAPI.Models.BenefitCategory", b =>
                {
                    b.HasOne("MPWebAPI.Models.Group", "Group")
                        .WithMany("BenefitCategories")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MPWebAPI.Models.BusinessUnit", b =>
                {
                    b.HasOne("MPWebAPI.Models.Organisation", "Organisation")
                        .WithMany("BusinessUnits")
                        .HasForeignKey("OrganisationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MPWebAPI.Models.FinancialAdjustment", b =>
                {
                    b.HasOne("MPWebAPI.Models.FinancialResourcePartition", "FinancialResourcePartition")
                        .WithMany("Adjustments")
                        .HasForeignKey("FinancialResourcePartitionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MPWebAPI.Models.StaffResource")
                        .WithMany("Adjustments")
                        .HasForeignKey("StaffResourceId");
                });

            modelBuilder.Entity("MPWebAPI.Models.FinancialResource", b =>
                {
                    b.HasOne("MPWebAPI.Models.ResourceScenario", "ResourceScenario")
                        .WithMany("FinancialResources")
                        .HasForeignKey("ResourceScenarioId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MPWebAPI.Models.FinancialResourceCategory", b =>
                {
                    b.HasOne("MPWebAPI.Models.Group", "Group")
                        .WithMany("FinancialResourceCategories")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MPWebAPI.Models.FinancialResourcePartition", b =>
                {
                    b.HasOne("MPWebAPI.Models.FinancialResource", "FinancialResource")
                        .WithMany("Partitions")
                        .HasForeignKey("FinancialResourceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MPWebAPI.Models.FinancialTransaction", b =>
                {
                    b.HasOne("MPWebAPI.Models.ProjectPhase", "ProjectPhase")
                        .WithMany("FinancialResources")
                        .HasForeignKey("ProjectPhaseId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MPWebAPI.Models.FinancialTransactionResourceCategory", b =>
                {
                    b.HasOne("MPWebAPI.Models.FinancialResourceCategory", "FinancialResourceCategory")
                        .WithMany("Transactions")
                        .HasForeignKey("FinancialResourceCategoryId");

                    b.HasOne("MPWebAPI.Models.FinancialTransaction", "FinancialTransaction")
                        .WithMany("Categories")
                        .HasForeignKey("FinancialTransactionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MPWebAPI.Models.Group", b =>
                {
                    b.HasOne("MPWebAPI.Models.Organisation", "Organisation")
                        .WithMany("Groups")
                        .HasForeignKey("OrganisationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MPWebAPI.Models.Group", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("MPWebAPI.Models.MerlinPlanUser", b =>
                {
                    b.HasOne("MPWebAPI.Models.Organisation", "Organisation")
                        .WithMany("Users")
                        .HasForeignKey("OrganisationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MPWebAPI.Models.StaffResource", "StaffResource")
                        .WithOne("UserData")
                        .HasForeignKey("MPWebAPI.Models.MerlinPlanUser", "StaffResourceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MPWebAPI.Models.PartitionResourceCategory", b =>
                {
                    b.HasOne("MPWebAPI.Models.FinancialResourceCategory", "FinancialResourceCategory")
                        .WithMany("FinancialPartitions")
                        .HasForeignKey("FinancialResourceCategoryId");

                    b.HasOne("MPWebAPI.Models.FinancialResourcePartition", "FinancialResourcePartition")
                        .WithMany("Categories")
                        .HasForeignKey("FinancialResourcePartitionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MPWebAPI.Models.PhaseConfig", b =>
                {
                    b.HasOne("MPWebAPI.Models.ProjectConfig", "ProjectConfig")
                        .WithMany("Phases")
                        .HasForeignKey("ProjectConfigId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MPWebAPI.Models.ProjectPhase", "ProjectPhase")
                        .WithMany()
                        .HasForeignKey("ProjectPhaseId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MPWebAPI.Models.Plan", b =>
                {
                    b.HasOne("MPWebAPI.Models.MerlinPlanUser", "ApprovedBy")
                        .WithMany("PlansApproved")
                        .HasForeignKey("ApprovedById");

                    b.HasOne("MPWebAPI.Models.MerlinPlanUser", "Creator")
                        .WithMany("Plans")
                        .HasForeignKey("CreatorId");

                    b.HasOne("MPWebAPI.Models.Group", "Group")
                        .WithMany("Plans")
                        .HasForeignKey("GroupId");
                });

            modelBuilder.Entity("MPWebAPI.Models.PlanUser", b =>
                {
                    b.HasOne("MPWebAPI.Models.Plan", "Plan")
                        .WithMany("ShareUser")
                        .HasForeignKey("PlanId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MPWebAPI.Models.MerlinPlanUser", "User")
                        .WithMany("SharedPlans")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MPWebAPI.Models.Project", b =>
                {
                    b.HasOne("MPWebAPI.Models.MerlinPlanUser", "Creator")
                        .WithMany("Projects")
                        .HasForeignKey("CreatorId");

                    b.HasOne("MPWebAPI.Models.Group", "Group")
                        .WithMany("Projects")
                        .HasForeignKey("GroupId");

                    b.HasOne("MPWebAPI.Models.BusinessUnit", "ImpactedBusinessUnit")
                        .WithMany("ProjectsImpacting")
                        .HasForeignKey("ImpactedBusinessUnitId");

                    b.HasOne("MPWebAPI.Models.StaffResource", "Owner")
                        .WithMany("ProjectsOwned")
                        .HasForeignKey("OwnerId");

                    b.HasOne("MPWebAPI.Models.BusinessUnit", "OwningBusinessUnit")
                        .WithMany("ProjectsOwned")
                        .HasForeignKey("OwningBusinessUnitId");
                });

            modelBuilder.Entity("MPWebAPI.Models.ProjectBenefit", b =>
                {
                    b.HasOne("MPWebAPI.Models.ProjectOption", "ProjectOption")
                        .WithMany("Benefits")
                        .HasForeignKey("ProjectOptionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MPWebAPI.Models.ProjectBenefitBenefitCategory", b =>
                {
                    b.HasOne("MPWebAPI.Models.BenefitCategory", "BenefitCategory")
                        .WithMany("ProjectBenefits")
                        .HasForeignKey("BenefitCategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MPWebAPI.Models.ProjectBenefit", "ProjectBenefit")
                        .WithMany("Categories")
                        .HasForeignKey("ProjectBenefitId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MPWebAPI.Models.ProjectConfig", b =>
                {
                    b.HasOne("MPWebAPI.Models.Plan", "Plan")
                        .WithMany("Projects")
                        .HasForeignKey("PlanId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MPWebAPI.Models.ProjectOption", "ProjectOption")
                        .WithMany()
                        .HasForeignKey("ProjectOptionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MPWebAPI.Models.ProjectDependency", b =>
                {
                    b.HasOne("MPWebAPI.Models.ProjectOption", "DependsOn")
                        .WithMany("Dependencies")
                        .HasForeignKey("DependsOnId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MPWebAPI.Models.ProjectOption", "RequiredBy")
                        .WithMany("RequiredBy")
                        .HasForeignKey("RequiredById")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MPWebAPI.Models.ProjectFinancialResourceCategory", b =>
                {
                    b.HasOne("MPWebAPI.Models.FinancialResourceCategory", "FinancialResourceCategory")
                        .WithMany("Projects")
                        .HasForeignKey("FinancialResourceCategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MPWebAPI.Models.Project", "Project")
                        .WithMany("FinancialResourceCategories")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MPWebAPI.Models.ProjectOption", b =>
                {
                    b.HasOne("MPWebAPI.Models.Project", "Project")
                        .WithMany("Options")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MPWebAPI.Models.ProjectPhase", b =>
                {
                    b.HasOne("MPWebAPI.Models.ProjectOption", "ProjectOption")
                        .WithMany("Phases")
                        .HasForeignKey("ProjectOptionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MPWebAPI.Models.ProjectUser", b =>
                {
                    b.HasOne("MPWebAPI.Models.Project", "Project")
                        .WithMany("ShareUser")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MPWebAPI.Models.MerlinPlanUser", "User")
                        .WithMany("SharedProjects")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MPWebAPI.Models.ResourceScenario", b =>
                {
                    b.HasOne("MPWebAPI.Models.MerlinPlanUser", "Creator")
                        .WithMany("ResourceScenarios")
                        .HasForeignKey("CreatorId");

                    b.HasOne("MPWebAPI.Models.Group", "Group")
                        .WithMany("ResourceScenarios")
                        .HasForeignKey("GroupId");
                });

            modelBuilder.Entity("MPWebAPI.Models.ResourceScenarioUser", b =>
                {
                    b.HasOne("MPWebAPI.Models.ResourceScenario", "ResourceScenario")
                        .WithMany("ShareUser")
                        .HasForeignKey("ResourceScenarioId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MPWebAPI.Models.MerlinPlanUser", "User")
                        .WithMany("SharedResourceScenarios")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MPWebAPI.Models.RiskCategory", b =>
                {
                    b.HasOne("MPWebAPI.Models.Group", "Group")
                        .WithMany("RiskCategories")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MPWebAPI.Models.RiskProfile", b =>
                {
                    b.HasOne("MPWebAPI.Models.ProjectOption", "ProjectOption")
                        .WithMany("RiskProfile")
                        .HasForeignKey("ProjectOptionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MPWebAPI.Models.RiskCategory", "RiskCategory")
                        .WithMany("RiskProfiles")
                        .HasForeignKey("RiskCategoryId");
                });

            modelBuilder.Entity("MPWebAPI.Models.StaffResource", b =>
                {
                    b.HasOne("MPWebAPI.Models.ResourceScenario", "ResourceScenario")
                        .WithMany("StaffResources")
                        .HasForeignKey("ResourceScenarioId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MPWebAPI.Models.StaffResourceCategory", b =>
                {
                    b.HasOne("MPWebAPI.Models.Group", "Group")
                        .WithMany("StaffResourceCategories")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MPWebAPI.Models.StaffResourceProject", b =>
                {
                    b.HasOne("MPWebAPI.Models.Project", "Project")
                        .WithMany("Managers")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MPWebAPI.Models.StaffResource", "StaffResource")
                        .WithMany("ProjectsManaged")
                        .HasForeignKey("StaffResourceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MPWebAPI.Models.StaffResourceStaffResourceCategory", b =>
                {
                    b.HasOne("MPWebAPI.Models.StaffResourceCategory", "StaffResourceCategory")
                        .WithMany("StaffResources")
                        .HasForeignKey("StaffResourceCategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MPWebAPI.Models.StaffResource", "StaffResource")
                        .WithMany("Categories")
                        .HasForeignKey("StaffResourceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MPWebAPI.Models.StaffTransaction", b =>
                {
                    b.HasOne("MPWebAPI.Models.ProjectPhase", "ProjectPhase")
                        .WithMany("StaffResources")
                        .HasForeignKey("ProjectPhaseId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MPWebAPI.Models.UserGroup", b =>
                {
                    b.HasOne("MPWebAPI.Models.Group", "Group")
                        .WithMany("Members")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MPWebAPI.Models.MerlinPlanUser", "User")
                        .WithMany("Groups")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("OpenIddict.OpenIddictAuthorization", b =>
                {
                    b.HasOne("MPWebAPI.Models.MerlinPlanUser")
                        .WithMany("Authorizations")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("OpenIddict.OpenIddictToken", b =>
                {
                    b.HasOne("OpenIddict.OpenIddictApplication")
                        .WithMany("Tokens")
                        .HasForeignKey("ApplicationId");

                    b.HasOne("OpenIddict.OpenIddictAuthorization")
                        .WithMany("Tokens")
                        .HasForeignKey("AuthorizationId");

                    b.HasOne("MPWebAPI.Models.MerlinPlanUser")
                        .WithMany("Tokens")
                        .HasForeignKey("UserId");
                });
        }
    }
}
