using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freem.Entities.Storage.PostgreSQL.Migrations.Instances
{
    /// <inheritdoc />
    public partial class EventsMvp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "core_entities",
                table: "users");

            migrationBuilder.DropColumn(
                name: "updated_at",
                schema: "core_entities",
                table: "users");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "core_entities",
                table: "tags");

            migrationBuilder.DropColumn(
                name: "updated_at",
                schema: "core_entities",
                table: "tags");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "core_entities",
                table: "running_records");

            migrationBuilder.DropColumn(
                name: "updated_at",
                schema: "core_entities",
                table: "running_records");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "core_entities",
                table: "records");

            migrationBuilder.DropColumn(
                name: "updated_at",
                schema: "core_entities",
                table: "records");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "core_entities",
                table: "categories");

            migrationBuilder.DropColumn(
                name: "updated_at",
                schema: "core_entities",
                table: "categories");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:core_entities.category_status", "active,archived")
                .Annotation("Npgsql:Enum:core_entities.event_action", "created,updated,removed")
                .OldAnnotation("Npgsql:Enum:core_entities.category_status", "active,archived");

            migrationBuilder.CreateTable(
                name: "events",
                schema: "core_entities",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    action = table.Column<int>(type: "event_action", nullable: false),
                    event_type = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    category_id = table.Column<string>(type: "text", nullable: true),
                    record_id = table.Column<string>(type: "text", nullable: true),
                    tag_id = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("events_pk", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "events",
                schema: "core_entities");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:core_entities.category_status", "active,archived")
                .OldAnnotation("Npgsql:Enum:core_entities.category_status", "active,archived")
                .OldAnnotation("Npgsql:Enum:core_entities.event_action", "created,updated,removed");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "created_at",
                schema: "core_entities",
                table: "users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)))
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "updated_at",
                schema: "core_entities",
                table: "users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "created_at",
                schema: "core_entities",
                table: "tags",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)))
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "updated_at",
                schema: "core_entities",
                table: "tags",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "created_at",
                schema: "core_entities",
                table: "running_records",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)))
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "updated_at",
                schema: "core_entities",
                table: "running_records",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "created_at",
                schema: "core_entities",
                table: "records",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)))
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "updated_at",
                schema: "core_entities",
                table: "records",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "created_at",
                schema: "core_entities",
                table: "categories",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)))
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "updated_at",
                schema: "core_entities",
                table: "categories",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
