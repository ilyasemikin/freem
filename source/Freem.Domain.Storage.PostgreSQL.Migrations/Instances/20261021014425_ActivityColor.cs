using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freem.Domain.Storage.PostgreSQL.Migrations.Instances
{
    /// <inheritdoc />
    public partial class ActivityColor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "color",
                schema: "core_entities",
                table: "activities",
                type: "character varying(7)",
                maxLength: 7,
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(@"
UPDATE core_entities.activities
SET color = (ARRAY[
    '#6366f1', '#10b981', '#f59e0b', '#ef4444', '#0ea5e9',
    '#8b5cf6', '#14b8a6', '#f97316', '#ec4899', '#84cc16'
])[abs(hashtext(id)::bigint) % 10 + 1];");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "color",
                schema: "core_entities",
                table: "activities");
        }
    }
}
