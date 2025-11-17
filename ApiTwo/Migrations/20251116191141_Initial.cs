using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiTwo.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaskInputs",
                columns: table => new
                {
                    TaskId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isComplete = table.Column<bool>(type: "bit", nullable: false),
                    DataCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskInputs", x => x.TaskId);
                });

            migrationBuilder.CreateTable(
                name: "TaskClients",
                columns: table => new
                {
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    TaskId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskClients", x => new { x.ClientId, x.TaskId });
                    table.ForeignKey(
                        name: "FK_TaskClients_TaskInputs_TaskId",
                        column: x => x.TaskId,
                        principalTable: "TaskInputs",
                        principalColumn: "TaskId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskClients_TaskId",
                table: "TaskClients",
                column: "TaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskClients");

            migrationBuilder.DropTable(
                name: "TaskInputs");
        }
    }
}
