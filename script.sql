IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240426145545_InitialSchema'
)
BEGIN
    CREATE TABLE [Writings] (
        [Id] uniqueidentifier NOT NULL,
        [Title] varchar(255) NOT NULL,
        [Body] varchar(max) NOT NULL,
        [Type] int NOT NULL,
        [YearOfCompletion] int NULL,
        [CreatedWhen] datetimeoffset NOT NULL,
        [LastEdited] datetimeoffset NULL,
        CONSTRAINT [PK_Writings] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240426145545_InitialSchema'
)
BEGIN
    CREATE TABLE [Tags] (
        [Id] uniqueidentifier NOT NULL,
        [WritingId] uniqueidentifier NOT NULL,
        [TagName] varchar(255) NOT NULL,
        [CreatedWhen] datetimeoffset NOT NULL,
        CONSTRAINT [PK_Tags] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Tags_Writings_WritingId] FOREIGN KEY ([WritingId]) REFERENCES [Writings] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240426145545_InitialSchema'
)
BEGIN
    CREATE INDEX [IX_Tags_WritingId] ON [Tags] ([WritingId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240426145545_InitialSchema'
)
BEGIN
    CREATE TRIGGER [dbo].[Writings_UPDATE] ON [dbo].[Writings]
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
                    END
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240426145545_InitialSchema'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240426145545_InitialSchema', N'8.0.4');
END;
GO

COMMIT;
GO

