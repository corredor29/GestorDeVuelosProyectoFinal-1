using GestorDeVuelosProyectoFinal.Moduls.Continents.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.Continents.Application.UseCases;

public sealed class DeleteContinentUseCase
{
    private readonly IContinentsRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteContinentUseCase(IContinentsRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(string name)
    {
        var continent = await _repository.GetByNameAsync(name)
            ?? throw new InvalidOperationException($"Continent '{name}' not found.");

        await _repository.DeleteByNameAsync(continent.Name.Value);
        await _unitOfWork.SaveChangesAsync();
    }
}