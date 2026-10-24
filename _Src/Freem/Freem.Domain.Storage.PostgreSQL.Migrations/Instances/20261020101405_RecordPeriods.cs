using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freem.Domain.Storage.PostgreSQL.Migrations.Instances
{
    /// <inheritdoc />
    public partial class RecordPeriods : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "records_users_check",
                schema: "core_entities",
                table: "records");

            migrationBuilder.DropColumn(
                name: "end_at",
                schema: "core_entities",
                table: "records");

            migrationBuilder.DropColumn(
                name: "start_at",
                schema: "core_entities",
                table: "records");

            migrationBuilder.CreateTable(
                name: "record_periods",
                schema: "core_entities",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    record_id = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    start_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    end_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("record_periods_pk", x => x.id);
                    table.CheckConstraint("record_periods_start_at_end_at_check", "start_at < end_at");
                    table.ForeignKey(
                        name: "record_periods_records_fk",
                        column: x => x.record_id,
                        principalSchema: "core_entities",
                        principalTable: "records",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "record_periods_record_id_idx",
                schema: "core_entities",
                table: "record_periods",
                column: "record_id");

            migrationBuilder.CreateIndex(
                name: "record_periods_start_at_end_at_idx",
                schema: "core_entities",
                table: "record_periods",
                columns: new[] { "start_at", "end_at" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "record_periods",
                schema: "core_entities");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "end_at",
                schema: "core_entities",
                table: "records",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "start_at",
                schema: "core_entities",
                table: "records",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddCheckConstraint(
                name: "records_users_check",
                schema: "core_entities",
                table: "records",
                sql: "start_at <= end_at");
        }
    }
}
