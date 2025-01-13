using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillCraft.Tools.Infrastructure.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class CreateCasteTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Castes");
        }
    }
}
