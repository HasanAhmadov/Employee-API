using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroServices.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class RenewTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendances");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeShiftId = table.Column<int>(type: "int", nullable: false),
                    EarliestEnterTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LatestExitTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MinutesLate = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attendances_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Attendances_Shifts_EmployeeShiftId",
                        column: x => x.EmployeeShiftId,
                        principalTable: "Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_EmployeeId",
                table: "Attendances",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_EmployeeShiftId",
                table: "Attendances",
                column: "EmployeeShiftId");
        }
    }
}
