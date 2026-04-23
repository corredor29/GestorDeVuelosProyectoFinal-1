using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Application.UseCases;

public sealed class CreatedFlightSeatUseCase
{
    private readonly IFlightSeatsRepository _repository;
    private readonly AppDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public CreatedFlightSeatUseCase(
        IFlightSeatsRepository repository,
        AppDbContext context, 
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task<FlightSeat> ExecuteAsync(
    int flightId,
    int cabinTypeId,
    int seatLocationTypeId,
    bool isOccupied,
    string code,
    CancellationToken cancellationToken = default)
{
        // 1. Validaciones usando Value Objects y el Contexto
        var seatCodeVo = FlightSeatsCode.Create(code);

        if (await _repository.ExistsByCodeAsync(seatCodeVo, cancellationToken))
            throw new InvalidOperationException($"Ya existe un asiento con el código '{code}' para este vuelo.");

        // Validamos existencia en las tablas maestras usando el DbContext
        if (!await _context.Flights.AsNoTracking().AnyAsync(x => x.Id == flightId, cancellationToken))
            throw new InvalidOperationException($"El vuelo con ID '{flightId}' no existe.");

        if (!await _context.CabinTypes.AsNoTracking().AnyAsync(x => x.Id == cabinTypeId, cancellationToken))
            throw new InvalidOperationException($"El tipo de cabina con ID '{cabinTypeId}' no existe.");

        // 2. Creación del Aggregate 
        var flightSeat = FlightSeat.Create(
            flightId,
            cabinTypeId,
            seatLocationTypeId,
            isOccupied,
            code);

        // 3. Persistencia
        await _repository.AddAsync(flightSeat, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return flightSeat;
    }   
}