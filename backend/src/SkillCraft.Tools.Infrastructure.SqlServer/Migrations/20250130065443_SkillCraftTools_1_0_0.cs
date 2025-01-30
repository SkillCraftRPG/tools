using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillCraft.Tools.Infrastructure.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class SkillCraftTools_1_0_0 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Aspects",
                columns: table => new
                {
                    AspectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UniqueSlug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UniqueSlugNormalized = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MandatoryAttribute1 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MandatoryAttribute2 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    OptionalAttribute1 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    OptionalAttribute2 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DiscountedSkill1 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DiscountedSkill2 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    StreamId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aspects", x => x.AspectId);
                });

            migrationBuilder.CreateTable(
                name: "Castes",
                columns: table => new
                {
                    CasteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UniqueSlug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UniqueSlugNormalized = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Skill = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    WealthRoll = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    StreamId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Castes", x => x.CasteId);
                });

            migrationBuilder.CreateTable(
                name: "Customizations",
                columns: table => new
                {
                    CustomizationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UniqueSlug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UniqueSlugNormalized = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StreamId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customizations", x => x.CustomizationId);
                });

            migrationBuilder.CreateTable(
                name: "Educations",
                columns: table => new
                {
                    EducationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UniqueSlug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UniqueSlugNormalized = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Skill = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    WealthMultiplier = table.Column<double>(type: "float", nullable: true),
                    StreamId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Educations", x => x.EducationId);
                });

            migrationBuilder.CreateTable(
                name: "Features",
                columns: table => new
                {
                    FeatureId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UniqueSlug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UniqueSlugNormalized = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StreamId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Features", x => x.FeatureId);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    LanguageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UniqueSlug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UniqueSlugNormalized = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypicalSpeakers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StreamId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.LanguageId);
                });

            migrationBuilder.CreateTable(
                name: "Lineages",
                columns: table => new
                {
                    LineageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    UniqueSlug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UniqueSlugNormalized = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Agility = table.Column<int>(type: "int", nullable: false),
                    Coordination = table.Column<int>(type: "int", nullable: false),
                    Intellect = table.Column<int>(type: "int", nullable: false),
                    Presence = table.Column<int>(type: "int", nullable: false),
                    Sensitivity = table.Column<int>(type: "int", nullable: false),
                    Spirit = table.Column<int>(type: "int", nullable: false),
                    Vigor = table.Column<int>(type: "int", nullable: false),
                    ExtraAttributes = table.Column<int>(type: "int", nullable: false),
                    ExtraLanguages = table.Column<int>(type: "int", nullable: false),
                    LanguagesText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NamesText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FamilyNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FemaleNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaleNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnisexNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WalkSpeed = table.Column<int>(type: "int", nullable: false),
                    ClimbSpeed = table.Column<int>(type: "int", nullable: false),
                    SwimSpeed = table.Column<int>(type: "int", nullable: false),
                    FlySpeed = table.Column<int>(type: "int", nullable: false),
                    HoverSpeed = table.Column<int>(type: "int", nullable: false),
                    BurrowSpeed = table.Column<int>(type: "int", nullable: false),
                    SizeCategory = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SizeRoll = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    StarvedRoll = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SkinnyRoll = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    NormalRoll = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    OverweightRoll = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ObeseRoll = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AdolescentAge = table.Column<int>(type: "int", nullable: true),
                    AdultAge = table.Column<int>(type: "int", nullable: true),
                    MatureAge = table.Column<int>(type: "int", nullable: true),
                    VenerableAge = table.Column<int>(type: "int", nullable: true),
                    StreamId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lineages", x => x.LineageId);
                    table.ForeignKey(
                        name: "FK_Lineages_Lineages_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Lineages",
                        principalColumn: "LineageId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Scripts",
                columns: table => new
                {
                    ScriptId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UniqueSlug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UniqueSlugNormalized = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StreamId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scripts", x => x.ScriptId);
                });

            migrationBuilder.CreateTable(
                name: "Talents",
                columns: table => new
                {
                    TalentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Tier = table.Column<int>(type: "int", nullable: false),
                    UniqueSlug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UniqueSlugNormalized = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AllowMultiplePurchases = table.Column<bool>(type: "bit", nullable: false),
                    RequiredTalentId = table.Column<int>(type: "int", nullable: true),
                    Skill = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    StreamId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Talents", x => x.TalentId);
                    table.ForeignKey(
                        name: "FK_Talents_Talents_RequiredTalentId",
                        column: x => x.RequiredTalentId,
                        principalTable: "Talents",
                        principalColumn: "TalentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Traits",
                columns: table => new
                {
                    TraitId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UniqueSlug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UniqueSlugNormalized = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StreamId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Traits", x => x.TraitId);
                });

            migrationBuilder.CreateTable(
                name: "Natures",
                columns: table => new
                {
                    NatureId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UniqueSlug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UniqueSlugNormalized = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Attribute = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    GiftId = table.Column<int>(type: "int", nullable: true),
                    StreamId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Natures", x => x.NatureId);
                    table.ForeignKey(
                        name: "FK_Natures_Customizations_GiftId",
                        column: x => x.GiftId,
                        principalTable: "Customizations",
                        principalColumn: "CustomizationId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "CasteFeatures",
                columns: table => new
                {
                    CasteId = table.Column<int>(type: "int", nullable: false),
                    FeatureId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CasteFeatures", x => new { x.CasteId, x.FeatureId });
                    table.ForeignKey(
                        name: "FK_CasteFeatures_Castes_CasteId",
                        column: x => x.CasteId,
                        principalTable: "Castes",
                        principalColumn: "CasteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CasteFeatures_Features_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Features",
                        principalColumn: "FeatureId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LineageLanguages",
                columns: table => new
                {
                    LineageId = table.Column<int>(type: "int", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineageLanguages", x => new { x.LineageId, x.LanguageId });
                    table.ForeignKey(
                        name: "FK_LineageLanguages_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "LanguageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LineageLanguages_Lineages_LineageId",
                        column: x => x.LineageId,
                        principalTable: "Lineages",
                        principalColumn: "LineageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LanguageScripts",
                columns: table => new
                {
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    ScriptId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageScripts", x => new { x.LanguageId, x.ScriptId });
                    table.ForeignKey(
                        name: "FK_LanguageScripts_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "LanguageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LanguageScripts_Scripts_ScriptId",
                        column: x => x.ScriptId,
                        principalTable: "Scripts",
                        principalColumn: "ScriptId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Specializations",
                columns: table => new
                {
                    SpecializationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Tier = table.Column<int>(type: "int", nullable: false),
                    UniqueSlug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UniqueSlugNormalized = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequiredTalentId = table.Column<int>(type: "int", nullable: true),
                    OtherRequirements = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtherOptions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReservedTalentName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ReservedTalentDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StreamId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specializations", x => x.SpecializationId);
                    table.ForeignKey(
                        name: "FK_Specializations_Talents_RequiredTalentId",
                        column: x => x.RequiredTalentId,
                        principalTable: "Talents",
                        principalColumn: "TalentId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "LineageTraits",
                columns: table => new
                {
                    LineageId = table.Column<int>(type: "int", nullable: false),
                    TraitId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineageTraits", x => new { x.LineageId, x.TraitId });
                    table.ForeignKey(
                        name: "FK_LineageTraits_Lineages_LineageId",
                        column: x => x.LineageId,
                        principalTable: "Lineages",
                        principalColumn: "LineageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LineageTraits_Traits_TraitId",
                        column: x => x.TraitId,
                        principalTable: "Traits",
                        principalColumn: "TraitId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpecializationOptionalTalents",
                columns: table => new
                {
                    SpecializationId = table.Column<int>(type: "int", nullable: false),
                    TalentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecializationOptionalTalents", x => new { x.SpecializationId, x.TalentId });
                    table.ForeignKey(
                        name: "FK_SpecializationOptionalTalents_Specializations_SpecializationId",
                        column: x => x.SpecializationId,
                        principalTable: "Specializations",
                        principalColumn: "SpecializationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpecializationOptionalTalents_Talents_TalentId",
                        column: x => x.TalentId,
                        principalTable: "Talents",
                        principalColumn: "TalentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Aspects_CreatedBy",
                table: "Aspects",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Aspects_CreatedOn",
                table: "Aspects",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Aspects_DiscountedSkill1",
                table: "Aspects",
                column: "DiscountedSkill1");

            migrationBuilder.CreateIndex(
                name: "IX_Aspects_DiscountedSkill2",
                table: "Aspects",
                column: "DiscountedSkill2");

            migrationBuilder.CreateIndex(
                name: "IX_Aspects_DisplayName",
                table: "Aspects",
                column: "DisplayName");

            migrationBuilder.CreateIndex(
                name: "IX_Aspects_Id",
                table: "Aspects",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Aspects_MandatoryAttribute1",
                table: "Aspects",
                column: "MandatoryAttribute1");

            migrationBuilder.CreateIndex(
                name: "IX_Aspects_MandatoryAttribute2",
                table: "Aspects",
                column: "MandatoryAttribute2");

            migrationBuilder.CreateIndex(
                name: "IX_Aspects_OptionalAttribute1",
                table: "Aspects",
                column: "OptionalAttribute1");

            migrationBuilder.CreateIndex(
                name: "IX_Aspects_OptionalAttribute2",
                table: "Aspects",
                column: "OptionalAttribute2");

            migrationBuilder.CreateIndex(
                name: "IX_Aspects_StreamId",
                table: "Aspects",
                column: "StreamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Aspects_UniqueSlug",
                table: "Aspects",
                column: "UniqueSlug");

            migrationBuilder.CreateIndex(
                name: "IX_Aspects_UniqueSlugNormalized",
                table: "Aspects",
                column: "UniqueSlugNormalized",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Aspects_UpdatedBy",
                table: "Aspects",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Aspects_UpdatedOn",
                table: "Aspects",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Aspects_Version",
                table: "Aspects",
                column: "Version");

            migrationBuilder.CreateIndex(
                name: "IX_CasteFeatures_FeatureId",
                table: "CasteFeatures",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_Castes_CreatedBy",
                table: "Castes",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Castes_CreatedOn",
                table: "Castes",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Castes_DisplayName",
                table: "Castes",
                column: "DisplayName");

            migrationBuilder.CreateIndex(
                name: "IX_Castes_Id",
                table: "Castes",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Castes_Skill",
                table: "Castes",
                column: "Skill");

            migrationBuilder.CreateIndex(
                name: "IX_Castes_StreamId",
                table: "Castes",
                column: "StreamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Castes_UniqueSlug",
                table: "Castes",
                column: "UniqueSlug");

            migrationBuilder.CreateIndex(
                name: "IX_Castes_UniqueSlugNormalized",
                table: "Castes",
                column: "UniqueSlugNormalized",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Castes_UpdatedBy",
                table: "Castes",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Castes_UpdatedOn",
                table: "Castes",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Castes_Version",
                table: "Castes",
                column: "Version");

            migrationBuilder.CreateIndex(
                name: "IX_Customizations_CreatedBy",
                table: "Customizations",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Customizations_CreatedOn",
                table: "Customizations",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Customizations_DisplayName",
                table: "Customizations",
                column: "DisplayName");

            migrationBuilder.CreateIndex(
                name: "IX_Customizations_Id",
                table: "Customizations",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customizations_StreamId",
                table: "Customizations",
                column: "StreamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customizations_Type",
                table: "Customizations",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Customizations_UniqueSlug",
                table: "Customizations",
                column: "UniqueSlug");

            migrationBuilder.CreateIndex(
                name: "IX_Customizations_UniqueSlugNormalized",
                table: "Customizations",
                column: "UniqueSlugNormalized",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customizations_UpdatedBy",
                table: "Customizations",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Customizations_UpdatedOn",
                table: "Customizations",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Customizations_Version",
                table: "Customizations",
                column: "Version");

            migrationBuilder.CreateIndex(
                name: "IX_Educations_CreatedBy",
                table: "Educations",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Educations_CreatedOn",
                table: "Educations",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Educations_DisplayName",
                table: "Educations",
                column: "DisplayName");

            migrationBuilder.CreateIndex(
                name: "IX_Educations_Id",
                table: "Educations",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Educations_Skill",
                table: "Educations",
                column: "Skill");

            migrationBuilder.CreateIndex(
                name: "IX_Educations_StreamId",
                table: "Educations",
                column: "StreamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Educations_UniqueSlug",
                table: "Educations",
                column: "UniqueSlug");

            migrationBuilder.CreateIndex(
                name: "IX_Educations_UniqueSlugNormalized",
                table: "Educations",
                column: "UniqueSlugNormalized",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Educations_UpdatedBy",
                table: "Educations",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Educations_UpdatedOn",
                table: "Educations",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Educations_Version",
                table: "Educations",
                column: "Version");

            migrationBuilder.CreateIndex(
                name: "IX_Features_CreatedBy",
                table: "Features",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Features_CreatedOn",
                table: "Features",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Features_DisplayName",
                table: "Features",
                column: "DisplayName");

            migrationBuilder.CreateIndex(
                name: "IX_Features_Id",
                table: "Features",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Features_StreamId",
                table: "Features",
                column: "StreamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Features_UniqueSlug",
                table: "Features",
                column: "UniqueSlug");

            migrationBuilder.CreateIndex(
                name: "IX_Features_UniqueSlugNormalized",
                table: "Features",
                column: "UniqueSlugNormalized",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Features_UpdatedBy",
                table: "Features",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Features_UpdatedOn",
                table: "Features",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Features_Version",
                table: "Features",
                column: "Version");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_CreatedBy",
                table: "Languages",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_CreatedOn",
                table: "Languages",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_DisplayName",
                table: "Languages",
                column: "DisplayName");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_Id",
                table: "Languages",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Languages_StreamId",
                table: "Languages",
                column: "StreamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Languages_UniqueSlug",
                table: "Languages",
                column: "UniqueSlug");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_UniqueSlugNormalized",
                table: "Languages",
                column: "UniqueSlugNormalized",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Languages_UpdatedBy",
                table: "Languages",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_UpdatedOn",
                table: "Languages",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_Version",
                table: "Languages",
                column: "Version");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageScripts_ScriptId",
                table: "LanguageScripts",
                column: "ScriptId");

            migrationBuilder.CreateIndex(
                name: "IX_LineageLanguages_LanguageId",
                table: "LineageLanguages",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Lineages_CreatedBy",
                table: "Lineages",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Lineages_CreatedOn",
                table: "Lineages",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Lineages_DisplayName",
                table: "Lineages",
                column: "DisplayName");

            migrationBuilder.CreateIndex(
                name: "IX_Lineages_Id",
                table: "Lineages",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lineages_ParentId",
                table: "Lineages",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Lineages_StreamId",
                table: "Lineages",
                column: "StreamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lineages_UniqueSlug",
                table: "Lineages",
                column: "UniqueSlug");

            migrationBuilder.CreateIndex(
                name: "IX_Lineages_UniqueSlugNormalized",
                table: "Lineages",
                column: "UniqueSlugNormalized",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lineages_UpdatedBy",
                table: "Lineages",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Lineages_UpdatedOn",
                table: "Lineages",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Lineages_Version",
                table: "Lineages",
                column: "Version");

            migrationBuilder.CreateIndex(
                name: "IX_LineageTraits_TraitId",
                table: "LineageTraits",
                column: "TraitId");

            migrationBuilder.CreateIndex(
                name: "IX_Natures_Attribute",
                table: "Natures",
                column: "Attribute");

            migrationBuilder.CreateIndex(
                name: "IX_Natures_CreatedBy",
                table: "Natures",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Natures_CreatedOn",
                table: "Natures",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Natures_DisplayName",
                table: "Natures",
                column: "DisplayName");

            migrationBuilder.CreateIndex(
                name: "IX_Natures_GiftId",
                table: "Natures",
                column: "GiftId");

            migrationBuilder.CreateIndex(
                name: "IX_Natures_Id",
                table: "Natures",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Natures_StreamId",
                table: "Natures",
                column: "StreamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Natures_UniqueSlug",
                table: "Natures",
                column: "UniqueSlug");

            migrationBuilder.CreateIndex(
                name: "IX_Natures_UniqueSlugNormalized",
                table: "Natures",
                column: "UniqueSlugNormalized",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Natures_UpdatedBy",
                table: "Natures",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Natures_UpdatedOn",
                table: "Natures",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Natures_Version",
                table: "Natures",
                column: "Version");

            migrationBuilder.CreateIndex(
                name: "IX_Scripts_CreatedBy",
                table: "Scripts",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Scripts_CreatedOn",
                table: "Scripts",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Scripts_DisplayName",
                table: "Scripts",
                column: "DisplayName");

            migrationBuilder.CreateIndex(
                name: "IX_Scripts_Id",
                table: "Scripts",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Scripts_StreamId",
                table: "Scripts",
                column: "StreamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Scripts_UniqueSlug",
                table: "Scripts",
                column: "UniqueSlug");

            migrationBuilder.CreateIndex(
                name: "IX_Scripts_UniqueSlugNormalized",
                table: "Scripts",
                column: "UniqueSlugNormalized",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Scripts_UpdatedBy",
                table: "Scripts",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Scripts_UpdatedOn",
                table: "Scripts",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Scripts_Version",
                table: "Scripts",
                column: "Version");

            migrationBuilder.CreateIndex(
                name: "IX_SpecializationOptionalTalents_TalentId",
                table: "SpecializationOptionalTalents",
                column: "TalentId");

            migrationBuilder.CreateIndex(
                name: "IX_Specializations_CreatedBy",
                table: "Specializations",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Specializations_CreatedOn",
                table: "Specializations",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Specializations_DisplayName",
                table: "Specializations",
                column: "DisplayName");

            migrationBuilder.CreateIndex(
                name: "IX_Specializations_Id",
                table: "Specializations",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Specializations_RequiredTalentId",
                table: "Specializations",
                column: "RequiredTalentId");

            migrationBuilder.CreateIndex(
                name: "IX_Specializations_ReservedTalentName",
                table: "Specializations",
                column: "ReservedTalentName");

            migrationBuilder.CreateIndex(
                name: "IX_Specializations_StreamId",
                table: "Specializations",
                column: "StreamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Specializations_Tier",
                table: "Specializations",
                column: "Tier");

            migrationBuilder.CreateIndex(
                name: "IX_Specializations_UniqueSlug",
                table: "Specializations",
                column: "UniqueSlug");

            migrationBuilder.CreateIndex(
                name: "IX_Specializations_UniqueSlugNormalized",
                table: "Specializations",
                column: "UniqueSlugNormalized",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Specializations_UpdatedBy",
                table: "Specializations",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Specializations_UpdatedOn",
                table: "Specializations",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Specializations_Version",
                table: "Specializations",
                column: "Version");

            migrationBuilder.CreateIndex(
                name: "IX_Talents_AllowMultiplePurchases",
                table: "Talents",
                column: "AllowMultiplePurchases");

            migrationBuilder.CreateIndex(
                name: "IX_Talents_CreatedBy",
                table: "Talents",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Talents_CreatedOn",
                table: "Talents",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Talents_DisplayName",
                table: "Talents",
                column: "DisplayName");

            migrationBuilder.CreateIndex(
                name: "IX_Talents_Id",
                table: "Talents",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Talents_RequiredTalentId",
                table: "Talents",
                column: "RequiredTalentId");

            migrationBuilder.CreateIndex(
                name: "IX_Talents_Skill",
                table: "Talents",
                column: "Skill");

            migrationBuilder.CreateIndex(
                name: "IX_Talents_StreamId",
                table: "Talents",
                column: "StreamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Talents_Tier",
                table: "Talents",
                column: "Tier");

            migrationBuilder.CreateIndex(
                name: "IX_Talents_UniqueSlug",
                table: "Talents",
                column: "UniqueSlug");

            migrationBuilder.CreateIndex(
                name: "IX_Talents_UniqueSlugNormalized",
                table: "Talents",
                column: "UniqueSlugNormalized",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Talents_UpdatedBy",
                table: "Talents",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Talents_UpdatedOn",
                table: "Talents",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Talents_Version",
                table: "Talents",
                column: "Version");

            migrationBuilder.CreateIndex(
                name: "IX_Traits_CreatedBy",
                table: "Traits",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Traits_CreatedOn",
                table: "Traits",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Traits_DisplayName",
                table: "Traits",
                column: "DisplayName");

            migrationBuilder.CreateIndex(
                name: "IX_Traits_Id",
                table: "Traits",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Traits_StreamId",
                table: "Traits",
                column: "StreamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Traits_UniqueSlug",
                table: "Traits",
                column: "UniqueSlug");

            migrationBuilder.CreateIndex(
                name: "IX_Traits_UniqueSlugNormalized",
                table: "Traits",
                column: "UniqueSlugNormalized",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Traits_UpdatedBy",
                table: "Traits",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Traits_UpdatedOn",
                table: "Traits",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Traits_Version",
                table: "Traits",
                column: "Version");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Aspects");

            migrationBuilder.DropTable(
                name: "CasteFeatures");

            migrationBuilder.DropTable(
                name: "Educations");

            migrationBuilder.DropTable(
                name: "LanguageScripts");

            migrationBuilder.DropTable(
                name: "LineageLanguages");

            migrationBuilder.DropTable(
                name: "LineageTraits");

            migrationBuilder.DropTable(
                name: "Natures");

            migrationBuilder.DropTable(
                name: "SpecializationOptionalTalents");

            migrationBuilder.DropTable(
                name: "Castes");

            migrationBuilder.DropTable(
                name: "Features");

            migrationBuilder.DropTable(
                name: "Scripts");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "Lineages");

            migrationBuilder.DropTable(
                name: "Traits");

            migrationBuilder.DropTable(
                name: "Customizations");

            migrationBuilder.DropTable(
                name: "Specializations");

            migrationBuilder.DropTable(
                name: "Talents");
        }
    }
}
