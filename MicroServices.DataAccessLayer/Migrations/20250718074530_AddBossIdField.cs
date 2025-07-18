﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroServices.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddBossIdField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BossId",
                table: "Employees",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BossId",
                table: "Employees");
        }
    }
}
