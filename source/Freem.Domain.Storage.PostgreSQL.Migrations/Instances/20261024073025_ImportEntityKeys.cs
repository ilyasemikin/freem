using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freem.Domain.Storage.PostgreSQL.Migrations.Instances
{
    /// <inheritdoc />
    public partial class ImportEntityKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "source_id",
                schema: "core_entities",
                table: "import_rows",
                newName: "import_record_id");

            migrationBuilder.RenameColumn(
                name: "source_tag_id",
                schema: "core_entities",
                table: "import_row_tags",
                newName: "import_tag_id");

            migrationBuilder.RenameColumn(
                name: "source_activity_id",
                schema: "core_entities",
                table: "import_row_activities",
                newName: "import_activity_id");

            migrationBuilder.RenameColumn(
                name: "source_id",
                schema: "core_entities",
                table: "import_document_tags",
                newName: "import_tag_id");

            migrationBuilder.RenameColumn(
                name: "tag_source_id",
                schema: "core_entities",
                table: "import_document_activity_tags",
                newName: "import_tag_id");

            migrationBuilder.RenameColumn(
                name: "activity_source_id",
                schema: "core_entities",
                table: "import_document_activity_tags",
                newName: "import_activity_id");

            migrationBuilder.RenameColumn(
                name: "source_id",
                schema: "core_entities",
                table: "import_document_activities",
                newName: "import_activity_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "import_record_id",
                schema: "core_entities",
                table: "import_rows",
                newName: "source_id");

            migrationBuilder.RenameColumn(
                name: "import_tag_id",
                schema: "core_entities",
                table: "import_row_tags",
                newName: "source_tag_id");

            migrationBuilder.RenameColumn(
                name: "import_activity_id",
                schema: "core_entities",
                table: "import_row_activities",
                newName: "source_activity_id");

            migrationBuilder.RenameColumn(
                name: "import_tag_id",
                schema: "core_entities",
                table: "import_document_tags",
                newName: "source_id");

            migrationBuilder.RenameColumn(
                name: "import_tag_id",
                schema: "core_entities",
                table: "import_document_activity_tags",
                newName: "tag_source_id");

            migrationBuilder.RenameColumn(
                name: "import_activity_id",
                schema: "core_entities",
                table: "import_document_activity_tags",
                newName: "activity_source_id");

            migrationBuilder.RenameColumn(
                name: "import_activity_id",
                schema: "core_entities",
                table: "import_document_activities",
                newName: "source_id");
        }
    }
}
