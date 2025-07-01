using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscountCodeGenerator.Db.Migrations
{
    /// <inheritdoc />
    public partial class AddRowVersionToDiscountCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "DiscountCodes",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "DiscountCodes");
        }
    }
}
