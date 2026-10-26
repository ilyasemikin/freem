using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freem.Domain.Storage.PostgreSQL.Migrations.Instances
{
    /// <inheritdoc />
    public partial class UserSettingsWeekStart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:core_entities.activity_status", "active,archived")
                .Annotation("Npgsql:Enum:core_entities.week_start", "monday,sunday")
                .OldAnnotation("Npgsql:Enum:core_entities.activity_status", "active,archived");

            migrationBuilder.AddColumn<int>(
                name: "week_start",
                schema: "core_entities",
                table: "user_settings",
                type: "core_entities.week_start",
                nullable: false,
                // Existing users get Monday; an integer default does not cast to the enum type
                defaultValueSql: "'monday'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "week_start",
                schema: "core_entities",
                table: "user_settings");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:core_entities.activity_status", "active,archived")
                .OldAnnotation("Npgsql:Enum:core_entities.activity_status", "active,archived")
                .OldAnnotation("Npgsql:Enum:core_entities.week_start", "monday,sunday");
        }
    }
}
