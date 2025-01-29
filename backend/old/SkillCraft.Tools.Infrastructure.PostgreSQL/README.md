﻿# SkillCraft.Tools.Infrastructure.PostgreSQL

Provides an implementation of a relational event store to be used with SkillCraft tool applications, Entity Framework Core and PostgreSQL.

## Migrations

This project is setup to use migrations. All the commands below must be executed in the solution directory.

### Create a migration

To create a new migration, execute the following command. Do not forget to provide a migration name!

```sh
dotnet ef migrations add <YOUR_MIGRATION_NAME> --context SkillCraftContext --project src/SkillCraft.Tools.Infrastructure.PostgreSQL --startup-project src/SkillCraft.Tools
```

### Remove a migration

To remove the latest unapplied migration, execute the following command.

```sh
dotnet ef migrations remove --context SkillCraftContext --project src/SkillCraft.Tools.Infrastructure.PostgreSQL --startup-project src/SkillCraft.Tools
```

### Generate a script

To generate a script, execute the following command. Do not forget to provide a source migration name!

```sh
dotnet ef migrations script <SOURCE_MIGRATION> --context SkillCraftContext --project src/SkillCraft.Tools.Infrastructure.PostgreSQL --startup-project src/SkillCraft.Tools
```