using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bulky.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class editPriceType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price50",
                table: "Products",
                type: "numeric(18,3)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float(3)",
                oldPrecision: 3);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price100",
                table: "Products",
                type: "numeric(18,3)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float(3)",
                oldPrecision: 3);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "numeric(18,3)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float(3)",
                oldPrecision: 3);

            migrationBuilder.AlterColumn<decimal>(
                name: "ListPrice",
                table: "Products",
                type: "numeric(18,3)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float(3)",
                oldPrecision: 3);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ListPrice", "Price", "Price100", "Price50" },
                values: new object[] { 99.99m, 97.99m, 84.99m, 89.99m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ListPrice", "Price", "Price100", "Price50" },
                values: new object[] { 79.99m, 78.39m, 67.99m, 71.99m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ListPrice", "Price", "Price100", "Price50" },
                values: new object[] { 129.99m, 127.39m, 110.49m, 116.99m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ListPrice", "Price", "Price100", "Price50" },
                values: new object[] { 59.99m, 58.79m, 50.99m, 53.99m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ListPrice", "Price", "Price100", "Price50" },
                values: new object[] { 99.99m, 97.99m, 84.99m, 89.99m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ListPrice", "Price", "Price100", "Price50" },
                values: new object[] { 79.99m, 78.39m, 67.99m, 71.99m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "ListPrice", "Price", "Price100", "Price50" },
                values: new object[] { 129.99m, 127.39m, 110.49m, 116.99m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "ListPrice", "Price", "Price100", "Price50" },
                values: new object[] { 59.99m, 58.79m, 50.99m, 53.99m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Price50",
                table: "Products",
                type: "float(3)",
                precision: 3,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,3)");

            migrationBuilder.AlterColumn<double>(
                name: "Price100",
                table: "Products",
                type: "float(3)",
                precision: 3,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,3)");

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Products",
                type: "float(3)",
                precision: 3,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,3)");

            migrationBuilder.AlterColumn<double>(
                name: "ListPrice",
                table: "Products",
                type: "float(3)",
                precision: 3,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,3)");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ListPrice", "Price", "Price100", "Price50" },
                values: new object[] { 99.989999999999995, 97.989999999999995, 84.989999999999995, 89.989999999999995 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ListPrice", "Price", "Price100", "Price50" },
                values: new object[] { 79.989999999999995, 78.390000000000001, 67.989999999999995, 71.989999999999995 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ListPrice", "Price", "Price100", "Price50" },
                values: new object[] { 129.99000000000001, 127.39, 110.48999999999999, 116.98999999999999 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ListPrice", "Price", "Price100", "Price50" },
                values: new object[] { 59.990000000000002, 58.789999999999999, 50.990000000000002, 53.990000000000002 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ListPrice", "Price", "Price100", "Price50" },
                values: new object[] { 99.989999999999995, 97.989999999999995, 84.989999999999995, 89.989999999999995 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ListPrice", "Price", "Price100", "Price50" },
                values: new object[] { 79.989999999999995, 78.390000000000001, 67.989999999999995, 71.989999999999995 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "ListPrice", "Price", "Price100", "Price50" },
                values: new object[] { 129.99000000000001, 127.39, 110.48999999999999, 116.98999999999999 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "ListPrice", "Price", "Price100", "Price50" },
                values: new object[] { 59.990000000000002, 58.789999999999999, 50.990000000000002, 53.990000000000002 });
        }
    }
}
