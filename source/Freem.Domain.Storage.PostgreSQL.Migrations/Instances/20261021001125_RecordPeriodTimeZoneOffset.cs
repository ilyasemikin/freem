using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freem.Domain.Storage.PostgreSQL.Migrations.Instances
{
    /// <inheritdoc />
    public partial class RecordPeriodTimeZoneOffset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "utc_offset_ticks",
                schema: "core_entities",
                table: "user_settings",
                newName: "time_zone_offset_ticks");

            migrationBuilder.AddColumn<long>(
                name: "time_zone_offset_ticks",
                schema: "core_entities",
                table: "record_periods",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "time_zone_offset_ticks",
                schema: "core_entities",
                table: "record_periods");

            migrationBuilder.RenameColumn(
                name: "time_zone_offset_ticks",
                schema: "core_entities",
                table: "user_settings",
                newName: "utc_offset_ticks");
        }
    }
}
