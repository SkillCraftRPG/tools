using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillCraft.Tools.Infrastructure.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class Release_0_5_0 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    ReservedTalentDescriptions = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                        onDelete: ReferentialAction.Restrict);
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpecializationOptionalTalents");

            migrationBuilder.DropTable(
                name: "Specializations");
        }
    }
}
