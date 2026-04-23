using GestorDeVuelosProyectoFinal.Moduls.Continents.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.Continents.Application.UseCases;

public sealed class UpdateContinentUseCase
{
    private readonly IContinentsRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateContinentUseCase(IContinentsRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(string currentName, string newName)
    {
        var continent = await _repository.GetByNameAsync(currentName)
            ?? throw new InvalidOperationException($"Continent '{currentName}' not found.");

        // Check new name is not already taken
        if (!string.Equals(currentName, newName, StringComparison.OrdinalIgnoreCase))
        {
            var duplicate = await _repository.GetByNameAsync(newName);
            if (duplicate is not null)
                throw new InvalidOperationException($"A continent named '{newName}' already exists.");
        }

        continent.UpdateName(newName);

        await _repository.UpdateAsync(continent);
        await _unitOfWork.SaveChangesAsync();
    }
}