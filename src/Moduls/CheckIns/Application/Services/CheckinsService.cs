using GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Application.Services;

public sealed class CheckinsService : ICheckinsService
{
    private readonly ICheckinsRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CheckinsService(ICheckinsRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Checkin> CreateAsync(
        int id, int ticketId, int staffId, int flightSeatId,
        DateTime checkedInAt, int checkinStatusId, string boardingPassNumber,
        CancellationToken cancellationToken = default)
    {
        var existingBoardingPass = await _repository.GetByBoardingPassAsync(
            CheckinsBoardingPassNumber.Create(boardingPassNumber));
        if (existingBoardingPass is not null)
            throw new InvalidOperationException($"Checkin with boarding pass '{boardingPassNumber}' already exists.");

        var existingTicket = await _repository.GetByTicketIdAsync(
            GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Domain.ValueObject.TicketId.Create(ticketId));
        if (existingTicket is not null)
            throw new InvalidOperationException($"Checkin for ticket id '{ticketId}' already exists.");

        var checkin = Checkin.Create(id, ticketId, staffId, flightSeatId, checkedInAt, checkinStatusId, boardingPassNumber);

        await _repository.SaveAsync(checkin);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return checkin;
    }

    public async Task<Checkin?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _repository.GetByIdAsync(CheckinsId.Create(id));

    public Task<IEnumerable<Checkin>> GetAllAsync(CancellationToken cancellationToken = default)
        => _repository.GetAllAsync();

    public async Task<Checkin> UpdateAsync(int id, int? newCheckinStatusId, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(CheckinsId.Create(id));
        if (existing is null)
            throw new KeyNotFoundException($"Checkin with id '{id}' was not found.");

        if (newCheckinStatusId is not null)
            existing.UpdateCheckinStatus(newCheckinStatusId.Value);

        await _repository.UpdateAsync(existing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return existing;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(CheckinsId.Create(id));
        if (existing is null)
            return false;

        await _repository.DeleteAsync(CheckinsId.Create(id));
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}