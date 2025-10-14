using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DirectoryService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Names_and_address_is_unique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_positions_name",
                table: "positions",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_locations_name",
                table: "locations",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_departments_name",
                table: "departments",
                column: "name",
                unique: true);

            migrationBuilder.Sql(@"
                CREATE UNIQUE INDEX ix_locations_address 
                ON locations (address)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_positions_name",
                table: "positions");

            migrationBuilder.DropIndex(
                name: "ix_locations_name",
                table: "locations");

            migrationBuilder.DropIndex(
                name: "ix_departments_name",
                table: "departments");

            migrationBuilder.DropIndex(
                name: "ix_locations_address",
                table: "locations");
        }
    }
}
