using GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Application.UseCases;

public sealed class DeleteAircraftManufacturersByCountryUseCase
{
    private readonly IAircraftManufacturersRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAircraftManufacturersByCountryUseCase(
        IAircraftManufacturersRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> ExecuteAsync(string country, CancellationToken cancellationToken = default)
    {
        var manufacturerCountry = AircraftManufacturersCountry.Create(country);
        var manufacturers = await _repository.GetByCountryAsync(manufacturerCountry, cancellationToken);

        if (manufacturers.Count == 0)
            throw new KeyNotFoundException($"No se encontraron fabricantes para el país '{country}'.");

        var blocked = new List<string>();

        foreach (var manufacturer in manufacturers)
        {
            if (await _repository.HasAircraftModelsAsync(manufacturer.Id, cancellationToken))
                blocked.Add(manufacturer.Name.Value);
        }

        if (blocked.Count > 0)
        {
            throw new InvalidOperationException(
                $"No se puede eliminar por país porque estos fabricantes tienen modelos asociados: {string.Join(", ", blocked)}.");
        }

        var deleted = await _repository.DeleteByCountryAsync(manufacturerCountry, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return deleted;
    }
}
