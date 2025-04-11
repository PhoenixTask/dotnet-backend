using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations;

/// <inheritdoc />
public partial class AddEntities : Migration
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
                id = table.Column<Guid>(type: "TEXT", nullable: false),
                email = table.Column<string>(type: "TEXT", nullable: false),
                first_name = table.Column<string>(type: "TEXT", nullable: true),
                last_name = table.Column<string>(type: "TEXT", nullable: true),
                user_name = table.Column<string>(type: "TEXT", nullable: false),
                is_change_password = table.Column<bool>(type: "INTEGER", nullable: false),
                phone_number = table.Column<string>(type: "TEXT", nullable: true),
                password_hash = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table => table.PrimaryKey("pk_users", x => x.id));

        migrationBuilder.CreateTable(
            name: "workspaces",
            schema: "public",
            columns: table => new
            {
                id = table.Column<Guid>(type: "TEXT", nullable: false),
                name = table.Column<string>(type: "TEXT", nullable: false),
                color = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table => table.PrimaryKey("pk_workspaces", x => x.id));

        migrationBuilder.CreateTable(
            name: "settings",
            schema: "public",
            columns: table => new
            {
                id = table.Column<Guid>(type: "TEXT", nullable: false),
                display_name = table.Column<string>(type: "TEXT", nullable: false),
                key = table.Column<string>(type: "TEXT", nullable: false),
                value = table.Column<string>(type: "TEXT", nullable: false),
                created_on_utc = table.Column<DateTime>(type: "TEXT", nullable: true),
                modified_on_utc = table.Column<DateTime>(type: "TEXT", nullable: true),
                created_by_id = table.Column<Guid>(type: "TEXT", nullable: true),
                modified_by_id = table.Column<Guid>(type: "TEXT", nullable: true)
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
            name: "invitations",
            schema: "public",
            columns: table => new
            {
                id = table.Column<Guid>(type: "TEXT", nullable: false),
                token = table.Column<string>(type: "TEXT", nullable: false),
                workspace_id = table.Column<Guid>(type: "TEXT", nullable: false),
                invited_id = table.Column<Guid>(type: "TEXT", nullable: false),
                is_approved = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                created_on_utc = table.Column<DateTime>(type: "TEXT", nullable: true),
                modified_on_utc = table.Column<DateTime>(type: "TEXT", nullable: true),
                created_by_id = table.Column<Guid>(type: "TEXT", nullable: true),
                modified_by_id = table.Column<Guid>(type: "TEXT", nullable: true)
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
            name: "projects",
            schema: "public",
            columns: table => new
            {
                id = table.Column<Guid>(type: "TEXT", nullable: false),
                name = table.Column<string>(type: "TEXT", nullable: false),
                workspace_id = table.Column<Guid>(type: "TEXT", nullable: false),
                created_on_utc = table.Column<DateTime>(type: "TEXT", nullable: true),
                modified_on_utc = table.Column<DateTime>(type: "TEXT", nullable: true),
                deleted_on_utc = table.Column<DateTime>(type: "TEXT", nullable: true),
                deleted = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                created_by_id = table.Column<Guid>(type: "TEXT", nullable: true),
                modified_by_id = table.Column<Guid>(type: "TEXT", nullable: true),
                deleted_by_id = table.Column<Guid>(type: "TEXT", nullable: true)
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
                id = table.Column<Guid>(type: "TEXT", nullable: false),
                name = table.Column<string>(type: "TEXT", nullable: false),
                color = table.Column<string>(type: "TEXT", nullable: false),
                order = table.Column<int>(type: "INTEGER", nullable: false),
                is_archive = table.Column<bool>(type: "INTEGER", nullable: false),
                created_on_utc = table.Column<DateTime>(type: "TEXT", nullable: true),
                modified_on_utc = table.Column<DateTime>(type: "TEXT", nullable: true),
                deleted_on_utc = table.Column<DateTime>(type: "TEXT", nullable: true),
                deleted = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                project_id = table.Column<Guid>(type: "TEXT", nullable: false),
                created_by_id = table.Column<Guid>(type: "TEXT", nullable: true),
                modified_by_id = table.Column<Guid>(type: "TEXT", nullable: true),
                deleted_by_id = table.Column<Guid>(type: "TEXT", nullable: true)
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
                id = table.Column<Guid>(type: "TEXT", nullable: false),
                name = table.Column<string>(type: "TEXT", nullable: false),
                description = table.Column<string>(type: "TEXT", nullable: false),
                dead_line = table.Column<DateOnly>(type: "TEXT", nullable: false),
                attachment = table.Column<string>(type: "TEXT", nullable: false),
                thumbnail = table.Column<string>(type: "TEXT", nullable: false),
                priority = table.Column<int>(type: "INTEGER", nullable: false),
                order = table.Column<int>(type: "INTEGER", nullable: false),
                board_id = table.Column<Guid>(type: "TEXT", nullable: false),
                created_on_utc = table.Column<DateTime>(type: "TEXT", nullable: true),
                modified_on_utc = table.Column<DateTime>(type: "TEXT", nullable: true),
                deleted_on_utc = table.Column<DateTime>(type: "TEXT", nullable: true),
                deleted = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                created_by_id = table.Column<Guid>(type: "TEXT", nullable: true),
                modified_by_id = table.Column<Guid>(type: "TEXT", nullable: true),
                deleted_by_id = table.Column<Guid>(type: "TEXT", nullable: true)
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
            name: "ix_users_user_name",
            schema: "public",
            table: "users",
            column: "user_name",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "invitations",
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
            name: "users",
            schema: "public");

        migrationBuilder.DropTable(
            name: "workspaces",
            schema: "public");
    }
}
