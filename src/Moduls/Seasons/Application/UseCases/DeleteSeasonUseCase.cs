using GestorDeVuelosProyectoFinal.Moduls.Seasons.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Seasons.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.Seasons.Application.UseCases;

public sealed class DeleteSeasonUseCase
{
    private readonly ISeasonsRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteSeasonUseCase(ISeasonsRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteByIdAsync(int id)
    {
        var existing = await _repository.GetByIdAsync(SeasonsId.Create(id));
        if (existing is null)
            throw new InvalidOperationException($"No se encontro la temporada con ID {id}.");

        await _repository.DeleteByIdAsync(SeasonsId.Create(id));
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ExecuteByNameAsync(string name)
    {
        var existing = await _repository.GetByNameAsync(SeasonsName.Create(name));
        if (existing is null)
            throw new InvalidOperationException($"No se encontro la temporada con nombre '{name}'.");

        await _repository.DeleteByNameAsync(SeasonsName.Create(name));
        await _unitOfWork.SaveChangesAsync();
    }
}
