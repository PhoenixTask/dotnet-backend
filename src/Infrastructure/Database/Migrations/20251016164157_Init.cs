using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations;

/// <inheritdoc />
public partial class Init : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "public");

        migrationBuilder.CreateTable(
            name: "users",
            schema: "public",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                email = table.Column<string>(type: "text", nullable: false),
                first_name = table.Column<string>(type: "text", nullable: true),
                last_name = table.Column<string>(type: "text", nullable: true),
                user_name = table.Column<string>(type: "text", nullable: false),
                is_change_password = table.Column<bool>(type: "boolean", nullable: false),
                phone_number = table.Column<string>(type: "text", nullable: true),
                password_hash = table.Column<string>(type: "text", nullable: true),
                normalized_user_name = table.Column<string>(type: "text", nullable: false),
                profile_image = table.Column<string>(type: "text", nullable: true),
                auth_provider = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
            },
            constraints: table => table.PrimaryKey("pk_users", x => x.id));

        migrationBuilder.CreateTable(
            name: "refresh_tokens",
            schema: "public",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                user_id = table.Column<Guid>(type: "uuid", nullable: false),
                token = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                token_type = table.Column<int>(type: "integer", nullable: false),
                expire_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_refresh_tokens", x => x.id);
                table.ForeignKey(
                    name: "fk_refresh_tokens_users_user_id",
                    column: x => x.user_id,
                    principalSchema: "public",
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "settings",
            schema: "public",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                display_name = table.Column<string>(type: "text", nullable: false),
                key = table.Column<string>(type: "text", nullable: false),
                value = table.Column<string>(type: "text", nullable: false),
                created_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                modified_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                created_by_id = table.Column<Guid>(type: "uuid", nullable: true),
                modified_by_id = table.Column<Guid>(type: "uuid", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_settings", x => x.id);
                table.ForeignKey(
                    name: "fk_settings_users_created_by_id",
                    column: x => x.created_by_id,
                    principalSchema: "public",
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_settings_users_modified_by_id",
                    column: x => x.modified_by_id,
                    principalSchema: "public",
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "workspaces",
            schema: "public",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                name = table.Column<string>(type: "text", nullable: false),
                color = table.Column<string>(type: "text", nullable: false),
                created_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                modified_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                deleted_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                deleted_by_id = table.Column<Guid>(type: "uuid", nullable: true),
                created_by_id = table.Column<Guid>(type: "uuid", nullable: true),
                modified_by_id = table.Column<Guid>(type: "uuid", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_workspaces", x => x.id);
                table.ForeignKey(
                    name: "fk_workspaces_users_created_by_id",
                    column: x => x.created_by_id,
                    principalSchema: "public",
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_workspaces_users_deleted_by_id",
                    column: x => x.deleted_by_id,
                    principalSchema: "public",
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_workspaces_users_modified_by_id",
                    column: x => x.modified_by_id,
                    principalSchema: "public",
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "invitations",
            schema: "public",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                token = table.Column<string>(type: "text", nullable: false),
                workspace_id = table.Column<Guid>(type: "uuid", nullable: false),
                invited_id = table.Column<Guid>(type: "uuid", nullable: false),
                is_approved = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                project_role = table.Column<int>(type: "integer", nullable: false),
                created_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                modified_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                created_by_id = table.Column<Guid>(type: "uuid", nullable: true),
                modified_by_id = table.Column<Guid>(type: "uuid", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_invitations", x => x.id);
                table.ForeignKey(
                    name: "fk_invitations_users_created_by_id",
                    column: x => x.created_by_id,
                    principalSchema: "public",
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_invitations_users_invited_id",
                    column: x => x.invited_id,
                    principalSchema: "public",
                    principalTable: "users",
                    principalColumn: "id");
                table.ForeignKey(
                    name: "fk_invitations_users_modified_by_id",
                    column: x => x.modified_by_id,
                    principalSchema: "public",
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_invitations_workspaces_workspace_id",
                    column: x => x.workspace_id,
                    principalSchema: "public",
                    principalTable: "workspaces",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "members",
            schema: "public",
            columns: table => new
            {
                user_id = table.Column<Guid>(type: "uuid", nullable: false),
                workspace_id = table.Column<Guid>(type: "uuid", nullable: false),
                role = table.Column<int>(type: "integer", nullable: false),
                created_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                modified_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                deleted_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                deleted_by_id = table.Column<Guid>(type: "uuid", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_members", x => new { x.user_id, x.workspace_id });
                table.ForeignKey(
                    name: "fk_members_users_deleted_by_id",
                    column: x => x.deleted_by_id,
                    principalSchema: "public",
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_members_users_user_id",
                    column: x => x.user_id,
                    principalSchema: "public",
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_members_workspaces_workspace_id",
                    column: x => x.workspace_id,
                    principalSchema: "public",
                    principalTable: "workspaces",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "projects",
            schema: "public",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                name = table.Column<string>(type: "text", nullable: false),
                color = table.Column<string>(type: "text", nullable: false),
                workspace_id = table.Column<Guid>(type: "uuid", nullable: false),
                created_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                modified_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                deleted_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                deleted_by_id = table.Column<Guid>(type: "uuid", nullable: true),
                created_by_id = table.Column<Guid>(type: "uuid", nullable: true),
                modified_by_id = table.Column<Guid>(type: "uuid", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_projects", x => x.id);
                table.ForeignKey(
                    name: "fk_projects_users_created_by_id",
                    column: x => x.created_by_id,
                    principalSchema: "public",
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_projects_users_deleted_by_id",
                    column: x => x.deleted_by_id,
                    principalSchema: "public",
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_projects_users_modified_by_id",
                    column: x => x.modified_by_id,
                    principalSchema: "public",
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_projects_workspaces_workspace_id",
                    column: x => x.workspace_id,
                    principalSchema: "public",
                    principalTable: "workspaces",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "boards",
            schema: "public",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                name = table.Column<string>(type: "text", nullable: false),
                color = table.Column<string>(type: "text", nullable: false),
                order = table.Column<int>(type: "integer", nullable: false),
                is_archive = table.Column<bool>(type: "boolean", nullable: false),
                created_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                modified_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                deleted_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                project_id = table.Column<Guid>(type: "uuid", nullable: false),
                deleted_by_id = table.Column<Guid>(type: "uuid", nullable: true),
                created_by_id = table.Column<Guid>(type: "uuid", nullable: true),
                modified_by_id = table.Column<Guid>(type: "uuid", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_boards", x => x.id);
                table.ForeignKey(
                    name: "fk_boards_projects_project_id",
                    column: x => x.project_id,
                    principalSchema: "public",
                    principalTable: "projects",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_boards_users_created_by_id",
                    column: x => x.created_by_id,
                    principalSchema: "public",
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_boards_users_deleted_by_id",
                    column: x => x.deleted_by_id,
                    principalSchema: "public",
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_boards_users_modified_by_id",
                    column: x => x.modified_by_id,
                    principalSchema: "public",
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "tasks",
            schema: "public",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                name = table.Column<string>(type: "text", nullable: false),
                description = table.Column<string>(type: "text", nullable: false),
                dead_line = table.Column<DateOnly>(type: "date", nullable: true),
                attachment = table.Column<string>(type: "text", nullable: true),
                thumbnail = table.Column<string>(type: "text", nullable: true),
                priority = table.Column<int>(type: "integer", nullable: false),
                order = table.Column<int>(type: "integer", nullable: false),
                board_id = table.Column<Guid>(type: "uuid", nullable: false),
                created_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                modified_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                deleted_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                deleted_by_id = table.Column<Guid>(type: "uuid", nullable: true),
                is_complete = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                created_by_id = table.Column<Guid>(type: "uuid", nullable: true),
                modified_by_id = table.Column<Guid>(type: "uuid", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_tasks", x => x.id);
                table.ForeignKey(
                    name: "fk_tasks_boards_board_id",
                    column: x => x.board_id,
                    principalSchema: "public",
                    principalTable: "boards",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_tasks_users_created_by_id",
                    column: x => x.created_by_id,
                    principalSchema: "public",
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_tasks_users_deleted_by_id",
                    column: x => x.deleted_by_id,
                    principalSchema: "public",
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_tasks_users_modified_by_id",
                    column: x => x.modified_by_id,
                    principalSchema: "public",
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "comments",
            schema: "public",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                content = table.Column<string>(type: "text", nullable: false),
                task_id = table.Column<Guid>(type: "uuid", nullable: false),
                created_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                modified_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                deleted_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                deleted_by_id = table.Column<Guid>(type: "uuid", nullable: true),
                created_by_id = table.Column<Guid>(type: "uuid", nullable: true),
                modified_by_id = table.Column<Guid>(type: "uuid", nullable: true)
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
            name: "ix_boards_created_by_id",
            schema: "public",
            table: "boards",
            column: "created_by_id");

        migrationBuilder.CreateIndex(
            name: "ix_boards_deleted_by_id",
            schema: "public",
            table: "boards",
            column: "deleted_by_id");

        migrationBuilder.CreateIndex(
            name: "ix_boards_modified_by_id",
            schema: "public",
            table: "boards",
            column: "modified_by_id");

        migrationBuilder.CreateIndex(
            name: "ix_boards_project_id",
            schema: "public",
            table: "boards",
            column: "project_id");

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

        migrationBuilder.CreateIndex(
            name: "ix_invitations_created_by_id",
            schema: "public",
            table: "invitations",
            column: "created_by_id");

        migrationBuilder.CreateIndex(
            name: "ix_invitations_invited_id",
            schema: "public",
            table: "invitations",
            column: "invited_id");

        migrationBuilder.CreateIndex(
            name: "ix_invitations_modified_by_id",
            schema: "public",
            table: "invitations",
            column: "modified_by_id");

        migrationBuilder.CreateIndex(
            name: "ix_invitations_token",
            schema: "public",
            table: "invitations",
            column: "token",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_invitations_workspace_id",
            schema: "public",
            table: "invitations",
            column: "workspace_id");

        migrationBuilder.CreateIndex(
            name: "ix_members_deleted_by_id",
            schema: "public",
            table: "members",
            column: "deleted_by_id");

        migrationBuilder.CreateIndex(
            name: "ix_members_workspace_id",
            schema: "public",
            table: "members",
            column: "workspace_id");

        migrationBuilder.CreateIndex(
            name: "ix_projects_created_by_id",
            schema: "public",
            table: "projects",
            column: "created_by_id");

        migrationBuilder.CreateIndex(
            name: "ix_projects_deleted_by_id",
            schema: "public",
            table: "projects",
            column: "deleted_by_id");

        migrationBuilder.CreateIndex(
            name: "ix_projects_modified_by_id",
            schema: "public",
            table: "projects",
            column: "modified_by_id");

        migrationBuilder.CreateIndex(
            name: "ix_projects_workspace_id",
            schema: "public",
            table: "projects",
            column: "workspace_id");

        migrationBuilder.CreateIndex(
            name: "ix_refresh_tokens_user_id",
            schema: "public",
            table: "refresh_tokens",
            column: "user_id");

        migrationBuilder.CreateIndex(
            name: "ix_settings_created_by_id",
            schema: "public",
            table: "settings",
            column: "created_by_id");

        migrationBuilder.CreateIndex(
            name: "ix_settings_key",
            schema: "public",
            table: "settings",
            column: "key",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_settings_modified_by_id",
            schema: "public",
            table: "settings",
            column: "modified_by_id");

        migrationBuilder.CreateIndex(
            name: "ix_tasks_board_id",
            schema: "public",
            table: "tasks",
            column: "board_id");

        migrationBuilder.CreateIndex(
            name: "ix_tasks_created_by_id",
            schema: "public",
            table: "tasks",
            column: "created_by_id");

        migrationBuilder.CreateIndex(
            name: "ix_tasks_deleted_by_id",
            schema: "public",
            table: "tasks",
            column: "deleted_by_id");

        migrationBuilder.CreateIndex(
            name: "ix_tasks_modified_by_id",
            schema: "public",
            table: "tasks",
            column: "modified_by_id");

        migrationBuilder.CreateIndex(
            name: "ix_users_email",
            schema: "public",
            table: "users",
            column: "email",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_users_normalized_user_name",
            schema: "public",
            table: "users",
            column: "normalized_user_name");

        migrationBuilder.CreateIndex(
            name: "ix_workspaces_created_by_id",
            schema: "public",
            table: "workspaces",
            column: "created_by_id");

        migrationBuilder.CreateIndex(
            name: "ix_workspaces_deleted_by_id",
            schema: "public",
            table: "workspaces",
            column: "deleted_by_id");

        migrationBuilder.CreateIndex(
            name: "ix_workspaces_modified_by_id",
            schema: "public",
            table: "workspaces",
            column: "modified_by_id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "comments",
            schema: "public");

        migrationBuilder.DropTable(
            name: "invitations",
            schema: "public");

        migrationBuilder.DropTable(
            name: "members",
            schema: "public");

        migrationBuilder.DropTable(
            name: "refresh_tokens",
            schema: "public");

        migrationBuilder.DropTable(
            name: "settings",
            schema: "public");

        migrationBuilder.DropTable(
            name: "tasks",
            schema: "public");

        migrationBuilder.DropTable(
            name: "boards",
            schema: "public");

        migrationBuilder.DropTable(
            name: "projects",
            schema: "public");

        migrationBuilder.DropTable(
            name: "workspaces",
            schema: "public");

        migrationBuilder.DropTable(
            name: "users",
            schema: "public");
    }
}
