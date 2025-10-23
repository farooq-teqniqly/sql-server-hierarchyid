using Microsoft.EntityFrameworkCore.Migrations;
using RegionApi.Utilities;

#nullable disable

namespace RegionApi.Migrations
{
    /// <inheritdoc />
    public partial class AddAddRegionsStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = SqlResourceHelper.ReadEmbeddedSql("RegionApi.Sql.Add_Region.sql");
            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"IF OBJECT_ID(N'dbo.Region_Add', N'P') IS NOT NULL DROP PROCEDURE dbo.Region_Add;"
            );
        }
    }
}
