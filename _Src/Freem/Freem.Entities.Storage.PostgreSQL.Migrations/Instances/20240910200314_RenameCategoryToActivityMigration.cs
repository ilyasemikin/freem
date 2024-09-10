using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freem.Entities.Storage.PostgreSQL.Migrations.Instances
{
    /// <inheritdoc />
    public partial class RenameCategoryToActivityMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "categories_tags",
                schema: "core_entities");

            migrationBuilder.DropTable(
                name: "records_categories",
                schema: "core_entities");

            migrationBuilder.DropTable(
                name: "running_records_categories",
                schema: "core_entities");

            migrationBuilder.DropTable(
                name: "categories",
                schema: "core_entities");

            migrationBuilder.DropCheckConstraint(
                name: "events_event_type_check",
                schema: "core_entities",
                table: "events");

            migrationBuilder.RenameColumn(
                name: "category_id",
                schema: "core_entities",
                table: "events",
                newName: "activity_id");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:core_entities.activity_status", "active,archived")
                .Annotation("Npgsql:Enum:core_entities.event_action", "created,updated,removed")
                .OldAnnotation("Npgsql:Enum:core_entities.category_status", "active,archived")
                .OldAnnotation("Npgsql:Enum:core_entities.event_action", "created,updated,removed");

            migrationBuilder.CreateTable(
                name: "activities",
                schema: "core_entities",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    status = table.Column<int>(type: "core_entities.activity_status", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("activities_pk", x => x.id);
                    table.ForeignKey(
                        name: "activities_users_fk",
                        column: x => x.user_id,
                        principalSchema: "core_entities",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "activities_tags",
                schema: "core_entities",
                columns: table => new
                {
                    activity_id = table.Column<string>(type: "text", nullable: false),
                    tag_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("activities_tags_pk", x => new { x.activity_id, x.tag_id });
                    table.ForeignKey(
                        name: "activities_tags_activities_fk",
                        column: x => x.activity_id,
                        principalSchema: "core_entities",
                        principalTable: "activities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "activities_tags_tags_fk",
                        column: x => x.tag_id,
                        principalSchema: "core_entities",
                        principalTable: "tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "records_activities",
                schema: "core_entities",
                columns: table => new
                {
                    record_id = table.Column<string>(type: "text", nullable: false),
                    activity_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("records_activities_pk", x => new { x.record_id, x.activity_id });
                    table.ForeignKey(
                        name: "records_activities_activities_fk",
                        column: x => x.activity_id,
                        principalSchema: "core_entities",
                        principalTable: "activities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "records_activities_records_fk",
                        column: x => x.record_id,
                        principalSchema: "core_entities",
                        principalTable: "records",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "running_records_activities",
                schema: "core_entities",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "text", nullable: false),
                    activity_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("running_records_activities_pk", x => new { x.user_id, x.activity_id });
                    table.ForeignKey(
                        name: "running_records_activities_activities_fk",
                        column: x => x.activity_id,
                        principalSchema: "core_entities",
                        principalTable: "activities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "running_records_activities_running_records_fk",
                        column: x => x.user_id,
                        principalSchema: "core_entities",
                        principalTable: "running_records",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddCheckConstraint(
                name: "events_event_type_check",
                schema: "core_entities",
                table: "events",
                sql: "(event_type = 'activity' and activity_id is not null and record_id is null and tag_id is null) or (event_type = 'record' and activity_id is null and record_id is not null and tag_id is null) or(event_type = 'running_record' and activity_id is null and record_id is null and tag_id is null) or (event_type = 'tag' and activity_id is null and record_id is null and tag_id is not null)");

            migrationBuilder.CreateIndex(
                name: "activities_user_id_idx",
                schema: "core_entities",
                table: "activities",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "activities_tags_tag_id_idx",
                schema: "core_entities",
                table: "activities_tags",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "records_activities_activity_id_idx",
                schema: "core_entities",
                table: "records_activities",
                column: "activity_id");

            migrationBuilder.CreateIndex(
                name: "running_records_activities_activity_id_idx",
                schema: "core_entities",
                table: "running_records_activities",
                column: "activity_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "activities_tags",
                schema: "core_entities");

            migrationBuilder.DropTable(
                name: "records_activities",
                schema: "core_entities");

            migrationBuilder.DropTable(
                name: "running_records_activities",
                schema: "core_entities");

            migrationBuilder.DropTable(
                name: "activities",
                schema: "core_entities");

            migrationBuilder.DropCheckConstraint(
                name: "events_event_type_check",
                schema: "core_entities",
                table: "events");

            migrationBuilder.RenameColumn(
                name: "activity_id",
                schema: "core_entities",
                table: "events",
                newName: "category_id");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:core_entities.category_status", "active,archived")
                .Annotation("Npgsql:Enum:core_entities.event_action", "created,updated,removed")
                .OldAnnotation("Npgsql:Enum:core_entities.activity_status", "active,archived")
                .OldAnnotation("Npgsql:Enum:core_entities.event_action", "created,updated,removed");

            migrationBuilder.CreateTable(
                name: "categories",
                schema: "core_entities",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    status = table.Column<int>(type: "core_entities.category_status", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("categories_pk", x => x.id);
                    table.ForeignKey(
                        name: "categories_users_fk",
                        column: x => x.user_id,
                        principalSchema: "core_entities",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "categories_tags",
                schema: "core_entities",
                columns: table => new
                {
                    category_id = table.Column<string>(type: "text", nullable: false),
                    tag_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("categories_tags_pk", x => new { x.category_id, x.tag_id });
                    table.ForeignKey(
                        name: "categories_tags_categories_fk",
                        column: x => x.category_id,
                        principalSchema: "core_entities",
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "categories_tags_tags_fk",
                        column: x => x.tag_id,
                        principalSchema: "core_entities",
                        principalTable: "tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "records_categories",
                schema: "core_entities",
                columns: table => new
                {
                    record_id = table.Column<string>(type: "text", nullable: false),
                    category_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("records_categories_pk", x => new { x.record_id, x.category_id });
                    table.ForeignKey(
                        name: "records_categories_categories_fk",
                        column: x => x.category_id,
                        principalSchema: "core_entities",
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "records_categories_records_fk",
                        column: x => x.record_id,
                        principalSchema: "core_entities",
                        principalTable: "records",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "running_records_categories",
                schema: "core_entities",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "text", nullable: false),
                    category_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("running_records_categories_pk", x => new { x.user_id, x.category_id });
                    table.ForeignKey(
                        name: "running_records_categories_categories_fk",
                        column: x => x.category_id,
                        principalSchema: "core_entities",
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "running_records_categories_running_records_fk",
                        column: x => x.user_id,
                        principalSchema: "core_entities",
                        principalTable: "running_records",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddCheckConstraint(
                name: "events_event_type_check",
                schema: "core_entities",
                table: "events",
                sql: "(event_type = 'category' and category_id is not null and record_id is null and tag_id is null) or(event_type = 'record' and category_id is null and record_id is not null and tag_id is null) or(event_type = 'running_record' and category_id is null and record_id is null and tag_id is null) or(event_type = 'tag' and category_id is null and record_id is null and tag_id is not null)");

            migrationBuilder.CreateIndex(
                name: "categories_user_id_idx",
                schema: "core_entities",
                table: "categories",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "categories_tags_tag_id_idx",
                schema: "core_entities",
                table: "categories_tags",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "records_categories_category_id_idx",
                schema: "core_entities",
                table: "records_categories",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "running_records_categories_category_id_idx",
                schema: "core_entities",
                table: "running_records_categories",
                column: "category_id");
        }
    }
}
