using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freem.Entities.Storage.PostgreSQL.Migrations.Instances
{
    /// <inheritdoc />
    public partial class AddNewEventsM1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "events_entity_name_check",
                schema: "core_entities",
                table: "events");

            migrationBuilder.AddCheckConstraint(
                name: "events_entity_name_check",
                schema: "core_entities",
                table: "events",
                sql: "entity_name = 'activity' and (action = 'created' or action = 'updated' or action = 'removed' or action = 'archived' or action = 'unarchived') or entity_name = 'record' and (action = 'created' or action = 'updated' or action = 'removed') or entity_name = 'running_record' and (action = 'started' or action = 'stopped' or action = 'updated' or action = 'removed') or entity_name = 'tag' and (action = 'created' or action = 'updated' or action = 'removed') or entity_name = 'user' and (action = 'signed_in')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "events_entity_name_check",
                schema: "core_entities",
                table: "events");

            migrationBuilder.AddCheckConstraint(
                name: "events_entity_name_check",
                schema: "core_entities",
                table: "events",
                sql: "entity_name = 'activity' and (action = 'created' or action = 'updated' or action = 'removed') or entity_name = 'record' and (action = 'created' or action = 'updated' or action = 'removed') or entity_name = 'running_record' and (action = 'started' or action = 'stopped') or entity_name = 'tag' and (action = 'created' or action = 'updated' or action = 'removed') or entity_name = 'user' and (action = 'signed_in')");
        }
    }
}
