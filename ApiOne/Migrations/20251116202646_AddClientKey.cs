using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiOne.Migrations
{
    /// <inheritdoc />
    public partial class AddClientKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ClientInputs",
                newName: "ClientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "ClientInputs",
                newName: "Id");
        }
    }
}
