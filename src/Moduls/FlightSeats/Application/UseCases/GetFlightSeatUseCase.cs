using GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.CabinTypes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.SeatLocationTypes.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Application.UseCases;

public sealed class GetFlightSeatUseCase
{
    private readonly IFlightSeatsRepository _repository;

    public GetFlightSeatUseCase(IFlightSeatsRepository repository)
    {
        _repository = repository;
    }

    public Task<IReadOnlyCollection<FlightSeat>> ExecuteAllAsync(CancellationToken cancellationToken = default) 
    {
        return _repository.GetAllAsync(cancellationToken);
    }
    
    public Task<FlightSeat?> ExecuteByIdAsync(int id, CancellationToken cancellationToken = default) =>
        _repository.GetByIdAsync(FlightSeatsId.Create(id), cancellationToken);

    public Task<IReadOnlyCollection<FlightSeat>> ExecuteByFlightIdAsync(int flightId, CancellationToken cancellationToken = default) =>
        _repository.GetByFlightIdAsync(FlightsId.Create(flightId), cancellationToken);

    public Task<FlightSeat?> ExecuteByCodeAsync(string code, CancellationToken cancellationToken = default) =>
        _repository.GetByCodeAsync(FlightSeatsCode.Create(code), cancellationToken);
    
    public Task<IReadOnlyCollection<FlightSeat>> ExecuteByCabinTypeIdAsync(int cabinTypeId, CancellationToken cancellationToken = default) =>
        _repository.GetByCabinTypeIdAsync(CabinTypesId.Create(cabinTypeId), cancellationToken);
    
    public Task<IReadOnlyCollection<FlightSeat>> ExecuteBySeatLocationTypeIdAsync(int seatLocationTypeId, CancellationToken cancellationToken = default) =>
        _repository.GetBySeatLocationTypeIdAsync(SeatLocationTypesId.Create(seatLocationTypeId), cancellationToken);
}
