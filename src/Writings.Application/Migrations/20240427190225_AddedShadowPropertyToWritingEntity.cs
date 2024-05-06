using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Writings.Application.Migrations
{
    /// <inheritdoc />
    public partial class AddedShadowPropertyToWritingEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Writings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Writings");
        }
    }
}
