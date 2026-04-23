using GestorDeVuelosProyectoFinal.Moduls.Cities.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.Cities.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.Cities.Application.UseCases;

public sealed class CreateCityUseCase
{
    private readonly ICityRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCityUseCase(ICityRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(string name, int regionId)
    {
        var existing = await _repository.GetByNameAsync(name);
        if (existing is not null)
            throw new InvalidOperationException($"A city named '{name}' already exists.");

        var city = City.Create(0, name, regionId);

        await _repository.SaveAsync(city);
        await _unitOfWork.SaveChangesAsync();
    }
}