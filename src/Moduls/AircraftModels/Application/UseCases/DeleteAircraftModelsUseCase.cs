using GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Application.UseCases;

public sealed class DeleteAircraftModelsUseCase
{
    private readonly IAircraftModelsRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAircraftModelsUseCase(IAircraftModelsRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        var modelId = AircraftModelId.Create(id);

        _ = await _repository.FindByIdAsync(modelId, cancellationToken)
            ?? throw new KeyNotFoundException($"AircraftModel con ID '{id}' no encontrado.");

        if (await _repository.HasAircraftAsync(modelId, cancellationToken))
            throw new InvalidOperationException("No se puede eliminar el modelo porque tiene aeronaves asociadas.");

        var deleted = await _repository.DeleteByIdAsync(modelId, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return deleted;
    }
}
