using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InDuckTor.Auth.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "auth",
                table: "Credentials");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserId",
                schema: "auth",
                table: "Credentials",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
