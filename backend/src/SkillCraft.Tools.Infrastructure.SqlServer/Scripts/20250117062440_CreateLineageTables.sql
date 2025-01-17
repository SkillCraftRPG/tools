BEGIN TRANSACTION;
CREATE TABLE [Lineages] (
    [LineageId] int NOT NULL IDENTITY,
    [Id] uniqueidentifier NOT NULL,
    [ParentId] int NULL,
    [UniqueSlug] nvarchar(255) NOT NULL,
    [UniqueSlugNormalized] nvarchar(255) NOT NULL,
    [DisplayName] nvarchar(255) NULL,
    [Description] nvarchar(max) NULL,
    [Agility] int NOT NULL,
    [Coordination] int NOT NULL,
    [Intellect] int NOT NULL,
    [Presence] int NOT NULL,
    [Sensitivity] int NOT NULL,
    [Spirit] int NOT NULL,
    [Vigor] int NOT NULL,
    [ExtraAttributes] int NOT NULL,
    [Traits] nvarchar(max) NULL,
    [ExtraLanguages] int NOT NULL,
    [LanguagesText] nvarchar(max) NULL,
    [NamesText] nvarchar(max) NULL,
    [FamilyNames] nvarchar(max) NULL,
    [FemaleNames] nvarchar(max) NULL,
    [MaleNames] nvarchar(max) NULL,
    [UnisexNames] nvarchar(max) NULL,
    [CustomNames] nvarchar(max) NULL,
    [WalkSpeed] int NOT NULL,
    [ClimbSpeed] int NOT NULL,
    [SwimSpeed] int NOT NULL,
    [FlySpeed] int NOT NULL,
    [HoverSpeed] int NOT NULL,
    [BurrowSpeed] int NOT NULL,
    [SizeCategory] nvarchar(255) NOT NULL,
    [SizeRoll] nvarchar(255) NULL,
    [StarvedRoll] nvarchar(255) NULL,
    [SkinnyRoll] nvarchar(255) NULL,
    [NormalRoll] nvarchar(255) NULL,
    [OverweightRoll] nvarchar(255) NULL,
    [ObeseRoll] nvarchar(255) NULL,
    [AdolescentAge] int NULL,
    [AdultAge] int NULL,
    [MatureAge] int NULL,
    [VenerableAge] int NULL,
    [StreamId] nvarchar(255) NOT NULL,
    [Version] bigint NOT NULL,
    [CreatedBy] nvarchar(255) NULL,
    [CreatedOn] datetime2 NOT NULL,
    [UpdatedBy] nvarchar(255) NULL,
    [UpdatedOn] datetime2 NOT NULL,
    CONSTRAINT [PK_Lineages] PRIMARY KEY ([LineageId]),
    CONSTRAINT [FK_Lineages_Lineages_ParentId] FOREIGN KEY ([ParentId]) REFERENCES [Lineages] ([LineageId]) ON DELETE NO ACTION
);

CREATE TABLE [LineageLanguages] (
    [LineageId] int NOT NULL,
    [LanguageId] int NOT NULL,
    CONSTRAINT [PK_LineageLanguages] PRIMARY KEY ([LineageId], [LanguageId]),
    CONSTRAINT [FK_LineageLanguages_Languages_LanguageId] FOREIGN KEY ([LanguageId]) REFERENCES [Languages] ([LanguageId]) ON DELETE CASCADE,
    CONSTRAINT [FK_LineageLanguages_Lineages_LineageId] FOREIGN KEY ([LineageId]) REFERENCES [Lineages] ([LineageId]) ON DELETE CASCADE
);

CREATE INDEX [IX_LineageLanguages_LanguageId] ON [LineageLanguages] ([LanguageId]);

CREATE INDEX [IX_Lineages_CreatedBy] ON [Lineages] ([CreatedBy]);

CREATE INDEX [IX_Lineages_CreatedOn] ON [Lineages] ([CreatedOn]);

CREATE INDEX [IX_Lineages_DisplayName] ON [Lineages] ([DisplayName]);

CREATE UNIQUE INDEX [IX_Lineages_Id] ON [Lineages] ([Id]);

CREATE INDEX [IX_Lineages_ParentId] ON [Lineages] ([ParentId]);

CREATE UNIQUE INDEX [IX_Lineages_StreamId] ON [Lineages] ([StreamId]);

CREATE INDEX [IX_Lineages_UniqueSlug] ON [Lineages] ([UniqueSlug]);

CREATE UNIQUE INDEX [IX_Lineages_UniqueSlugNormalized] ON [Lineages] ([UniqueSlugNormalized]);

CREATE INDEX [IX_Lineages_UpdatedBy] ON [Lineages] ([UpdatedBy]);

CREATE INDEX [IX_Lineages_UpdatedOn] ON [Lineages] ([UpdatedOn]);

CREATE INDEX [IX_Lineages_Version] ON [Lineages] ([Version]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250117062440_CreateLineageTables', N'9.0.0');

COMMIT;
GO
