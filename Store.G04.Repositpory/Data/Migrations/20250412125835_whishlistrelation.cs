using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.G04.Repositpory.Data.Migrations
{
    /// <inheritdoc />
    public partial class whishlistrelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_MachineId",
                table: "Wishlists",
                column: "MachineId");

            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_MaterialId",
                table: "Wishlists",
                column: "MaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlists_Machine_MachineId",
                table: "Wishlists",
                column: "MachineId",
                principalTable: "Machine",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlists_RawMaterial_MaterialId",
                table: "Wishlists",
                column: "MaterialId",
                principalTable: "RawMaterial",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wishlists_Machine_MachineId",
                table: "Wishlists");

            migrationBuilder.DropForeignKey(
                name: "FK_Wishlists_RawMaterial_MaterialId",
                table: "Wishlists");

            migrationBuilder.DropIndex(
                name: "IX_Wishlists_MachineId",
                table: "Wishlists");

            migrationBuilder.DropIndex(
                name: "IX_Wishlists_MaterialId",
                table: "Wishlists");
        }
    }
}
