using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations;

/// <inheritdoc />
public partial class AddWorkspaceBlamableColumns : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "created_by_id",
            schema: "public",
            table: "workspaces",
            type: "TEXT",
            nullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "created_on_utc",
            schema: "public",
            table: "workspaces",
            type: "TEXT",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "deleted",
            schema: "public",
            table: "workspaces",
            type: "INTEGER",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<Guid>(
            name: "deleted_by_id",
            schema: "public",
            table: "workspaces",
            type: "TEXT",
            nullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "deleted_on_utc",
            schema: "public",
            table: "workspaces",
            type: "TEXT",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "modified_by_id",
            schema: "public",
            table: "workspaces",
            type: "TEXT",
            nullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "modified_on_utc",
            schema: "public",
            table: "workspaces",
            type: "TEXT",
            nullable: true);

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

        migrationBuilder.AddForeignKey(
            name: "fk_workspaces_users_created_by_id",
            schema: "public",
            table: "workspaces",
            column: "created_by_id",
            principalSchema: "public",
            principalTable: "users",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "fk_workspaces_users_deleted_by_id",
            schema: "public",
            table: "workspaces",
            column: "deleted_by_id",
            principalSchema: "public",
            principalTable: "users",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "fk_workspaces_users_modified_by_id",
            schema: "public",
            table: "workspaces",
            column: "modified_by_id",
            principalSchema: "public",
            principalTable: "users",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "fk_workspaces_users_created_by_id",
            schema: "public",
            table: "workspaces");

        migrationBuilder.DropForeignKey(
            name: "fk_workspaces_users_deleted_by_id",
            schema: "public",
            table: "workspaces");

        migrationBuilder.DropForeignKey(
            name: "fk_workspaces_users_modified_by_id",
            schema: "public",
            table: "workspaces");

        migrationBuilder.DropIndex(
            name: "ix_workspaces_created_by_id",
            schema: "public",
            table: "workspaces");

        migrationBuilder.DropIndex(
            name: "ix_workspaces_deleted_by_id",
            schema: "public",
            table: "workspaces");

        migrationBuilder.DropIndex(
            name: "ix_workspaces_modified_by_id",
            schema: "public",
            table: "workspaces");

        migrationBuilder.DropColumn(
            name: "created_by_id",
            schema: "public",
            table: "workspaces");

        migrationBuilder.DropColumn(
            name: "created_on_utc",
            schema: "public",
            table: "workspaces");

        migrationBuilder.DropColumn(
            name: "deleted",
            schema: "public",
            table: "workspaces");

        migrationBuilder.DropColumn(
            name: "deleted_by_id",
            schema: "public",
            table: "workspaces");

        migrationBuilder.DropColumn(
            name: "deleted_on_utc",
            schema: "public",
            table: "workspaces");

        migrationBuilder.DropColumn(
            name: "modified_by_id",
            schema: "public",
            table: "workspaces");

        migrationBuilder.DropColumn(
            name: "modified_on_utc",
            schema: "public",
            table: "workspaces");
    }
}
