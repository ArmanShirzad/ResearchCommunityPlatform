using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResearchCommunityPlatform.Migrations
{
    public partial class correctedfilepath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "5ce64c9b-3bad-4fc9-8cd0-fdeb0ee2273c", "65d5eef3-a77f-4227-b9a3-aaa827b9a364" });

            migrationBuilder.UpdateData(
                table: "Files",
                keyColumn: "FileID",
                keyValue: 1,
                column: "FilePath",
                value: "/Files/pub-1.pdf");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "d3b6855d-f08e-4c06-8040-13bf1ab6226c", "90b0b919-a5f2-42fe-bbeb-abbca1647c08" });

            migrationBuilder.UpdateData(
                table: "Files",
                keyColumn: "FileID",
                keyValue: 1,
                column: "FilePath",
                value: "~\\Files\\pub-1.pdf");
        }
    }
}
