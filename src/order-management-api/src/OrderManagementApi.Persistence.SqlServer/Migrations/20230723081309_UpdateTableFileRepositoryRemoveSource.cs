using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagementApi.Persistence.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableFileRepositoryRemoveSource : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Source",
                table: "FileRepository");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "FileRepository",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);
        }
    }
}
