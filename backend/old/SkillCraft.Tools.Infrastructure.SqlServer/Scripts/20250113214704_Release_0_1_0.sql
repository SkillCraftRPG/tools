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
CREATE TABLE [Actors] (
    [ActorId] int NOT NULL IDENTITY,
    [Id] uniqueidentifier NOT NULL,
    [Key] nvarchar(255) NOT NULL,
    [Type] nvarchar(255) NOT NULL,
    [IsDeleted] bit NOT NULL,
    [DisplayName] nvarchar(255) NOT NULL,
    [EmailAddress] nvarchar(255) NULL,
    [PictureUrl] nvarchar(2048) NULL,
    CONSTRAINT [PK_Actors] PRIMARY KEY ([ActorId])
);

CREATE TABLE [Aspects] (
    [AspectId] int NOT NULL IDENTITY,
    [Id] uniqueidentifier NOT NULL,
    [UniqueSlug] nvarchar(255) NOT NULL,
    [UniqueSlugNormalized] nvarchar(255) NOT NULL,
    [DisplayName] nvarchar(255) NULL,
    [Description] nvarchar(max) NULL,
    [MandatoryAttribute1] nvarchar(255) NULL,
    [MandatoryAttribute2] nvarchar(255) NULL,
    [OptionalAttribute1] nvarchar(255) NULL,
    [OptionalAttribute2] nvarchar(255) NULL,
    [DiscountedSkill1] nvarchar(255) NULL,
    [DiscountedSkill2] nvarchar(255) NULL,
    [StreamId] nvarchar(255) NOT NULL,
    [Version] bigint NOT NULL,
    [CreatedBy] nvarchar(255) NULL,
    [CreatedOn] datetime2 NOT NULL,
    [UpdatedBy] nvarchar(255) NULL,
    [UpdatedOn] datetime2 NOT NULL,
    CONSTRAINT [PK_Aspects] PRIMARY KEY ([AspectId])
);

CREATE TABLE [Castes] (
    [CasteId] int NOT NULL IDENTITY,
    [Id] uniqueidentifier NOT NULL,
    [UniqueSlug] nvarchar(255) NOT NULL,
    [UniqueSlugNormalized] nvarchar(255) NOT NULL,
    [DisplayName] nvarchar(255) NULL,
    [Description] nvarchar(max) NULL,
    [Skill] nvarchar(255) NULL,
    [WealthRoll] nvarchar(255) NULL,
    [Features] nvarchar(max) NULL,
    [StreamId] nvarchar(255) NOT NULL,
    [Version] bigint NOT NULL,
    [CreatedBy] nvarchar(255) NULL,
    [CreatedOn] datetime2 NOT NULL,
    [UpdatedBy] nvarchar(255) NULL,
    [UpdatedOn] datetime2 NOT NULL,
    CONSTRAINT [PK_Castes] PRIMARY KEY ([CasteId])
);

CREATE TABLE [Customizations] (
    [CustomizationId] int NOT NULL IDENTITY,
    [Id] uniqueidentifier NOT NULL,
    [Type] nvarchar(255) NOT NULL,
    [UniqueSlug] nvarchar(255) NOT NULL,
    [UniqueSlugNormalized] nvarchar(255) NOT NULL,
    [DisplayName] nvarchar(255) NULL,
    [Description] nvarchar(max) NULL,
    [StreamId] nvarchar(255) NOT NULL,
    [Version] bigint NOT NULL,
    [CreatedBy] nvarchar(255) NULL,
    [CreatedOn] datetime2 NOT NULL,
    [UpdatedBy] nvarchar(255) NULL,
    [UpdatedOn] datetime2 NOT NULL,
    CONSTRAINT [PK_Customizations] PRIMARY KEY ([CustomizationId])
);

CREATE TABLE [Educations] (
    [EducationId] int NOT NULL IDENTITY,
    [Id] uniqueidentifier NOT NULL,
    [UniqueSlug] nvarchar(255) NOT NULL,
    [UniqueSlugNormalized] nvarchar(255) NOT NULL,
    [DisplayName] nvarchar(255) NULL,
    [Description] nvarchar(max) NULL,
    [Skill] nvarchar(255) NULL,
    [WealthMultiplier] float NULL,
    [StreamId] nvarchar(255) NOT NULL,
    [Version] bigint NOT NULL,
    [CreatedBy] nvarchar(255) NULL,
    [CreatedOn] datetime2 NOT NULL,
    [UpdatedBy] nvarchar(255) NULL,
    [UpdatedOn] datetime2 NOT NULL,
    CONSTRAINT [PK_Educations] PRIMARY KEY ([EducationId])
);

CREATE TABLE [Languages] (
    [LanguageId] int NOT NULL IDENTITY,
    [Id] uniqueidentifier NOT NULL,
    [UniqueSlug] nvarchar(255) NOT NULL,
    [UniqueSlugNormalized] nvarchar(255) NOT NULL,
    [DisplayName] nvarchar(255) NULL,
    [Description] nvarchar(max) NULL,
    [Script] nvarchar(255) NULL,
    [TypicalSpeakers] nvarchar(max) NULL,
    [StreamId] nvarchar(255) NOT NULL,
    [Version] bigint NOT NULL,
    [CreatedBy] nvarchar(255) NULL,
    [CreatedOn] datetime2 NOT NULL,
    [UpdatedBy] nvarchar(255) NULL,
    [UpdatedOn] datetime2 NOT NULL,
    CONSTRAINT [PK_Languages] PRIMARY KEY ([LanguageId])
);

CREATE TABLE [Talents] (
    [TalentId] int NOT NULL IDENTITY,
    [Id] uniqueidentifier NOT NULL,
    [Tier] int NOT NULL,
    [UniqueSlug] nvarchar(255) NOT NULL,
    [UniqueSlugNormalized] nvarchar(255) NOT NULL,
    [DisplayName] nvarchar(255) NULL,
    [Description] nvarchar(max) NULL,
    [AllowMultiplePurchases] bit NOT NULL,
    [RequiredTalentId] int NULL,
    [Skill] nvarchar(255) NULL,
    [StreamId] nvarchar(255) NOT NULL,
    [Version] bigint NOT NULL,
    [CreatedBy] nvarchar(255) NULL,
    [CreatedOn] datetime2 NOT NULL,
    [UpdatedBy] nvarchar(255) NULL,
    [UpdatedOn] datetime2 NOT NULL,
    CONSTRAINT [PK_Talents] PRIMARY KEY ([TalentId]),
    CONSTRAINT [FK_Talents_Talents_RequiredTalentId] FOREIGN KEY ([RequiredTalentId]) REFERENCES [Talents] ([TalentId]) ON DELETE NO ACTION
);

CREATE TABLE [Natures] (
    [NatureId] int NOT NULL IDENTITY,
    [Id] uniqueidentifier NOT NULL,
    [UniqueSlug] nvarchar(255) NOT NULL,
    [UniqueSlugNormalized] nvarchar(255) NOT NULL,
    [DisplayName] nvarchar(255) NULL,
    [Description] nvarchar(max) NULL,
    [Attribute] nvarchar(255) NULL,
    [GiftId] int NULL,
    [StreamId] nvarchar(255) NOT NULL,
    [Version] bigint NOT NULL,
    [CreatedBy] nvarchar(255) NULL,
    [CreatedOn] datetime2 NOT NULL,
    [UpdatedBy] nvarchar(255) NULL,
    [UpdatedOn] datetime2 NOT NULL,
    CONSTRAINT [PK_Natures] PRIMARY KEY ([NatureId]),
    CONSTRAINT [FK_Natures_Customizations_GiftId] FOREIGN KEY ([GiftId]) REFERENCES [Customizations] ([CustomizationId]) ON DELETE NO ACTION
);

CREATE INDEX [IX_Actors_DisplayName] ON [Actors] ([DisplayName]);

CREATE INDEX [IX_Actors_EmailAddress] ON [Actors] ([EmailAddress]);

CREATE UNIQUE INDEX [IX_Actors_Id] ON [Actors] ([Id]);

CREATE INDEX [IX_Actors_IsDeleted] ON [Actors] ([IsDeleted]);

CREATE UNIQUE INDEX [IX_Actors_Key] ON [Actors] ([Key]);

CREATE INDEX [IX_Actors_Type] ON [Actors] ([Type]);

CREATE INDEX [IX_Aspects_CreatedBy] ON [Aspects] ([CreatedBy]);

CREATE INDEX [IX_Aspects_CreatedOn] ON [Aspects] ([CreatedOn]);

CREATE INDEX [IX_Aspects_DiscountedSkill1] ON [Aspects] ([DiscountedSkill1]);

CREATE INDEX [IX_Aspects_DiscountedSkill2] ON [Aspects] ([DiscountedSkill2]);

CREATE INDEX [IX_Aspects_DisplayName] ON [Aspects] ([DisplayName]);

CREATE UNIQUE INDEX [IX_Aspects_Id] ON [Aspects] ([Id]);

CREATE INDEX [IX_Aspects_MandatoryAttribute1] ON [Aspects] ([MandatoryAttribute1]);

CREATE INDEX [IX_Aspects_MandatoryAttribute2] ON [Aspects] ([MandatoryAttribute2]);

CREATE INDEX [IX_Aspects_OptionalAttribute1] ON [Aspects] ([OptionalAttribute1]);

CREATE INDEX [IX_Aspects_OptionalAttribute2] ON [Aspects] ([OptionalAttribute2]);

CREATE UNIQUE INDEX [IX_Aspects_StreamId] ON [Aspects] ([StreamId]);

CREATE INDEX [IX_Aspects_UniqueSlug] ON [Aspects] ([UniqueSlug]);

CREATE UNIQUE INDEX [IX_Aspects_UniqueSlugNormalized] ON [Aspects] ([UniqueSlugNormalized]);

CREATE INDEX [IX_Aspects_UpdatedBy] ON [Aspects] ([UpdatedBy]);

CREATE INDEX [IX_Aspects_UpdatedOn] ON [Aspects] ([UpdatedOn]);

CREATE INDEX [IX_Aspects_Version] ON [Aspects] ([Version]);

CREATE INDEX [IX_Castes_CreatedBy] ON [Castes] ([CreatedBy]);

CREATE INDEX [IX_Castes_CreatedOn] ON [Castes] ([CreatedOn]);

CREATE INDEX [IX_Castes_DisplayName] ON [Castes] ([DisplayName]);

CREATE UNIQUE INDEX [IX_Castes_Id] ON [Castes] ([Id]);

CREATE INDEX [IX_Castes_Skill] ON [Castes] ([Skill]);

CREATE UNIQUE INDEX [IX_Castes_StreamId] ON [Castes] ([StreamId]);

CREATE INDEX [IX_Castes_UniqueSlug] ON [Castes] ([UniqueSlug]);

CREATE UNIQUE INDEX [IX_Castes_UniqueSlugNormalized] ON [Castes] ([UniqueSlugNormalized]);

CREATE INDEX [IX_Castes_UpdatedBy] ON [Castes] ([UpdatedBy]);

CREATE INDEX [IX_Castes_UpdatedOn] ON [Castes] ([UpdatedOn]);

CREATE INDEX [IX_Castes_Version] ON [Castes] ([Version]);

CREATE INDEX [IX_Customizations_CreatedBy] ON [Customizations] ([CreatedBy]);

CREATE INDEX [IX_Customizations_CreatedOn] ON [Customizations] ([CreatedOn]);

CREATE INDEX [IX_Customizations_DisplayName] ON [Customizations] ([DisplayName]);

CREATE UNIQUE INDEX [IX_Customizations_Id] ON [Customizations] ([Id]);

CREATE UNIQUE INDEX [IX_Customizations_StreamId] ON [Customizations] ([StreamId]);

CREATE INDEX [IX_Customizations_Type] ON [Customizations] ([Type]);

CREATE INDEX [IX_Customizations_UniqueSlug] ON [Customizations] ([UniqueSlug]);

CREATE UNIQUE INDEX [IX_Customizations_UniqueSlugNormalized] ON [Customizations] ([UniqueSlugNormalized]);

CREATE INDEX [IX_Customizations_UpdatedBy] ON [Customizations] ([UpdatedBy]);

CREATE INDEX [IX_Customizations_UpdatedOn] ON [Customizations] ([UpdatedOn]);

CREATE INDEX [IX_Customizations_Version] ON [Customizations] ([Version]);

CREATE INDEX [IX_Educations_CreatedBy] ON [Educations] ([CreatedBy]);

CREATE INDEX [IX_Educations_CreatedOn] ON [Educations] ([CreatedOn]);

CREATE INDEX [IX_Educations_DisplayName] ON [Educations] ([DisplayName]);

CREATE UNIQUE INDEX [IX_Educations_Id] ON [Educations] ([Id]);

CREATE INDEX [IX_Educations_Skill] ON [Educations] ([Skill]);

CREATE UNIQUE INDEX [IX_Educations_StreamId] ON [Educations] ([StreamId]);

CREATE INDEX [IX_Educations_UniqueSlug] ON [Educations] ([UniqueSlug]);

CREATE UNIQUE INDEX [IX_Educations_UniqueSlugNormalized] ON [Educations] ([UniqueSlugNormalized]);

CREATE INDEX [IX_Educations_UpdatedBy] ON [Educations] ([UpdatedBy]);

CREATE INDEX [IX_Educations_UpdatedOn] ON [Educations] ([UpdatedOn]);

CREATE INDEX [IX_Educations_Version] ON [Educations] ([Version]);

CREATE INDEX [IX_Languages_CreatedBy] ON [Languages] ([CreatedBy]);

CREATE INDEX [IX_Languages_CreatedOn] ON [Languages] ([CreatedOn]);

CREATE INDEX [IX_Languages_DisplayName] ON [Languages] ([DisplayName]);

CREATE UNIQUE INDEX [IX_Languages_Id] ON [Languages] ([Id]);

CREATE INDEX [IX_Languages_Script] ON [Languages] ([Script]);

CREATE UNIQUE INDEX [IX_Languages_StreamId] ON [Languages] ([StreamId]);

CREATE INDEX [IX_Languages_UniqueSlug] ON [Languages] ([UniqueSlug]);

CREATE UNIQUE INDEX [IX_Languages_UniqueSlugNormalized] ON [Languages] ([UniqueSlugNormalized]);

CREATE INDEX [IX_Languages_UpdatedBy] ON [Languages] ([UpdatedBy]);

CREATE INDEX [IX_Languages_UpdatedOn] ON [Languages] ([UpdatedOn]);

CREATE INDEX [IX_Languages_Version] ON [Languages] ([Version]);

CREATE INDEX [IX_Natures_Attribute] ON [Natures] ([Attribute]);

CREATE INDEX [IX_Natures_CreatedBy] ON [Natures] ([CreatedBy]);

CREATE INDEX [IX_Natures_CreatedOn] ON [Natures] ([CreatedOn]);

CREATE INDEX [IX_Natures_DisplayName] ON [Natures] ([DisplayName]);

CREATE INDEX [IX_Natures_GiftId] ON [Natures] ([GiftId]);

CREATE UNIQUE INDEX [IX_Natures_Id] ON [Natures] ([Id]);

CREATE UNIQUE INDEX [IX_Natures_StreamId] ON [Natures] ([StreamId]);

CREATE INDEX [IX_Natures_UniqueSlug] ON [Natures] ([UniqueSlug]);

CREATE UNIQUE INDEX [IX_Natures_UniqueSlugNormalized] ON [Natures] ([UniqueSlugNormalized]);

CREATE INDEX [IX_Natures_UpdatedBy] ON [Natures] ([UpdatedBy]);

CREATE INDEX [IX_Natures_UpdatedOn] ON [Natures] ([UpdatedOn]);

CREATE INDEX [IX_Natures_Version] ON [Natures] ([Version]);

CREATE INDEX [IX_Talents_AllowMultiplePurchases] ON [Talents] ([AllowMultiplePurchases]);

CREATE INDEX [IX_Talents_CreatedBy] ON [Talents] ([CreatedBy]);

CREATE INDEX [IX_Talents_CreatedOn] ON [Talents] ([CreatedOn]);

CREATE INDEX [IX_Talents_DisplayName] ON [Talents] ([DisplayName]);

CREATE UNIQUE INDEX [IX_Talents_Id] ON [Talents] ([Id]);

CREATE INDEX [IX_Talents_RequiredTalentId] ON [Talents] ([RequiredTalentId]);

CREATE INDEX [IX_Talents_Skill] ON [Talents] ([Skill]);

CREATE UNIQUE INDEX [IX_Talents_StreamId] ON [Talents] ([StreamId]);

CREATE INDEX [IX_Talents_Tier] ON [Talents] ([Tier]);

CREATE INDEX [IX_Talents_UniqueSlug] ON [Talents] ([UniqueSlug]);

CREATE UNIQUE INDEX [IX_Talents_UniqueSlugNormalized] ON [Talents] ([UniqueSlugNormalized]);

CREATE INDEX [IX_Talents_UpdatedBy] ON [Talents] ([UpdatedBy]);

CREATE INDEX [IX_Talents_UpdatedOn] ON [Talents] ([UpdatedOn]);

CREATE INDEX [IX_Talents_Version] ON [Talents] ([Version]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250113214704_Release_0_1_0', N'9.0.0');

COMMIT;
GO
