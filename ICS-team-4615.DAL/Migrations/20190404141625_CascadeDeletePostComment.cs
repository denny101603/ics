using Microsoft.EntityFrameworkCore.Migrations;

namespace ICS_team_4615.DAL.Migrations
{
    public partial class CascadeDeletePostComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Posts_ParentPostId",
                table: "Comments");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Posts_ParentPostId",
                table: "Comments",
                column: "ParentPostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Posts_ParentPostId",
                table: "Comments");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Posts_ParentPostId",
                table: "Comments",
                column: "ParentPostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
