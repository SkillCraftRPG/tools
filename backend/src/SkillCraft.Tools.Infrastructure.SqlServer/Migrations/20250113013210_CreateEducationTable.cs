using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillCraft.Tools.Infrastructure.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class CreateEducationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Educations");
        }
    }
}
