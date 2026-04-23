using GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Application.UseCases;

public sealed class DeleteAircraftUseCase
{
    private readonly IAircraftRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAircraftUseCase(IAircraftRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        var aircraft = await _repository.GetByIdAsync(AircraftId.Create(id), cancellationToken)
            ?? throw new KeyNotFoundException($"Avión con id '{id}' no encontrado.");

        if (await _repository.HasFutureFlightsAsync(aircraft.Id, cancellationToken))
            throw new InvalidOperationException("No se puede desactivar la aeronave porque tiene vuelos futuros.");

        aircraft.Deactivate();
        await _repository.UpdateAsync(aircraft, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
