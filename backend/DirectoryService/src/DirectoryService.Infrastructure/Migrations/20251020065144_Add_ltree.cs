using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DirectoryService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_ltree : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_departments_path",
                table: "departments");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:ltree", ",,");

            migrationBuilder.Sql(@"ALTER TABLE departments ALTER COLUMN path TYPE ltree USING path::ltree");

            migrationBuilder.CreateIndex(
                name: "ix_departments_path",
                table: "departments",
                column: "path")
                .Annotation("Npgsql:IndexMethod", "gist");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_departments_path",
                table: "departments");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:PostgresExtension:ltree", ",,");

            migrationBuilder.Sql(@"ALTER TABLE departments ALTER COLUMN path TYPE text USING path::text");

            migrationBuilder.CreateIndex(
                name: "ix_departments_path",
                table: "departments",
                column: "path",
                unique: true);
        }
    }
}
