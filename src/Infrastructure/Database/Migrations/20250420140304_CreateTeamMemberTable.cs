using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations;

/// <inheritdoc />
public partial class CreateTeamMemberTable : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "members",
            schema: "public",
            columns: table => new
            {
                user_id = table.Column<Guid>(type: "TEXT", nullable: false),
                workspace_id = table.Column<Guid>(type: "TEXT", nullable: false),
                role = table.Column<int>(type: "INTEGER", nullable: false),
                created_on_utc = table.Column<DateTime>(type: "TEXT", nullable: true),
                modified_on_utc = table.Column<DateTime>(type: "TEXT", nullable: true),
                deleted_on_utc = table.Column<DateTime>(type: "TEXT", nullable: true),
                deleted = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                deleted_by_id = table.Column<Guid>(type: "TEXT", nullable: true)
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
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "members",
            schema: "public");
    }
}
