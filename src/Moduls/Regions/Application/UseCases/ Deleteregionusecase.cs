using GestorDeVuelosProyectoFinal.Moduls.Regions.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.Regions.Application.UseCases;

public sealed class DeleteRegionUseCase
{
    private readonly IRegionsRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteRegionUseCase(IRegionsRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task DeleteByNameAsync(string name)
    {
        var region = await _repository.GetByNameAsync(name)
            ?? throw new InvalidOperationException($"Region '{name}' not found.");

        await _repository.DeleteByNameAsync(region.Name.Value);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteByTypeAsync(string type)
    {
        await _repository.DeleteByTypeAsync(type);
        await _unitOfWork.SaveChangesAsync();
    }
}