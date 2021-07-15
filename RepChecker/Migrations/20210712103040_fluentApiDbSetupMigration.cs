using Microsoft.EntityFrameworkCore.Migrations;

namespace RepChecker.Migrations
{
    public partial class fluentApiDbSetupMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserReputations_ApplicationUsers_BattleTag",
                table: "UserReputations");

            migrationBuilder.DropIndex(
                name: "IX_UserReputations_BattleTag",
                table: "UserReputations");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserBattleTag",
                table: "UserReputations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserReputations_ApplicationUserBattleTag",
                table: "UserReputations",
                column: "ApplicationUserBattleTag");

            migrationBuilder.AddForeignKey(
                name: "FK_UserReputations_ApplicationUsers_ApplicationUserBattleTag",
                table: "UserReputations",
                column: "ApplicationUserBattleTag",
                principalTable: "ApplicationUsers",
                principalColumn: "BattleTag",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserReputations_ApplicationUsers_ApplicationUserBattleTag",
                table: "UserReputations");

            migrationBuilder.DropIndex(
                name: "IX_UserReputations_ApplicationUserBattleTag",
                table: "UserReputations");

            migrationBuilder.DropColumn(
                name: "ApplicationUserBattleTag",
                table: "UserReputations");

            migrationBuilder.CreateIndex(
                name: "IX_UserReputations_BattleTag",
                table: "UserReputations",
                column: "BattleTag");

            migrationBuilder.AddForeignKey(
                name: "FK_UserReputations_ApplicationUsers_BattleTag",
                table: "UserReputations",
                column: "BattleTag",
                principalTable: "ApplicationUsers",
                principalColumn: "BattleTag",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
