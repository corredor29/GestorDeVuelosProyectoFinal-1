using GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Application.Services;

public sealed class InvoiceItemTypesService : IInvoiceItemTypesService
{
    private readonly IInvoiceItemTypesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public InvoiceItemTypesService(
        IInvoiceItemTypesRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<InvoiceItemType> CreateAsync(
        int id,
        string name,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByNameAsync(InvoiceItemTypesName.Create(name));
        if (existing is not null)
            throw new InvalidOperationException($"InvoiceItemType with name '{name}' already exists.");

        var invoiceItemType = InvoiceItemType.Create(id, name);

        await _repository.SaveAsync(invoiceItemType);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return invoiceItemType;
    }

    public async Task<InvoiceItemType?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _repository.GetByIdAsync(InvoiceItemTypesId.Create(id));
    }

    public Task<IEnumerable<InvoiceItemType>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return _repository.GetAllAsync();
    }

    public async Task<InvoiceItemType> UpdateAsync(
        int id,
        string newName,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(InvoiceItemTypesId.Create(id));
        if (existing is null)
            throw new KeyNotFoundException($"InvoiceItemType with id '{id}' was not found.");

        var nameInUse = await _repository.GetByNameAsync(InvoiceItemTypesName.Create(newName));
        if (nameInUse is not null)
            throw new InvalidOperationException($"InvoiceItemType with name '{newName}' already exists.");

        existing.UpdateName(newName);

        await _repository.UpdateAsync(existing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return existing;
    }

    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(InvoiceItemTypesId.Create(id));
        if (existing is null)
            return false;

        await _repository.DeleteAsync(InvoiceItemTypesId.Create(id));
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
