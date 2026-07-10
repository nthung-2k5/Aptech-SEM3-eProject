using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GiveAID.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDescriptionFromPartner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "CorporatePartners");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "CorporatePartners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
