using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResearchCommunityPlatform.Migrations
{
    public partial class testAUthorizationREsourceBased : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "60ed6041-cee0-4e59-8116-b31276b35d9f", "ae7c2ab6-3b13-4a50-8178-86d5fab30bec" });

            migrationBuilder.UpdateData(
                table: "Publications",
                keyColumn: "PublicationId",
                keyValue: 1,
                column: "UploadDate",
                value: "2024-05-15");

            migrationBuilder.UpdateData(
                table: "Publications",
                keyColumn: "PublicationId",
                keyValue: 2,
                column: "UploadDate",
                value: "2024-05-15");

            migrationBuilder.UpdateData(
                table: "Publications",
                keyColumn: "PublicationId",
                keyValue: 3,
                column: "UploadDate",
                value: "2024-05-15");

            migrationBuilder.UpdateData(
                table: "Publications",
                keyColumn: "PublicationId",
                keyValue: 4,
                column: "UploadDate",
                value: "2024-05-15");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "bb7d0cd2-cb6e-401b-91d4-e55064aec30a", "1b552e92-e528-4795-bd15-6ca1be72371a" });

            migrationBuilder.UpdateData(
                table: "Publications",
                keyColumn: "PublicationId",
                keyValue: 1,
                column: "UploadDate",
                value: "2024-05-02");

            migrationBuilder.UpdateData(
                table: "Publications",
                keyColumn: "PublicationId",
                keyValue: 2,
                column: "UploadDate",
                value: "2024-05-02");

            migrationBuilder.UpdateData(
                table: "Publications",
                keyColumn: "PublicationId",
                keyValue: 3,
                column: "UploadDate",
                value: "2024-05-02");

            migrationBuilder.UpdateData(
                table: "Publications",
                keyColumn: "PublicationId",
                keyValue: 4,
                column: "UploadDate",
                value: "2024-05-02");
        }
    }
}
