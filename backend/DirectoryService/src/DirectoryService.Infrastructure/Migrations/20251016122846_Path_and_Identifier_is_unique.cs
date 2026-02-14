using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DirectoryService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Path_and_Identifier_is_unique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_departments_identifier",
                table: "departments",
                column: "identifier",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_departments_path",
                table: "departments",
                column: "path",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_departments_identifier",
                table: "departments");

            migrationBuilder.DropIndex(
                name: "ix_departments_path",
                table: "departments");
        }
    }
}
