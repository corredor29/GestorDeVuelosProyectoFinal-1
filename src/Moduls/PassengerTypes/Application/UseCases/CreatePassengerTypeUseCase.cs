using GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Application.UseCases;

public sealed class CreatePassengerTypeUseCase
{
    private readonly IPassengerTypesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePassengerTypeUseCase(IPassengerTypesRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(string name, int? minAge, int? maxAge, CancellationToken cancellationToken = default)
    {
        var nameVo = PassengerTypesName.Create(name);
        var existing = await _repository.GetByNameAsync(nameVo, cancellationToken);
        if (existing is not null)
            throw new InvalidOperationException($"Ya existe un tipo de pasajero con el nombre '{nameVo.Value}'.");

        var aggregate = PassengerType.Create(name, minAge, maxAge);
        await _repository.SaveAsync(aggregate, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
