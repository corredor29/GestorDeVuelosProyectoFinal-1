using GestorDeVuelosProyectoFinal.Moduls.Cities.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Cities.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.Cities.Application.UseCases;

public sealed class UpdateCityUseCase
{
    private readonly ICityRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCityUseCase(ICityRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(int id, string newName, int newRegionId)
    {
        var city = await _repository.GetByIdAsync(CityId.Create(id))
            ?? throw new InvalidOperationException($"City with id '{id}' not found.");

       
        if (!string.Equals(city.Name.Value, newName, StringComparison.OrdinalIgnoreCase))
        {
            var duplicate = await _repository.GetByNameAsync(newName);
            if (duplicate is not null)
                throw new InvalidOperationException($"A city named '{newName}' already exists.");
        }

        city.UpdateName(newName);
        city.UpdateRegion(newRegionId);

        await _repository.SaveAsync(city);
        await _unitOfWork.SaveChangesAsync();
    }
}