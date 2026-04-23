using GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Application.UseCases;

public sealed class DeleteAircraftManufacturerByIdUseCase
{
    private readonly IAircraftManufacturersRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAircraftManufacturerByIdUseCase(
        IAircraftManufacturersRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        var manufacturerId = AircraftManufacturersId.Create(id);
        var manufacturer = await _repository.GetByIdAsync(manufacturerId, cancellationToken)
            ?? throw new KeyNotFoundException($"No se encontró el fabricante con ID '{id}'.");

        if (await _repository.HasAircraftModelsAsync(manufacturer.Id, cancellationToken))
            throw new InvalidOperationException("No se puede eliminar el fabricante porque tiene modelos asociados.");

        var deleted = await _repository.DeleteByIdAsync(manufacturer.Id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return deleted;
    }
}
