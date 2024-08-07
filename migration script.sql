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
GO

CREATE TABLE [Tags] (
    [Id] uniqueidentifier NOT NULL,
    [WritingId] uniqueidentifier NOT NULL,
    [TagName] varchar(255) NOT NULL,
    [CreatedWhen] datetimeoffset NOT NULL,
    CONSTRAINT [PK_Tags] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Tags_Writings_WritingId] FOREIGN KEY ([WritingId]) REFERENCES [Writings] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Tags_WritingId] ON [Tags] ([WritingId]);
GO

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
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240426145545_InitialSchema', N'8.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Writings] ADD [Deleted] bit NOT NULL DEFAULT CAST(0 AS bit);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240427190225_AddedShadowPropertyToWritingEntity', N'8.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DROP TRIGGER IF EXISTS [dbo].[Writings_UPDATE]
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240427211429_RemoveUpdateTrigger', N'8.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Writings] ADD [Slug] varchar(max) NOT NULL DEFAULT '';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240428185734_AddedSlugProperty', N'8.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Writings]') AND [c].[name] = N'Slug');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Writings] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Writings] DROP COLUMN [Slug];
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240428192626_RemoveSlugProperty', N'8.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Writings] ADD [ConcurrencyToken] rowversion NOT NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240502175556_AddRowVersionToWritingsTable', N'8.0.5');
GO

COMMIT;
GO


