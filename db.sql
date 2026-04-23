-- ============================================================
--  AIRLINE TICKET MANAGEMENT SYSTEM
--  Full DDL — 63 tables
-- ============================================================

CREATE DATABASE IF NOT EXISTS airline_ticket_system
USE airline_ticket_system;

-- ============================================================
-- 1. GEOGRAPHY
--    Hierarchy: Continent → Country → Region/State/Department → City
-- ============================================================

CREATE TABLE continents (
    id      INT         AUTO_INCREMENT PRIMARY KEY,
    name    VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE countries (
    id              INT          AUTO_INCREMENT PRIMARY KEY,
    name            VARCHAR(100) NOT NULL,
    iso_code        VARCHAR(3)   NOT NULL UNIQUE,
    continent_id    INT          NOT NULL,
    FOREIGN KEY (continent_id) REFERENCES continents(id)
);

-- Covers states, departments, provinces and regions depending on the country
CREATE TABLE regions (
    id          INT          AUTO_INCREMENT PRIMARY KEY,
    name        VARCHAR(100) NOT NULL,
    type        VARCHAR(30)  NOT NULL,   -- State, Department, Province, Region, Community...
    country_id  INT          NOT NULL,
    FOREIGN KEY (country_id) REFERENCES countries(id)
);

CREATE TABLE cities (
    id          INT          AUTO_INCREMENT PRIMARY KEY,
    name        VARCHAR(100) NOT NULL,
    region_id   INT          NOT NULL,
    FOREIGN KEY (region_id) REFERENCES regions(id)
);

-- ============================================================
-- 2. NORMALIZED ADDRESS
-- ============================================================

CREATE TABLE street_types (
    id      INT         AUTO_INCREMENT PRIMARY KEY,
    name    VARCHAR(50) NOT NULL    -- Street, Avenue, Boulevard, Road...
);

CREATE TABLE addresses (
    id              INT          AUTO_INCREMENT PRIMARY KEY,
    street_type_id  INT          NOT NULL,
    street_name     VARCHAR(100) NOT NULL,
    number          VARCHAR(20)  NULL,
    complement      VARCHAR(100) NULL,      -- Apt 301, Tower B
    city_id         INT          NOT NULL,
    postal_code     VARCHAR(20)  NULL,
    FOREIGN KEY (street_type_id) REFERENCES street_types(id),
    FOREIGN KEY (city_id)        REFERENCES cities(id)
);

-- ============================================================
-- 3. PERSONS  (shared base for clients, passengers and staff)
-- ============================================================

CREATE TABLE document_types (
    id      INT         AUTO_INCREMENT PRIMARY KEY,
    name    VARCHAR(50) NOT NULL,
    code    VARCHAR(10) NOT NULL UNIQUE     -- PASSPORT, ID, DNI...
);

CREATE TABLE persons (
    id                  INT          AUTO_INCREMENT PRIMARY KEY,
    document_type_id    INT          NOT NULL,
    document_number     VARCHAR(30)  NOT NULL,
    first_name          VARCHAR(100) NOT NULL,
    last_name           VARCHAR(100) NOT NULL,
    birth_date          DATE         NULL,
    gender              CHAR(1)      NULL,  -- M / F / N
    address_id          INT          NULL,
    created_at          DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at          DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP
                                            ON UPDATE CURRENT_TIMESTAMP,
    UNIQUE (document_type_id, document_number),
    FOREIGN KEY (document_type_id) REFERENCES document_types(id),
    FOREIGN KEY (address_id)       REFERENCES addresses(id)
);

-- ============================================================
-- 4. EMAILS AND PHONES
--    Applies to any person: clients, passengers and staff.
--    Related directly to persons, not to clients.
-- ============================================================

CREATE TABLE email_domains (
    id      INT          AUTO_INCREMENT PRIMARY KEY,
    domain  VARCHAR(100) NOT NULL UNIQUE    -- gmail.com, outlook.com...
);

CREATE TABLE phone_codes (
    id           INT         AUTO_INCREMENT PRIMARY KEY,
    country_code VARCHAR(5)  NOT NULL UNIQUE,   -- +57, +34...
    country_name VARCHAR(100) NOT NULL
);

CREATE TABLE person_emails (
    id              INT          AUTO_INCREMENT PRIMARY KEY,
    person_id       INT          NOT NULL,
    email_user      VARCHAR(100) NOT NULL,       -- part before @
    email_domain_id INT          NOT NULL,
    is_primary      TINYINT(1)   NOT NULL DEFAULT 0,
    FOREIGN KEY (person_id)       REFERENCES persons(id),
    FOREIGN KEY (email_domain_id) REFERENCES email_domains(id)
);

CREATE TABLE person_phones (
    id              INT          AUTO_INCREMENT PRIMARY KEY,
    person_id       INT          NOT NULL,
    phone_code_id   INT          NOT NULL,
    phone_number    VARCHAR(20)  NOT NULL,
    is_primary      TINYINT(1)   NOT NULL DEFAULT 0,
    FOREIGN KEY (person_id)    REFERENCES persons(id),
    FOREIGN KEY (phone_code_id) REFERENCES phone_codes(id)
);

-- ============================================================
-- 5. CLIENTS  (buyers — extends persons)
-- ============================================================

CREATE TABLE clients (
    id          INT      AUTO_INCREMENT PRIMARY KEY,
    person_id   INT      NOT NULL UNIQUE,
    created_at  DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (person_id) REFERENCES persons(id)
);

-- ============================================================
-- 6. AIRLINES AND AIRPORTS
--    Airports carry IATA (3 letters) and ICAO (4 letters) codes.
--    Airline relationship is managed in airport_airline via FK.
-- ============================================================

CREATE TABLE airlines (
    id              INT          AUTO_INCREMENT PRIMARY KEY,
    name            VARCHAR(150) NOT NULL,
    iata_code       VARCHAR(3)   NOT NULL UNIQUE,
    origin_country_id INT        NOT NULL,
    is_active       TINYINT(1)   NOT NULL DEFAULT 1,
    created_at      DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at      DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP
                                         ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (origin_country_id) REFERENCES countries(id)
);

CREATE TABLE airports (
    id          INT          AUTO_INCREMENT PRIMARY KEY,
    name        VARCHAR(150) NOT NULL,
    iata_code   VARCHAR(3)   NOT NULL UNIQUE,
    icao_code   VARCHAR(4)   NULL     UNIQUE,
    city_id     INT          NOT NULL,
    FOREIGN KEY (city_id) REFERENCES cities(id)
);

-- ============================================================
-- 7. AIRLINES OPERATING AT EACH AIRPORT
--    Relationship uses airline_id (FK), not the IATA code.
-- ============================================================

CREATE TABLE airport_airline (
    id              INT          AUTO_INCREMENT PRIMARY KEY,
    airport_id      INT          NOT NULL,
    airline_id      INT          NOT NULL,
    terminal        VARCHAR(20)  NULL,
    start_date      DATE         NOT NULL,
    end_date        DATE         NULL,           -- NULL = still operating
    is_active       TINYINT(1)   NOT NULL DEFAULT 1,
    UNIQUE (airport_id, airline_id),
    FOREIGN KEY (airport_id) REFERENCES airports(id),
    FOREIGN KEY (airline_id) REFERENCES airlines(id)
);

-- ============================================================
-- 8. STAFF
--    Covers flight crew (pilots, cabin crew) and ground staff
--    (check-in agents, administrative).
-- ============================================================

CREATE TABLE staff_positions (
    id      INT         AUTO_INCREMENT PRIMARY KEY,
    name    VARCHAR(100) NOT NULL UNIQUE   -- Pilot, Copilot, Check-In Agent, Administrative...
);

CREATE TABLE staff (
    id              INT          AUTO_INCREMENT PRIMARY KEY,
    person_id       INT          NOT NULL UNIQUE,
    position_id     INT          NOT NULL,
    airline_id      INT          NULL,   -- airline staff
    airport_id      INT          NULL,   -- airport staff
    hire_date       DATE         NOT NULL,
    is_active       TINYINT(1)   NOT NULL DEFAULT 1,
    created_at      DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at      DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP
                                         ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (person_id)   REFERENCES persons(id),
    FOREIGN KEY (position_id) REFERENCES staff_positions(id),
    FOREIGN KEY (airline_id)  REFERENCES airlines(id),
    FOREIGN KEY (airport_id)  REFERENCES airports(id)
);

-- ============================================================
-- 9. STAFF AVAILABILITY
--    Time blocks per employee.
--    Checked before creating a flight_crew_assignment.
-- ============================================================

CREATE TABLE availability_statuses (
    id      INT         AUTO_INCREMENT PRIMARY KEY,
    name    VARCHAR(50) NOT NULL UNIQUE   -- Available, Assigned, Vacation, Leave, Inactive
);

CREATE TABLE staff_availability (
    id                      INT          AUTO_INCREMENT PRIMARY KEY,
    staff_id                INT          NOT NULL,
    availability_status_id  INT          NOT NULL,
    starts_at               DATETIME     NOT NULL,
    ends_at                 DATETIME     NOT NULL,
    notes                   VARCHAR(255) NULL,
    FOREIGN KEY (staff_id)               REFERENCES staff(id),
    FOREIGN KEY (availability_status_id) REFERENCES availability_statuses(id),
    CONSTRAINT chk_availability_dates CHECK (ends_at > starts_at)
);

-- ============================================================
-- 10. AIRCRAFT AND CABIN CONFIGURATION
--     Operational data (fuel, weight, speed) belongs to the model.
-- ============================================================

CREATE TABLE aircraft_manufacturers (
    id      INT          AUTO_INCREMENT PRIMARY KEY,
    name    VARCHAR(100) NOT NULL,
    country VARCHAR(100) NOT NULL
);

CREATE TABLE aircraft_models (
    id                      INT            AUTO_INCREMENT PRIMARY KEY,
    manufacturer_id         INT            NOT NULL,
    model_name              VARCHAR(100)   NOT NULL,   -- Boeing 737-800, Airbus A320...
    max_capacity            INT            NOT NULL,
    max_takeoff_weight_kg   DECIMAL(10,2)  NULL,
    fuel_consumption_kg_h   DECIMAL(8,2)   NULL,
    cruise_speed_kmh        INT            NULL,
    cruise_altitude_ft      INT            NULL,
    FOREIGN KEY (manufacturer_id) REFERENCES aircraft_manufacturers(id)
);

CREATE TABLE aircraft (
    id                  INT          AUTO_INCREMENT PRIMARY KEY,
    model_id            INT          NOT NULL,
    airline_id          INT          NOT NULL,
    registration        VARCHAR(20)  NOT NULL UNIQUE,
    manufactured_date   DATE         NULL,
    is_active           TINYINT(1)   NOT NULL DEFAULT 1,
    FOREIGN KEY (model_id)   REFERENCES aircraft_models(id),
    FOREIGN KEY (airline_id) REFERENCES airlines(id)
);

-- Cabin classes: Economy, Business, First, VIP
CREATE TABLE cabin_types (
    id      INT         AUTO_INCREMENT PRIMARY KEY,
    name    VARCHAR(50) NOT NULL UNIQUE
);

-- Physical cabin layout inside a specific aircraft
CREATE TABLE cabin_configurations (
    id              INT          AUTO_INCREMENT PRIMARY KEY,
    aircraft_id     INT          NOT NULL,
    cabin_type_id   INT          NOT NULL,
    row_start       INT          NOT NULL,
    row_end         INT          NOT NULL,
    seats_per_row   INT          NOT NULL,
    seat_letters    VARCHAR(10)  NOT NULL,  -- "ABCDEF", "ABC"
    UNIQUE (aircraft_id, cabin_type_id),
    FOREIGN KEY (aircraft_id)   REFERENCES aircraft(id),
    FOREIGN KEY (cabin_type_id) REFERENCES cabin_types(id)
);

-- ============================================================
-- 11. ROUTES AND STOPOVERS
--     Operational data goes in aircraft_models, not routes.
--     Price varies by stopovers via fares table.
-- ============================================================

CREATE TABLE routes (
    id                      INT AUTO_INCREMENT PRIMARY KEY,
    origin_airport_id       INT NOT NULL,
    destination_airport_id  INT NOT NULL,
    distance_km             INT NULL,
    estimated_duration_min  INT NULL,
    UNIQUE (origin_airport_id, destination_airport_id),
    FOREIGN KEY (origin_airport_id)      REFERENCES airports(id),
    FOREIGN KEY (destination_airport_id) REFERENCES airports(id)
);

CREATE TABLE route_stopovers (
    id                  INT AUTO_INCREMENT PRIMARY KEY,
    route_id            INT NOT NULL,
    stopover_airport_id INT NOT NULL,
    stop_order          INT NOT NULL,
    layover_min         INT NOT NULL DEFAULT 0,
    UNIQUE (route_id, stop_order),
    FOREIGN KEY (route_id)            REFERENCES routes(id),
    FOREIGN KEY (stopover_airport_id) REFERENCES airports(id)
);

-- ============================================================
-- 12. SEASONS
--     Own table for seasons replacing VARCHAR field in fares.
-- ============================================================

CREATE TABLE seasons (
    id              INT            AUTO_INCREMENT PRIMARY KEY,
    name            VARCHAR(50)    NOT NULL UNIQUE,   -- High, Low, Mid, Christmas, Easter...
    description     VARCHAR(150)   NULL,
    price_factor    DECIMAL(5,4)   NOT NULL DEFAULT 1.0000  -- multiplier: 1.20 = +20%
);

-- ============================================================
-- 13. PASSENGER TYPES AND FARES
--     Base price = route + cabin type + passenger type + season.
-- ============================================================

CREATE TABLE passenger_types (
    id          INT         AUTO_INCREMENT PRIMARY KEY,
    name        VARCHAR(50) NOT NULL UNIQUE,  -- Infant, Child, Youth, Adult, Senior
    min_age     INT         NULL,
    max_age     INT         NULL
);

CREATE TABLE fares (
    id                  INT            AUTO_INCREMENT PRIMARY KEY,
    route_id            INT            NOT NULL,
    cabin_type_id       INT            NOT NULL,
    passenger_type_id   INT            NOT NULL,
    season_id           INT            NOT NULL,
    base_price          DECIMAL(18,2)  NOT NULL,
    valid_from          DATE           NULL,
    valid_until         DATE           NULL,
    FOREIGN KEY (route_id)          REFERENCES routes(id),
    FOREIGN KEY (cabin_type_id)     REFERENCES cabin_types(id),
    FOREIGN KEY (passenger_type_id) REFERENCES passenger_types(id),
    FOREIGN KEY (season_id)         REFERENCES seasons(id),
    CONSTRAINT chk_base_price CHECK (base_price >= 0)
);

-- ============================================================
-- 14. FLIGHTS
--     name field in statuses (not code).
--     rescheduled_at: Could Have.
-- ============================================================

CREATE TABLE flight_statuses (
    id      INT         AUTO_INCREMENT PRIMARY KEY,
    name    VARCHAR(50) NOT NULL UNIQUE   -- Scheduled, Boarding, In Flight, Cancelled, Completed, Rescheduled
);

-- Valid state transitions — only listed ones are allowed.
-- Enforced at Domain layer.
CREATE TABLE flight_status_transitions (
    id              INT AUTO_INCREMENT PRIMARY KEY,
    from_status_id  INT NOT NULL,
    to_status_id    INT NOT NULL,
    UNIQUE (from_status_id, to_status_id),
    FOREIGN KEY (from_status_id) REFERENCES flight_statuses(id),
    FOREIGN KEY (to_status_id)   REFERENCES flight_statuses(id)
);

CREATE TABLE flights (
    id                      INT          AUTO_INCREMENT PRIMARY KEY,
    flight_code             VARCHAR(10)  NOT NULL UNIQUE,
    airline_id              INT          NOT NULL,
    route_id                INT          NOT NULL,
    aircraft_id             INT          NOT NULL,
    departure_at            DATETIME     NOT NULL,
    estimated_arrival_at    DATETIME     NOT NULL,
    total_capacity          INT          NOT NULL,
    available_seats         INT          NOT NULL,
    flight_status_id        INT          NOT NULL,
    rescheduled_at          DATETIME     NULL,
    created_at              DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at              DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP
                                                  ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (airline_id)       REFERENCES airlines(id),
    FOREIGN KEY (route_id)         REFERENCES routes(id),
    FOREIGN KEY (aircraft_id)      REFERENCES aircraft(id),
    FOREIGN KEY (flight_status_id) REFERENCES flight_statuses(id),
    CONSTRAINT chk_total_capacity   CHECK (total_capacity > 0),
    CONSTRAINT chk_available_seats  CHECK (available_seats >= 0)
);

-- ============================================================
-- 15. FLIGHT CREW ROLES AND ASSIGNMENTS
-- ============================================================

CREATE TABLE flight_crew_roles (
    id      INT         AUTO_INCREMENT PRIMARY KEY,
    name    VARCHAR(100) NOT NULL UNIQUE   -- Captain, Copilot, Cabin Chief, Flight Attendant...
);

-- One staff member appears at most once per flight
CREATE TABLE flight_crew_assignments (
    id              INT AUTO_INCREMENT PRIMARY KEY,
    flight_id       INT NOT NULL,
    staff_id        INT NOT NULL,
    crew_role_id    INT NOT NULL,
    UNIQUE (flight_id, staff_id),
    FOREIGN KEY (flight_id)    REFERENCES flights(id),
    FOREIGN KEY (staff_id)     REFERENCES staff(id),
    FOREIGN KEY (crew_role_id) REFERENCES flight_crew_roles(id)
);

-- ============================================================
-- 16. FLIGHT SEATS
--     Window / Aisle / Middle + cabin class.
--     is_occupied updated when seat is assigned at check-in.
-- ============================================================

CREATE TABLE seat_location_types (
    id      INT         AUTO_INCREMENT PRIMARY KEY,
    name    VARCHAR(50) NOT NULL UNIQUE   -- Window, Aisle, Middle
);

CREATE TABLE flight_seats (
    id                      INT          AUTO_INCREMENT PRIMARY KEY,
    flight_id               INT          NOT NULL,
    seat_code               VARCHAR(5)   NOT NULL,  -- 12A, 14B...
    cabin_type_id           INT          NOT NULL,
    seat_location_type_id   INT          NOT NULL,
    is_occupied             TINYINT(1)   NOT NULL DEFAULT 0,
    UNIQUE (flight_id, seat_code),
    FOREIGN KEY (flight_id)             REFERENCES flights(id),
    FOREIGN KEY (cabin_type_id)         REFERENCES cabin_types(id),
    FOREIGN KEY (seat_location_type_id) REFERENCES seat_location_types(id)
);

-- ============================================================
-- 17. PASSENGERS  (travelers — extends persons)
-- ============================================================

CREATE TABLE passengers (
    id                  INT AUTO_INCREMENT PRIMARY KEY,
    person_id           INT NOT NULL UNIQUE,
    passenger_type_id   INT NOT NULL,
    FOREIGN KEY (person_id)          REFERENCES persons(id),
    FOREIGN KEY (passenger_type_id)  REFERENCES passenger_types(id)
);

-- ============================================================
-- 18. BOOKINGS
--     Seat is NOT assigned here — assigned at check-in.
--     expires_at: Could Have (pending booking expiry policy).
--
--     TICKET EMISSION RULE (Must Have):
--     Tickets are issued ONLY when:
--       booking.status = Confirmed  AND  payment.status = Paid
-- ============================================================

CREATE TABLE booking_statuses (
    id      INT         AUTO_INCREMENT PRIMARY KEY,
    name    VARCHAR(50) NOT NULL UNIQUE   -- Pending, Confirmed, Cancelled, Expired
);

-- Explicit state transitions (Should Have)
CREATE TABLE booking_status_transitions (
    id              INT AUTO_INCREMENT PRIMARY KEY,
    from_status_id  INT NOT NULL,
    to_status_id    INT NOT NULL,
    UNIQUE (from_status_id, to_status_id),
    FOREIGN KEY (from_status_id) REFERENCES booking_statuses(id),
    FOREIGN KEY (to_status_id)   REFERENCES booking_statuses(id)
);

CREATE TABLE bookings (
    id                  INT            AUTO_INCREMENT PRIMARY KEY,
    client_id           INT            NOT NULL,
    booked_at           DATETIME       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    booking_status_id   INT            NOT NULL,
    total_amount        DECIMAL(18,2)  NOT NULL,
    expires_at          DATETIME       NULL,
    created_at          DATETIME       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at          DATETIME       NOT NULL DEFAULT CURRENT_TIMESTAMP
                                                ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (client_id)         REFERENCES clients(id),
    FOREIGN KEY (booking_status_id) REFERENCES booking_statuses(id),
    CONSTRAINT chk_total_amount CHECK (total_amount >= 0)
);

-- Bridge: one booking can cover N flights (multi-leg itinerary)
CREATE TABLE booking_flights (
    id              INT            AUTO_INCREMENT PRIMARY KEY,
    booking_id      INT            NOT NULL,
    flight_id       INT            NOT NULL,
    partial_amount  DECIMAL(18,2)  NOT NULL DEFAULT 0,
    UNIQUE (booking_id, flight_id),
    FOREIGN KEY (booking_id) REFERENCES bookings(id),
    FOREIGN KEY (flight_id)  REFERENCES flights(id),
    CONSTRAINT chk_partial_amount CHECK (partial_amount >= 0)
);

-- Per flight in the booking, each passenger is registered.
-- Seat is NOT assigned here — assigned at check-in.
CREATE TABLE booking_passengers (
    id                  INT AUTO_INCREMENT PRIMARY KEY,
    booking_flight_id   INT NOT NULL,
    passenger_id        INT NOT NULL,
    UNIQUE (booking_flight_id, passenger_id),
    FOREIGN KEY (booking_flight_id) REFERENCES booking_flights(id),
    FOREIGN KEY (passenger_id)      REFERENCES passengers(id)
);

-- ============================================================
-- 19. TICKETS
--     Issued ONLY from a confirmed booking with approved payment.
--     One ticket per passenger per flight.
-- ============================================================

CREATE TABLE ticket_statuses (
    id      INT         AUTO_INCREMENT PRIMARY KEY,
    name    VARCHAR(50) NOT NULL UNIQUE   -- Issued, Voided, Used
);

CREATE TABLE tickets (
    id                      INT          AUTO_INCREMENT PRIMARY KEY,
    booking_passenger_id    INT          NOT NULL UNIQUE,
    ticket_code             VARCHAR(30)  NOT NULL UNIQUE,
    issued_at               DATETIME     NOT NULL,
    ticket_status_id        INT          NOT NULL,
    created_at              DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at              DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP
                                                 ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (booking_passenger_id) REFERENCES booking_passengers(id),
    FOREIGN KEY (ticket_status_id)     REFERENCES ticket_statuses(id)
);

-- ============================================================
-- 20. CHECK-IN
--     Seat is assigned here.
--     Baggage is registered in its own tables (baggage_types
--     and baggage) — not as loose fields in check_ins.
-- ============================================================

CREATE TABLE checkin_statuses (
    id      INT         AUTO_INCREMENT PRIMARY KEY,
    name    VARCHAR(50) NOT NULL UNIQUE   -- Pending, Completed, No Show
);

CREATE TABLE check_ins (
    id                      INT          AUTO_INCREMENT PRIMARY KEY,
    ticket_id               INT          NOT NULL UNIQUE,
    staff_id                INT          NOT NULL,
    flight_seat_id          INT          NOT NULL UNIQUE,
    checked_in_at           DATETIME     NOT NULL,
    checkin_status_id       INT          NOT NULL,
    boarding_pass_number    VARCHAR(20)  NOT NULL UNIQUE,
    FOREIGN KEY (ticket_id)         REFERENCES tickets(id),
    FOREIGN KEY (staff_id)          REFERENCES staff(id),
    FOREIGN KEY (flight_seat_id)    REFERENCES flight_seats(id),
    FOREIGN KEY (checkin_status_id) REFERENCES checkin_statuses(id)
);

-- ============================================================
-- 20b. BAGGAGE
--      Baggage is its own entity linked to check-in.
--      A passenger can have N bags of different types.
--      charged_price is also reflected in invoice_items.
-- ============================================================

CREATE TABLE baggage_types (
    id              INT            AUTO_INCREMENT PRIMARY KEY,
    name            VARCHAR(50)    NOT NULL UNIQUE,   -- Checked, Carry-on Paid, Special, Sports...
    max_weight_kg   DECIMAL(5,2)   NOT NULL,
    base_price      DECIMAL(18,2)  NOT NULL DEFAULT 0
);

CREATE TABLE baggage (
    id              INT            AUTO_INCREMENT PRIMARY KEY,
    checkin_id      INT            NOT NULL,
    baggage_type_id INT            NOT NULL,
    weight_kg       DECIMAL(5,2)   NOT NULL,
    charged_price   DECIMAL(18,2)  NOT NULL DEFAULT 0,
    FOREIGN KEY (checkin_id)      REFERENCES check_ins(id),
    FOREIGN KEY (baggage_type_id) REFERENCES baggage_types(id),
    CONSTRAINT chk_weight_kg    CHECK (weight_kg     >= 0),
    CONSTRAINT chk_charged_price CHECK (charged_price >= 0)
);

-- ============================================================
-- 21. INVOICES AND INVOICE ITEMS
--     Invoice generated from a booking.
--     Items detail base ticket price plus extras:
--     additional baggage, cabin upgrade, special meals, etc.
-- ============================================================

CREATE TABLE invoice_item_types (
    id      INT          AUTO_INCREMENT PRIMARY KEY,
    name    VARCHAR(100) NOT NULL UNIQUE   -- Base Ticket, Extra Baggage, Cabin Upgrade...
);

CREATE TABLE invoices (
    id              INT            AUTO_INCREMENT PRIMARY KEY,
    booking_id      INT            NOT NULL UNIQUE,
    invoice_number  VARCHAR(30)    NOT NULL UNIQUE,
    issued_at       DATETIME       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    subtotal        DECIMAL(18,2)  NOT NULL DEFAULT 0,
    taxes           DECIMAL(18,2)  NOT NULL DEFAULT 0,
    total           DECIMAL(18,2)  NOT NULL DEFAULT 0,
    created_at      DATETIME       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (booking_id) REFERENCES bookings(id),
    CONSTRAINT chk_subtotal CHECK (subtotal >= 0),
    CONSTRAINT chk_taxes    CHECK (taxes    >= 0),
    CONSTRAINT chk_total    CHECK (total    >= 0)
);

CREATE TABLE invoice_items (
    id                      INT            AUTO_INCREMENT PRIMARY KEY,
    invoice_id              INT            NOT NULL,
    item_type_id            INT            NOT NULL,
    description             VARCHAR(200)   NOT NULL,
    quantity                INT            NOT NULL DEFAULT 1,
    unit_price              DECIMAL(18,2)  NOT NULL,
    subtotal                DECIMAL(18,2)  NOT NULL,   -- quantity * unit_price
    booking_passenger_id    INT            NULL,        -- item linked to specific passenger
    FOREIGN KEY (invoice_id)            REFERENCES invoices(id),
    FOREIGN KEY (item_type_id)          REFERENCES invoice_item_types(id),
    FOREIGN KEY (booking_passenger_id)  REFERENCES booking_passengers(id),
    CONSTRAINT chk_item_quantity  CHECK (quantity   >= 1),
    CONSTRAINT chk_item_price     CHECK (unit_price >= 0),
    CONSTRAINT chk_item_subtotal  CHECK (subtotal   >= 0)
);

-- ============================================================
-- 22. PAYMENTS
--     Simulated statuses: Pending, Paid, Rejected, Refunded.
--     Ticket emission requires payment_status = Paid.
-- ============================================================

CREATE TABLE payment_statuses (
    id      INT         AUTO_INCREMENT PRIMARY KEY,
    name    VARCHAR(50) NOT NULL UNIQUE   -- Pending, Paid, Rejected, Refunded
);

CREATE TABLE payment_method_types (
    id      INT         AUTO_INCREMENT PRIMARY KEY,
    name    VARCHAR(50) NOT NULL UNIQUE   -- Cash, Card, Transfer, PSE
);

CREATE TABLE card_types (
    id      INT         AUTO_INCREMENT PRIMARY KEY,
    name    VARCHAR(50) NOT NULL UNIQUE   -- Credit, Debit, Prepaid
);

CREATE TABLE card_issuers (
    id      INT         AUTO_INCREMENT PRIMARY KEY,
    name    VARCHAR(50) NOT NULL UNIQUE   -- Visa, Mastercard, Amex, Diners
);

CREATE TABLE payment_methods (
    id                      INT          AUTO_INCREMENT PRIMARY KEY,
    payment_method_type_id  INT          NOT NULL,
    card_type_id            INT          NULL,
    card_issuer_id          INT          NULL,
    display_name            VARCHAR(50)  NOT NULL UNIQUE,
    FOREIGN KEY (payment_method_type_id) REFERENCES payment_method_types(id),
    FOREIGN KEY (card_type_id)           REFERENCES card_types(id),
    FOREIGN KEY (card_issuer_id)         REFERENCES card_issuers(id)
);

CREATE TABLE payments (
    id                  INT            AUTO_INCREMENT PRIMARY KEY,
    booking_id          INT            NOT NULL,
    amount              DECIMAL(18,2)  NOT NULL,
    paid_at             DATETIME       NOT NULL,
    payment_status_id   INT            NOT NULL,
    payment_method_id   INT            NOT NULL,
    created_at          DATETIME       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at          DATETIME       NOT NULL DEFAULT CURRENT_TIMESTAMP
                                                ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (booking_id)         REFERENCES bookings(id),
    FOREIGN KEY (payment_status_id)  REFERENCES payment_statuses(id),
    FOREIGN KEY (payment_method_id)  REFERENCES payment_methods(id),
    CONSTRAINT chk_payment_amount CHECK (amount >= 0)
);

-- ============================================================
-- 23. AUTHENTICATION AND AUTHORIZATION
--     Console login module.
--     Only 2 roles: Admin and Client.
--     Sessions track every login for traceability.
-- ============================================================

CREATE TABLE system_roles (
    id          INT         AUTO_INCREMENT PRIMARY KEY,
    name        VARCHAR(50) NOT NULL UNIQUE,
    description VARCHAR(150) NULL
);

CREATE TABLE permissions (
    id          INT          AUTO_INCREMENT PRIMARY KEY,
    name        VARCHAR(100) NOT NULL UNIQUE,
    description VARCHAR(200) NULL
);

CREATE TABLE role_permissions (
    id            INT AUTO_INCREMENT PRIMARY KEY,
    role_id       INT NOT NULL,
    permission_id INT NOT NULL,
    UNIQUE (role_id, permission_id),
    FOREIGN KEY (role_id)       REFERENCES system_roles(id),
    FOREIGN KEY (permission_id) REFERENCES permissions(id)
);

CREATE TABLE users (
    id              INT          AUTO_INCREMENT PRIMARY KEY,
    username        VARCHAR(50)  NOT NULL UNIQUE,
    password_hash   VARCHAR(255) NOT NULL,
    person_id       INT          NULL UNIQUE,
    role_id         INT          NOT NULL,
    is_active       TINYINT(1)   NOT NULL DEFAULT 1,
    last_access     DATETIME     NULL,
    created_at      DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at      DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP
                                         ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (person_id) REFERENCES persons(id),
    FOREIGN KEY (role_id)   REFERENCES system_roles(id)
);

CREATE TABLE sessions (
    id          INT         AUTO_INCREMENT PRIMARY KEY,
    user_id     INT         NOT NULL,
    started_at  DATETIME    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ended_at    DATETIME    NULL,
    ip_address  VARCHAR(45) NULL,
    is_active   TINYINT(1)  NOT NULL DEFAULT 1,
    FOREIGN KEY (user_id) REFERENCES users(id)
);

-- ============================================================
-- END OF SCRIPT — 63 tables
-- ============================================================