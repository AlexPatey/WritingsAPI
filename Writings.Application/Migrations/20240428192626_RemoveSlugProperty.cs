using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Writings.Application.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSlugProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Writings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Writings",
                type: "varchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
