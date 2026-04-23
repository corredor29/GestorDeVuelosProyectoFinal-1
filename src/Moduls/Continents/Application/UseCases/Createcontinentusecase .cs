using GestorDeVuelosProyectoFinal.Moduls.Continents.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.Continents.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.Continents.Application.UseCases;

public sealed class CreateContinentUseCase
{
    private readonly IContinentsRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateContinentUseCase(IContinentsRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(string name)
    {
        // Check for duplicates
        var existing = await _repository.GetByNameAsync(name);
        if (existing is not null)
            throw new InvalidOperationException($"A continent named '{name}' already exists.");

        var continent = Continent.Create(0, name);

        await _repository.SaveAsync(continent);
        await _unitOfWork.SaveChangesAsync();
    }
}