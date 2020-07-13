using Microsoft.EntityFrameworkCore.Migrations;

namespace ICS_team_4615.DAL.Migrations
{
    public partial class testdeletecascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Teams_TeamId",
                table: "Posts");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Teams_TeamId",
                table: "Posts",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Teams_TeamId",
                table: "Posts");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Teams_TeamId",
                table: "Posts",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
