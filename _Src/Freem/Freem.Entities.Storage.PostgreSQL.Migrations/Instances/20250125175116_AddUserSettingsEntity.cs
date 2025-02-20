using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freem.Entities.Storage.PostgreSQL.Migrations.Instances
{
    /// <inheritdoc />
    public partial class AddUserSettingsEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "events_entity_name_check",
                schema: "core_entities",
                table: "events");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "updated_at",
                schema: "core_entities",
                table: "user_telegram_integrations",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");

            migrationBuilder.CreateTable(
                name: "user_settings",
                schema: "core_entities",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "text", nullable: false),
                    utc_offset_ticks = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_settings_pk", x => x.user_id);
                    table.ForeignKey(
                        name: "user_settings_users_fk",
                        column: x => x.user_id,
                        principalSchema: "core_entities",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddCheckConstraint(
                name: "events_entity_name_check",
                schema: "core_entities",
                table: "events",
                sql: "entity_name = 'activity' and (action = 'created' or action = 'updated' or action = 'removed' or action = 'archived' or action = 'unarchived') or entity_name = 'record' and (action = 'created' or action = 'updated' or action = 'removed') or entity_name = 'running_record' and (action = 'started' or action = 'stopped' or action = 'updated' or action = 'removed') or entity_name = 'tag' and (action = 'created' or action = 'updated' or action = 'removed') or entity_name = 'user' and (action = 'registered' or action = 'signed_in' or action = 'settings_changed' or action = 'password_credentials_changed' or action = 'telegram_integration_changed')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_settings",
                schema: "core_entities");

            migrationBuilder.DropCheckConstraint(
                name: "events_entity_name_check",
                schema: "core_entities",
                table: "events");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "updated_at",
                schema: "core_entities",
                table: "user_telegram_integrations",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AddCheckConstraint(
                name: "events_entity_name_check",
                schema: "core_entities",
                table: "events",
                sql: "entity_name = 'activity' and (action = 'created' or action = 'updated' or action = 'removed' or action = 'archived' or action = 'unarchived') or entity_name = 'record' and (action = 'created' or action = 'updated' or action = 'removed') or entity_name = 'running_record' and (action = 'started' or action = 'stopped' or action = 'updated' or action = 'removed') or entity_name = 'tag' and (action = 'created' or action = 'updated' or action = 'removed') or entity_name = 'user' and (action = 'registered' or action = 'password_credentials_changed' or action = 'telegram_integration_changed' or action = 'signed_in')");
        }
    }
}
