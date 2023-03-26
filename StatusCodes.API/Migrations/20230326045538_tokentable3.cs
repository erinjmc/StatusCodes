using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StatusCodes.API.Migrations
{
    /// <inheritdoc />
    public partial class tokentable3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Tokens",
                newName: "Email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Tokens",
                newName: "Username");
        }
    }
}
