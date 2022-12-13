using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Authentication.Migrations
{
    /// <inheritdoc />
    public partial class combinedonetomanyrelationshipsforleagueteamsusersteams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "LeagueTeams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_LeagueTeams_UserId",
                table: "LeagueTeams",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_LeagueTeams_Users_UserId",
                table: "LeagueTeams",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeagueTeams_Users_UserId",
                table: "LeagueTeams");

            migrationBuilder.DropIndex(
                name: "IX_LeagueTeams_UserId",
                table: "LeagueTeams");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "LeagueTeams");
        }
    }
}
