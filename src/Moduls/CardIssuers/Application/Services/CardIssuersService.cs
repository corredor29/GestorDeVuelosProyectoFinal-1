using GestorDeVuelosProyectoFinal.Moduls.CardIssuers.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.CardIssuers.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.CardIssuers.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;
using DomainAggregate = GestorDeVuelosProyectoFinal.Moduls.CardIssuers.Domain.Aggregate.CardIssuer;

namespace GestorDeVuelosProyectoFinal.Moduls.CardIssuers.Application.Services;

public sealed class CardIssuersService : ICardIssuersService
{
    private readonly ICardIssuersRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CardIssuersService(
        ICardIssuersRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<DomainAggregate> CreateAsync(
        int id,
        string name,
        string issuerNumber,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByNameAsync(CardIssuersName.Create(name));
        if (existing is not null)
            throw new InvalidOperationException($"CardIssuer with name '{name}' already exists.");

        var cardIssuer = DomainAggregate.Create(id, name, issuerNumber);

        await _repository.SaveAsync(cardIssuer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return cardIssuer;
    }

    public async Task<DomainAggregate?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _repository.GetByIdAsync(CardIssuersId.Create(id));
    }

    public Task<IEnumerable<DomainAggregate>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return _repository.GetAllAsync();
    }

    public async Task<DomainAggregate> UpdateAsync(
        int id,
        string? newName,
        string? newIssuerNumber,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(CardIssuersId.Create(id));
        if (existing is null)
            throw new KeyNotFoundException($"CardIssuer with id '{id}' was not found.");

        if (newName is not null)
        {
            var nameInUse = await _repository.GetByNameAsync(CardIssuersName.Create(newName));
            if (nameInUse is not null)
                throw new InvalidOperationException($"CardIssuer with name '{newName}' already exists.");

            existing.UpdateName(newName);
        }

        if (newIssuerNumber is not null)
            existing.UpdateIssuerNumber(newIssuerNumber);

        await _repository.UpdateAsync(existing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return existing;
    }

    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(CardIssuersId.Create(id));
        if (existing is null)
            return false;

        await _repository.DeleteAsync(CardIssuersId.Create(id));
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}