using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.Migrations
{
    /// <inheritdoc />
    public partial class AuthorId_To_User_and_Task_To_TaskId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Tasks_TaskId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_TaskId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Tasks");

            migrationBuilder.AddColumn<string>(
                name: "AuthorEmailAddress",
                table: "Tasks",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AuthorEmailAddress",
                table: "Tasks",
                column: "AuthorEmailAddress");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Users_AuthorEmailAddress",
                table: "Tasks",
                column: "AuthorEmailAddress",
                principalTable: "Users",
                principalColumn: "EmailAddress");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Users_AuthorEmailAddress",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_AuthorEmailAddress",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "AuthorEmailAddress",
                table: "Tasks");

            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TaskId",
                table: "Users",
                column: "TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Tasks_TaskId",
                table: "Users",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id");
        }
    }
}
