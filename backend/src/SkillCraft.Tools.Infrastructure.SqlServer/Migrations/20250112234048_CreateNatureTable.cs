using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillCraft.Tools.Infrastructure.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class CreateNatureTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Talents_StreamId",
                table: "Talents");

            migrationBuilder.DropIndex(
                name: "IX_Customizations_StreamId",
                table: "Customizations");

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
                name: "IX_Talents_StreamId",
                table: "Talents",
                column: "StreamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customizations_StreamId",
                table: "Customizations",
                column: "StreamId",
                unique: true);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Natures");

            migrationBuilder.DropIndex(
                name: "IX_Talents_StreamId",
                table: "Talents");

            migrationBuilder.DropIndex(
                name: "IX_Customizations_StreamId",
                table: "Customizations");

            migrationBuilder.CreateIndex(
                name: "IX_Talents_StreamId",
                table: "Talents",
                column: "StreamId");

            migrationBuilder.CreateIndex(
                name: "IX_Customizations_StreamId",
                table: "Customizations",
                column: "StreamId");
        }
    }
}
