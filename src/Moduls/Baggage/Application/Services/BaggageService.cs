using GestorDeVuelosProyectoFinal.src.Moduls.Baggage.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Baggage.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Baggage.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Baggage.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Baggage.Application.Services;

public sealed class BaggageService : IBaggageService
{
    private readonly IBaggageRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public BaggageService(
        IBaggageRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<BaggageRoot> CreateAsync(
        int id,
        int checkinId,
        int baggageTypeId,
        decimal weightKg,
        decimal chargedPrice,
        CancellationToken cancellationToken = default)
    {
        var baggage = BaggageRoot.Create(id, checkinId, baggageTypeId, weightKg, chargedPrice);

        await _repository.SaveAsync(baggage);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return baggage;
    }

    public async Task<BaggageRoot?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _repository.FindByIdAsync(BaggageId.Create(id));
    }

    public Task<IEnumerable<BaggageRoot>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return _repository.FindAllAsync();
    }

    public async Task<BaggageRoot> UpdateAsync(
        int id,
        decimal? newWeightKg,
        decimal? newChargedPrice,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindByIdAsync(BaggageId.Create(id));
        if (existing is null)
            throw new KeyNotFoundException($"Baggage with id '{id}' was not found.");

        if (newWeightKg is not null)
            existing.UpdateWeightKg(newWeightKg.Value);

        if (newChargedPrice is not null)
            existing.UpdateChargedPrice(newChargedPrice.Value);

        await _repository.UpdateAsync(existing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return existing;
    }

    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindByIdAsync(BaggageId.Create(id));
        if (existing is null)
            return false;

        await _repository.DeleteAsync(BaggageId.Create(id));
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}