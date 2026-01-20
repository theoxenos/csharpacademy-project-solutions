using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WardrobeInventory.Blazor.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WardrobeItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Brand = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Category = table.Column<int>(type: "INTEGER", nullable: false),
                    Size = table.Column<int>(type: "INTEGER", nullable: false),
                    ImageData = table.Column<byte[]>(type: "BLOB", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WardrobeItems", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "WardrobeItems",
                columns: new[] { "Id", "Brand", "Category", "ImageData", "Name", "Size" },
                values: new object[,]
                {
                    { 1, "BrandA", 0, null, "Shirt", 0 },
                    { 2, "BrandB", 1, null, "Pants", 1 },
                    { 3, "BrandC", 3, null, "Shoes", 2 },
                    { 4, "BrandD", 2, null, "Dress", 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WardrobeItems");
        }
    }
}
