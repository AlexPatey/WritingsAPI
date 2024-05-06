using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Writings.Application.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUpdateTrigger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
            @"DROP TRIGGER IF EXISTS [dbo].[Writings_UPDATE]"
            );
        
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
