using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResearchCommunityPlatform.Migrations
{
    public partial class fourthpublication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "13a38417-e268-4259-9348-2f8e6d5acf51", "464e0f5e-eac6-4fc6-9d88-d1da7baf2e72" });

            migrationBuilder.InsertData(
                table: "Publications",
                columns: new[] { "PublicationId", "AuthorsSerialized", "DOI", "DateOfPublish", "Description", "PubType", "Title", "UploadDate", "UserId" },
                values: new object[] { 4, "[\"Sara Rosenbaum\"]", "https://doi.org/10.1111/j.1475-6773.2010.01140.x", "2010-08-02", "U.S. health policy is engaged in a struggle over access to health information...", "journal", "Data Governance and Stewardship: Designing Data Stewardship Entities and Advancing Data Access", "2024-03-17", "1" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Publications",
                keyColumn: "PublicationId",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "5ce64c9b-3bad-4fc9-8cd0-fdeb0ee2273c", "65d5eef3-a77f-4227-b9a3-aaa827b9a364" });
        }
    }
}
