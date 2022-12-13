using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Authentication.Migrations
{
    /// <inheritdoc />
    public partial class tweakedtablename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_League_Team_Leagues_LeagueId",
                table: "League_Team");

            migrationBuilder.DropForeignKey(
                name: "FK_League_Team_Team_TeamId",
                table: "League_Team");

            migrationBuilder.DropPrimaryKey(
                name: "PK_League_Team",
                table: "League_Team");

            migrationBuilder.RenameTable(
                name: "League_Team",
                newName: "LeagueTeams");

            migrationBuilder.RenameIndex(
                name: "IX_League_Team_TeamId",
                table: "LeagueTeams",
                newName: "IX_LeagueTeams_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_League_Team_LeagueId",
                table: "LeagueTeams",
                newName: "IX_LeagueTeams_LeagueId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LeagueTeams",
                table: "LeagueTeams",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LeagueTeams_Leagues_LeagueId",
                table: "LeagueTeams",
                column: "LeagueId",
                principalTable: "Leagues",
                principalColumn: "LeagueId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LeagueTeams_Team_TeamId",
                table: "LeagueTeams",
                column: "TeamId",
                principalTable: "Team",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeagueTeams_Leagues_LeagueId",
                table: "LeagueTeams");

            migrationBuilder.DropForeignKey(
                name: "FK_LeagueTeams_Team_TeamId",
                table: "LeagueTeams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LeagueTeams",
                table: "LeagueTeams");

            migrationBuilder.RenameTable(
                name: "LeagueTeams",
                newName: "League_Team");

            migrationBuilder.RenameIndex(
                name: "IX_LeagueTeams_TeamId",
                table: "League_Team",
                newName: "IX_League_Team_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_LeagueTeams_LeagueId",
                table: "League_Team",
                newName: "IX_League_Team_LeagueId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_League_Team",
                table: "League_Team",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_League_Team_Leagues_LeagueId",
                table: "League_Team",
                column: "LeagueId",
                principalTable: "Leagues",
                principalColumn: "LeagueId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_League_Team_Team_TeamId",
                table: "League_Team",
                column: "TeamId",
                principalTable: "Team",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
