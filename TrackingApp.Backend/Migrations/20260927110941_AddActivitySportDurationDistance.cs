using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackingApp.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddActivitySportDurationDistance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DistanceKilometers",
                table: "Activities",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DurationMinutes",
                table: "Activities",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sport",
                table: "Activities",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "Other");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DistanceKilometers",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "DurationMinutes",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "Sport",
                table: "Activities");
        }
    }
}
