using GiaHuy.Models;
using System;
using Bogus;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace practice2.Migrations
{
    /// <inheritdoc />
    public partial class initdb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "sinhVien",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BirthDay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Address = table.Column<string>(type: "ntext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sinhVien", x => x.Id);
                });
            
            Randomizer.Seed = new Random(8675309);
            var faker = new Faker<SinhVien>();
            faker.RuleFor(sinhvien=>sinhvien.Name, faker=>faker.Lorem.Sentence(6,20));
            faker.RuleFor(sinhvien=>sinhvien.BirthDay, faker=>faker.Date.Between(new DateTime(2000,1,1), new DateTime(2008,1,1)));
            faker.RuleFor(Sinhvien=>Sinhvien.Address,faker=>faker.Lorem.Sentence());
            for(int  i=0; i<60; i++)
            {
                SinhVien sinhVien = faker.Generate();
                migrationBuilder.InsertData(
                    table: "sinhVien",
                    columns: new[]{"Name","BirthDay","Address"},
                    values: new object[]{
                        sinhVien.Name,
                        sinhVien.BirthDay,
                        sinhVien.Address
                    }
                );
            }
                        
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sinhVien");
        }
    }
}

