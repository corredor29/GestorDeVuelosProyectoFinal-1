using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;
using AircraftAggregate = GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Domain.Aggregate.Aircraft;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Application.UseCases;

public sealed class UpdtaeAircraftUseCase
{
    private readonly IAircraftRepository _repository;
    private readonly AppDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public UpdtaeAircraftUseCase(IAircraftRepository repository, AppDbContext context, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task<AircraftAggregate> ExecuteAsync(
        int id,
        int modelId,
        int airlineId,
        string registration,
        DateTime? manufacturedDate,
        bool isActive,
        CancellationToken cancellationToken = default)
    {
        var aircraft = await _repository.GetByIdAsync(AircraftId.Create(id), cancellationToken)
            ?? throw new KeyNotFoundException($"Avión con id '{id}' no encontrado.");

        if (!await _context.AircraftModels.AsNoTracking().AnyAsync(x => x.Id == modelId, cancellationToken))
            throw new InvalidOperationException($"El modelo con ID '{modelId}' no existe.");

        if (!await _context.Airlines.AsNoTracking().AnyAsync(x => x.Id == airlineId, cancellationToken))
            throw new InvalidOperationException($"La aerolínea con ID '{airlineId}' no existe.");

        var registrationVo = AircraftRegistration.Create(registration);
        var duplicate = await _repository.GetByRegistrationAsync(registrationVo, cancellationToken);
        if (duplicate is not null && duplicate.Id.Value != id)
            throw new InvalidOperationException($"Ya existe otra aeronave con la matrícula '{registrationVo.Value}'.");

        aircraft.Update(modelId, airlineId, registration, manufacturedDate, isActive);
        await _repository.UpdateAsync(aircraft, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return aircraft;
    }
}
