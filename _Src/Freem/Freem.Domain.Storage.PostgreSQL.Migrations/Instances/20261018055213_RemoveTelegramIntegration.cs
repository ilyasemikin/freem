using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freem.Domain.Storage.PostgreSQL.Migrations.Instances
{
    /// <inheritdoc />
    public partial class RemoveTelegramIntegration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_telegram_integrations",
                schema: "core_entities");

            migrationBuilder.DropCheckConstraint(
                name: "events_entity_name_check",
                schema: "core_entities",
                table: "events");

            migrationBuilder.Sql(
                "DELETE FROM core_entities.events " +
                "WHERE entity_name = 'user' AND action = 'telegram_integration_changed';");

            migrationBuilder.AddCheckConstraint(
                name: "events_entity_name_check",
                schema: "core_entities",
                table: "events",
                sql: "entity_name = 'activity' and (action = 'created' or action = 'updated' or action = 'removed' or action = 'archived' or action = 'unarchived') or entity_name = 'record' and (action = 'created' or action = 'updated' or action = 'removed') or entity_name = 'running_record' and (action = 'started' or action = 'stopped' or action = 'updated' or action = 'removed') or entity_name = 'tag' and (action = 'created' or action = 'updated' or action = 'removed') or entity_name = 'user' and (action = 'registered' or action = 'settings_changed')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "events_entity_name_check",
                schema: "core_entities",
                table: "events");

            migrationBuilder.CreateTable(
                name: "user_telegram_integrations",
                schema: "core_entities",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    telegram_user_id = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_telegram_integrations_pk", x => x.user_id);
                    table.ForeignKey(
                        name: "user_telegram_integrations_users_fk",
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
                sql: "entity_name = 'activity' and (action = 'created' or action = 'updated' or action = 'removed' or action = 'archived' or action = 'unarchived') or entity_name = 'record' and (action = 'created' or action = 'updated' or action = 'removed') or entity_name = 'running_record' and (action = 'started' or action = 'stopped' or action = 'updated' or action = 'removed') or entity_name = 'tag' and (action = 'created' or action = 'updated' or action = 'removed') or entity_name = 'user' and (action = 'registered' or action = 'settings_changed' or action = 'telegram_integration_changed')");

            migrationBuilder.CreateIndex(
                name: "user_telegram_integrations_telegram_user_id_unique",
                schema: "core_entities",
                table: "user_telegram_integrations",
                column: "telegram_user_id",
                unique: true);
        }
    }
}
