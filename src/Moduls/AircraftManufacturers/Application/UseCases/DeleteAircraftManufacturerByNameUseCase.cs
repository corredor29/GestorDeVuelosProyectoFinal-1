using GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Application.UseCases;

public sealed class DeleteAircraftManufacturerByNameUseCase
{
    private readonly IAircraftManufacturersRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAircraftManufacturerByNameUseCase(
        IAircraftManufacturersRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> ExecuteAsync(string name, CancellationToken cancellationToken = default)
    {
        var manufacturer = await _repository.GetByNameAsync(
            Domain.ValueObject.AircraftManufacturersName.Create(name),
            cancellationToken)
            ?? throw new KeyNotFoundException($"No se encontró el fabricante con nombre '{name}'.");

        if (await _repository.HasAircraftModelsAsync(manufacturer.Id, cancellationToken))
            throw new InvalidOperationException("No se puede eliminar el fabricante porque tiene modelos asociados.");

        var deleted = await _repository.DeleteByIdAsync(manufacturer.Id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return deleted;
    }
}
