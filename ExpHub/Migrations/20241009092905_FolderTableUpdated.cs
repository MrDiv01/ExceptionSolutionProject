using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpHub.Migrations
{
    /// <inheritdoc />
    public partial class FolderTableUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FolderDescription",
                table: "Folders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FolderDescription",
                table: "Folders");
        }
    }
}
