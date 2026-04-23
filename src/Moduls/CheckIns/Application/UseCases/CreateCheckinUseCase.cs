using GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Application.UseCases;

public sealed class CreateCheckinUseCase
{
    private readonly ICheckinsRepository _repository;

    public CreateCheckinUseCase(ICheckinsRepository repository)
    {
        _repository = repository;
    }

    public async Task<Checkin> ExecuteAsync(
        int id,
        int ticketId,
        int staffId,
        int flightSeatId,
        DateTime checkedInAt,
        int checkinStatusId,
        string boardingPassNumber,
        CancellationToken cancellationToken = default)
    {
        var existingBoardingPass = await _repository.GetByBoardingPassAsync(
            CheckinsBoardingPassNumber.Create(boardingPassNumber));
        if (existingBoardingPass is not null)
            throw new InvalidOperationException($"Checkin with boarding pass '{boardingPassNumber}' already exists.");

        var existingTicket = await _repository.GetByTicketIdAsync(TicketId.Create(ticketId));
        if (existingTicket is not null)
            throw new InvalidOperationException($"Checkin for ticket id '{ticketId}' already exists.");

        var checkin = Checkin.Create(id, ticketId, staffId, flightSeatId, checkedInAt, checkinStatusId, boardingPassNumber);

        await _repository.SaveAsync(checkin);

        return checkin;
    }
}