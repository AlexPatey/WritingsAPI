using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Writings.Application.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Writings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Body = table.Column<string>(type: "varchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    YearOfCompletion = table.Column<int>(type: "int", nullable: true),
                    CreatedWhen = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastEdited = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Writings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WritingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TagName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    CreatedWhen = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tags_Writings_WritingId",
                        column: x => x.WritingId,
                        principalTable: "Writings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tags_WritingId",
                table: "Tags",
                column: "WritingId");

            migrationBuilder.Sql(
            @"CREATE TRIGGER [dbo].[Writings_UPDATE] ON [dbo].[Writings]
                AFTER UPDATE
                AS
                BEGIN
                    SET NOCOUNT ON;

                    IF ((SELECT TRIGGER_NESTLEVEL()) > 1) RETURN;

                    DECLARE @Id uniqueidentifier

                    SELECT @Id = INSERTED.Id
                    FROM INSERTED

                    UPDATE dbo.Writings
                    SET LastEdited = GETDATE()
                    WHERE Id = @Id
                END"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Writings");

            migrationBuilder.Sql(
            @"DROP TRIGGER IF EXISTS [dbo].[Writings_UPDATE]"
            );
        }
    }
}
