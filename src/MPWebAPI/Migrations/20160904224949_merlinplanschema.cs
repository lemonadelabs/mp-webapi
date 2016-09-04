using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MPWebAPI.Migrations
{
    public partial class merlinplanschema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Organisation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGeneratedOnAdd", true),
                    Address1 = table.Column<string>(nullable: true),
                    Address2 = table.Column<string>(nullable: true),
                    Address3 = table.Column<string>(nullable: true),
                    Contact = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    FinancialYearStart = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    PostCode = table.Column<string>(nullable: true),
                    WebDomain = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organisation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BusinessUnit",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGeneratedOnAdd", true),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    OrganisationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessUnit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessUnit_Organisation_OrganisationId",
                        column: x => x.OrganisationId,
                        principalTable: "Organisation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Group",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGeneratedOnAdd", true),
                    Description = table.Column<string>(nullable: true),
                    OrganisationId = table.Column<int>(nullable: false),
                    ParentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Group_Organisation_OrganisationId",
                        column: x => x.OrganisationId,
                        principalTable: "Organisation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Group_Group_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "AlignmentCategory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGeneratedOnAdd", true),
                    Area = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    GroupId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlignmentCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlignmentCategory_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BenefitCategory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGeneratedOnAdd", true),
                    Description = table.Column<string>(nullable: true),
                    GroupId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BenefitCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BenefitCategory_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FinancialResourceCategory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGeneratedOnAdd", true),
                    Description = table.Column<string>(nullable: true),
                    GroupId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialResourceCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinancialResourceCategory_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Plan",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGeneratedOnAdd", true),
                    Approved = table.Column<bool>(nullable: false),
                    ApprovedById = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false, defaultValueSql: "now()")
                        .Annotation("Npgsql:ValueGeneratedOnAdd", true),
                    CreatorId = table.Column<string>(nullable: true),
                    EndYear = table.Column<DateTime>(nullable: false),
                    GroupId = table.Column<int>(nullable: true),
                    Modified = table.Column<DateTime>(nullable: false, defaultValueSql: "now()"),
                    Name = table.Column<string>(nullable: false),
                    ShareAll = table.Column<bool>(nullable: false),
                    ShareGroup = table.Column<bool>(nullable: false),
                    StartYear = table.Column<DateTime>(nullable: false),
                    TimeScale = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Plan_AspNetUsers_ApprovedById",
                        column: x => x.ApprovedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Plan_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Plan_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ResourceScenario",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGeneratedOnAdd", true),
                    Created = table.Column<DateTime>(nullable: false, defaultValueSql: "now()")
                        .Annotation("Npgsql:ValueGeneratedOnAdd", true),
                    CreatorId = table.Column<string>(nullable: true),
                    GroupId = table.Column<int>(nullable: true),
                    Modified = table.Column<DateTime>(nullable: false, defaultValueSql: "now()"),
                    Name = table.Column<string>(nullable: false),
                    ShareAll = table.Column<bool>(nullable: false),
                    ShareGroup = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceScenario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResourceScenario_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResourceScenario_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RiskCategory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGeneratedOnAdd", true),
                    Description = table.Column<string>(nullable: true),
                    GroupId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiskCategory_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StaffResourceCategory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGeneratedOnAdd", true),
                    Description = table.Column<string>(nullable: true),
                    GroupId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffResourceCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaffResourceCategory_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserGroup",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    GroupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroup", x => new { x.UserId, x.GroupId });
                    table.ForeignKey(
                        name: "FK_UserGroup_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGroup_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanUser",
                columns: table => new
                {
                    PlanId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanUser", x => new { x.PlanId, x.UserId });
                    table.ForeignKey(
                        name: "FK_PlanUser_Plan_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanUser_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FinancialResource",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGeneratedOnAdd", true),
                    EndDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    ResourceScenarioId = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialResource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinancialResource_ResourceScenario_ResourceScenarioId",
                        column: x => x.ResourceScenarioId,
                        principalTable: "ResourceScenario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResourceScenarioUser",
                columns: table => new
                {
                    ResourceScenarioId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceScenarioUser", x => new { x.ResourceScenarioId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ResourceScenarioUser_ResourceScenario_ResourceScenarioId",
                        column: x => x.ResourceScenarioId,
                        principalTable: "ResourceScenario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResourceScenarioUser_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StaffResource",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGeneratedOnAdd", true),
                    EndDate = table.Column<DateTime>(nullable: false),
                    FteOutput = table.Column<float>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    ResourceScenarioId = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffResource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaffResource_ResourceScenario_ResourceScenarioId",
                        column: x => x.ResourceScenarioId,
                        principalTable: "ResourceScenario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FinancialResourcePartition",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGeneratedOnAdd", true),
                    FinancialResourceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialResourcePartition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinancialResourcePartition_FinancialResource_FinancialResourceId",
                        column: x => x.FinancialResourceId,
                        principalTable: "FinancialResource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGeneratedOnAdd", true),
                    Created = table.Column<DateTime>(nullable: false, defaultValueSql: "now()")
                        .Annotation("Npgsql:ValueGeneratedOnAdd", true),
                    CreatorId = table.Column<string>(nullable: true),
                    GroupId = table.Column<int>(nullable: true),
                    ImpactedBusinessUnitId = table.Column<int>(nullable: true),
                    Modified = table.Column<DateTime>(nullable: false, defaultValueSql: "now()"),
                    Name = table.Column<string>(nullable: false),
                    OwnerId = table.Column<int>(nullable: true),
                    OwningBusinessUnitId = table.Column<int>(nullable: true),
                    Reference = table.Column<string>(nullable: true),
                    ShareAll = table.Column<bool>(nullable: false),
                    ShareGroup = table.Column<bool>(nullable: false),
                    Summary = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Project_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_BusinessUnit_ImpactedBusinessUnitId",
                        column: x => x.ImpactedBusinessUnitId,
                        principalTable: "BusinessUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_StaffResource_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "StaffResource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_BusinessUnit_OwningBusinessUnitId",
                        column: x => x.OwningBusinessUnitId,
                        principalTable: "BusinessUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StaffResourceStaffResourceCategory",
                columns: table => new
                {
                    StaffResourceId = table.Column<int>(nullable: false),
                    StaffResourceCategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffResourceStaffResourceCategory", x => new { x.StaffResourceId, x.StaffResourceCategoryId });
                    table.ForeignKey(
                        name: "FK_StaffResourceStaffResourceCategory_StaffResourceCategory_StaffResourceCategoryId",
                        column: x => x.StaffResourceCategoryId,
                        principalTable: "StaffResourceCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StaffResourceStaffResourceCategory_StaffResource_StaffResourceId",
                        column: x => x.StaffResourceId,
                        principalTable: "StaffResource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FinancialAdjustment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGeneratedOnAdd", true),
                    Actual = table.Column<bool>(nullable: false),
                    Additive = table.Column<bool>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    FinancialResourcePartitionId = table.Column<int>(nullable: false),
                    StaffResourceId = table.Column<int>(nullable: true),
                    Value = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialAdjustment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinancialAdjustment_FinancialResourcePartition_FinancialResourcePartitionId",
                        column: x => x.FinancialResourcePartitionId,
                        principalTable: "FinancialResourcePartition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FinancialAdjustment_StaffResource_StaffResourceId",
                        column: x => x.StaffResourceId,
                        principalTable: "StaffResource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PartitionResourceCategory",
                columns: table => new
                {
                    FinancialResourcePartitionId = table.Column<int>(nullable: false),
                    FinancialResourceCategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartitionResourceCategory", x => new { x.FinancialResourcePartitionId, x.FinancialResourceCategoryId });
                    table.ForeignKey(
                        name: "FK_PartitionResourceCategory_FinancialResourceCategory_FinancialResourceCategoryId",
                        column: x => x.FinancialResourceCategoryId,
                        principalTable: "FinancialResourceCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PartitionResourceCategory_FinancialResourcePartition_FinancialResourcePartitionId",
                        column: x => x.FinancialResourcePartitionId,
                        principalTable: "FinancialResourcePartition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectFinancialResourceCategory",
                columns: table => new
                {
                    ProjectId = table.Column<int>(nullable: false),
                    FinancialResourceCategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectFinancialResourceCategory", x => new { x.ProjectId, x.FinancialResourceCategoryId });
                    table.ForeignKey(
                        name: "FK_ProjectFinancialResourceCategory_FinancialResourceCategory_FinancialResourceCategoryId",
                        column: x => x.FinancialResourceCategoryId,
                        principalTable: "FinancialResourceCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectFinancialResourceCategory_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectOption",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGeneratedOnAdd", true),
                    Complexity = table.Column<float>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Priority = table.Column<float>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectOption", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectOption_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectUser",
                columns: table => new
                {
                    ProjectId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectUser", x => new { x.ProjectId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ProjectUser_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectUser_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StaffResourceProject",
                columns: table => new
                {
                    StaffResourceId = table.Column<int>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffResourceProject", x => new { x.StaffResourceId, x.ProjectId });
                    table.ForeignKey(
                        name: "FK_StaffResourceProject_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StaffResourceProject_StaffResource_StaffResourceId",
                        column: x => x.StaffResourceId,
                        principalTable: "StaffResource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectBenefit",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGeneratedOnAdd", true),
                    Achieved = table.Column<bool>(nullable: false),
                    AchievedValue = table.Column<float>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    ProjectOptionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectBenefit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectBenefit_ProjectOption_ProjectOptionId",
                        column: x => x.ProjectOptionId,
                        principalTable: "ProjectOption",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectConfig",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGeneratedOnAdd", true),
                    PlanId = table.Column<int>(nullable: false),
                    ProjectOptionId = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectConfig", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectConfig_Plan_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectConfig_ProjectOption_ProjectOptionId",
                        column: x => x.ProjectOptionId,
                        principalTable: "ProjectOption",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectDependency",
                columns: table => new
                {
                    DependsOnId = table.Column<int>(nullable: false),
                    RequiredById = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectDependency", x => new { x.DependsOnId, x.RequiredById });
                    table.ForeignKey(
                        name: "FK_ProjectDependency_ProjectOption_DependsOnId",
                        column: x => x.DependsOnId,
                        principalTable: "ProjectOption",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectDependency_ProjectOption_RequiredById",
                        column: x => x.RequiredById,
                        principalTable: "ProjectOption",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectPhase",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGeneratedOnAdd", true),
                    Description = table.Column<string>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    ProjectOptionId = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectPhase", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectPhase_ProjectOption_ProjectOptionId",
                        column: x => x.ProjectOptionId,
                        principalTable: "ProjectOption",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RiskProfile",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGeneratedOnAdd", true),
                    Actual = table.Column<bool>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Impact = table.Column<float>(nullable: false),
                    Mitigation = table.Column<float>(nullable: false),
                    Probability = table.Column<float>(nullable: false),
                    ProjectOptionId = table.Column<int>(nullable: false),
                    Residual = table.Column<float>(nullable: false),
                    RiskCategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiskProfile_ProjectOption_ProjectOptionId",
                        column: x => x.ProjectOptionId,
                        principalTable: "ProjectOption",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RiskProfile_RiskCategory_RiskCategoryId",
                        column: x => x.RiskCategoryId,
                        principalTable: "RiskCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Alignment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGeneratedOnAdd", true),
                    Actual = table.Column<bool>(nullable: false),
                    AlignmentCategoryId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    ProjectBenefitId = table.Column<int>(nullable: false),
                    Value = table.Column<float>(nullable: false),
                    Weight = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alignment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alignment_AlignmentCategory_AlignmentCategoryId",
                        column: x => x.AlignmentCategoryId,
                        principalTable: "AlignmentCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Alignment_ProjectBenefit_ProjectBenefitId",
                        column: x => x.ProjectBenefitId,
                        principalTable: "ProjectBenefit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectBenefitBenefitCategory",
                columns: table => new
                {
                    ProjectBenefitId = table.Column<int>(nullable: false),
                    BenefitCategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectBenefitBenefitCategory", x => new { x.ProjectBenefitId, x.BenefitCategoryId });
                    table.ForeignKey(
                        name: "FK_ProjectBenefitBenefitCategory_BenefitCategory_BenefitCategoryId",
                        column: x => x.BenefitCategoryId,
                        principalTable: "BenefitCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectBenefitBenefitCategory_ProjectBenefit_ProjectBenefitId",
                        column: x => x.ProjectBenefitId,
                        principalTable: "ProjectBenefit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FinancialTransaction",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGeneratedOnAdd", true),
                    Actual = table.Column<bool>(nullable: false),
                    Additive = table.Column<bool>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    ProjectPhaseId = table.Column<int>(nullable: false),
                    Reference = table.Column<string>(nullable: true),
                    Value = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialTransaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinancialTransaction_ProjectPhase_ProjectPhaseId",
                        column: x => x.ProjectPhaseId,
                        principalTable: "ProjectPhase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhaseConfig",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGeneratedOnAdd", true),
                    EndDate = table.Column<DateTime>(nullable: false),
                    ProjectConfigId = table.Column<int>(nullable: false),
                    ProjectPhaseId = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhaseConfig", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhaseConfig_ProjectConfig_ProjectConfigId",
                        column: x => x.ProjectConfigId,
                        principalTable: "ProjectConfig",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PhaseConfig_ProjectPhase_ProjectPhaseId",
                        column: x => x.ProjectPhaseId,
                        principalTable: "ProjectPhase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StaffTransaction",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGeneratedOnAdd", true),
                    Actual = table.Column<bool>(nullable: false),
                    Additive = table.Column<bool>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    ProjectPhaseId = table.Column<int>(nullable: false),
                    Value = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffTransaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaffTransaction_ProjectPhase_ProjectPhaseId",
                        column: x => x.ProjectPhaseId,
                        principalTable: "ProjectPhase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FinancialTransactionResourceCategory",
                columns: table => new
                {
                    FinancialResourceCategoryId = table.Column<int>(nullable: false),
                    FinancialTransactionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialTransactionResourceCategory", x => new { x.FinancialResourceCategoryId, x.FinancialTransactionId });
                    table.ForeignKey(
                        name: "FK_FinancialTransactionResourceCategory_FinancialResourceCategory_FinancialResourceCategoryId",
                        column: x => x.FinancialResourceCategoryId,
                        principalTable: "FinancialResourceCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FinancialTransactionResourceCategory_FinancialTransaction_FinancialTransactionId",
                        column: x => x.FinancialTransactionId,
                        principalTable: "FinancialTransaction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddColumn<string>(
                name: "EmployeeId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrganisationId",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StaffResourceId",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_OrganisationId",
                table: "AspNetUsers",
                column: "OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_StaffResourceId",
                table: "AspNetUsers",
                column: "StaffResourceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Alignment_AlignmentCategoryId",
                table: "Alignment",
                column: "AlignmentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Alignment_ProjectBenefitId",
                table: "Alignment",
                column: "ProjectBenefitId");

            migrationBuilder.CreateIndex(
                name: "IX_AlignmentCategory_GroupId",
                table: "AlignmentCategory",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_BenefitCategory_GroupId",
                table: "BenefitCategory",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessUnit_OrganisationId",
                table: "BusinessUnit",
                column: "OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialAdjustment_FinancialResourcePartitionId",
                table: "FinancialAdjustment",
                column: "FinancialResourcePartitionId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialAdjustment_StaffResourceId",
                table: "FinancialAdjustment",
                column: "StaffResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialResource_ResourceScenarioId",
                table: "FinancialResource",
                column: "ResourceScenarioId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialResourceCategory_GroupId",
                table: "FinancialResourceCategory",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialResourcePartition_FinancialResourceId",
                table: "FinancialResourcePartition",
                column: "FinancialResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialTransaction_ProjectPhaseId",
                table: "FinancialTransaction",
                column: "ProjectPhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialTransactionResourceCategory_FinancialResourceCategoryId",
                table: "FinancialTransactionResourceCategory",
                column: "FinancialResourceCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialTransactionResourceCategory_FinancialTransactionId",
                table: "FinancialTransactionResourceCategory",
                column: "FinancialTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Group_OrganisationId",
                table: "Group",
                column: "OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_Group_ParentId",
                table: "Group",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_PartitionResourceCategory_FinancialResourceCategoryId",
                table: "PartitionResourceCategory",
                column: "FinancialResourceCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PartitionResourceCategory_FinancialResourcePartitionId",
                table: "PartitionResourceCategory",
                column: "FinancialResourcePartitionId");

            migrationBuilder.CreateIndex(
                name: "IX_PhaseConfig_ProjectConfigId",
                table: "PhaseConfig",
                column: "ProjectConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_PhaseConfig_ProjectPhaseId",
                table: "PhaseConfig",
                column: "ProjectPhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Plan_ApprovedById",
                table: "Plan",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_Plan_CreatorId",
                table: "Plan",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Plan_GroupId",
                table: "Plan",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanUser_PlanId",
                table: "PlanUser",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanUser_UserId",
                table: "PlanUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_CreatorId",
                table: "Project",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_GroupId",
                table: "Project",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_ImpactedBusinessUnitId",
                table: "Project",
                column: "ImpactedBusinessUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_OwnerId",
                table: "Project",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_OwningBusinessUnitId",
                table: "Project",
                column: "OwningBusinessUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectBenefit_ProjectOptionId",
                table: "ProjectBenefit",
                column: "ProjectOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectBenefitBenefitCategory_BenefitCategoryId",
                table: "ProjectBenefitBenefitCategory",
                column: "BenefitCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectBenefitBenefitCategory_ProjectBenefitId",
                table: "ProjectBenefitBenefitCategory",
                column: "ProjectBenefitId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectConfig_PlanId",
                table: "ProjectConfig",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectConfig_ProjectOptionId",
                table: "ProjectConfig",
                column: "ProjectOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDependency_DependsOnId",
                table: "ProjectDependency",
                column: "DependsOnId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDependency_RequiredById",
                table: "ProjectDependency",
                column: "RequiredById");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFinancialResourceCategory_FinancialResourceCategoryId",
                table: "ProjectFinancialResourceCategory",
                column: "FinancialResourceCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFinancialResourceCategory_ProjectId",
                table: "ProjectFinancialResourceCategory",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectOption_ProjectId",
                table: "ProjectOption",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectPhase_ProjectOptionId",
                table: "ProjectPhase",
                column: "ProjectOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUser_ProjectId",
                table: "ProjectUser",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUser_UserId",
                table: "ProjectUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceScenario_CreatorId",
                table: "ResourceScenario",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceScenario_GroupId",
                table: "ResourceScenario",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceScenarioUser_ResourceScenarioId",
                table: "ResourceScenarioUser",
                column: "ResourceScenarioId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceScenarioUser_UserId",
                table: "ResourceScenarioUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskCategory_GroupId",
                table: "RiskCategory",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskProfile_ProjectOptionId",
                table: "RiskProfile",
                column: "ProjectOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskProfile_RiskCategoryId",
                table: "RiskProfile",
                column: "RiskCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffResource_ResourceScenarioId",
                table: "StaffResource",
                column: "ResourceScenarioId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffResourceCategory_GroupId",
                table: "StaffResourceCategory",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffResourceProject_ProjectId",
                table: "StaffResourceProject",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffResourceProject_StaffResourceId",
                table: "StaffResourceProject",
                column: "StaffResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffResourceStaffResourceCategory_StaffResourceCategoryId",
                table: "StaffResourceStaffResourceCategory",
                column: "StaffResourceCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffResourceStaffResourceCategory_StaffResourceId",
                table: "StaffResourceStaffResourceCategory",
                column: "StaffResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffTransaction_ProjectPhaseId",
                table: "StaffTransaction",
                column: "ProjectPhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroup_GroupId",
                table: "UserGroup",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroup_UserId",
                table: "UserGroup",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Organisation_OrganisationId",
                table: "AspNetUsers",
                column: "OrganisationId",
                principalTable: "Organisation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_StaffResource_StaffResourceId",
                table: "AspNetUsers",
                column: "StaffResourceId",
                principalTable: "StaffResource",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Organisation_OrganisationId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_StaffResource_StaffResourceId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_OrganisationId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_StaffResourceId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OrganisationId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StaffResourceId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Alignment");

            migrationBuilder.DropTable(
                name: "FinancialAdjustment");

            migrationBuilder.DropTable(
                name: "FinancialTransactionResourceCategory");

            migrationBuilder.DropTable(
                name: "PartitionResourceCategory");

            migrationBuilder.DropTable(
                name: "PhaseConfig");

            migrationBuilder.DropTable(
                name: "PlanUser");

            migrationBuilder.DropTable(
                name: "ProjectBenefitBenefitCategory");

            migrationBuilder.DropTable(
                name: "ProjectDependency");

            migrationBuilder.DropTable(
                name: "ProjectFinancialResourceCategory");

            migrationBuilder.DropTable(
                name: "ProjectUser");

            migrationBuilder.DropTable(
                name: "ResourceScenarioUser");

            migrationBuilder.DropTable(
                name: "RiskProfile");

            migrationBuilder.DropTable(
                name: "StaffResourceProject");

            migrationBuilder.DropTable(
                name: "StaffResourceStaffResourceCategory");

            migrationBuilder.DropTable(
                name: "StaffTransaction");

            migrationBuilder.DropTable(
                name: "UserGroup");

            migrationBuilder.DropTable(
                name: "AlignmentCategory");

            migrationBuilder.DropTable(
                name: "FinancialTransaction");

            migrationBuilder.DropTable(
                name: "FinancialResourcePartition");

            migrationBuilder.DropTable(
                name: "ProjectConfig");

            migrationBuilder.DropTable(
                name: "BenefitCategory");

            migrationBuilder.DropTable(
                name: "ProjectBenefit");

            migrationBuilder.DropTable(
                name: "FinancialResourceCategory");

            migrationBuilder.DropTable(
                name: "RiskCategory");

            migrationBuilder.DropTable(
                name: "StaffResourceCategory");

            migrationBuilder.DropTable(
                name: "ProjectPhase");

            migrationBuilder.DropTable(
                name: "FinancialResource");

            migrationBuilder.DropTable(
                name: "Plan");

            migrationBuilder.DropTable(
                name: "ProjectOption");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropTable(
                name: "BusinessUnit");

            migrationBuilder.DropTable(
                name: "StaffResource");

            migrationBuilder.DropTable(
                name: "ResourceScenario");

            migrationBuilder.DropTable(
                name: "Group");

            migrationBuilder.DropTable(
                name: "Organisation");
        }
    }
}
