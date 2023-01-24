using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sample.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class IsFreshlyPreparedFood : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFreshlyPrepared",
                table: "Foods",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFreshlyPrepared",
                table: "Foods");
        }
    }
}
