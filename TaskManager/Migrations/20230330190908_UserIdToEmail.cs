using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.Migrations
{
    /// <inheritdoc />
    public partial class UserIdToEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workspaces_Users_Users_UserId",
                table: "Workspaces_Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Workspaces_Users",
                table: "Workspaces_Users");

            migrationBuilder.DropIndex(
                name: "IX_Workspaces_Users_UserId",
                table: "Workspaces_Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Workspaces_Users");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "EmailAddress",
                table: "Workspaces_Users",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "EmailAddress",
                table: "Users",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Workspaces_Users",
                table: "Workspaces_Users",
                columns: new[] { "WorkspaceId", "EmailAddress" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "EmailAddress");

            migrationBuilder.CreateIndex(
                name: "IX_Workspaces_Users_EmailAddress",
                table: "Workspaces_Users",
                column: "EmailAddress");

            migrationBuilder.AddForeignKey(
                name: "FK_Workspaces_Users_Users_EmailAddress",
                table: "Workspaces_Users",
                column: "EmailAddress",
                principalTable: "Users",
                principalColumn: "EmailAddress",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workspaces_Users_Users_EmailAddress",
                table: "Workspaces_Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Workspaces_Users",
                table: "Workspaces_Users");

            migrationBuilder.DropIndex(
                name: "IX_Workspaces_Users_EmailAddress",
                table: "Workspaces_Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EmailAddress",
                table: "Workspaces_Users");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Workspaces_Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "EmailAddress",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Workspaces_Users",
                table: "Workspaces_Users",
                columns: new[] { "WorkspaceId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Workspaces_Users_UserId",
                table: "Workspaces_Users",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Workspaces_Users_Users_UserId",
                table: "Workspaces_Users",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
