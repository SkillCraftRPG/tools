using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillCraft.Tools.Infrastructure.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class CreateCustomizationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                column: "StreamId");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customizations");
        }
    }
}
