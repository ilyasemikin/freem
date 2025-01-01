using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freem.Entities.Storage.PostgreSQL.Migrations.Instances
{
    /// <inheritdoc />
    public partial class AddUserLoginCredentialsEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "events_entity_name_check",
                schema: "core_entities",
                table: "events");

            migrationBuilder.CreateTable(
                name: "user_password_credentials",
                schema: "core_entities",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "text", nullable: false),
                    login = table.Column<string>(type: "text", nullable: false),
                    password_hash_algorithm = table.Column<string>(type: "text", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    password_salt = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_password_credentials_pk", x => x.user_id);
                    table.ForeignKey(
                        name: "user_password_credentials_users_fk",
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
                sql: "entity_name = 'activity' and (action = 'created' or action = 'updated' or action = 'removed' or action = 'archived' or action = 'unarchived') or entity_name = 'record' and (action = 'created' or action = 'updated' or action = 'removed') or entity_name = 'running_record' and (action = 'started' or action = 'stopped' or action = 'updated' or action = 'removed') or entity_name = 'tag' and (action = 'created' or action = 'updated' or action = 'removed') or entity_name = 'user' and (action = 'registered' or action = 'password_credentials_changed' or action = 'signed_in')");

            migrationBuilder.CreateIndex(
                name: "user_password_credentials_login_unique",
                schema: "core_entities",
                table: "user_password_credentials",
                column: "login",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_password_credentials",
                schema: "core_entities");

            migrationBuilder.DropCheckConstraint(
                name: "events_entity_name_check",
                schema: "core_entities",
                table: "events");

            migrationBuilder.AddCheckConstraint(
                name: "events_entity_name_check",
                schema: "core_entities",
                table: "events",
                sql: "entity_name = 'activity' and (action = 'created' or action = 'updated' or action = 'removed' or action = 'archived' or action = 'unarchived') or entity_name = 'record' and (action = 'created' or action = 'updated' or action = 'removed') or entity_name = 'running_record' and (action = 'started' or action = 'stopped' or action = 'updated' or action = 'removed') or entity_name = 'tag' and (action = 'created' or action = 'updated' or action = 'removed') or entity_name = 'user' and (action = 'signed_in')");
        }
    }
}
