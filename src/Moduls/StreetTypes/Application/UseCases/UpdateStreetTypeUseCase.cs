using GestorDeVuelosProyectoFinal.Moduls.StreetTypes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.StreetTypes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.StreetTypes.Application.UseCases;

public sealed class UpdateStreetTypeUseCase
{
    private readonly IStreetTypesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateStreetTypeUseCase(IStreetTypesRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(int id, string newName)
    {
        var streetType = await _repository.GetByIdAsync(StreetTypeId.Create(id))
            ?? throw new InvalidOperationException($"No existe tipo de vía con id {id}.");

        streetType.UpdateName(newName);
        await _repository.SaveAsync(streetType);
        await _unitOfWork.SaveChangesAsync();
    }
}
