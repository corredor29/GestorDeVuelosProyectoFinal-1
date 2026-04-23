using GestorDeVuelosProyectoFinal.Moduls.Cities.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Cities.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Regions.Domain.ValueObject; // Asegúrate de tener este using
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.Cities.Application.UseCases;

public sealed class DeleteCityUseCase
{
    private readonly ICityRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCityUseCase(ICityRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(int id)
    {
        await _repository.DeleteAsync(CityId.Create(id));
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ExecuteByNameAsync(string name)
    {
        await _repository.DeleteByNameAsync(name);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ExecuteByRegionAsync(int regionId)
    {
        // CORRECCIÓN: Usar RegionId.Create(regionId)
        await _repository.DeleteByCountryAsync(RegionId.Create(regionId));
        await _unitOfWork.SaveChangesAsync();
    }
}