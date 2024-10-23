using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoginRegisterAPI.Migrations
{
    /// <inheritdoc />
    public partial class addEdNewHeader : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "AspNetUsers",
                newName: "Specialty");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Surename",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Surename",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Specialty",
                table: "AspNetUsers",
                newName: "FullName");
        }
    }
}
