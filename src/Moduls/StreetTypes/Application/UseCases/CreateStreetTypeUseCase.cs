using GestorDeVuelosProyectoFinal.Moduls.StreetTypes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.StreetTypes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.StreetTypes.Application.UseCases;

public sealed class CreateStreetTypeUseCase
{
    private readonly IStreetTypesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateStreetTypeUseCase(IStreetTypesRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(string name)
    {
        var streetType = StreetType.Create(0, name); // 0 porque es AUTO_INCREMENT
        await _repository.AddAsync(streetType);
        await _unitOfWork.SaveChangesAsync();
    }
}