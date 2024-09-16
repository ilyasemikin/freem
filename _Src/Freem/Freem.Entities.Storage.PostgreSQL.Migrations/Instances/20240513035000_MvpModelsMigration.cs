using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freem.Entities.Storage.PostgreSQL.Migrations.Instances
{
    /// <inheritdoc />
    public partial class MvpModelsMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "core_entities");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:core_entities.category_status", "active,archived");

            migrationBuilder.CreateTable(
                name: "users",
                schema: "core_entities",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    nickname = table.Column<string>(type: "text", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("users_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "categories",
                schema: "core_entities",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    status = table.Column<int>(type: "core_entities.category_status", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("categories_pk", x => x.id);
                    table.ForeignKey(
                        name: "categories_users_fk",
                        column: x => x.user_id,
                        principalSchema: "core_entities",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "records",
                schema: "core_entities",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    start_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    end_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("records_pk", x => x.id);
                    table.CheckConstraint("records_users_check", "start_at <= end_at");
                    table.ForeignKey(
                        name: "records_users_fk",
                        column: x => x.user_id,
                        principalSchema: "core_entities",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "running_records",
                schema: "core_entities",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    start_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("running_records_pk", x => x.user_id);
                    table.ForeignKey(
                        name: "running_records_fk",
                        column: x => x.user_id,
                        principalSchema: "core_entities",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tags",
                schema: "core_entities",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("tags_pk", x => x.id);
                    table.ForeignKey(
                        name: "tags_users_fk",
                        column: x => x.user_id,
                        principalSchema: "core_entities",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "records_categories",
                schema: "core_entities",
                columns: table => new
                {
                    record_id = table.Column<string>(type: "text", nullable: false),
                    category_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("records_categories_pk", x => new { x.record_id, x.category_id });
                    table.ForeignKey(
                        name: "records_categories_categories_fk",
                        column: x => x.category_id,
                        principalSchema: "core_entities",
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "records_categories_records_fk",
                        column: x => x.record_id,
                        principalSchema: "core_entities",
                        principalTable: "records",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "running_records_categories",
                schema: "core_entities",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "text", nullable: false),
                    category_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("running_records_categories_pk", x => new { x.user_id, x.category_id });
                    table.ForeignKey(
                        name: "running_records_categories_categories_fk",
                        column: x => x.category_id,
                        principalSchema: "core_entities",
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "running_records_categories_running_records_fk",
                        column: x => x.user_id,
                        principalSchema: "core_entities",
                        principalTable: "running_records",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "categories_tags",
                schema: "core_entities",
                columns: table => new
                {
                    category_id = table.Column<string>(type: "text", nullable: false),
                    tag_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("categories_tags_pk", x => new { x.category_id, x.tag_id });
                    table.ForeignKey(
                        name: "categories_tags_categories_fk",
                        column: x => x.category_id,
                        principalSchema: "core_entities",
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "categories_tags_tags_fk",
                        column: x => x.tag_id,
                        principalSchema: "core_entities",
                        principalTable: "tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "records_tags",
                schema: "core_entities",
                columns: table => new
                {
                    record_id = table.Column<string>(type: "text", nullable: false),
                    tag_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("records_tags_pk", x => new { x.record_id, x.tag_id });
                    table.ForeignKey(
                        name: "records_tags_records_fk",
                        column: x => x.record_id,
                        principalSchema: "core_entities",
                        principalTable: "records",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "records_tags_tags_fk",
                        column: x => x.tag_id,
                        principalSchema: "core_entities",
                        principalTable: "tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "running_records_tags",
                schema: "core_entities",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "text", nullable: false),
                    tag_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("running_records_tags_pk", x => new { x.user_id, x.tag_id });
                    table.ForeignKey(
                        name: "running_records_tags_running_records_fk",
                        column: x => x.user_id,
                        principalSchema: "core_entities",
                        principalTable: "running_records",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "running_records_tags_tags_fk",
                        column: x => x.tag_id,
                        principalSchema: "core_entities",
                        principalTable: "tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "categories_user_id_idx",
                schema: "core_entities",
                table: "categories",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "categories_tags_tag_id_idx",
                schema: "core_entities",
                table: "categories_tags",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "records_user_id_idx",
                schema: "core_entities",
                table: "records",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "records_categories_category_id_idx",
                schema: "core_entities",
                table: "records_categories",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "records_tags_tag_id_idx",
                schema: "core_entities",
                table: "records_tags",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "running_records_categories_category_id_idx",
                schema: "core_entities",
                table: "running_records_categories",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "running_records_tags_tag_id_idx",
                schema: "core_entities",
                table: "running_records_tags",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "tags_name_unique",
                schema: "core_entities",
                table: "tags",
                columns: new[] { "name", "user_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "tags_user_id_unique",
                schema: "core_entities",
                table: "tags",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "categories_tags",
                schema: "core_entities");

            migrationBuilder.DropTable(
                name: "records_categories",
                schema: "core_entities");

            migrationBuilder.DropTable(
                name: "records_tags",
                schema: "core_entities");

            migrationBuilder.DropTable(
                name: "running_records_categories",
                schema: "core_entities");

            migrationBuilder.DropTable(
                name: "running_records_tags",
                schema: "core_entities");

            migrationBuilder.DropTable(
                name: "records",
                schema: "core_entities");

            migrationBuilder.DropTable(
                name: "categories",
                schema: "core_entities");

            migrationBuilder.DropTable(
                name: "running_records",
                schema: "core_entities");

            migrationBuilder.DropTable(
                name: "tags",
                schema: "core_entities");

            migrationBuilder.DropTable(
                name: "users",
                schema: "core_entities");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:Enum:core_entities.category_status", "active,archived");
        }
    }
}
