using GestorDeVuelosProyectoFinal.Moduls.StreetTypes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.StreetTypes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.StreetTypes.Application.UseCases;

public sealed class DeleteStreetTypeUseCase
{
    private readonly IStreetTypesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteStreetTypeUseCase(IStreetTypesRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(int id)
    {
        await _repository.DeleteAsync(StreetTypeId.Create(id));
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ExecuteByNameAsync(string name)
    {
        await _repository.DeleteByNameAsync(name);
        await _unitOfWork.SaveChangesAsync();
    }
}