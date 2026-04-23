using GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Application.UseCases;

public sealed class UpdatePassengerTypeUseCase
{
    private readonly IPassengerTypesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePassengerTypeUseCase(IPassengerTypesRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(int id, string name, int? minAge, int? maxAge, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(PassengerTypesId.Create(id), cancellationToken);
        if (existing is null)
            throw new InvalidOperationException($"No se encontro el tipo de pasajero con id {id}.");

        var newName = PassengerTypesName.Create(name);
        var duplicate = await _repository.GetByNameAsync(newName, cancellationToken);
        if (duplicate is not null && duplicate.Id?.Value != id)
            throw new InvalidOperationException($"Ya existe otro tipo con el nombre '{newName.Value}'.");

        existing.Update(name, minAge, maxAge);
        await _repository.UpdateAsync(existing, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
