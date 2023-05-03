using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bulky.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StreetAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Zipcode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companis", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Companis",
                columns: new[] { "Id", "City", "Country", "Name", "PhoneNumber", "StreetAddress", "Zipcode" },
                values: new object[,]
                {
                    { 1, "New York", "New York", "MetroMind", "+1 202-918-2132", "237-205 E 12th St", "10003" },
                    { 2, "Hamtramck", "Michigan", "OptiTrip", "+1 505-554-9438", "2475 Grayling St", "48212" },
                    { 3, "München", "Germany", "EliteTransit", "+49 170 77261952", "Klenzestraße 18", "80469 " }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Companis");
        }
    }
}
