using Microsoft.EntityFrameworkCore.Migrations;

namespace UsersApi.Migrations
{
    public partial class PermitionsAdm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.InsertData(table: "AspNetUserClaims",
                                        columns: new string[]
                                        {
                                            "Id",
                                            "UserId",
                                            "ClaimType",
                                            "ClaimValue"
                                        },
                                        values: new object[]
                                        {
                                        1,
                                        "7a98e5a2-de61-4803-ab57-5a432742549d",
                                        "UsersAdm",
                                        "1"
                                        });

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(table: "AspNetUserClaims", keyColumn: "Id", keyValue: 1);
        }
    }
}
