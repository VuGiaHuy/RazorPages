using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace practice2.Migrations
{
    /// <inheritdoc />
    public partial class SeedUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            for(int i=150; i<160; i++)
            {
                migrationBuilder.InsertData(
                    table: "Users",
                    columns: new []{"Id",
                                    "UserName",
                                    "NormalizedUserName",
                                    "Email",
                                    "NormalizedEmail",
                                    "EmailConfirmed",
                                    "SecurityStamp",
                                    "PhoneNumber",
                                    "PhoneNumberConfirmed",
                                    "TwoFactorEnabled",
                                    "LockoutEnd",
                                    "LockoutEnabled",
                                    "AccessFailedCount"
                                    },
                    values: new object[]{
                        Guid.NewGuid().ToString(),
                        "User"+i.ToString("D3"),
                        "user"+i.ToString("D3"),
                        "user"+i.ToString()+"@gmail.com",
                        "user"+i.ToString()+"@gmail.com",
                        true,
                        Guid.NewGuid().ToString(),
                        i.ToString(),
                        false,
                        false,
                        new DateTimeOffset(),
                        false,
                        2
                    }
                );
            }
        }
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
