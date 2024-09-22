using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freem.Entities.Storage.PostgreSQL.Migrations.Instances
{
    /// <inheritdoc />
    public partial class RemoveEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "events",
                schema: "core_entities");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:core_entities.activity_status", "active,archived")
                .OldAnnotation("Npgsql:Enum:core_entities.activity_status", "active,archived")
                .OldAnnotation("Npgsql:Enum:core_entities.event_action", "created,updated,removed");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:core_entities.activity_status", "active,archived")
                .Annotation("Npgsql:Enum:core_entities.event_action", "created,updated,removed")
                .OldAnnotation("Npgsql:Enum:core_entities.activity_status", "active,archived");

            migrationBuilder.CreateTable(
                name: "events",
                schema: "core_entities",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    action = table.Column<int>(type: "core_entities.event_action", nullable: false),
                    event_type = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    activity_id = table.Column<string>(type: "text", nullable: true),
                    record_id = table.Column<string>(type: "text", nullable: true),
                    tag_id = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("events_pk", x => x.id);
                    table.CheckConstraint("events_event_type_check", "(event_type = 'activity' and activity_id is not null and record_id is null and tag_id is null) or (event_type = 'record' and activity_id is null and record_id is not null and tag_id is null) or(event_type = 'running_record' and activity_id is null and record_id is null and tag_id is null) or (event_type = 'tag' and activity_id is null and record_id is null and tag_id is not null)");
                    table.CheckConstraint("events_users_check", "user_id is not null");
                });
        }
    }
}
