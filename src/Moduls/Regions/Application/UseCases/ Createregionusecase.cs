using GestorDeVuelosProyectoFinal.Moduls.Regions.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.Regions.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.Regions.Application.UseCases;

public sealed class CreateRegionUseCase
{
    private readonly IRegionsRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRegionUseCase(IRegionsRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(string name, string type, int countryId)
    {
        var existing = await _repository.GetByNameAsync(name);
        if (existing is not null)
            throw new InvalidOperationException($"A region named '{name}' already exists.");

        var region = Region.Create(0, name, type, countryId);

        await _repository.SaveAsync(region);
        await _unitOfWork.SaveChangesAsync();
    }
}
