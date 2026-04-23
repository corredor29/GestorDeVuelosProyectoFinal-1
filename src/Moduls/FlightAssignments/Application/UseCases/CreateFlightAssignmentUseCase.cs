using GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.FlightAssignments.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.FlightAssignments.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Personal.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.FlightAssignments.Application.UseCases;

public sealed class CreateFlightAssignmentUseCase
{
    private const string AssignedStateName = "Assigned";

    private readonly IFlightAssignmentsRepository _repository;
    private readonly IFlightsRepository _flightsRepository;
    private readonly IPersonalRepository _personalRepository;
    private readonly IFlightRolesRepository _flightRolesRepository;
    private readonly IAvailabilityStatesRepository _availabilityStatesRepository;
    private readonly IStaffAvailabilityRepository _staffAvailabilityRepository;
    private readonly CheckStaffAvailableUseCase _checkStaffAvailableUseCase;
    private readonly IUnitOfWork _unitOfWork;

    public CreateFlightAssignmentUseCase(
        IFlightAssignmentsRepository repository,
        IFlightsRepository flightsRepository,
        IPersonalRepository personalRepository,
        IFlightRolesRepository flightRolesRepository,
        IAvailabilityStatesRepository availabilityStatesRepository,
        IStaffAvailabilityRepository staffAvailabilityRepository,
        CheckStaffAvailableUseCase checkStaffAvailableUseCase,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _flightsRepository = flightsRepository;
        _personalRepository = personalRepository;
        _flightRolesRepository = flightRolesRepository;
        _availabilityStatesRepository = availabilityStatesRepository;
        _staffAvailabilityRepository = staffAvailabilityRepository;
        _checkStaffAvailableUseCase = checkStaffAvailableUseCase;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(int flightId, int staffId, int flightRoleId, CancellationToken cancellationToken = default)
    {
        var flight = await FlightAssignmentRules.GetExistingFlightAsync(_flightsRepository, flightId, cancellationToken);
        await FlightAssignmentRules.EnsureStaffExistsAsync(_personalRepository, staffId);
        await FlightAssignmentRules.EnsureRoleExistsAsync(_flightRolesRepository, flightRoleId, cancellationToken);

        var duplicate = await _repository.GetByFlightAndStaffAsync(flight.Id!, FlightAssignmentRules.CreateStaffId(staffId), cancellationToken);
        if (duplicate is not null)
            throw new InvalidOperationException($"El empleado {staffId} ya esta asignado al vuelo {flightId}.");

        var hasBlockingAvailability = await _checkStaffAvailableUseCase.ExecuteAsync(
            staffId,
            flight.DepartureAt.Value,
            flight.EstimatedArrivalAt.Value);

        if (hasBlockingAvailability)
            throw new InvalidOperationException("El empleado no esta disponible en el rango del vuelo.");

        var assignedState = await _availabilityStatesRepository.GetByNameAsync(AvailabilityStatesName.Create(AssignedStateName))
            ?? throw new InvalidOperationException("No existe el estado de disponibilidad 'Assigned'.");

        await _repository.SaveAsync(FlightAssignment.Create(flightId, staffId, flightRoleId), cancellationToken);

        await _staffAvailabilityRepository.SaveAsync(StaffAvailabilityRecord.Create(
            staffId,
            assignedState.Id!.Value,
            flight.DepartureAt.Value,
            flight.EstimatedArrivalAt.Value,
            FlightAssignmentRules.BuildAvailabilityNote(flightId)));

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
