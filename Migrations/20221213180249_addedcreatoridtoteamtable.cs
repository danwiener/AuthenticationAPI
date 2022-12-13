using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Authentication.Migrations
{
    /// <inheritdoc />
    public partial class addedcreatoridtoteamtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeagueTeams_Team_TeamId",
                table: "LeagueTeams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Team",
                table: "Team");

            migrationBuilder.RenameTable(
                name: "Team",
                newName: "Teams");

            migrationBuilder.RenameIndex(
                name: "IX_Team_TeamName",
                table: "Teams",
                newName: "IX_Teams_TeamName");

            migrationBuilder.AddColumn<int>(
                name: "Creator",
                table: "Teams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Teams",
                table: "Teams",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_LeagueTeams_Teams_TeamId",
                table: "LeagueTeams",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeagueTeams_Teams_TeamId",
                table: "LeagueTeams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Teams",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "Teams");

            migrationBuilder.RenameTable(
                name: "Teams",
                newName: "Team");

            migrationBuilder.RenameIndex(
                name: "IX_Teams_TeamName",
                table: "Team",
                newName: "IX_Team_TeamName");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Team",
                table: "Team",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_LeagueTeams_Team_TeamId",
                table: "LeagueTeams",
                column: "TeamId",
                principalTable: "Team",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
