using System;
using Freem.Domain.Storage.PostgreSQL.Migrations.Scripts.Extensions;
using Freem.Domain.Storage.PostgreSQL.Migrations.Scripts.Models;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freem.Domain.Storage.PostgreSQL.Migrations.Instances
{
    /// <inheritdoc />
    public partial class RecreateEvents : Migration
    {
        public const string EventsTriggersScriptFileName = "Freem.Domain.Storage.PostgreSQL.Migrations.Instances.Raw._20261020055037_RecreateEvents.events_triggers.psql";

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RunScript(EventsDbConstraintMigration.EventsTriggersScriptFileName, ScriptPart.Droppings);

            migrationBuilder.DropTable(
                name: "events",
                schema: "core_entities");

            migrationBuilder.CreateTable(
                name: "events",
                schema: "core_entities",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    occurred_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    event_type = table.Column<string>(type: "text", nullable: false),
                    entity_type = table.Column<string>(type: "text", nullable: false),
                    entity_ref = table.Column<string>(type: "text", nullable: false),
                    payload = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("events_pk", x => x.id);
                    table.CheckConstraint("events_entity_type_check", "entity_type = 'activity' or entity_type = 'record' or entity_type = 'running_record' or entity_type = 'tag' or entity_type = 'user'");
                });

            migrationBuilder.CreateIndex(
                name: "events_user_id_entity_type_entity_ref_idx",
                schema: "core_entities",
                table: "events",
                columns: new[] { "user_id", "entity_type", "entity_ref" });

            migrationBuilder.CreateIndex(
                name: "events_user_id_occurred_at_idx",
                schema: "core_entities",
                table: "events",
                columns: new[] { "user_id", "occurred_at" });

            migrationBuilder.RunScript(EventsTriggersScriptFileName, ScriptPart.Declarations);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RunScript(EventsTriggersScriptFileName, ScriptPart.Droppings);

            migrationBuilder.DropTable(
                name: "events",
                schema: "core_entities");

            migrationBuilder.CreateTable(
                name: "events",
                schema: "core_entities",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    entity_name = table.Column<string>(type: "text", nullable: false),
                    entity_id = table.Column<string>(type: "text", nullable: false),
                    action = table.Column<string>(type: "text", nullable: false),
                    additional_data = table.Column<string>(type: "jsonb", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("events_pk", x => x.id);
                    table.CheckConstraint("events_entity_name_check", "entity_name = 'activity' and (action = 'created' or action = 'updated' or action = 'removed' or action = 'archived' or action = 'unarchived') or entity_name = 'record' and (action = 'created' or action = 'updated' or action = 'removed') or entity_name = 'running_record' and (action = 'started' or action = 'stopped' or action = 'updated' or action = 'removed') or entity_name = 'tag' and (action = 'created' or action = 'updated' or action = 'removed') or entity_name = 'user' and (action = 'registered' or action = 'settings_changed')");
                });

            migrationBuilder.RunScript(EventsDbConstraintMigration.EventsTriggersScriptFileName, ScriptPart.Declarations);
        }
    }
}
