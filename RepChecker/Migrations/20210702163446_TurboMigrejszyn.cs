using Microsoft.EntityFrameworkCore.Migrations;

namespace RepChecker.Migrations
{
    public partial class TurboMigrejszyn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationUsers",
                columns: table => new
                {
                    BattleTag = table.Column<string>(type: "TEXT", nullable: false),
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUsers", x => x.BattleTag);
                });

            migrationBuilder.CreateTable(
                name: "UserReputations",
                columns: table => new
                {
                    ReputationId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReputationName = table.Column<string>(type: "TEXT", nullable: true),
                    Character = table.Column<string>(type: "TEXT", nullable: true),
                    Realm = table.Column<string>(type: "TEXT", nullable: true),
                    FactionHref = table.Column<string>(type: "TEXT", nullable: true),
                    BattleTag = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserReputations", x => x.ReputationId);
                    table.ForeignKey(
                        name: "FK_UserReputations_ApplicationUsers_BattleTag",
                        column: x => x.BattleTag,
                        principalTable: "ApplicationUsers",
                        principalColumn: "BattleTag",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Standings",
                columns: table => new
                {
                    StandingId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Raw = table.Column<int>(type: "INTEGER", nullable: false),
                    Max = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrentValue = table.Column<int>(type: "INTEGER", nullable: false),
                    Tier = table.Column<int>(type: "INTEGER", nullable: false),
                    Level = table.Column<string>(type: "TEXT", nullable: true),
                    ReputationId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Standings", x => x.StandingId);
                    table.ForeignKey(
                        name: "FK_Standings_UserReputations_ReputationId",
                        column: x => x.ReputationId,
                        principalTable: "UserReputations",
                        principalColumn: "ReputationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Standings_ReputationId",
                table: "Standings",
                column: "ReputationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserReputations_BattleTag",
                table: "UserReputations",
                column: "BattleTag");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Standings");

            migrationBuilder.DropTable(
                name: "UserReputations");

            migrationBuilder.DropTable(
                name: "ApplicationUsers");
        }
    }
}
