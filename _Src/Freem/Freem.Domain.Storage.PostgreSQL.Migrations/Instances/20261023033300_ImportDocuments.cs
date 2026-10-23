using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freem.Domain.Storage.PostgreSQL.Migrations.Instances
{
    /// <inheritdoc />
    public partial class ImportDocuments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "import_documents",
                schema: "core_entities",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    format = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    file_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    time_zone_offset_ticks = table.Column<long>(type: "bigint", nullable: false),
                    status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("import_documents_pk", x => x.id);
                    table.CheckConstraint("import_documents_status_check", "status IN ('Created', 'Parsing', 'Parsed', 'Failed')");
                    table.ForeignKey(
                        name: "import_documents_users_fk",
                        column: x => x.user_id,
                        principalSchema: "core_entities",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "import_document_activities",
                schema: "core_entities",
                columns: table => new
                {
                    document_id = table.Column<string>(type: "text", nullable: false),
                    source_id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    color = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    create_new = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    resolved_activity_id = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("import_document_activities_pk", x => new { x.document_id, x.source_id });
                    table.CheckConstraint("import_document_activities_resolution_check", "NOT (create_new AND resolved_activity_id IS NOT NULL)");
                    table.ForeignKey(
                        name: "import_document_activities_activities_fk",
                        column: x => x.resolved_activity_id,
                        principalSchema: "core_entities",
                        principalTable: "activities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "import_document_activities_import_documents_fk",
                        column: x => x.document_id,
                        principalSchema: "core_entities",
                        principalTable: "import_documents",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "import_document_tags",
                schema: "core_entities",
                columns: table => new
                {
                    document_id = table.Column<string>(type: "text", nullable: false),
                    source_id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    create_new = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    resolved_tag_id = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("import_document_tags_pk", x => new { x.document_id, x.source_id });
                    table.CheckConstraint("import_document_tags_resolution_check", "NOT (create_new AND resolved_tag_id IS NOT NULL)");
                    table.ForeignKey(
                        name: "import_document_tags_import_documents_fk",
                        column: x => x.document_id,
                        principalSchema: "core_entities",
                        principalTable: "import_documents",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "import_document_tags_tags_fk",
                        column: x => x.resolved_tag_id,
                        principalSchema: "core_entities",
                        principalTable: "tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "import_rows",
                schema: "core_entities",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    document_id = table.Column<string>(type: "text", nullable: false),
                    source_id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    first_start_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("import_rows_pk", x => x.id);
                    table.ForeignKey(
                        name: "import_rows_import_documents_fk",
                        column: x => x.document_id,
                        principalSchema: "core_entities",
                        principalTable: "import_documents",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "import_document_activity_tags",
                schema: "core_entities",
                columns: table => new
                {
                    document_id = table.Column<string>(type: "text", nullable: false),
                    activity_source_id = table.Column<string>(type: "text", nullable: false),
                    tag_source_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("import_document_activity_tags_pk", x => new { x.document_id, x.activity_source_id, x.tag_source_id });
                    table.ForeignKey(
                        name: "import_document_activity_tags_import_document_activities_fk",
                        columns: x => new { x.document_id, x.activity_source_id },
                        principalSchema: "core_entities",
                        principalTable: "import_document_activities",
                        principalColumns: new[] { "document_id", "source_id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "import_row_activities",
                schema: "core_entities",
                columns: table => new
                {
                    row_id = table.Column<string>(type: "text", nullable: false),
                    source_activity_id = table.Column<string>(type: "text", nullable: false),
                    position = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("import_row_activities_pk", x => new { x.row_id, x.source_activity_id });
                    table.ForeignKey(
                        name: "import_row_activities_import_rows_fk",
                        column: x => x.row_id,
                        principalSchema: "core_entities",
                        principalTable: "import_rows",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "import_row_periods",
                schema: "core_entities",
                columns: table => new
                {
                    row_id = table.Column<string>(type: "text", nullable: false),
                    position = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    start_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    end_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    time_zone_offset_ticks = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("import_row_periods_pk", x => new { x.row_id, x.position });
                    table.CheckConstraint("import_row_periods_start_at_end_at_check", "start_at < end_at");
                    table.ForeignKey(
                        name: "import_row_periods_import_rows_fk",
                        column: x => x.row_id,
                        principalSchema: "core_entities",
                        principalTable: "import_rows",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "import_row_tags",
                schema: "core_entities",
                columns: table => new
                {
                    row_id = table.Column<string>(type: "text", nullable: false),
                    source_tag_id = table.Column<string>(type: "text", nullable: false),
                    position = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("import_row_tags_pk", x => new { x.row_id, x.source_tag_id });
                    table.ForeignKey(
                        name: "import_row_tags_import_rows_fk",
                        column: x => x.row_id,
                        principalSchema: "core_entities",
                        principalTable: "import_rows",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "import_document_activities_resolved_activity_id_idx",
                schema: "core_entities",
                table: "import_document_activities",
                column: "resolved_activity_id");

            migrationBuilder.CreateIndex(
                name: "import_document_tags_resolved_tag_id_idx",
                schema: "core_entities",
                table: "import_document_tags",
                column: "resolved_tag_id");

            migrationBuilder.CreateIndex(
                name: "import_documents_user_id_idx",
                schema: "core_entities",
                table: "import_documents",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "import_row_periods_start_at_end_at_idx",
                schema: "core_entities",
                table: "import_row_periods",
                columns: new[] { "start_at", "end_at" });

            migrationBuilder.CreateIndex(
                name: "import_rows_document_id_first_start_at_id_idx",
                schema: "core_entities",
                table: "import_rows",
                columns: new[] { "document_id", "first_start_at", "id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "import_document_activity_tags",
                schema: "core_entities");

            migrationBuilder.DropTable(
                name: "import_document_tags",
                schema: "core_entities");

            migrationBuilder.DropTable(
                name: "import_row_activities",
                schema: "core_entities");

            migrationBuilder.DropTable(
                name: "import_row_periods",
                schema: "core_entities");

            migrationBuilder.DropTable(
                name: "import_row_tags",
                schema: "core_entities");

            migrationBuilder.DropTable(
                name: "import_document_activities",
                schema: "core_entities");

            migrationBuilder.DropTable(
                name: "import_rows",
                schema: "core_entities");

            migrationBuilder.DropTable(
                name: "import_documents",
                schema: "core_entities");
        }
    }
}
