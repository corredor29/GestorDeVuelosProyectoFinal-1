using GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Application.UseCases;

public sealed class DeleteFlightRoleUseCase
{
    private readonly IFlightRolesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteFlightRoleUseCase(IFlightRolesRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(FlightRolesId.Create(id), cancellationToken);
        if (existing is null)
            throw new InvalidOperationException($"No se encontro el rol de tripulacion con id {id}.");

        await _repository.DeleteAsync(FlightRolesId.Create(id), cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task ExecuteByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var deleted = await _repository.DeleteByNameAsync(FlightRolesName.Create(name), cancellationToken);
        if (!deleted)
            throw new InvalidOperationException($"No se encontro el rol de tripulacion con nombre '{name}'.");

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
