using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnitOfWorkDesignPattern.Migrations
{
    /// <inheritdoc />
    public partial class Seed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id" },
                values: new object[,]
                {
                    {1L}, 
                    {2L}, 
                    {3L}
                }
            );
            
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Name", "Description" },
                values: new object[,]
                {
                    {"Product 1", "Description 1"}, 
                    {"Product 2", "Description 2"}, 
                    {"Product 3", "Description 3"},
                    {"Product 4", "Description 4"},
                    {"Product 5", "Description 5"},
                    {"Product 6", "Description 6"}
                }
            );  
            
            migrationBuilder.InsertData(
                table: "OrderProducts",
                columns: new[] { "OrderId", "ProductId" },
                values: new object[,]
                {
                    {1L, 1L}, 
                    {1L, 2L}, 
                    {2L, 3L},
                    {2L, 4L},
                    {3L, 5L},
                    {3L, 6L}
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OrderProducts",
                keyColumn: "OrderId",
                keyValue: 1L
            );
            
            migrationBuilder.DeleteData(
                table: "OrderProducts",
                keyColumn: "OrderId",
                keyValue: 2L
            );
            
            migrationBuilder.DeleteData(
                table: "OrderProducts",
                keyColumn: "OrderId",
                keyValue: 3L
            );
            
            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1L
            );
            
            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 2L
            );
            
            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 3L
            );
            
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1L
            );
            
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2L
            );
            
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3L
            );
            
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4L
            );
            
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5L
            );
            
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6L
            );
        }
    }
}
