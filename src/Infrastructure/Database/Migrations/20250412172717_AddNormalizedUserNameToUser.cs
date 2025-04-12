using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations;

/// <inheritdoc />
public partial class AddNormalizedUserNameToUser : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "ix_users_user_name",
            schema: "public",
            table: "users");

        migrationBuilder.AddColumn<string>(
            name: "normalized_user_name",
            schema: "public",
            table: "users",
            type: "TEXT",
            nullable: false,
            defaultValue: "");

        migrationBuilder.CreateIndex(
            name: "ix_users_normalized_user_name",
            schema: "public",
            table: "users",
            column: "normalized_user_name");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "ix_users_normalized_user_name",
            schema: "public",
            table: "users");

        migrationBuilder.DropColumn(
            name: "normalized_user_name",
            schema: "public",
            table: "users");

        migrationBuilder.CreateIndex(
            name: "ix_users_user_name",
            schema: "public",
            table: "users",
            column: "user_name",
            unique: true);
    }
}
