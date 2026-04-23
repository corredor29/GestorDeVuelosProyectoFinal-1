using GestorDeVuelosProyectoFinal.src.Moduls.CardTypes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.CardTypes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.CardTypes.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;
using DomainAggregate = GestorDeVuelosProyectoFinal.src.Moduls.CardTypes.Domain.Aggregate.CardTypes;

namespace GestorDeVuelosProyectoFinal.src.Moduls.CardTypes.Application.Services;

public sealed class CardTypesService : ICardTypesService
{
    private readonly ICardTypesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CardTypesService(
        ICardTypesRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<DomainAggregate> CreateAsync(
        int id,
        string name,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByNameAsync(CardTypesName.Create(name));
        if (existing is not null)
            throw new InvalidOperationException($"CardType with name '{name}' already exists.");

        var cardType = DomainAggregate.Create(id, name);

        await _repository.SaveAsync(cardType);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return cardType;
    }

    public async Task<DomainAggregate?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _repository.GetByIdAsync(CardTypesId.Create(id));
    }

    public Task<IEnumerable<DomainAggregate>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return _repository.GetAllAsync();
    }

    public async Task<DomainAggregate> UpdateAsync(
        int id,
        string? newName,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(CardTypesId.Create(id));
        if (existing is null)
            throw new KeyNotFoundException($"CardType with id '{id}' was not found.");

        if (newName is not null)
        {
            var nameInUse = await _repository.GetByNameAsync(CardTypesName.Create(newName));
            if (nameInUse is not null)
                throw new InvalidOperationException($"CardType with name '{newName}' already exists.");

            existing.UpdateName(newName);
        }

        await _repository.UpdateAsync(existing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return existing;
    }

    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(CardTypesId.Create(id));
        if (existing is null)
            return false;

        await _repository.DeleteAsync(CardTypesId.Create(id));
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}