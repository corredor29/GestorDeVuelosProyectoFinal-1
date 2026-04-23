using GestorDeVuelosProyectoFinal.Moduls.CabinTypes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.CabinTypes.Domain.Repositories;

namespace GestorDeVuelosProyectoFinal.Moduls.CabinTypes.Application.UseCases;

public sealed class GetCabinTypesUseCase
{
    private readonly ICabinTypesRepository _repository;

    public GetCabinTypesUseCase(ICabinTypesRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<CabinType>> Execute()
        => _repository.GetAllAsync();
}
