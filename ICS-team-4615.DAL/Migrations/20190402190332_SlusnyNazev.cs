using Microsoft.EntityFrameworkCore.Migrations;

namespace ICS_team_4615.DAL.Migrations
{
    public partial class SlusnyNazev : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Posts_ParentPostId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Teams_TeamId",
                table: "Posts");

            migrationBuilder.DropTable(
                name: "UsersInTeams");

            migrationBuilder.CreateTable(
                name: "UserTeams",
                columns: table => new
                {
                    userId = table.Column<int>(nullable: false),
                    teamId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTeams", x => new { x.userId, x.teamId });
                    table.ForeignKey(
                        name: "FK_UserTeams_Teams_teamId",
                        column: x => x.teamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTeams_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTeams_teamId",
                table: "UserTeams",
                column: "teamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Posts_ParentPostId",
                table: "Comments",
                column: "ParentPostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Teams_TeamId",
                table: "Posts",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Posts_ParentPostId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Teams_TeamId",
                table: "Posts");

            migrationBuilder.DropTable(
                name: "UserTeams");

            migrationBuilder.CreateTable(
                name: "UsersInTeams",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    TeamId = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersInTeams", x => new { x.UserId, x.TeamId });
                    table.ForeignKey(
                        name: "FK_UsersInTeams_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersInTeams_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersInTeams_TeamId",
                table: "UsersInTeams",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Posts_ParentPostId",
                table: "Comments",
                column: "ParentPostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Teams_TeamId",
                table: "Posts",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
