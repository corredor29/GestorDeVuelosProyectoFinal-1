using GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Application.UseCases;

public sealed class DeletePassengerTypesUseCase
{
    private readonly IPassengerTypesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeletePassengerTypesUseCase(IPassengerTypesRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(PassengerTypesId.Create(id), cancellationToken);
        if (existing is null)
            throw new InvalidOperationException($"No se encontro el tipo de pasajero con id {id}.");

        await _repository.DeleteAsync(PassengerTypesId.Create(id), cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var nameVo = PassengerTypesName.Create(name);
        var existing = await _repository.GetByNameAsync(nameVo, cancellationToken);
        if (existing is null)
            throw new InvalidOperationException($"No se encontro el tipo de pasajero con nombre '{nameVo.Value}'.");

        await _repository.DeleteByNameAsync(nameVo, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> DeleteByAgeAsync(int ageInYears, CancellationToken cancellationToken = default)
    {
        var n = await _repository.DeleteByAgeAsync(ageInYears, cancellationToken);
        if (n > 0)
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        return n;
    }
}
