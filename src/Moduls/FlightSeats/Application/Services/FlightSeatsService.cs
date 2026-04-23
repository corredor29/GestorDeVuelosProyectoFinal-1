using GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Application.UseCases;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Application.Services;

public class FlightSeatsService : IFlightSeatsService
{
    private readonly GetFlightSeatUseCase _get;
    private readonly CreatedFlightSeatUseCase _create;
    private readonly UpdateFlightSeatUseCase _update;
    private readonly DeleteFlightSeatUseCase _delete;
    private readonly ReactiveFlightSeatUseCase _reactive;

    public FlightSeatsService(
        GetFlightSeatUseCase get,
        CreatedFlightSeatUseCase create,
        UpdateFlightSeatUseCase update,
        DeleteFlightSeatUseCase delete,
        ReactiveFlightSeatUseCase reactive)
    {
        _get = get;
        _create = create;
        _update = update;
        _delete = delete;
        _reactive = reactive;
    }

    public Task<FlightSeat> CreateAsync(int flightId, int cabinTypeId, int seatLocationTypeId, bool isOccupied, string code, CancellationToken cancellationToken = default)
        => _create.ExecuteAsync(flightId, cabinTypeId, seatLocationTypeId, isOccupied, code, cancellationToken);

    public Task<FlightSeat?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => _get.ExecuteByIdAsync(id);

    public Task<FlightSeat?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
        => _get.ExecuteByCodeAsync(code);

    public Task<IReadOnlyCollection<FlightSeat>> GetByFlightIdAsync(int flightId, CancellationToken cancellationToken = default)
        => _get.ExecuteByFlightIdAsync(flightId);

    public Task<IReadOnlyCollection<FlightSeat>> GetAllAsync(CancellationToken cancellationToken = default)
        => _get.ExecuteAllAsync(cancellationToken);

    public Task<FlightSeat> UpdateAsync(int id, int flightId, int cabinTypeId, int seatLocationTypeId, bool isOccupied, string code, CancellationToken cancellationToken = default)
        => _update.ExecuteAsync(id, cabinTypeId, seatLocationTypeId, code, isOccupied, cancellationToken);

    public async Task<bool> isOccupiedAsync(int id, CancellationToken cancellationToken = default)
    {
        var seat = await _get.ExecuteByIdAsync(id);
        return seat?.IsOccupied.Value ?? false;
    }
    public Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
        => _delete.ExecuteAsync(id);

    // Métodos faltantes de la interfaz
    public Task<IReadOnlyCollection<FlightSeat>> GetByCabinTypeIdAsync(int cabinTypeId, CancellationToken cancellationToken = default)
        => _get.ExecuteByCabinTypeIdAsync(cabinTypeId, cancellationToken); 

    public Task<IReadOnlyCollection<FlightSeat>> GetBySeatLocationTypeIdAsync(int seatLocationTypeId, CancellationToken cancellationToken = default)
         => _get.ExecuteBySeatLocationTypeIdAsync(seatLocationTypeId, cancellationToken);
}