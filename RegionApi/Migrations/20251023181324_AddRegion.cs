using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegionApi.Migrations
{
    /// <inheritdoc />
    public partial class AddRegion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"IF OBJECT_ID('dbo.Regions', 'U') IS NULL
                  BEGIN
                    CREATE TABLE dbo.Regions
                    (
                        RegionId UNIQUEIDENTIFIER NOT NULL
                            CONSTRAINT PK_Regions PRIMARY KEY NONCLUSTERED,
                        Name NVARCHAR(50) NOT NULL,
                        Node HIERARCHYID  NOT NULL
                    );
                   END"
            );

            migrationBuilder.Sql(
                @"IF COL_LENGTH('dbo.Regions', 'ParentNode') IS NULL
                  BEGIN
                    ALTER TABLE dbo.Regions
                    ADD ParentNode AS Node.GetAncestor(1) PERSISTED;
                  END"
            );

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UX_Regions_ParentNode_Name' AND object_id = OBJECT_ID('dbo.Regions')) 
                  BEGIN
                    CREATE UNIQUE INDEX UX_Regions_ParentNode_Name ON dbo.Regions(ParentNode, Name);
                  END"
            );

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'CX_Regions_Node' AND object_id = OBJECT_ID('dbo.Regions')) 
                  BEGIN
                    CREATE UNIQUE CLUSTERED INDEX CX_Regions_Node ON dbo.Regions(Node);
                  END"
            );

            migrationBuilder.Sql(
                @"IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Regions_Name' AND object_id = OBJECT_ID('dbo.Regions'))
                  BEGIN
                    CREATE NONCLUSTERED INDEX IX_Regions_Name ON dbo.Regions(Name);
                  END"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"IF OBJECT_ID('dbo.Regions', 'U') IS NOT NULL
                  BEGIN
                    DROP TABLE dbo.Regions;
                  END"
            );
        }
    }
}
