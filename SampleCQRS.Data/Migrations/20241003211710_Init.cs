using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SampleCQRS.Data.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Humans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    MiddleName = table.Column<string>(type: "text", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Humans", x => x.Id);
                });

            migrationBuilder.InsertData(table: "Humans",
                                        columns: ["Id", "LastName", "FirstName", "MiddleName", "DateOfBirth"],
                                        values: new object[,]
                                        {
                                            { Guid.NewGuid(), "Иванов", "Иван", "Иванович", new DateTime(1980, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                                            { Guid.NewGuid(), "Петров", "Петр", "Петрович", new DateTime(1985, 5, 15, 0, 0, 0, DateTimeKind.Utc) },
                                            { Guid.NewGuid(), "Сидоров", "Сидор", "Сидорович", new DateTime(1990, 10, 20, 0, 0, 0, DateTimeKind.Utc) }
                                        });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Humans");
        }
    }
}
