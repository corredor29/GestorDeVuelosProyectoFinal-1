using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using FlightSeatAggregate = GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Domain.Aggregate.FlightSeat;

namespace GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Application.UseCases;

public sealed class UpdateFlightSeatUseCase
{
    private readonly IFlightSeatsRepository _repository;
    private readonly AppDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateFlightSeatUseCase(IFlightSeatsRepository repository, AppDbContext context, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task<FlightSeatAggregate> ExecuteAsync(
        int id,
        int cabinTypeId,
        int seatLocationTypeId,
        string code,
        bool isOccupied,
        CancellationToken cancellationToken = default)

    {
            var flightSeat = await _repository.GetByIdAsync(FlightSeatsId.Create(id), cancellationToken)
                ?? throw new InvalidOperationException($"No se encontro el asiento con id {id}.");
            
            if (flightSeat.FlightId is null)
                throw new InvalidOperationException($"El asiento con id {id} no pertenece a ninguna vuelta.");
            
            if (flightSeat.CabinTypeId is null)
                throw new InvalidOperationException($"El asiento con id {id} no pertenece a ningun tipo de cabina.");
            
            if (flightSeat.SeatLocationTypeId is null)
                throw new InvalidOperationException($"El asiento con id {id} no pertenece a ningun tipo de asiento.");

            var duplicate = await _repository.GetByCodeAsync(FlightSeatsCode.Create(code), cancellationToken);
            if (duplicate is not null && duplicate.Id?.Value != id)
                throw new InvalidOperationException($"Ya existe otro asiento con el código '{code}'.");

            flightSeat.Update(cabinTypeId, seatLocationTypeId, code, isOccupied);
            await _repository.UpdateAsync(flightSeat, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return flightSeat;
    }
}
