using GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.FlightAssignments.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.FlightAssignments.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Personal.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.FlightAssignments.Application.UseCases;

public sealed class UpdateFlightAssignmentUseCase
{
    private const string AssignedStateName = "Assigned";

    private readonly IFlightAssignmentsRepository _repository;
    private readonly IFlightsRepository _flightsRepository;
    private readonly IPersonalRepository _personalRepository;
    private readonly IFlightRolesRepository _flightRolesRepository;
    private readonly IAvailabilityStatesRepository _availabilityStatesRepository;
    private readonly CheckStaffAvailableUseCase _checkStaffAvailableUseCase;
    private readonly GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.Domain.Repositories.IStaffAvailabilityRepository _staffAvailabilityRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateFlightAssignmentUseCase(
        IFlightAssignmentsRepository repository,
        IFlightsRepository flightsRepository,
        IPersonalRepository personalRepository,
        IFlightRolesRepository flightRolesRepository,
        IAvailabilityStatesRepository availabilityStatesRepository,
        GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.Domain.Repositories.IStaffAvailabilityRepository staffAvailabilityRepository,
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

    public async Task ExecuteAsync(int id, int flightId, int staffId, int flightRoleId, CancellationToken cancellationToken = default)
    {
        var assignment = await _repository.GetByIdAsync(FlightAssignmentId.Create(id), cancellationToken);
        if (assignment is null)
            throw new InvalidOperationException($"No se encontro la asignacion de tripulacion con id {id}.");

        var oldFlight = await FlightAssignmentRules.GetExistingFlightAsync(_flightsRepository, assignment.FlightId.Value, cancellationToken);
        var newFlight = await FlightAssignmentRules.GetExistingFlightAsync(_flightsRepository, flightId, cancellationToken);
        await FlightAssignmentRules.EnsureStaffExistsAsync(_personalRepository, staffId);
        await FlightAssignmentRules.EnsureRoleExistsAsync(_flightRolesRepository, flightRoleId, cancellationToken);

        var duplicate = await _repository.GetByFlightAndStaffAsync(
            FlightAssignmentRules.CreateFlightId(flightId),
            FlightAssignmentRules.CreateStaffId(staffId),
            cancellationToken);

        if (duplicate is not null && duplicate.Id?.Value != id)
            throw new InvalidOperationException($"El empleado {staffId} ya esta asignado al vuelo {flightId}.");

        var assignedState = await _availabilityStatesRepository.GetByNameAsync(AvailabilityStatesName.Create(AssignedStateName))
            ?? throw new InvalidOperationException("No existe el estado de disponibilidad 'Assigned'.");

        var changingScheduleOrStaff =
            assignment.FlightId.Value != flightId ||
            assignment.StaffId.Value != staffId;

        if (changingScheduleOrStaff)
        {
            bool hasBlockingAvailability;

            if (assignment.StaffId.Value == staffId)
            {
                hasBlockingAvailability = await _staffAvailabilityRepository.HasBlockingAvailabilityExcludingExactBlockAsync(
                    assignment.StaffId,
                    newFlight.DepartureAt.Value,
                    newFlight.EstimatedArrivalAt.Value,
                    assignedState.Id!,
                    oldFlight.DepartureAt.Value,
                    oldFlight.EstimatedArrivalAt.Value,
                    FlightAssignmentRules.BuildAvailabilityNote(oldFlight.Id!.Value));
            }
            else
            {
                hasBlockingAvailability = await _checkStaffAvailableUseCase.ExecuteAsync(
                    staffId,
                    newFlight.DepartureAt.Value,
                    newFlight.EstimatedArrivalAt.Value);
            }

            if (hasBlockingAvailability)
                throw new InvalidOperationException("El empleado no esta disponible en el rango del vuelo.");
        }

        if (changingScheduleOrStaff)
        {
            await _staffAvailabilityRepository.DeleteExactBlockAsync(
                assignment.StaffId,
                assignedState.Id!,
                oldFlight.DepartureAt.Value,
                oldFlight.EstimatedArrivalAt.Value,
                FlightAssignmentRules.BuildAvailabilityNote(oldFlight.Id!.Value));
        }

        assignment.Update(flightId, staffId, flightRoleId);
        await _repository.UpdateAsync(assignment, cancellationToken);

        if (changingScheduleOrStaff)
        {
            await _staffAvailabilityRepository.SaveAsync(
                GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.Domain.Aggregate.StaffAvailabilityRecord.Create(
                    staffId,
                    assignedState.Id!.Value,
                    newFlight.DepartureAt.Value,
                    newFlight.EstimatedArrivalAt.Value,
                    FlightAssignmentRules.BuildAvailabilityNote(flightId)));
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
