using GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Application.UseCases;

public sealed class UpdateFlightRoleUseCase
{
    private readonly IFlightRolesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateFlightRoleUseCase(IFlightRolesRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(int id, string newName, CancellationToken cancellationToken = default)
    {
        var flightRole = await _repository.GetByIdAsync(FlightRolesId.Create(id), cancellationToken);
        if (flightRole is null)
            throw new InvalidOperationException($"No se encontro el rol de tripulacion con id {id}.");

        var duplicate = await _repository.GetByNameStringAsync(newName, cancellationToken);
        if (duplicate is not null && duplicate.Id?.Value != id)
            throw new InvalidOperationException($"Ya existe otro rol de tripulacion con el nombre '{newName}'.");

        flightRole.UpdateName(newName);
        await _repository.UpdateAsync(flightRole, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
