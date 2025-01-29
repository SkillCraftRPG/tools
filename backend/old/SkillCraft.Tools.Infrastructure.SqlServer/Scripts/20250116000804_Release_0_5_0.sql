BEGIN TRANSACTION;
CREATE TABLE [Specializations] (
    [SpecializationId] int NOT NULL IDENTITY,
    [Id] uniqueidentifier NOT NULL,
    [Tier] int NOT NULL,
    [UniqueSlug] nvarchar(255) NOT NULL,
    [UniqueSlugNormalized] nvarchar(255) NOT NULL,
    [DisplayName] nvarchar(255) NULL,
    [Description] nvarchar(max) NULL,
    [RequiredTalentId] int NULL,
    [OtherRequirements] nvarchar(max) NULL,
    [OtherOptions] nvarchar(max) NULL,
    [ReservedTalentName] nvarchar(255) NULL,
    [ReservedTalentDescriptions] nvarchar(max) NULL,
    [StreamId] nvarchar(255) NOT NULL,
    [Version] bigint NOT NULL,
    [CreatedBy] nvarchar(255) NULL,
    [CreatedOn] datetime2 NOT NULL,
    [UpdatedBy] nvarchar(255) NULL,
    [UpdatedOn] datetime2 NOT NULL,
    CONSTRAINT [PK_Specializations] PRIMARY KEY ([SpecializationId]),
    CONSTRAINT [FK_Specializations_Talents_RequiredTalentId] FOREIGN KEY ([RequiredTalentId]) REFERENCES [Talents] ([TalentId]) ON DELETE NO ACTION
);

CREATE TABLE [SpecializationOptionalTalents] (
    [SpecializationId] int NOT NULL,
    [TalentId] int NOT NULL,
    CONSTRAINT [PK_SpecializationOptionalTalents] PRIMARY KEY ([SpecializationId], [TalentId]),
    CONSTRAINT [FK_SpecializationOptionalTalents_Specializations_SpecializationId] FOREIGN KEY ([SpecializationId]) REFERENCES [Specializations] ([SpecializationId]) ON DELETE CASCADE,
    CONSTRAINT [FK_SpecializationOptionalTalents_Talents_TalentId] FOREIGN KEY ([TalentId]) REFERENCES [Talents] ([TalentId]) ON DELETE CASCADE
);

CREATE INDEX [IX_SpecializationOptionalTalents_TalentId] ON [SpecializationOptionalTalents] ([TalentId]);

CREATE INDEX [IX_Specializations_CreatedBy] ON [Specializations] ([CreatedBy]);

CREATE INDEX [IX_Specializations_CreatedOn] ON [Specializations] ([CreatedOn]);

CREATE INDEX [IX_Specializations_DisplayName] ON [Specializations] ([DisplayName]);

CREATE UNIQUE INDEX [IX_Specializations_Id] ON [Specializations] ([Id]);

CREATE INDEX [IX_Specializations_RequiredTalentId] ON [Specializations] ([RequiredTalentId]);

CREATE INDEX [IX_Specializations_ReservedTalentName] ON [Specializations] ([ReservedTalentName]);

CREATE UNIQUE INDEX [IX_Specializations_StreamId] ON [Specializations] ([StreamId]);

CREATE INDEX [IX_Specializations_Tier] ON [Specializations] ([Tier]);

CREATE INDEX [IX_Specializations_UniqueSlug] ON [Specializations] ([UniqueSlug]);

CREATE UNIQUE INDEX [IX_Specializations_UniqueSlugNormalized] ON [Specializations] ([UniqueSlugNormalized]);

CREATE INDEX [IX_Specializations_UpdatedBy] ON [Specializations] ([UpdatedBy]);

CREATE INDEX [IX_Specializations_UpdatedOn] ON [Specializations] ([UpdatedOn]);

CREATE INDEX [IX_Specializations_Version] ON [Specializations] ([Version]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250116000804_Release_0_5_0', N'9.0.0');

COMMIT;
GO
