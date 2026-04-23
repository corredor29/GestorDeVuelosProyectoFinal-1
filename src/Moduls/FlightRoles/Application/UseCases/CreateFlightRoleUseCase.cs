using GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Application.UseCases;

public sealed class CreateFlightRoleUseCase
{
    private readonly IFlightRolesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateFlightRoleUseCase(IFlightRolesRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(string name, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByNameStringAsync(name, cancellationToken);
        if (existing is not null)
            throw new InvalidOperationException($"Ya existe un rol de tripulacion con el nombre '{existing.Name.Value}'.");

        var aggregate = FlightRole.Create(name);
        await _repository.SaveAsync(aggregate, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
