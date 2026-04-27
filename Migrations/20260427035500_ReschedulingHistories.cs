using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestorDeVuelosProyectoFinal.Migrations
{
    /// <inheritdoc />
    public partial class ReschedulingHistories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_addresses_cities_CityId1",
                table: "addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_addresses_street_types_StreetTypeId1",
                table: "addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_cities_regions_RegionId1",
                table: "cities");

            migrationBuilder.DropIndex(
                name: "IX_cities_RegionId1",
                table: "cities");

            migrationBuilder.DropIndex(
                name: "IX_addresses_CityId1",
                table: "addresses");

            migrationBuilder.DropIndex(
                name: "IX_addresses_StreetTypeId1",
                table: "addresses");

            migrationBuilder.CreateTable(
                name: "flight_rescheduling_history",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    booking_id = table.Column<int>(type: "int", nullable: false),
                    previous_flight_id = table.Column<int>(type: "int", nullable: false),
                    new_flight_id = table.Column<int>(type: "int", nullable: false),
                    changed_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    reason = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BookingEntityId = table.Column<int>(type: "int", nullable: true),
                    FlightEntityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_flight_rescheduling_history", x => x.id);
                    table.ForeignKey(
                        name: "FK_flight_rescheduling_history_bookings_BookingEntityId",
                        column: x => x.BookingEntityId,
                        principalTable: "bookings",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_flight_rescheduling_history_flights_FlightEntityId",
                        column: x => x.FlightEntityId,
                        principalTable: "flights",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_rescheduling_booking",
                        column: x => x.booking_id,
                        principalTable: "bookings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_rescheduling_new_flight",
                        column: x => x.new_flight_id,
                        principalTable: "flights",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_rescheduling_previous_flight",
                        column: x => x.previous_flight_id,
                        principalTable: "flights",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "waiting_list",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    booking_id = table.Column<int>(type: "int", nullable: false),
                    flight_id = table.Column<int>(type: "int", nullable: false),
                    requested_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    priority = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_waiting_list", x => x.id);
                    table.ForeignKey(
                        name: "FK_waiting_list_bookings_booking_id",
                        column: x => x.booking_id,
                        principalTable: "bookings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_waiting_list_flights_flight_id",
                        column: x => x.flight_id,
                        principalTable: "flights",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_flight_rescheduling_history_booking_id",
                table: "flight_rescheduling_history",
                column: "booking_id");

            migrationBuilder.CreateIndex(
                name: "IX_flight_rescheduling_history_BookingEntityId",
                table: "flight_rescheduling_history",
                column: "BookingEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_flight_rescheduling_history_FlightEntityId",
                table: "flight_rescheduling_history",
                column: "FlightEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_flight_rescheduling_history_new_flight_id",
                table: "flight_rescheduling_history",
                column: "new_flight_id");

            migrationBuilder.CreateIndex(
                name: "IX_flight_rescheduling_history_previous_flight_id",
                table: "flight_rescheduling_history",
                column: "previous_flight_id");

            migrationBuilder.CreateIndex(
                name: "IX_waiting_list_booking_id",
                table: "waiting_list",
                column: "booking_id");

            migrationBuilder.CreateIndex(
                name: "IX_waiting_list_booking_id_flight_id",
                table: "waiting_list",
                columns: new[] { "booking_id", "flight_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_waiting_list_flight_id",
                table: "waiting_list",
                column: "flight_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "flight_rescheduling_history");

            migrationBuilder.DropTable(
                name: "waiting_list");

            migrationBuilder.CreateIndex(
                name: "IX_cities_RegionId1",
                table: "cities",
                column: "RegionId1");

            migrationBuilder.CreateIndex(
                name: "IX_addresses_CityId1",
                table: "addresses",
                column: "CityId1");

            migrationBuilder.CreateIndex(
                name: "IX_addresses_StreetTypeId1",
                table: "addresses",
                column: "StreetTypeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_addresses_cities_CityId1",
                table: "addresses",
                column: "CityId1",
                principalTable: "cities",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_addresses_street_types_StreetTypeId1",
                table: "addresses",
                column: "StreetTypeId1",
                principalTable: "street_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_cities_regions_RegionId1",
                table: "cities",
                column: "RegionId1",
                principalTable: "regions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
