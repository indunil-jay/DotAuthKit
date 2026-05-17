# 🗄️ EF Core Database Migrations Guide

This guide documents the Entity Framework Core migration commands for the **DotAuthKit** project. Since the solution uses Central Package Management (CPM) and follows Clean Architecture, these commands are tailored to run seamlessly from the **root directory**.

---

## 🛠️ Prerequisite
Ensure that you have installed the `dotnet-ef` global CLI tool on your development machine:
```bash
dotnet tool install --global dotnet-ef
```
To update the tool to the latest version at any time:
```bash
dotnet tool update --global dotnet-ef
```

---

## 🚀 Running Migrations (From the Root Directory)

Because your database configuration lives in the **Infrastructure** project, and your settings (`appsettings.json`) live in the **WebApi** project, you must specify their paths in the command.

### 1. Add a New Migration
Generates a new migration class and places it inside the **`src/Infrastructure/Persistence/Migrations`** folder:
```bash
dotnet ef migrations add <MigrationName> --project src/Infrastructure --startup-project src/WebApi --context ApplicationDbContext --output-dir Persistence/Migrations
```
*Example:*
```bash
dotnet ef migrations add AddUsersTable --project src/Infrastructure --startup-project src/WebApi --context ApplicationDbContext --output-dir Persistence/Migrations
```

### 2. Apply Migrations to the Database
Applies all pending migrations to the PostgreSQL database instance:
```bash
dotnet ef database update --project src/Infrastructure --startup-project src/WebApi --context ApplicationDbContext
```

### 3. Remove the Last Migration
Removes the last migration files that were added. *Note: You can only remove a migration if it has **not** been applied to the database yet, or if you roll it back first.*
```bash
dotnet ef migrations remove --project src/Infrastructure --startup-project src/WebApi --context ApplicationDbContext
```

---

## 🔄 Rollbacks & Database Reset

### 1. Rollback to a Specific Migration
To roll back the database schema to a specific migration, run `database update` and specify the name of the migration you want to revert to:
```bash
dotnet ef database update <TargetMigrationName> --project src/Infrastructure --startup-project src/WebApi --context ApplicationDbContext
```

### 2. Revert All Migrations (Empty Database)
To roll back all migrations, leaving the database completely empty:
```bash
dotnet ef database update 0 --project src/Infrastructure --startup-project src/WebApi --context ApplicationDbContext
```

### 3. Drop the Database
Deletes the entire database from PostgreSQL (use with caution!):
```bash
dotnet ef database drop --project src/Infrastructure --startup-project src/WebApi --context ApplicationDbContext --force
```

---

## 📝 Parameter Definitions Reference

| Argument | Shorthand | Target | Description |
| :--- | :--- | :--- | :--- |
| `--project` | `-p` | `src/Infrastructure` | The project containing the `DbContext` and migrations. |
| `--startup-project` | `-s` | `src/WebApi` | The project containing the settings and run configurations. |
| `--context` | `-c` | `ApplicationDbContext` | The specific `DbContext` class to run command against. |
| `--output-dir` | `-o` | `Persistence/Migrations` | Custom directory where migrations are created relative to the project. |
