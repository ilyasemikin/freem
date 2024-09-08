using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freem.Entities.Storage.PostgreSQL.Migrations.Instances
{
    /// <inheritdoc />
    public partial class EventsDbChecksMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "events_event_type_check",
                schema: "core_entities",
                table: "events",
                sql: "(event_type = 'category' and category_id is not null and record_id is null and tag_id is null) or(event_type = 'record' and category_id is null and record_id is not null and tag_id is null) or(event_type = 'running_record' and category_id is null and record_id is null and tag_id is null) or(event_type = 'tag' and category_id is null and record_id is null and tag_id is not null)");

            migrationBuilder.AddCheckConstraint(
                name: "events_users_check",
                schema: "core_entities",
                table: "events",
                sql: "user_id is not null");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "events_event_type_check",
                schema: "core_entities",
                table: "events");

            migrationBuilder.DropCheckConstraint(
                name: "events_users_check",
                schema: "core_entities",
                table: "events");
        }
    }
}
