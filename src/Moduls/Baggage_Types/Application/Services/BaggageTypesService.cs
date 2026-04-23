using GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Application.Services;

public sealed class BaggageTypesService : IBaggageTypesService
{
    private readonly IBaggageTypesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public BaggageTypesService(
        IBaggageTypesRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<BaggageType> CreateAsync(
        int id,
        string name,
        decimal maxWeightKg,
        decimal basePrice,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByNameAsync(BaggageTypeName.Create(name));
        if (existing is not null)
            throw new InvalidOperationException($"BaggageType with name '{name}' already exists.");

        var baggageType = BaggageType.Create(id, name, maxWeightKg, basePrice);

        await _repository.SaveAsync(baggageType);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return baggageType;
    }

    public async Task<BaggageType?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _repository.GetByIdAsync(BaggageTypeId.Create(id));
    }

    public Task<IEnumerable<BaggageType>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return _repository.GetAllAsync();
    }

    public async Task<BaggageType> UpdateAsync(
        int id,
        string? newName,
        decimal? newMaxWeightKg,
        decimal? newBasePrice,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(BaggageTypeId.Create(id));
        if (existing is null)
            throw new KeyNotFoundException($"BaggageType with id '{id}' was not found.");

        if (newName is not null)
        {
            var nameInUse = await _repository.GetByNameAsync(BaggageTypeName.Create(newName));
            if (nameInUse is not null)
                throw new InvalidOperationException($"BaggageType with name '{newName}' already exists.");

            existing.UpdateName(newName);
        }

        if (newMaxWeightKg is not null)
            existing.UpdateMaxWeight(newMaxWeightKg.Value);

        if (newBasePrice is not null)
            existing.UpdateBasePrice(newBasePrice.Value);

        await _repository.UpdateAsync(existing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return existing;
    }

    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(BaggageTypeId.Create(id));
        if (existing is null)
            return false;

        await _repository.DeleteAsync(BaggageTypeId.Create(id));
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
