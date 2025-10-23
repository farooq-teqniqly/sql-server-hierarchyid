# SQL Server HierarchyId Sample

This project demonstrates the use of SQL Server's HierarchyId data type for managing hierarchical data structures, specifically for organizing regions in a tree-like structure.

## Project Structure

- **RegionApi**: ASP.NET Core Web API for managing regions
- **RegionApiSql**: SQL Server Database Project containing the database schema

## Prerequisites

- .NET 9.0 SDK
- SQL Server 2022 or later
- Visual Studio 2022 or VS Code with SQL Server extension

## Building the SQL Server Database Project

### 1. Build the Database Project

Navigate to the project root and build the SQL database project:

```powershell
dotnet build RegionApiSql/RegionApiSql.sqlproj
```

This will create a `.dacpac` file in `RegionApiSql\bin\Debug\RegionApiSql.dacpac`.

### 2. Deploy to SQL Server

Use `sqlpackage` to deploy the database to your SQL Server instance:

```powershell
sqlpackage /Action:Publish /SourceFile:.\RegionApiSql\bin\debug\RegionApiSql.dacpac /TargetConnectionString:"YOUR SQL SERVER CONNECTION STRING"
```

### 3. Verify Deployment

Connect to your SQL Server instance and verify that the `regiondb` database was created with the following objects:

- **Tables**: `dbo.Regions`
- **Stored Procedures**: `dbo.AddRegion`
- **Indexes**: Various indexes on the Regions table

## Database Schema

The main table `Regions` uses SQL Server's `HIERARCHYID` data type to represent hierarchical relationships:

```sql
CREATE TABLE [dbo].[Regions]
(
  RegionId UNIQUEIDENTIFIER NOT NULL
    CONSTRAINT PK_Regions PRIMARY KEY NONCLUSTERED,
  Name NVARCHAR(50) NOT NULL,
  Node HIERARCHYID NOT NULL,
  ParentNode AS Node.GetAncestor(1) PERSISTED
);
```

## Running the API

After deploying the database, you can run the Web API:

```powershell
cd RegionApi
dotnet run
```

The API will be available at `https://localhost:5215` (or check the console output for the exact port).

## Troubleshooting

### SQL Server Version Mismatch

If you encounter an error about SQL Server version compatibility, ensure the `.sqlproj` file targets the correct SQL Server version:

- For SQL Server 2022: `Sql160DatabaseSchemaProvider`
- For SQL Server 2019: `Sql150DatabaseSchemaProvider`
- For SQL Server 2017: `Sql140DatabaseSchemaProvider`

Update the `DSP` property in `RegionApiSql.sqlproj` accordingly.