using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestorDeVuelosProyectoFinal.Migrations
{
    /// <inheritdoc />
    public partial class Continents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "aircraft_manufacturers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    country = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aircraft_manufacturers", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "availability_statuses",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_availability_statuses", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "baggage_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    max_weight_kg = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    base_price = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_baggage_types", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "booking_statuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_booking_statuses", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "cabin_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cabin_types", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "card_issuers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    issuer_number = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_card_issuers", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "card_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_card_types", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "checkin_states",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_checkin_states", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "continents",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_continents", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "document_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    code = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_document_types", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "email_domains",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    domain = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_email_domains", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "flight_crew_roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_flight_crew_roles", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "flight_statuses",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_flight_statuses", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "invoice_item_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_invoice_item_types", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "passenger_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    min_age = table.Column<int>(type: "int", nullable: true),
                    max_age = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_passenger_types", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "payment_method_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_method_types", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "payment_statuses",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_statuses", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "permissions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permissions", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "phone_codes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    country_code = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    country_name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_phone_codes", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "seasons",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    price_factor = table.Column<decimal>(type: "decimal(5,4)", nullable: false, defaultValue: 1.0000m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seasons", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "seat_location_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seat_location_types", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "staff_positions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_staff_positions", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "street_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_street_types", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SystemRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemRoles", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ticket_states",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ticket_states", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "aircraft_models",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    manufacturer_id = table.Column<int>(type: "int", nullable: false),
                    model_name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    max_capacity = table.Column<int>(type: "int", nullable: false),
                    max_takeoff_weight_kg = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    fuel_consumption_kg_h = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    cruise_speed_kmh = table.Column<int>(type: "int", nullable: true),
                    cruise_altitude_ft = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aircraft_models", x => x.id);
                    table.ForeignKey(
                        name: "FK_aircraft_models_aircraft_manufacturers_manufacturer_id",
                        column: x => x.manufacturer_id,
                        principalTable: "aircraft_manufacturers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "booking_status_transitions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    from_status_id = table.Column<int>(type: "int", nullable: false),
                    to_status_id = table.Column<int>(type: "int", nullable: false),
                    BookingStatusesEntityId = table.Column<int>(type: "int", nullable: true),
                    BookingStatusesEntityId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_booking_status_transitions", x => x.id);
                    table.ForeignKey(
                        name: "FK_booking_status_transitions_booking_statuses_BookingStatusesE~",
                        column: x => x.BookingStatusesEntityId,
                        principalTable: "booking_statuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_booking_status_transitions_booking_statuses_BookingStatuses~1",
                        column: x => x.BookingStatusesEntityId1,
                        principalTable: "booking_statuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_booking_status_transitions_booking_statuses_from_status_id",
                        column: x => x.from_status_id,
                        principalTable: "booking_statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_booking_status_transitions_booking_statuses_to_status_id",
                        column: x => x.to_status_id,
                        principalTable: "booking_statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "countries",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    iso_code = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    continent_id = table.Column<int>(type: "int", nullable: false),
                    CountryEntityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_countries", x => x.id);
                    table.ForeignKey(
                        name: "FK_countries_continents_continent_id",
                        column: x => x.continent_id,
                        principalTable: "continents",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_countries_countries_CountryEntityId",
                        column: x => x.CountryEntityId,
                        principalTable: "countries",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "flight_status_transitions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    from_status_id = table.Column<int>(type: "int", nullable: false),
                    to_status_id = table.Column<int>(type: "int", nullable: false),
                    FlightStatusEntityId = table.Column<int>(type: "int", nullable: true),
                    FlightStatusEntityId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_flight_status_transitions", x => x.id);
                    table.ForeignKey(
                        name: "FK_flight_status_transitions_flight_statuses_FlightStatusEntity~",
                        column: x => x.FlightStatusEntityId,
                        principalTable: "flight_statuses",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_flight_status_transitions_flight_statuses_FlightStatusEntit~1",
                        column: x => x.FlightStatusEntityId1,
                        principalTable: "flight_statuses",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_flight_status_transitions_flight_statuses_from_status_id",
                        column: x => x.from_status_id,
                        principalTable: "flight_statuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_flight_status_transitions_flight_statuses_to_status_id",
                        column: x => x.to_status_id,
                        principalTable: "flight_statuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "payment_methods",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    payment_method_type_id = table.Column<int>(type: "int", nullable: false),
                    card_type_id = table.Column<int>(type: "int", nullable: true),
                    card_issuer_id = table.Column<int>(type: "int", nullable: true),
                    display_name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_methods", x => x.id);
                    table.ForeignKey(
                        name: "fk_payment_methods_card_issuers",
                        column: x => x.card_issuer_id,
                        principalTable: "card_issuers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_payment_methods_card_types",
                        column: x => x.card_type_id,
                        principalTable: "card_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_payment_methods_payment_medium_types",
                        column: x => x.payment_method_type_id,
                        principalTable: "payment_method_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "role_permissions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    role_id = table.Column<int>(type: "int", nullable: false),
                    permission_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role_permissions", x => x.id);
                    table.ForeignKey(
                        name: "FK_role_permissions_SystemRoles_role_id",
                        column: x => x.role_id,
                        principalTable: "SystemRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_role_permissions_permissions_permission_id",
                        column: x => x.permission_id,
                        principalTable: "permissions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "airlines",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    iata_code = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    origin_country_id = table.Column<int>(type: "int", nullable: false),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_airlines", x => x.id);
                    table.ForeignKey(
                        name: "FK_airlines_countries_origin_country_id",
                        column: x => x.origin_country_id,
                        principalTable: "countries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "regions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    type = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    country_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_regions", x => x.id);
                    table.ForeignKey(
                        name: "FK_regions_countries_country_id",
                        column: x => x.country_id,
                        principalTable: "countries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "aircraft",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    model_id = table.Column<int>(type: "int", nullable: false),
                    airline_id = table.Column<int>(type: "int", nullable: false),
                    registration = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    manufactured_date = table.Column<DateTime>(type: "date", nullable: true),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AirlineEntityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aircraft", x => x.id);
                    table.ForeignKey(
                        name: "FK_aircraft_aircraft_models_model_id",
                        column: x => x.model_id,
                        principalTable: "aircraft_models",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_aircraft_airlines_AirlineEntityId",
                        column: x => x.AirlineEntityId,
                        principalTable: "airlines",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_aircraft_airlines_airline_id",
                        column: x => x.airline_id,
                        principalTable: "airlines",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "cities",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    region_id = table.Column<int>(type: "int", nullable: false),
                    RegionId1 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cities", x => x.id);
                    table.ForeignKey(
                        name: "FK_cities_regions_RegionId1",
                        column: x => x.RegionId1,
                        principalTable: "regions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cities_regions_region_id",
                        column: x => x.region_id,
                        principalTable: "regions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "cabin_configurations",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    aircraft_id = table.Column<int>(type: "int", nullable: false),
                    cabin_type_id = table.Column<int>(type: "int", nullable: false),
                    row_start = table.Column<int>(type: "int", nullable: false),
                    row_end = table.Column<int>(type: "int", nullable: false),
                    seats_per_row = table.Column<int>(type: "int", nullable: false),
                    seat_letters = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AircraftEntityId = table.Column<int>(type: "int", nullable: true),
                    CabinTypeEntityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cabin_configurations", x => x.id);
                    table.ForeignKey(
                        name: "FK_cabin_configurations_aircraft_AircraftEntityId",
                        column: x => x.AircraftEntityId,
                        principalTable: "aircraft",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_cabin_configurations_cabin_types_CabinTypeEntityId",
                        column: x => x.CabinTypeEntityId,
                        principalTable: "cabin_types",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_cabin_configuration_aircraft",
                        column: x => x.aircraft_id,
                        principalTable: "aircraft",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_cabin_configuration_cabin_type",
                        column: x => x.cabin_type_id,
                        principalTable: "cabin_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "addresses",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    street_type_id = table.Column<int>(type: "int", nullable: false),
                    street_name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    number = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    complement = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    city_id = table.Column<int>(type: "int", nullable: false),
                    postal_code = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StreetTypeId1 = table.Column<int>(type: "int", nullable: false),
                    CityId1 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_addresses", x => x.id);
                    table.ForeignKey(
                        name: "FK_addresses_cities_CityId1",
                        column: x => x.CityId1,
                        principalTable: "cities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_addresses_cities_city_id",
                        column: x => x.city_id,
                        principalTable: "cities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_addresses_street_types_StreetTypeId1",
                        column: x => x.StreetTypeId1,
                        principalTable: "street_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_addresses_street_types_street_type_id",
                        column: x => x.street_type_id,
                        principalTable: "street_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "airports",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    iata_code = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    icao_code = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    city_id = table.Column<int>(type: "int", nullable: false),
                    CityEntityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_airports", x => x.id);
                    table.ForeignKey(
                        name: "FK_airports_cities_CityEntityId",
                        column: x => x.CityEntityId,
                        principalTable: "cities",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_airports_cities_city_id",
                        column: x => x.city_id,
                        principalTable: "cities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "persons",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    document_type_id = table.Column<int>(type: "int", nullable: false),
                    document_number = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    first_name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    last_name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    birth_date = table.Column<DateTime>(type: "date", nullable: true),
                    gender = table.Column<string>(type: "char(1)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    address_id = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    AddressEntityId = table.Column<int>(type: "int", nullable: true),
                    DocumentTypeEntityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_persons", x => x.id);
                    table.ForeignKey(
                        name: "FK_persons_addresses_AddressEntityId",
                        column: x => x.AddressEntityId,
                        principalTable: "addresses",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_persons_addresses_address_id",
                        column: x => x.address_id,
                        principalTable: "addresses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_persons_document_types_DocumentTypeEntityId",
                        column: x => x.DocumentTypeEntityId,
                        principalTable: "document_types",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_persons_document_types_document_type_id",
                        column: x => x.document_type_id,
                        principalTable: "document_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "airport_airline",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    airport_id = table.Column<int>(type: "int", nullable: false),
                    airline_id = table.Column<int>(type: "int", nullable: false),
                    terminal = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    start_date = table.Column<DateTime>(type: "date", nullable: false),
                    end_date = table.Column<DateTime>(type: "date", nullable: true),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AirlineEntityId = table.Column<int>(type: "int", nullable: true),
                    AirportEntityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_airport_airline", x => x.id);
                    table.ForeignKey(
                        name: "FK_airport_airline_airlines_AirlineEntityId",
                        column: x => x.AirlineEntityId,
                        principalTable: "airlines",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_airport_airline_airlines_airline_id",
                        column: x => x.airline_id,
                        principalTable: "airlines",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_airport_airline_airports_AirportEntityId",
                        column: x => x.AirportEntityId,
                        principalTable: "airports",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_airport_airline_airports_airport_id",
                        column: x => x.airport_id,
                        principalTable: "airports",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "routes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    origin_airport_id = table.Column<int>(type: "int", nullable: false),
                    destination_airport_id = table.Column<int>(type: "int", nullable: false),
                    distance_km = table.Column<int>(type: "int", nullable: true),
                    estimated_duration_min = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_routes", x => x.id);
                    table.ForeignKey(
                        name: "FK_routes_airports_destination_airport_id",
                        column: x => x.destination_airport_id,
                        principalTable: "airports",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_routes_airports_origin_airport_id",
                        column: x => x.origin_airport_id,
                        principalTable: "airports",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "clients",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    person_id = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clients", x => x.id);
                    table.ForeignKey(
                        name: "fk_clients_person",
                        column: x => x.person_id,
                        principalTable: "persons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "passengers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    person_id = table.Column<int>(type: "int", nullable: false),
                    passenger_type_id = table.Column<int>(type: "int", nullable: false),
                    PassengerTypeEntityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_passengers", x => x.id);
                    table.ForeignKey(
                        name: "FK_passengers_passenger_types_PassengerTypeEntityId",
                        column: x => x.PassengerTypeEntityId,
                        principalTable: "passenger_types",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_passengers_passenger_types_passenger_type_id",
                        column: x => x.passenger_type_id,
                        principalTable: "passenger_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_passengers_persons_person_id",
                        column: x => x.person_id,
                        principalTable: "persons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "person_emails",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    person_id = table.Column<int>(type: "int", nullable: false),
                    email_user = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email_domain_id = table.Column<int>(type: "int", nullable: false),
                    is_primary = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    EmailDomainsEntityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_person_emails", x => x.id);
                    table.ForeignKey(
                        name: "FK_person_emails_email_domains_EmailDomainsEntityId",
                        column: x => x.EmailDomainsEntityId,
                        principalTable: "email_domains",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_person_emails_email_domain",
                        column: x => x.email_domain_id,
                        principalTable: "email_domains",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_person_emails_person",
                        column: x => x.person_id,
                        principalTable: "persons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "person_phones",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    person_id = table.Column<int>(type: "int", nullable: false),
                    phone_code_id = table.Column<int>(type: "int", nullable: false),
                    phone_number = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_primary = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_person_phones", x => x.id);
                    table.ForeignKey(
                        name: "FK_person_phones_persons_person_id",
                        column: x => x.person_id,
                        principalTable: "persons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_person_phones_phone_codes_phone_code_id",
                        column: x => x.phone_code_id,
                        principalTable: "phone_codes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "staff",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    person_id = table.Column<int>(type: "int", nullable: false),
                    position_id = table.Column<int>(type: "int", nullable: false),
                    airline_id = table.Column<int>(type: "int", nullable: true),
                    airport_id = table.Column<int>(type: "int", nullable: true),
                    hire_date = table.Column<DateTime>(type: "date", nullable: false),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    AirlineEntityId = table.Column<int>(type: "int", nullable: true),
                    AirportEntityId = table.Column<int>(type: "int", nullable: true),
                    PersonalPositionEntityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_staff", x => x.id);
                    table.ForeignKey(
                        name: "FK_staff_airlines_AirlineEntityId",
                        column: x => x.AirlineEntityId,
                        principalTable: "airlines",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_staff_airlines_airline_id",
                        column: x => x.airline_id,
                        principalTable: "airlines",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_staff_airports_AirportEntityId",
                        column: x => x.AirportEntityId,
                        principalTable: "airports",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_staff_airports_airport_id",
                        column: x => x.airport_id,
                        principalTable: "airports",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_staff_persons_person_id",
                        column: x => x.person_id,
                        principalTable: "persons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_staff_staff_positions_PersonalPositionEntityId",
                        column: x => x.PersonalPositionEntityId,
                        principalTable: "staff_positions",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_staff_staff_positions_position_id",
                        column: x => x.position_id,
                        principalTable: "staff_positions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    username = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password_hash = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    person_id = table.Column<int>(type: "int", nullable: true),
                    role_id = table.Column<int>(type: "int", nullable: false),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    last_access = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SystemRolesEntityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.ForeignKey(
                        name: "FK_users_SystemRoles_SystemRolesEntityId",
                        column: x => x.SystemRolesEntityId,
                        principalTable: "SystemRoles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_users_SystemRoles_role_id",
                        column: x => x.role_id,
                        principalTable: "SystemRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_users_persons_person_id",
                        column: x => x.person_id,
                        principalTable: "persons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "fares",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    route_id = table.Column<int>(type: "int", nullable: false),
                    cabin_type_id = table.Column<int>(type: "int", nullable: false),
                    passenger_type_id = table.Column<int>(type: "int", nullable: false),
                    season_id = table.Column<int>(type: "int", nullable: false),
                    base_price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    valid_from = table.Column<DateOnly>(type: "date", nullable: true),
                    valid_until = table.Column<DateOnly>(type: "date", nullable: true),
                    CabinTypeEntityId = table.Column<int>(type: "int", nullable: true),
                    PassengerTypeEntityId = table.Column<int>(type: "int", nullable: true),
                    RouteEntityId = table.Column<int>(type: "int", nullable: true),
                    SeasonEntityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fares", x => x.id);
                    table.CheckConstraint("chk_base_price", "base_price >= 0");
                    table.ForeignKey(
                        name: "FK_fares_cabin_types_CabinTypeEntityId",
                        column: x => x.CabinTypeEntityId,
                        principalTable: "cabin_types",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_fares_cabin_types_cabin_type_id",
                        column: x => x.cabin_type_id,
                        principalTable: "cabin_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_fares_passenger_types_PassengerTypeEntityId",
                        column: x => x.PassengerTypeEntityId,
                        principalTable: "passenger_types",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_fares_passenger_types_passenger_type_id",
                        column: x => x.passenger_type_id,
                        principalTable: "passenger_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_fares_routes_RouteEntityId",
                        column: x => x.RouteEntityId,
                        principalTable: "routes",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_fares_routes_route_id",
                        column: x => x.route_id,
                        principalTable: "routes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_fares_seasons_SeasonEntityId",
                        column: x => x.SeasonEntityId,
                        principalTable: "seasons",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_fares_seasons_season_id",
                        column: x => x.season_id,
                        principalTable: "seasons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "flights",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    flight_code = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    airline_id = table.Column<int>(type: "int", nullable: false),
                    route_id = table.Column<int>(type: "int", nullable: false),
                    aircraft_id = table.Column<int>(type: "int", nullable: false),
                    departure_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    estimated_arrival_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    total_capacity = table.Column<int>(type: "int", nullable: false),
                    available_seats = table.Column<int>(type: "int", nullable: false),
                    flight_status_id = table.Column<int>(type: "int", nullable: false),
                    rescheduled_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    AircraftEntityId = table.Column<int>(type: "int", nullable: true),
                    AirlineEntityId = table.Column<int>(type: "int", nullable: true),
                    FlightStatusEntityId = table.Column<int>(type: "int", nullable: true),
                    RouteEntityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_flights", x => x.id);
                    table.ForeignKey(
                        name: "FK_flights_aircraft_AircraftEntityId",
                        column: x => x.AircraftEntityId,
                        principalTable: "aircraft",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_flights_aircraft_aircraft_id",
                        column: x => x.aircraft_id,
                        principalTable: "aircraft",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_flights_airlines_AirlineEntityId",
                        column: x => x.AirlineEntityId,
                        principalTable: "airlines",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_flights_airlines_airline_id",
                        column: x => x.airline_id,
                        principalTable: "airlines",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_flights_flight_statuses_FlightStatusEntityId",
                        column: x => x.FlightStatusEntityId,
                        principalTable: "flight_statuses",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_flights_flight_statuses_flight_status_id",
                        column: x => x.flight_status_id,
                        principalTable: "flight_statuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_flights_routes_RouteEntityId",
                        column: x => x.RouteEntityId,
                        principalTable: "routes",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_flights_routes_route_id",
                        column: x => x.route_id,
                        principalTable: "routes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "route_stopovers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    route_id = table.Column<int>(type: "int", nullable: false),
                    stopover_airport_id = table.Column<int>(type: "int", nullable: false),
                    stop_order = table.Column<int>(type: "int", nullable: false),
                    layover_min = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    AirportEntityId = table.Column<int>(type: "int", nullable: true),
                    RouteEntityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_route_stopovers", x => x.id);
                    table.ForeignKey(
                        name: "FK_route_stopovers_airports_AirportEntityId",
                        column: x => x.AirportEntityId,
                        principalTable: "airports",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_route_stopovers_routes_RouteEntityId",
                        column: x => x.RouteEntityId,
                        principalTable: "routes",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_route_stopovers_route",
                        column: x => x.route_id,
                        principalTable: "routes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_route_stopovers_stopover_airport",
                        column: x => x.stopover_airport_id,
                        principalTable: "airports",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "bookings",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    client_id = table.Column<int>(type: "int", nullable: false),
                    booked_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    booking_status_id = table.Column<int>(type: "int", nullable: false),
                    total_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    expires_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    BookingStatusesEntityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookings", x => x.id);
                    table.CheckConstraint("chk_total_amount", "total_amount >= 0");
                    table.ForeignKey(
                        name: "FK_bookings_booking_statuses_BookingStatusesEntityId",
                        column: x => x.BookingStatusesEntityId,
                        principalTable: "booking_statuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "fk_bookings_booking_status",
                        column: x => x.booking_status_id,
                        principalTable: "booking_statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_bookings_client",
                        column: x => x.client_id,
                        principalTable: "clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "staff_availability",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    staff_id = table.Column<int>(type: "int", nullable: false),
                    availability_status_id = table.Column<int>(type: "int", nullable: false),
                    starts_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    ends_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    notes = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AvailabilityStateEntityId = table.Column<int>(type: "int", nullable: true),
                    StaffEntityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_staff_availability", x => x.id);
                    table.CheckConstraint("chk_availability_dates", "ends_at > starts_at");
                    table.ForeignKey(
                        name: "FK_staff_availability_availability_statuses_AvailabilityStateEn~",
                        column: x => x.AvailabilityStateEntityId,
                        principalTable: "availability_statuses",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_staff_availability_availability_statuses_availability_status~",
                        column: x => x.availability_status_id,
                        principalTable: "availability_statuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_staff_availability_staff_StaffEntityId",
                        column: x => x.StaffEntityId,
                        principalTable: "staff",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_staff_availability_staff_staff_id",
                        column: x => x.staff_id,
                        principalTable: "staff",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "sessions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    started_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ended_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ip_address = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sessions", x => x.id);
                    table.ForeignKey(
                        name: "FK_sessions_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "flight_assignments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    flight_id = table.Column<int>(type: "int", nullable: false),
                    staff_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_flight_assignments", x => x.id);
                    table.ForeignKey(
                        name: "FK_flight_assignments_flights_flight_id",
                        column: x => x.flight_id,
                        principalTable: "flights",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "flight_crew_assignments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    flight_id = table.Column<int>(type: "int", nullable: false),
                    staff_id = table.Column<int>(type: "int", nullable: false),
                    flight_role_id = table.Column<int>(type: "int", nullable: false),
                    FlightRolesEntityId = table.Column<int>(type: "int", nullable: true),
                    StaffEntityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_flight_crew_assignments", x => x.id);
                    table.ForeignKey(
                        name: "FK_flight_crew_assignments_flight_crew_roles_FlightRolesEntityId",
                        column: x => x.FlightRolesEntityId,
                        principalTable: "flight_crew_roles",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_flight_crew_assignments_flight_crew_roles_flight_role_id",
                        column: x => x.flight_role_id,
                        principalTable: "flight_crew_roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_flight_crew_assignments_flights_flight_id",
                        column: x => x.flight_id,
                        principalTable: "flights",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_flight_crew_assignments_staff_StaffEntityId",
                        column: x => x.StaffEntityId,
                        principalTable: "staff",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_flight_crew_assignments_staff_staff_id",
                        column: x => x.staff_id,
                        principalTable: "staff",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "flight_seats",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    flight_id = table.Column<int>(type: "int", nullable: false),
                    code = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cabin_type_id = table.Column<int>(type: "int", nullable: false),
                    seat_location_type_id = table.Column<int>(type: "int", nullable: false),
                    is_occupied = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CabinTypeEntityId = table.Column<int>(type: "int", nullable: true),
                    FlightEntityId = table.Column<int>(type: "int", nullable: true),
                    SeatLocationTypesEntityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_flight_seats", x => x.id);
                    table.ForeignKey(
                        name: "FK_flight_seats_cabin_types_CabinTypeEntityId",
                        column: x => x.CabinTypeEntityId,
                        principalTable: "cabin_types",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_flight_seats_cabin_types_cabin_type_id",
                        column: x => x.cabin_type_id,
                        principalTable: "cabin_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_flight_seats_flights_FlightEntityId",
                        column: x => x.FlightEntityId,
                        principalTable: "flights",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_flight_seats_flights_flight_id",
                        column: x => x.flight_id,
                        principalTable: "flights",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_flight_seats_seat_location_types_SeatLocationTypesEntityId",
                        column: x => x.SeatLocationTypesEntityId,
                        principalTable: "seat_location_types",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_flight_seats_seat_location_types_seat_location_type_id",
                        column: x => x.seat_location_type_id,
                        principalTable: "seat_location_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "booking_flights",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    booking_id = table.Column<int>(type: "int", nullable: false),
                    flight_id = table.Column<int>(type: "int", nullable: false),
                    partial_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BookingEntityId = table.Column<int>(type: "int", nullable: true),
                    FlightEntityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_booking_flights", x => x.id);
                    table.CheckConstraint("chk_partial_amount", "partial_amount >= 0");
                    table.ForeignKey(
                        name: "FK_booking_flights_bookings_BookingEntityId",
                        column: x => x.BookingEntityId,
                        principalTable: "bookings",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_booking_flights_flights_FlightEntityId",
                        column: x => x.FlightEntityId,
                        principalTable: "flights",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_booking_flights_booking",
                        column: x => x.booking_id,
                        principalTable: "bookings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_booking_flights_flight",
                        column: x => x.flight_id,
                        principalTable: "flights",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "invoices",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    booking_id = table.Column<int>(type: "int", nullable: false),
                    invoice_number = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    issued_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    taxes = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    total = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_invoices", x => x.id);
                    table.ForeignKey(
                        name: "fk_invoices_booking",
                        column: x => x.booking_id,
                        principalTable: "bookings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    booking_id = table.Column<int>(type: "int", nullable: false),
                    amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    paid_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    payment_status_id = table.Column<int>(type: "int", nullable: false),
                    payment_method_id = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payments", x => x.id);
                    table.ForeignKey(
                        name: "fk_payments_bookings",
                        column: x => x.booking_id,
                        principalTable: "bookings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_payments_payment_methods",
                        column: x => x.payment_method_id,
                        principalTable: "payment_methods",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_payments_payment_statuses",
                        column: x => x.payment_status_id,
                        principalTable: "payment_statuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "flight_reservations",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    booking_flight_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_flight_reservations", x => x.id);
                    table.ForeignKey(
                        name: "FK_flight_reservations_booking_flights_booking_flight_id",
                        column: x => x.booking_flight_id,
                        principalTable: "booking_flights",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "invoice_items",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    invoice_id = table.Column<int>(type: "int", nullable: false),
                    item_type_id = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    quantity = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    unit_price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    booking_passenger_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_invoice_items", x => x.id);
                    table.ForeignKey(
                        name: "FK_invoice_items_invoice_item_types_item_type_id",
                        column: x => x.item_type_id,
                        principalTable: "invoice_item_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_invoice_items_invoices_invoice_id",
                        column: x => x.invoice_id,
                        principalTable: "invoices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "passenger_reservations",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    flight_reservation_id = table.Column<int>(type: "int", nullable: false),
                    passenger_id = table.Column<int>(type: "int", nullable: false),
                    PassengersEntityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_passenger_reservations", x => x.id);
                    table.ForeignKey(
                        name: "FK_passenger_reservations_passengers_PassengersEntityId",
                        column: x => x.PassengersEntityId,
                        principalTable: "passengers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_passenger_reservations_flight_reservation",
                        column: x => x.flight_reservation_id,
                        principalTable: "flight_reservations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_passenger_reservations_passenger",
                        column: x => x.passenger_id,
                        principalTable: "passengers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tickets",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    code = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    issue_date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    passenger_reservation_id = table.Column<int>(type: "int", nullable: false),
                    ticket_state_id = table.Column<int>(type: "int", nullable: false),
                    PassengerReservationsEntityId = table.Column<int>(type: "int", nullable: true),
                    TicketStatesEntityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tickets", x => x.id);
                    table.ForeignKey(
                        name: "FK_tickets_passenger_reservations_PassengerReservationsEntityId",
                        column: x => x.PassengerReservationsEntityId,
                        principalTable: "passenger_reservations",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tickets_passenger_reservations_passenger_reservation_id",
                        column: x => x.passenger_reservation_id,
                        principalTable: "passenger_reservations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tickets_ticket_states_TicketStatesEntityId",
                        column: x => x.TicketStatesEntityId,
                        principalTable: "ticket_states",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tickets_ticket_states_ticket_state_id",
                        column: x => x.ticket_state_id,
                        principalTable: "ticket_states",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "check_ins",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ticket_id = table.Column<int>(type: "int", nullable: false),
                    staff_id = table.Column<int>(type: "int", nullable: false),
                    flight_seat_id = table.Column<int>(type: "int", nullable: false),
                    checked_in_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    checkin_status_id = table.Column<int>(type: "int", nullable: false),
                    boarding_pass_number = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CheckinStatesEntityId = table.Column<int>(type: "int", nullable: true),
                    StaffEntityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_check_ins", x => x.id);
                    table.ForeignKey(
                        name: "FK_check_ins_checkin_states_CheckinStatesEntityId",
                        column: x => x.CheckinStatesEntityId,
                        principalTable: "checkin_states",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_check_ins_staff_StaffEntityId",
                        column: x => x.StaffEntityId,
                        principalTable: "staff",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_checkins_checkin_status",
                        column: x => x.checkin_status_id,
                        principalTable: "checkin_states",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_checkins_flight_seat",
                        column: x => x.flight_seat_id,
                        principalTable: "flight_seats",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_checkins_staff",
                        column: x => x.staff_id,
                        principalTable: "staff",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_checkins_ticket",
                        column: x => x.ticket_id,
                        principalTable: "tickets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "baggage",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    checkin_id = table.Column<int>(type: "int", nullable: false),
                    baggage_type_id = table.Column<int>(type: "int", nullable: false),
                    weight_kg = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    charged_price = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_baggage", x => x.id);
                    table.ForeignKey(
                        name: "fk_baggage_baggage_type",
                        column: x => x.baggage_type_id,
                        principalTable: "baggage_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_baggage_checkin",
                        column: x => x.checkin_id,
                        principalTable: "check_ins",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "SystemRoles",
                columns: new[] { "Id", "description", "name" },
                values: new object[] { 1, "Administrador del sistema", "Admin" });

            migrationBuilder.CreateIndex(
                name: "IX_addresses_city_id",
                table: "addresses",
                column: "city_id");

            migrationBuilder.CreateIndex(
                name: "IX_addresses_CityId1",
                table: "addresses",
                column: "CityId1");

            migrationBuilder.CreateIndex(
                name: "IX_addresses_street_type_id",
                table: "addresses",
                column: "street_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_addresses_StreetTypeId1",
                table: "addresses",
                column: "StreetTypeId1");

            migrationBuilder.CreateIndex(
                name: "IX_aircraft_airline_id",
                table: "aircraft",
                column: "airline_id");

            migrationBuilder.CreateIndex(
                name: "IX_aircraft_AirlineEntityId",
                table: "aircraft",
                column: "AirlineEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_aircraft_model_id",
                table: "aircraft",
                column: "model_id");

            migrationBuilder.CreateIndex(
                name: "IX_aircraft_registration",
                table: "aircraft",
                column: "registration",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_aircraft_manufacturers_name",
                table: "aircraft_manufacturers",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_aircraft_models_manufacturer_id_model_name",
                table: "aircraft_models",
                columns: new[] { "manufacturer_id", "model_name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_airlines_iata_code",
                table: "airlines",
                column: "iata_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_airlines_origin_country_id",
                table: "airlines",
                column: "origin_country_id");

            migrationBuilder.CreateIndex(
                name: "IX_airport_airline_airline_id",
                table: "airport_airline",
                column: "airline_id");

            migrationBuilder.CreateIndex(
                name: "IX_airport_airline_AirlineEntityId",
                table: "airport_airline",
                column: "AirlineEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_airport_airline_airport_id_airline_id",
                table: "airport_airline",
                columns: new[] { "airport_id", "airline_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_airport_airline_AirportEntityId",
                table: "airport_airline",
                column: "AirportEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_airports_city_id",
                table: "airports",
                column: "city_id");

            migrationBuilder.CreateIndex(
                name: "IX_airports_CityEntityId",
                table: "airports",
                column: "CityEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_airports_iata_code",
                table: "airports",
                column: "iata_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_airports_icao_code",
                table: "airports",
                column: "icao_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_availability_statuses_name",
                table: "availability_statuses",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_baggage_baggage_type_id",
                table: "baggage",
                column: "baggage_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_baggage_checkin_id",
                table: "baggage",
                column: "checkin_id");

            migrationBuilder.CreateIndex(
                name: "IX_baggage_types_name",
                table: "baggage_types",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_booking_flights_booking_id",
                table: "booking_flights",
                column: "booking_id");

            migrationBuilder.CreateIndex(
                name: "IX_booking_flights_booking_id_flight_id",
                table: "booking_flights",
                columns: new[] { "booking_id", "flight_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_booking_flights_BookingEntityId",
                table: "booking_flights",
                column: "BookingEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_booking_flights_flight_id",
                table: "booking_flights",
                column: "flight_id");

            migrationBuilder.CreateIndex(
                name: "IX_booking_flights_FlightEntityId",
                table: "booking_flights",
                column: "FlightEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_booking_status_transitions_BookingStatusesEntityId",
                table: "booking_status_transitions",
                column: "BookingStatusesEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_booking_status_transitions_BookingStatusesEntityId1",
                table: "booking_status_transitions",
                column: "BookingStatusesEntityId1");

            migrationBuilder.CreateIndex(
                name: "IX_booking_status_transitions_from_status_id_to_status_id",
                table: "booking_status_transitions",
                columns: new[] { "from_status_id", "to_status_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_booking_status_transitions_to_status_id",
                table: "booking_status_transitions",
                column: "to_status_id");

            migrationBuilder.CreateIndex(
                name: "IX_booking_statuses_Name",
                table: "booking_statuses",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bookings_booked_at",
                table: "bookings",
                column: "booked_at");

            migrationBuilder.CreateIndex(
                name: "IX_bookings_booking_status_id",
                table: "bookings",
                column: "booking_status_id");

            migrationBuilder.CreateIndex(
                name: "IX_bookings_BookingStatusesEntityId",
                table: "bookings",
                column: "BookingStatusesEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_bookings_client_id",
                table: "bookings",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "IX_cabin_configurations_aircraft_id_cabin_type_id",
                table: "cabin_configurations",
                columns: new[] { "aircraft_id", "cabin_type_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cabin_configurations_AircraftEntityId",
                table: "cabin_configurations",
                column: "AircraftEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_cabin_configurations_cabin_type_id",
                table: "cabin_configurations",
                column: "cabin_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_cabin_configurations_CabinTypeEntityId",
                table: "cabin_configurations",
                column: "CabinTypeEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_cabin_types_name",
                table: "cabin_types",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_card_issuers_name",
                table: "card_issuers",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_card_types_name",
                table: "card_types",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_check_ins_boarding_pass_number",
                table: "check_ins",
                column: "boarding_pass_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_check_ins_checkin_status_id",
                table: "check_ins",
                column: "checkin_status_id");

            migrationBuilder.CreateIndex(
                name: "IX_check_ins_CheckinStatesEntityId",
                table: "check_ins",
                column: "CheckinStatesEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_check_ins_flight_seat_id",
                table: "check_ins",
                column: "flight_seat_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_check_ins_staff_id",
                table: "check_ins",
                column: "staff_id");

            migrationBuilder.CreateIndex(
                name: "IX_check_ins_StaffEntityId",
                table: "check_ins",
                column: "StaffEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_check_ins_ticket_id",
                table: "check_ins",
                column: "ticket_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cities_region_id",
                table: "cities",
                column: "region_id");

            migrationBuilder.CreateIndex(
                name: "IX_cities_RegionId1",
                table: "cities",
                column: "RegionId1");

            migrationBuilder.CreateIndex(
                name: "IX_clients_person_id",
                table: "clients",
                column: "person_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_continents_name",
                table: "continents",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_countries_continent_id",
                table: "countries",
                column: "continent_id");

            migrationBuilder.CreateIndex(
                name: "IX_countries_CountryEntityId",
                table: "countries",
                column: "CountryEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_countries_iso_code",
                table: "countries",
                column: "iso_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_document_types_code",
                table: "document_types",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_document_types_name",
                table: "document_types",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_email_domains_domain",
                table: "email_domains",
                column: "domain",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_fares_cabin_type_id",
                table: "fares",
                column: "cabin_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_fares_CabinTypeEntityId",
                table: "fares",
                column: "CabinTypeEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_fares_passenger_type_id",
                table: "fares",
                column: "passenger_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_fares_PassengerTypeEntityId",
                table: "fares",
                column: "PassengerTypeEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_fares_route_id",
                table: "fares",
                column: "route_id");

            migrationBuilder.CreateIndex(
                name: "IX_fares_route_id_cabin_type_id_passenger_type_id_season_id",
                table: "fares",
                columns: new[] { "route_id", "cabin_type_id", "passenger_type_id", "season_id" });

            migrationBuilder.CreateIndex(
                name: "IX_fares_RouteEntityId",
                table: "fares",
                column: "RouteEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_fares_season_id",
                table: "fares",
                column: "season_id");

            migrationBuilder.CreateIndex(
                name: "IX_fares_SeasonEntityId",
                table: "fares",
                column: "SeasonEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_flight_assignments_flight_id",
                table: "flight_assignments",
                column: "flight_id");

            migrationBuilder.CreateIndex(
                name: "IX_flight_crew_assignments_flight_id_staff_id",
                table: "flight_crew_assignments",
                columns: new[] { "flight_id", "staff_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_flight_crew_assignments_flight_role_id",
                table: "flight_crew_assignments",
                column: "flight_role_id");

            migrationBuilder.CreateIndex(
                name: "IX_flight_crew_assignments_FlightRolesEntityId",
                table: "flight_crew_assignments",
                column: "FlightRolesEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_flight_crew_assignments_staff_id",
                table: "flight_crew_assignments",
                column: "staff_id");

            migrationBuilder.CreateIndex(
                name: "IX_flight_crew_assignments_StaffEntityId",
                table: "flight_crew_assignments",
                column: "StaffEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_flight_crew_roles_name",
                table: "flight_crew_roles",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_flight_reservations_booking_flight_id",
                table: "flight_reservations",
                column: "booking_flight_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_flight_seats_cabin_type_id",
                table: "flight_seats",
                column: "cabin_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_flight_seats_CabinTypeEntityId",
                table: "flight_seats",
                column: "CabinTypeEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_flight_seats_flight_id",
                table: "flight_seats",
                column: "flight_id");

            migrationBuilder.CreateIndex(
                name: "IX_flight_seats_FlightEntityId",
                table: "flight_seats",
                column: "FlightEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_flight_seats_seat_location_type_id",
                table: "flight_seats",
                column: "seat_location_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_flight_seats_SeatLocationTypesEntityId",
                table: "flight_seats",
                column: "SeatLocationTypesEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_flight_status_transitions_FlightStatusEntityId",
                table: "flight_status_transitions",
                column: "FlightStatusEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_flight_status_transitions_FlightStatusEntityId1",
                table: "flight_status_transitions",
                column: "FlightStatusEntityId1");

            migrationBuilder.CreateIndex(
                name: "IX_flight_status_transitions_from_status_id_to_status_id",
                table: "flight_status_transitions",
                columns: new[] { "from_status_id", "to_status_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_flight_status_transitions_to_status_id",
                table: "flight_status_transitions",
                column: "to_status_id");

            migrationBuilder.CreateIndex(
                name: "IX_flight_statuses_name",
                table: "flight_statuses",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_flights_aircraft_id",
                table: "flights",
                column: "aircraft_id");

            migrationBuilder.CreateIndex(
                name: "IX_flights_AircraftEntityId",
                table: "flights",
                column: "AircraftEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_flights_airline_id",
                table: "flights",
                column: "airline_id");

            migrationBuilder.CreateIndex(
                name: "IX_flights_AirlineEntityId",
                table: "flights",
                column: "AirlineEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_flights_flight_code",
                table: "flights",
                column: "flight_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_flights_flight_status_id",
                table: "flights",
                column: "flight_status_id");

            migrationBuilder.CreateIndex(
                name: "IX_flights_FlightStatusEntityId",
                table: "flights",
                column: "FlightStatusEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_flights_route_id",
                table: "flights",
                column: "route_id");

            migrationBuilder.CreateIndex(
                name: "IX_flights_RouteEntityId",
                table: "flights",
                column: "RouteEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_invoice_item_types_name",
                table: "invoice_item_types",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_invoice_items_invoice_id",
                table: "invoice_items",
                column: "invoice_id");

            migrationBuilder.CreateIndex(
                name: "IX_invoice_items_item_type_id",
                table: "invoice_items",
                column: "item_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_invoices_booking_id",
                table: "invoices",
                column: "booking_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_invoices_invoice_number",
                table: "invoices",
                column: "invoice_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_passenger_reservations_flight_reservation_id_passenger_id",
                table: "passenger_reservations",
                columns: new[] { "flight_reservation_id", "passenger_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_passenger_reservations_passenger_id",
                table: "passenger_reservations",
                column: "passenger_id");

            migrationBuilder.CreateIndex(
                name: "IX_passenger_reservations_PassengersEntityId",
                table: "passenger_reservations",
                column: "PassengersEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_passenger_types_name",
                table: "passenger_types",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_passengers_passenger_type_id",
                table: "passengers",
                column: "passenger_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_passengers_PassengerTypeEntityId",
                table: "passengers",
                column: "PassengerTypeEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_passengers_person_id",
                table: "passengers",
                column: "person_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_payment_method_types_name",
                table: "payment_method_types",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_payment_methods_card_issuer_id",
                table: "payment_methods",
                column: "card_issuer_id");

            migrationBuilder.CreateIndex(
                name: "IX_payment_methods_card_type_id",
                table: "payment_methods",
                column: "card_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_payment_methods_display_name",
                table: "payment_methods",
                column: "display_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_payment_methods_payment_method_type_id",
                table: "payment_methods",
                column: "payment_method_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_payment_statuses_name",
                table: "payment_statuses",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_payments_booking_id",
                table: "payments",
                column: "booking_id");

            migrationBuilder.CreateIndex(
                name: "IX_payments_payment_method_id",
                table: "payments",
                column: "payment_method_id");

            migrationBuilder.CreateIndex(
                name: "IX_payments_payment_status_id",
                table: "payments",
                column: "payment_status_id");

            migrationBuilder.CreateIndex(
                name: "IX_permissions_name",
                table: "permissions",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_person_emails_email_domain_id",
                table: "person_emails",
                column: "email_domain_id");

            migrationBuilder.CreateIndex(
                name: "IX_person_emails_EmailDomainsEntityId",
                table: "person_emails",
                column: "EmailDomainsEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_person_emails_person_id_email_user_email_domain_id",
                table: "person_emails",
                columns: new[] { "person_id", "email_user", "email_domain_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_person_phones_person_id",
                table: "person_phones",
                column: "person_id");

            migrationBuilder.CreateIndex(
                name: "IX_person_phones_phone_code_id",
                table: "person_phones",
                column: "phone_code_id");

            migrationBuilder.CreateIndex(
                name: "IX_persons_address_id",
                table: "persons",
                column: "address_id");

            migrationBuilder.CreateIndex(
                name: "IX_persons_AddressEntityId",
                table: "persons",
                column: "AddressEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_persons_document_type_id_document_number",
                table: "persons",
                columns: new[] { "document_type_id", "document_number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_persons_DocumentTypeEntityId",
                table: "persons",
                column: "DocumentTypeEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_phone_codes_country_code",
                table: "phone_codes",
                column: "country_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_regions_country_id",
                table: "regions",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "IX_role_permissions_permission_id",
                table: "role_permissions",
                column: "permission_id");

            migrationBuilder.CreateIndex(
                name: "IX_role_permissions_role_id_permission_id",
                table: "role_permissions",
                columns: new[] { "role_id", "permission_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_route_stopovers_AirportEntityId",
                table: "route_stopovers",
                column: "AirportEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_route_stopovers_RouteEntityId",
                table: "route_stopovers",
                column: "RouteEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_route_stopovers_stopover_airport_id",
                table: "route_stopovers",
                column: "stopover_airport_id");

            migrationBuilder.CreateIndex(
                name: "uk_route_stopovers_route_stop_order",
                table: "route_stopovers",
                columns: new[] { "route_id", "stop_order" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_routes_destination_airport_id",
                table: "routes",
                column: "destination_airport_id");

            migrationBuilder.CreateIndex(
                name: "IX_routes_origin_airport_id_destination_airport_id",
                table: "routes",
                columns: new[] { "origin_airport_id", "destination_airport_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_seasons_name",
                table: "seasons",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_seat_location_types_name",
                table: "seat_location_types",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sessions_user_id",
                table: "sessions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_staff_airline_id",
                table: "staff",
                column: "airline_id");

            migrationBuilder.CreateIndex(
                name: "IX_staff_AirlineEntityId",
                table: "staff",
                column: "AirlineEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_staff_airport_id",
                table: "staff",
                column: "airport_id");

            migrationBuilder.CreateIndex(
                name: "IX_staff_AirportEntityId",
                table: "staff",
                column: "AirportEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_staff_person_id",
                table: "staff",
                column: "person_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_staff_PersonalPositionEntityId",
                table: "staff",
                column: "PersonalPositionEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_staff_position_id",
                table: "staff",
                column: "position_id");

            migrationBuilder.CreateIndex(
                name: "IX_staff_availability_availability_status_id",
                table: "staff_availability",
                column: "availability_status_id");

            migrationBuilder.CreateIndex(
                name: "IX_staff_availability_AvailabilityStateEntityId",
                table: "staff_availability",
                column: "AvailabilityStateEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_staff_availability_staff_id",
                table: "staff_availability",
                column: "staff_id");

            migrationBuilder.CreateIndex(
                name: "IX_staff_availability_StaffEntityId",
                table: "staff_availability",
                column: "StaffEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_staff_positions_name",
                table: "staff_positions",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SystemRoles_name",
                table: "SystemRoles",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tickets_passenger_reservation_id",
                table: "tickets",
                column: "passenger_reservation_id");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_PassengerReservationsEntityId",
                table: "tickets",
                column: "PassengerReservationsEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_ticket_state_id",
                table: "tickets",
                column: "ticket_state_id");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_TicketStatesEntityId",
                table: "tickets",
                column: "TicketStatesEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_users_person_id",
                table: "users",
                column: "person_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_role_id",
                table: "users",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_SystemRolesEntityId",
                table: "users",
                column: "SystemRolesEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_users_username",
                table: "users",
                column: "username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "airport_airline");

            migrationBuilder.DropTable(
                name: "baggage");

            migrationBuilder.DropTable(
                name: "booking_status_transitions");

            migrationBuilder.DropTable(
                name: "cabin_configurations");

            migrationBuilder.DropTable(
                name: "fares");

            migrationBuilder.DropTable(
                name: "flight_assignments");

            migrationBuilder.DropTable(
                name: "flight_crew_assignments");

            migrationBuilder.DropTable(
                name: "flight_status_transitions");

            migrationBuilder.DropTable(
                name: "invoice_items");

            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropTable(
                name: "person_emails");

            migrationBuilder.DropTable(
                name: "person_phones");

            migrationBuilder.DropTable(
                name: "role_permissions");

            migrationBuilder.DropTable(
                name: "route_stopovers");

            migrationBuilder.DropTable(
                name: "sessions");

            migrationBuilder.DropTable(
                name: "staff_availability");

            migrationBuilder.DropTable(
                name: "baggage_types");

            migrationBuilder.DropTable(
                name: "check_ins");

            migrationBuilder.DropTable(
                name: "seasons");

            migrationBuilder.DropTable(
                name: "flight_crew_roles");

            migrationBuilder.DropTable(
                name: "invoice_item_types");

            migrationBuilder.DropTable(
                name: "invoices");

            migrationBuilder.DropTable(
                name: "payment_methods");

            migrationBuilder.DropTable(
                name: "payment_statuses");

            migrationBuilder.DropTable(
                name: "email_domains");

            migrationBuilder.DropTable(
                name: "phone_codes");

            migrationBuilder.DropTable(
                name: "permissions");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "availability_statuses");

            migrationBuilder.DropTable(
                name: "checkin_states");

            migrationBuilder.DropTable(
                name: "staff");

            migrationBuilder.DropTable(
                name: "flight_seats");

            migrationBuilder.DropTable(
                name: "tickets");

            migrationBuilder.DropTable(
                name: "card_issuers");

            migrationBuilder.DropTable(
                name: "card_types");

            migrationBuilder.DropTable(
                name: "payment_method_types");

            migrationBuilder.DropTable(
                name: "SystemRoles");

            migrationBuilder.DropTable(
                name: "staff_positions");

            migrationBuilder.DropTable(
                name: "cabin_types");

            migrationBuilder.DropTable(
                name: "seat_location_types");

            migrationBuilder.DropTable(
                name: "passenger_reservations");

            migrationBuilder.DropTable(
                name: "ticket_states");

            migrationBuilder.DropTable(
                name: "passengers");

            migrationBuilder.DropTable(
                name: "flight_reservations");

            migrationBuilder.DropTable(
                name: "passenger_types");

            migrationBuilder.DropTable(
                name: "booking_flights");

            migrationBuilder.DropTable(
                name: "bookings");

            migrationBuilder.DropTable(
                name: "flights");

            migrationBuilder.DropTable(
                name: "booking_statuses");

            migrationBuilder.DropTable(
                name: "clients");

            migrationBuilder.DropTable(
                name: "aircraft");

            migrationBuilder.DropTable(
                name: "flight_statuses");

            migrationBuilder.DropTable(
                name: "routes");

            migrationBuilder.DropTable(
                name: "persons");

            migrationBuilder.DropTable(
                name: "aircraft_models");

            migrationBuilder.DropTable(
                name: "airlines");

            migrationBuilder.DropTable(
                name: "airports");

            migrationBuilder.DropTable(
                name: "addresses");

            migrationBuilder.DropTable(
                name: "document_types");

            migrationBuilder.DropTable(
                name: "aircraft_manufacturers");

            migrationBuilder.DropTable(
                name: "cities");

            migrationBuilder.DropTable(
                name: "street_types");

            migrationBuilder.DropTable(
                name: "regions");

            migrationBuilder.DropTable(
                name: "countries");

            migrationBuilder.DropTable(
                name: "continents");
        }
    }
}
