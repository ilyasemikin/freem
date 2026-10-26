using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freem.Domain.Storage.PostgreSQL.Migrations.Instances
{
    /// <inheritdoc />
    public partial class ActivityNameUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "activities_name_unique",
                schema: "core_entities",
                table: "activities",
                columns: new[] { "name", "user_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "activities_name_unique",
                schema: "core_entities",
                table: "activities");
        }
    }
}
