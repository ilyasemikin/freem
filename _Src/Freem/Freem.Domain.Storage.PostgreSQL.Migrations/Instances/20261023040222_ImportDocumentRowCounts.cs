using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freem.Domain.Storage.PostgreSQL.Migrations.Instances
{
    /// <inheritdoc />
    public partial class ImportDocumentRowCounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "import_row_periods_start_at_end_at_check",
                schema: "core_entities",
                table: "import_row_periods");

            migrationBuilder.AddColumn<int>(
                name: "rows_in_file",
                schema: "core_entities",
                table: "import_documents",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "rows_parsed",
                schema: "core_entities",
                table: "import_documents",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddCheckConstraint(
                name: "import_row_periods_start_at_end_at_check",
                schema: "core_entities",
                table: "import_row_periods",
                sql: "start_at <= end_at");

            migrationBuilder.AddCheckConstraint(
                name: "import_documents_rows_check",
                schema: "core_entities",
                table: "import_documents",
                sql: "rows_parsed >= 0 AND rows_parsed <= rows_in_file");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "import_row_periods_start_at_end_at_check",
                schema: "core_entities",
                table: "import_row_periods");

            migrationBuilder.DropCheckConstraint(
                name: "import_documents_rows_check",
                schema: "core_entities",
                table: "import_documents");

            migrationBuilder.DropColumn(
                name: "rows_in_file",
                schema: "core_entities",
                table: "import_documents");

            migrationBuilder.DropColumn(
                name: "rows_parsed",
                schema: "core_entities",
                table: "import_documents");

            migrationBuilder.AddCheckConstraint(
                name: "import_row_periods_start_at_end_at_check",
                schema: "core_entities",
                table: "import_row_periods",
                sql: "start_at < end_at");
        }
    }
}
