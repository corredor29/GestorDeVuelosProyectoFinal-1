using GestorDeVuelosProyectoFinal.Moduls.Routes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Routes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.Routes.Application.UseCases;

public sealed class DeleteRouteUseCase
{
    private readonly IRoutesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteRouteUseCase(IRoutesRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteByIdAsync(int id)
    {
        var existing = await _repository.GetByIdAsync(RouteId.Create(id));
        if (existing is null)
            throw new InvalidOperationException($"No se encontro la ruta con ID {id}.");

        await _repository.DeleteByIdAsync(RouteId.Create(id));
        await _unitOfWork.SaveChangesAsync();
    }
}
