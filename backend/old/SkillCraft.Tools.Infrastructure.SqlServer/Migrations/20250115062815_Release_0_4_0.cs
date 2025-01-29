using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillCraft.Tools.Infrastructure.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class Release_0_4_0 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Actors");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Actors",
                columns: table => new
                {
                    ActorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DisplayName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PictureUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actors", x => x.ActorId);
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
        }
    }
}
