using GestorDeVuelosProyectoFinal.Moduls.SeatLocationTypes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.SeatLocationTypes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.SeatLocationTypes.Application.UseCases;

public sealed class CreateSeatLocationTypeUseCase
{
    private readonly ISeatLocationTypesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSeatLocationTypeUseCase(ISeatLocationTypesRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(string name, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByNameStringAsync(name, cancellationToken);
        if (existing is not null)
            throw new InvalidOperationException($"Already exists a type of seat with the name '{existing.Name.Value}'.");

        var aggregate = SeatLocationType.Create(name);
        await _repository.SaveAsync(aggregate, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
