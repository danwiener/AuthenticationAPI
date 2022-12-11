using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Authentication.Migrations
{
    /// <inheritdoc />
    public partial class addedleaguerules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LeagueName",
                table: "Leagues",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "LeagueRules",
                columns: table => new
                {
                    LeagueRulesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeagueId = table.Column<int>(type: "int", nullable: false),
                    MaxTeams = table.Column<int>(type: "int", nullable: false),
                    QbCount = table.Column<int>(type: "int", nullable: false),
                    RbCount = table.Column<int>(type: "int", nullable: false),
                    WrCount = table.Column<int>(type: "int", nullable: false),
                    TeCount = table.Column<int>(type: "int", nullable: false),
                    DCount = table.Column<int>(type: "int", nullable: false),
                    KCount = table.Column<int>(type: "int", nullable: false),
                    PassingTDPoints = table.Column<int>(type: "int", nullable: false),
                    PPC = table.Column<double>(type: "float", nullable: false),
                    PPI = table.Column<double>(type: "float", nullable: false),
                    PPTwentyFiveYdsPass = table.Column<int>(type: "int", nullable: false),
                    FortyYardPassBonus = table.Column<int>(type: "int", nullable: false),
                    SixtyYardPassBonus = table.Column<int>(type: "int", nullable: false),
                    ThreeHundredYardPassBonus = table.Column<int>(type: "int", nullable: false),
                    FiveHundredYardPassBonus = table.Column<int>(type: "int", nullable: false),
                    RushingTDPoints = table.Column<int>(type: "int", nullable: false),
                    ReceivingTDPoints = table.Column<int>(type: "int", nullable: false),
                    PPTenRush = table.Column<int>(type: "int", nullable: false),
                    FortyYardRushReceivingBonus = table.Column<int>(type: "int", nullable: false),
                    SixtyYardRushReceivingBonus = table.Column<int>(type: "int", nullable: false),
                    OneHundredYardRushReceivingBonus = table.Column<int>(type: "int", nullable: false),
                    TwoHundredYardRushReceivingBonus = table.Column<int>(type: "int", nullable: false),
                    PPR = table.Column<double>(type: "float", nullable: false),
                    TwoPointConversion = table.Column<int>(type: "int", nullable: false),
                    InterceptionOffense = table.Column<int>(type: "int", nullable: false),
                    FumbleOffense = table.Column<int>(type: "int", nullable: false),
                    SafetyOffense = table.Column<int>(type: "int", nullable: false),
                    SackDefense = table.Column<int>(type: "int", nullable: false),
                    TackleDefense = table.Column<int>(type: "int", nullable: false),
                    FgPuntBlock = table.Column<int>(type: "int", nullable: false),
                    InterceptionDefense = table.Column<int>(type: "int", nullable: false),
                    FumbleDefense = table.Column<int>(type: "int", nullable: false),
                    SafetyDefense = table.Column<int>(type: "int", nullable: false),
                    IntTd = table.Column<int>(type: "int", nullable: false),
                    FumbleTd = table.Column<int>(type: "int", nullable: false),
                    ReturnTd = table.Column<int>(type: "int", nullable: false),
                    FgTenToTwenty = table.Column<int>(type: "int", nullable: false),
                    FgMissedTen = table.Column<int>(type: "int", nullable: false),
                    FgTwentyToThirty = table.Column<int>(type: "int", nullable: false),
                    FgMissedTwenty = table.Column<int>(type: "int", nullable: false),
                    FgThirtyToForty = table.Column<int>(type: "int", nullable: false),
                    FgMissedThirty = table.Column<int>(type: "int", nullable: false),
                    FgFortyToFifty = table.Column<int>(type: "int", nullable: false),
                    FgMissedforty = table.Column<int>(type: "int", nullable: false),
                    FgFiftyToSixty = table.Column<int>(type: "int", nullable: false),
                    FgMissedFifty = table.Column<int>(type: "int", nullable: false),
                    FgSixtyPlus = table.Column<int>(type: "int", nullable: false),
                    FgMissedSixty = table.Column<int>(type: "int", nullable: false),
                    XpMade = table.Column<int>(type: "int", nullable: false),
                    XpMissed = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeagueRules", x => x.LeagueRulesId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Leagues_LeagueName",
                table: "Leagues",
                column: "LeagueName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeagueRules");

            migrationBuilder.DropIndex(
                name: "IX_Leagues_LeagueName",
                table: "Leagues");

            migrationBuilder.AlterColumn<string>(
                name: "LeagueName",
                table: "Leagues",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
