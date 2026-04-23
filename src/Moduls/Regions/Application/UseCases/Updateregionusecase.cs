using GestorDeVuelosProyectoFinal.Moduls.Regions.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Regions.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.Regions.Application.UseCases;

public sealed class UpdateRegionUseCase
{
    private readonly IRegionsRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRegionUseCase(IRegionsRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(int id, string newName, string newType, int newCountryId)
    {
        var region = await _repository.GetByIdAsync(RegionId.Create(id))
            ?? throw new InvalidOperationException($"Region with id '{id}' not found.");

        // Check new name not taken by another region
        if (!string.Equals(region.Name.Value, newName, StringComparison.OrdinalIgnoreCase))
        {
            var duplicate = await _repository.GetByNameAsync(newName);
            if (duplicate is not null)
                throw new InvalidOperationException($"A region named '{newName}' already exists.");
        }

        region.UpdateName(newName);
        region.UpdateType(newType);
        region.UpdateCountry(newCountryId);

        await _repository.UpdateAsync(region);
        await _unitOfWork.SaveChangesAsync();
    }
}