BEGIN TRANSACTION;
DROP TABLE [Actors];

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250115062815_Release_0_4_0', N'9.0.0');

COMMIT;
GO
