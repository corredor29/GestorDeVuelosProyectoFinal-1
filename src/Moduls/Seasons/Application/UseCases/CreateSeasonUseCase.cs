using GestorDeVuelosProyectoFinal.Moduls.Seasons.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.Seasons.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Seasons.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.Moduls.Seasons.Application.UseCases;

public sealed class CreateSeasonUseCase
{
    private readonly ISeasonsRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSeasonUseCase(ISeasonsRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(string name, string? description, decimal priceFactor)
    {
        var duplicate = await _repository.GetByNameAsync(SeasonsName.Create(name));
        if (duplicate is not null)
            throw new InvalidOperationException($"Ya existe una temporada con nombre '{name}'.");

        try
        {
            var season = Season.Create(name, description, priceFactor);
            await _repository.SaveAsync(season);
            await _unitOfWork.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            var inner = ex.InnerException?.Message;
            throw new InvalidOperationException(inner is null
                ? "No se pudo guardar la temporada. Verifique los datos e intente de nuevo."
                : $"No se pudo guardar la temporada: {inner}");
        }
    }
}
