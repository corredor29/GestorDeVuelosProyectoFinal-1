using GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.FlightAssignments.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.FlightAssignments.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.FlightAssignments.Application.UseCases;

public sealed class DeleteFlightAssignmentUseCase
{
    private const string AssignedStateName = "Assigned";

    private readonly IFlightAssignmentsRepository _repository;
    private readonly IFlightsRepository _flightsRepository;
    private readonly IAvailabilityStatesRepository _availabilityStatesRepository;
    private readonly IStaffAvailabilityRepository _staffAvailabilityRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteFlightAssignmentUseCase(
        IFlightAssignmentsRepository repository,
        IFlightsRepository flightsRepository,
        IAvailabilityStatesRepository availabilityStatesRepository,
        IStaffAvailabilityRepository staffAvailabilityRepository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _flightsRepository = flightsRepository;
        _availabilityStatesRepository = availabilityStatesRepository;
        _staffAvailabilityRepository = staffAvailabilityRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var assignment = await _repository.GetByIdAsync(FlightAssignmentId.Create(id), cancellationToken);
        if (assignment is null)
            throw new InvalidOperationException($"No se encontro la asignacion de tripulacion con id {id}.");

        var flight = await FlightAssignmentRules.GetExistingFlightAsync(_flightsRepository, assignment.FlightId.Value, cancellationToken);
        var assignedState = await _availabilityStatesRepository.GetByNameAsync(AvailabilityStatesName.Create(AssignedStateName))
            ?? throw new InvalidOperationException("No existe el estado de disponibilidad 'Assigned'.");

        await _staffAvailabilityRepository.DeleteExactBlockAsync(
            assignment.StaffId,
            assignedState.Id!,
            flight.DepartureAt.Value,
            flight.EstimatedArrivalAt.Value,
            FlightAssignmentRules.BuildAvailabilityNote(flight.Id!.Value));

        await _repository.DeleteAsync(FlightAssignmentId.Create(id), cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
