using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace uServiceDemo.Infrastructure.Migrations
{
    public partial class AddWindEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WindId",
                table: "WeatherForecast",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Wind",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Speed = table.Column<long>(type: "bigint", nullable: false),
                    Direction = table.Column<int>(type: "integer", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "timezone('UTC', now())"),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "timezone('UTC', now())"),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    Version = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wind", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeatherForecast_WindId",
                table: "WeatherForecast",
                column: "WindId");

            migrationBuilder.AddForeignKey(
                name: "FK_WeatherForecast_Wind_WindId",
                table: "WeatherForecast",
                column: "WindId",
                principalTable: "Wind",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeatherForecast_Wind_WindId",
                table: "WeatherForecast");

            migrationBuilder.DropTable(
                name: "Wind");

            migrationBuilder.DropIndex(
                name: "IX_WeatherForecast_WindId",
                table: "WeatherForecast");

            migrationBuilder.DropColumn(
                name: "WindId",
                table: "WeatherForecast");
        }
    }
}
