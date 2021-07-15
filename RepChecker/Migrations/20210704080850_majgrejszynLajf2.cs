using Microsoft.EntityFrameworkCore.Migrations;

namespace RepChecker.Migrations
{
    public partial class majgrejszynLajf2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserReputations_ApplicationUsers_BattleTag",
                table: "UserReputations");

            migrationBuilder.AddForeignKey(
                name: "FK_UserReputations_ApplicationUsers_BattleTag",
                table: "UserReputations",
                column: "BattleTag",
                principalTable: "ApplicationUsers",
                principalColumn: "BattleTag",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserReputations_ApplicationUsers_BattleTag",
                table: "UserReputations");

            migrationBuilder.AddForeignKey(
                name: "FK_UserReputations_ApplicationUsers_BattleTag",
                table: "UserReputations",
                column: "BattleTag",
                principalTable: "ApplicationUsers",
                principalColumn: "BattleTag",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
