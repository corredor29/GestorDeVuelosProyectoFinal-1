using GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Personal.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Personal.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.ValueObject;
using DomainFlight = GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.Aggregate.Flights;

namespace GestorDeVuelosProyectoFinal.Moduls.FlightAssignments.Application.UseCases;

// // agrupa reglas y validaciones reutilizables para el proceso de asignar personal a vuelos
internal static class FlightAssignmentRules
{
    // me convierte int a objecto 
    public static FlightsId CreateFlightId(int flightId) => FlightsId.Create(flightId);

    public static PersonalId CreateStaffId(int staffId) => PersonalId.Create(staffId);

    // texto descriptivo de la asignacion
    public static string BuildAvailabilityNote(int flightId)
        => $"Asignacion de tripulacion para vuelo {flightId}";

    // verifica que el vuelo existe
    public static async Task<DomainFlight> GetExistingFlightAsync(
        IFlightsRepository flightsRepository,
        int flightId,
        CancellationToken cancellationToken)
    {
        return await flightsRepository.GetByIdAsync(FlightsId.Create(flightId), cancellationToken)
            ?? throw new InvalidOperationException($"No se encontro el vuelo con id {flightId}.");
    }

    public static async Task EnsureStaffExistsAsync(IPersonalRepository personalRepository, int staffId)
    {
        var staff = await personalRepository.GetByIdAsync(PersonalId.Create(staffId));
        if (staff is null)
            throw new InvalidOperationException($"No se encontro el personal con id {staffId}.");
    }

    public static async Task EnsureRoleExistsAsync(
        IFlightRolesRepository flightRolesRepository,
        int flightRoleId,
        CancellationToken cancellationToken)
    {
        var role = await flightRolesRepository.GetByIdAsync(FlightRolesId.Create(flightRoleId), cancellationToken);
        if (role is null)
            throw new InvalidOperationException($"No se encontro el rol de tripulacion con id {flightRoleId}.");
    }
}
