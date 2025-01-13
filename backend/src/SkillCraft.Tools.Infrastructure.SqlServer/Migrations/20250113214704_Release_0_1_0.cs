using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillCraft.Tools.Infrastructure.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class Release_0_1_0 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Actors",
                columns: table => new
                {
                    ActorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PictureUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actors", x => x.ActorId);
                });

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
                    Features = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    Script = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
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
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Actors_DisplayName",
                table: "Actors",
                column: "DisplayName");

            migrationBuilder.CreateIndex(
                name: "IX_Actors_EmailAddress",
                table: "Actors",
                column: "EmailAddress");

            migrationBuilder.CreateIndex(
                name: "IX_Actors_Id",
                table: "Actors",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Actors_IsDeleted",
                table: "Actors",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Actors_Key",
                table: "Actors",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Actors_Type",
                table: "Actors",
                column: "Type");

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
                name: "IX_Languages_Script",
                table: "Languages",
                column: "Script");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Actors");

            migrationBuilder.DropTable(
                name: "Aspects");

            migrationBuilder.DropTable(
                name: "Castes");

            migrationBuilder.DropTable(
                name: "Educations");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "Natures");

            migrationBuilder.DropTable(
                name: "Talents");

            migrationBuilder.DropTable(
                name: "Customizations");
        }
    }
}
