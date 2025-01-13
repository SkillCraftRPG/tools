using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillCraft.Tools.Infrastructure.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class CreateAspectTable : Migration
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Aspects");
        }
    }
}
