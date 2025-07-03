using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations;

/// <inheritdoc />
public partial class Create_Comment_Table : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "comments",
            schema: "public",
            columns: table => new
            {
                id = table.Column<Guid>(type: "TEXT", nullable: false),
                content = table.Column<string>(type: "TEXT", nullable: false),
                task_id = table.Column<Guid>(type: "TEXT", nullable: false),
                created_on_utc = table.Column<DateTime>(type: "TEXT", nullable: true),
                modified_on_utc = table.Column<DateTime>(type: "TEXT", nullable: true),
                deleted_on_utc = table.Column<DateTime>(type: "TEXT", nullable: true),
                deleted = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                deleted_by_id = table.Column<Guid>(type: "TEXT", nullable: true),
                created_by_id = table.Column<Guid>(type: "TEXT", nullable: true),
                modified_by_id = table.Column<Guid>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_comments", x => x.id);
                table.ForeignKey(
                    name: "fk_comments_tasks_task_id",
                    column: x => x.task_id,
                    principalSchema: "public",
                    principalTable: "tasks",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_comments_users_created_by_id",
                    column: x => x.created_by_id,
                    principalSchema: "public",
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_comments_users_deleted_by_id",
                    column: x => x.deleted_by_id,
                    principalSchema: "public",
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_comments_users_modified_by_id",
                    column: x => x.modified_by_id,
                    principalSchema: "public",
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "ix_comments_created_by_id",
            schema: "public",
            table: "comments",
            column: "created_by_id");

        migrationBuilder.CreateIndex(
            name: "ix_comments_deleted_by_id",
            schema: "public",
            table: "comments",
            column: "deleted_by_id");

        migrationBuilder.CreateIndex(
            name: "ix_comments_modified_by_id",
            schema: "public",
            table: "comments",
            column: "modified_by_id");

        migrationBuilder.CreateIndex(
            name: "ix_comments_task_id",
            schema: "public",
            table: "comments",
            column: "task_id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "comments",
            schema: "public");
    }
}
