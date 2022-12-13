using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Authentication.Migrations
{
    /// <inheritdoc />
    public partial class fixedteamtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeagueId",
                table: "Team");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Team");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LeagueId",
                table: "Team",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Team",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
