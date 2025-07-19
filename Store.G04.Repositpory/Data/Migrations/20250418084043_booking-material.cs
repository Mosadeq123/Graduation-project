using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.G04.Repositpory.Data.Migrations
{
    /// <inheritdoc />
    public partial class bookingmaterial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookingMaterials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    Qantity = table.Column<int>(type: "int", nullable: false),
                    StitchLength = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MachineId = table.Column<int>(type: "int", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingMaterials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingMaterials_Machine_MachineId",
                        column: x => x.MachineId,
                        principalTable: "Machine",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BookingMaterials_RawMaterial_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "RawMaterial",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingMaterials_MachineId",
                table: "BookingMaterials",
                column: "MachineId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingMaterials_MaterialId",
                table: "BookingMaterials",
                column: "MaterialId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingMaterials");
        }
    }
}
