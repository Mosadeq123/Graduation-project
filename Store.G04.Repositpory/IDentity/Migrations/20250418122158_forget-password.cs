﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.G04.Repositpory.Identity.Migrations
{
    /// <inheritdoc />
    public partial class forgetpassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmailConfirmResetCode",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EmailConfirmResetCodeExpiry",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailConfirmResetCode",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmailConfirmResetCodeExpiry",
                table: "AspNetUsers");
        }
    }
}
