using GestorDeVuelosProyectoFinal.Moduls.StreetTypes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.StreetTypes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.StreetTypes.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.StreetTypes.Application.UseCases;

public sealed class GetStreetTypesUseCase
{
    private readonly IStreetTypesRepository _repository;
    public GetStreetTypesUseCase(IStreetTypesRepository repository) => _repository = repository;

    public async Task<IEnumerable<StreetType>> ExecuteAllAsync() => await _repository.GetAllAsync();
    public async Task<StreetType?> ExecuteByIdAsync(int id) => await _repository.GetByIdAsync(StreetTypeId.Create(id));
    public async Task<StreetType?> ExecuteByNameAsync(string name) => await _repository.GetByNameAsync(name);
}