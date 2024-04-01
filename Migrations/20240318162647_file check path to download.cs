using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResearchCommunityPlatform.Migrations
{
    public partial class filecheckpathtodownload : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "6b6a8d38-2b67-428c-851d-aff9213382bd", "45446d33-bb88-41f5-b386-75ce9cbfce69" });

            migrationBuilder.UpdateData(
                table: "Files",
                keyColumn: "FileID",
                keyValue: 1,
                columns: new[] { "FileName", "FilePath" },
                values: new object[] { "user1pub001.pdf", "/Files/user1pub001.pdf" });

            migrationBuilder.UpdateData(
                table: "Publications",
                keyColumn: "PublicationId",
                keyValue: 1,
                column: "UploadDate",
                value: "2024-03-18");

            migrationBuilder.UpdateData(
                table: "Publications",
                keyColumn: "PublicationId",
                keyValue: 2,
                column: "UploadDate",
                value: "2024-03-18");

            migrationBuilder.UpdateData(
                table: "Publications",
                keyColumn: "PublicationId",
                keyValue: 3,
                column: "UploadDate",
                value: "2024-03-18");

            migrationBuilder.UpdateData(
                table: "Publications",
                keyColumn: "PublicationId",
                keyValue: 4,
                column: "UploadDate",
                value: "2024-03-18");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "13a38417-e268-4259-9348-2f8e6d5acf51", "464e0f5e-eac6-4fc6-9d88-d1da7baf2e72" });

            migrationBuilder.UpdateData(
                table: "Files",
                keyColumn: "FileID",
                keyValue: 1,
                columns: new[] { "FileName", "FilePath" },
                values: new object[] { "pub-1.pdf", "/Files/pub-1.pdf" });

            migrationBuilder.UpdateData(
                table: "Publications",
                keyColumn: "PublicationId",
                keyValue: 1,
                column: "UploadDate",
                value: "2024-03-17");

            migrationBuilder.UpdateData(
                table: "Publications",
                keyColumn: "PublicationId",
                keyValue: 2,
                column: "UploadDate",
                value: "2024-03-17");

            migrationBuilder.UpdateData(
                table: "Publications",
                keyColumn: "PublicationId",
                keyValue: 3,
                column: "UploadDate",
                value: "2024-03-17");

            migrationBuilder.UpdateData(
                table: "Publications",
                keyColumn: "PublicationId",
                keyValue: 4,
                column: "UploadDate",
                value: "2024-03-17");
        }
    }
}
