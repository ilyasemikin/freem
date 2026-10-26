using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freem.Domain.Storage.PostgreSQL.Migrations.Instances
{
    /// <inheritdoc />
    public partial class RecordSource : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "source_import_document_id",
                schema: "core_entities",
                table: "records",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "source_import_format",
                schema: "core_entities",
                table: "records",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "source_import_record_id",
                schema: "core_entities",
                table: "records",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "source_import_row_id",
                schema: "core_entities",
                table: "records",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "source_type",
                schema: "core_entities",
                table: "records",
                type: "text",
                nullable: false,
                defaultValue: "Manual");

            migrationBuilder.AddCheckConstraint(
                name: "records_source_import_check",
                schema: "core_entities",
                table: "records",
                sql: "(source_type = 'Import' AND source_import_document_id IS NOT NULL AND source_import_row_id IS NOT NULL AND source_import_format IS NOT NULL AND source_import_record_id IS NOT NULL) OR (source_type <> 'Import' AND source_import_document_id IS NULL AND source_import_row_id IS NULL AND source_import_format IS NULL AND source_import_record_id IS NULL)");

            migrationBuilder.AddCheckConstraint(
                name: "records_source_type_check",
                schema: "core_entities",
                table: "records",
                sql: "source_type IN ('Manual', 'RunningRecord', 'Import')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "records_source_import_check",
                schema: "core_entities",
                table: "records");

            migrationBuilder.DropCheckConstraint(
                name: "records_source_type_check",
                schema: "core_entities",
                table: "records");

            migrationBuilder.DropColumn(
                name: "source_import_document_id",
                schema: "core_entities",
                table: "records");

            migrationBuilder.DropColumn(
                name: "source_import_format",
                schema: "core_entities",
                table: "records");

            migrationBuilder.DropColumn(
                name: "source_import_record_id",
                schema: "core_entities",
                table: "records");

            migrationBuilder.DropColumn(
                name: "source_import_row_id",
                schema: "core_entities",
                table: "records");

            migrationBuilder.DropColumn(
                name: "source_type",
                schema: "core_entities",
                table: "records");
        }
    }
}
