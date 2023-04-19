using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bulky.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddProductTODB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ListPrice = table.Column<double>(type: "float(3)", precision: 3, nullable: false),
                    Price = table.Column<double>(type: "float(3)", precision: 3, nullable: false),
                    Price50 = table.Column<double>(type: "float(3)", precision: 3, nullable: false),
                    Price100 = table.Column<double>(type: "float(3)", precision: 3, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Author", "CategoryId", "Description", "ISBN", "ListPrice", "Price", "Price100", "Price50", "Title" },
                values: new object[,]
                {
                    { 1, "J.D. Salinger", 1, "A novel about a teenage boy's experiences and thoughts in the days following his expulsion from a prestigious prep school.", "0316769177", 99.989999999999995, 97.989999999999995, 84.989999999999995, 89.989999999999995, "The Catcher in the Rye" },
                    { 2, "Harper Lee", 2, "A novel set in the American South during the 1930s, dealing with racial injustice and moral growth through the eyes of a young girl.", "0060935464", 79.989999999999995, 78.390000000000001, 67.989999999999995, 71.989999999999995, "To Kill a Mockingbird" },
                    { 3, "F. Scott Fitzgerald", 1, "A novel depicting the lavish and decadent lifestyle of the wealthy elite in the Roaring Twenties, and the disillusionment and emptiness that lies beneath.", "0743273567", 129.99000000000001, 127.39, 110.48999999999999, 116.98999999999999, "The Great Gatsby" },
                    { 4, "Jane Austen", 3, "A classic romance novel about the Bennet family and their five daughters, particularly the headstrong Elizabeth Bennet and her budding relationship with the wealthy Mr. Darcy.", "0141439513", 59.990000000000002, 58.789999999999999, 50.990000000000002, 53.990000000000002, "Pride and Prejudice" },
                    { 5, "Aria Blackwell", 2, "A thrilling space opera about a group of explorers who embark on a dangerous journey to uncover the truth behind an ancient alien artifact.", "9781534453542", 99.989999999999995, 97.989999999999995, 84.989999999999995, 89.989999999999995, "The Stars Beyond" },
                    { 6, "Xander Steele", 2, "A gripping tale of a lone starship captain who must navigate through treacherous space battles and unravel a conspiracy that threatens the fate of the galaxy.", "9781982143463", 79.989999999999995, 78.390000000000001, 67.989999999999995, 71.989999999999995, "Nebula's Edge" },
                    { 7, "Zara Nova", 4, "A mind-bending story of time travel and alternate realities, where the fate of humanity hangs in the balance as a group of unlikely heroes race against time to save the future.", "9781728314557", 129.99000000000001, 127.39, 110.48999999999999, 116.98999999999999, "Time Warp Chronicles" },
                    { 8, "Raven Cross", 4, "A gritty cyberpunk tale set in a dystopian future, where a lone hacker fights against corrupt corporations and ruthless mercenaries in a battle for survival.", "9781954893208", 59.990000000000002, 58.789999999999999, 50.990000000000002, 53.990000000000002, "Cyberpunk Revolution" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(40)",
                oldMaxLength: 40);
        }
    }
}
