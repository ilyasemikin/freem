using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freem.Domain.Storage.PostgreSQL.Migrations.Instances
{
    /// <inheritdoc />
    public partial class ImportSourceMappings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "import_document_tags_resolution_check",
                schema: "core_entities",
                table: "import_document_tags");

            migrationBuilder.DropCheckConstraint(
                name: "import_document_activities_resolution_check",
                schema: "core_entities",
                table: "import_document_activities");

            migrationBuilder.DropColumn(
                name: "create_new",
                schema: "core_entities",
                table: "import_document_tags");

            migrationBuilder.DropColumn(
                name: "create_new",
                schema: "core_entities",
                table: "import_document_activities");

            migrationBuilder.RenameColumn(
                name: "resolved_tag_id",
                schema: "core_entities",
                table: "import_document_tags",
                newName: "mapped_tag_id");

            migrationBuilder.RenameIndex(
                name: "import_document_tags_resolved_tag_id_idx",
                schema: "core_entities",
                table: "import_document_tags",
                newName: "import_document_tags_mapped_tag_id_idx");

            migrationBuilder.RenameColumn(
                name: "resolved_activity_id",
                schema: "core_entities",
                table: "import_document_activities",
                newName: "mapped_activity_id");

            migrationBuilder.RenameIndex(
                name: "import_document_activities_resolved_activity_id_idx",
                schema: "core_entities",
                table: "import_document_activities",
                newName: "import_document_activities_mapped_activity_id_idx");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "mapped_tag_id",
                schema: "core_entities",
                table: "import_document_tags",
                newName: "resolved_tag_id");

            migrationBuilder.RenameIndex(
                name: "import_document_tags_mapped_tag_id_idx",
                schema: "core_entities",
                table: "import_document_tags",
                newName: "import_document_tags_resolved_tag_id_idx");

            migrationBuilder.RenameColumn(
                name: "mapped_activity_id",
                schema: "core_entities",
                table: "import_document_activities",
                newName: "resolved_activity_id");

            migrationBuilder.RenameIndex(
                name: "import_document_activities_mapped_activity_id_idx",
                schema: "core_entities",
                table: "import_document_activities",
                newName: "import_document_activities_resolved_activity_id_idx");

            migrationBuilder.AddColumn<bool>(
                name: "create_new",
                schema: "core_entities",
                table: "import_document_tags",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "create_new",
                schema: "core_entities",
                table: "import_document_activities",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddCheckConstraint(
                name: "import_document_tags_resolution_check",
                schema: "core_entities",
                table: "import_document_tags",
                sql: "NOT (create_new AND resolved_tag_id IS NOT NULL)");

            migrationBuilder.AddCheckConstraint(
                name: "import_document_activities_resolution_check",
                schema: "core_entities",
                table: "import_document_activities",
                sql: "NOT (create_new AND resolved_activity_id IS NOT NULL)");
        }
    }
}
