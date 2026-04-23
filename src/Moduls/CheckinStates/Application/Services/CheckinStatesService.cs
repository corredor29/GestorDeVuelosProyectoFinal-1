using GestorDeVuelosProyectoFinal.src.Moduls.CheckinStates.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.CheckinStates.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.CheckinStates.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.CheckinStates.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.CheckinStates.Application.Services;

public sealed class CheckinStatesService : ICheckinStatesService
{
    private readonly ICheckinStatesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CheckinStatesService(
        ICheckinStatesRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CheckinState> CreateAsync(
        int id,
        string name,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(CheckinStatesId.Create(id), cancellationToken);

        if (existing is not null)
            throw new InvalidOperationException($"CheckinState with id '{id}' already exists.");

        var checkinState = CheckinState.Create(id, name);

        await _repository.SaveAsync(checkinState, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return checkinState;
    }

    public async Task<CheckinState?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var checkinStateId = CheckinStatesId.Create(id);
        return await _repository.GetByIdAsync(checkinStateId, cancellationToken);
    }

    public Task<IEnumerable<CheckinState>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return _repository.GetAllAsync(cancellationToken);
    }

    public async Task<CheckinState> UpdateAsync(
        int id,
        string newName,
        CancellationToken cancellationToken = default)
    {
        var checkinStateId = CheckinStatesId.Create(id);

        var existing = await _repository.GetByIdAsync(checkinStateId, cancellationToken);

        if (existing is null)
            throw new KeyNotFoundException($"CheckinState with id '{id}' was not found.");

        existing.UpdateName(newName);

        await _repository.SaveAsync(existing, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return existing;
    }

    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var checkinStateId = CheckinStatesId.Create(id);

        var existing = await _repository.GetByIdAsync(checkinStateId, cancellationToken);

        if (existing is null)
            return false;

        await _repository.DeleteAsync(checkinStateId, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}