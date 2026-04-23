using GestorDeVuelosProyectoFinal.src.Moduls.TicketStates.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.TicketStates.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.TicketStates.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.TicketStates.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.TicketStates.Application.Services;

public sealed class TicketStatesService : ITicketStatesService
{
    private readonly ITicketStatesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public TicketStatesService(
        ITicketStatesRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TicketState> CreateAsync(
        int id,
        string name,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(TicketStatesId.Create(id), cancellationToken);

        if (existing is not null)
            throw new InvalidOperationException($"TicketState with id '{id}' already exists.");

        var ticketState = TicketState.Create(id, name);

        await _repository.SaveAsync(ticketState, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ticketState;
    }

    public async Task<TicketState?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var ticketStateId = TicketStatesId.Create(id);
        return await _repository.GetByIdAsync(ticketStateId, cancellationToken);
    }

    public Task<IReadOnlyCollection<TicketState>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return _repository.GetAllAsync(cancellationToken);
    }

    public async Task<TicketState> UpdateAsync(
        int id,
        string name,
        CancellationToken cancellationToken = default)
    {
        var ticketStateId = TicketStatesId.Create(id);

        var existing = await _repository.GetByIdAsync(ticketStateId, cancellationToken);

        if (existing is null)
            throw new KeyNotFoundException($"TicketState with id '{id}' was not found.");

        existing.UpdateName(name);

        await _repository.SaveAsync(existing, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return existing;
    }

    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var ticketStateId = TicketStatesId.Create(id);

        var existing = await _repository.GetByIdAsync(ticketStateId, cancellationToken);

        if (existing is null)
            return false;

        await _repository.DeleteAsync(ticketStateId, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}