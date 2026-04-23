using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;
using AircraftAggregate = GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Domain.Aggregate.Aircraft;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Application.UseCases;

public sealed class CreateAircraftUseCase
{
    private readonly IAircraftRepository _repository;
    private readonly AppDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAircraftUseCase(IAircraftRepository repository, AppDbContext context, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task<AircraftAggregate> ExecuteAsync(
        int modelId,
        int airlineId,
        string registration,
        DateTime? manufacturedDate,
        bool isActive,
        CancellationToken cancellationToken = default)
    {
        var registrationVo = AircraftRegistration.Create(registration);

        if (await _repository.ExistsByRegistrationAsync(registrationVo, cancellationToken))
            throw new InvalidOperationException($"Ya existe una aeronave con la matrícula '{registrationVo.Value}'.");

        if (!await _context.AircraftModels.AsNoTracking().AnyAsync(x => x.Id == modelId, cancellationToken))
            throw new InvalidOperationException($"El modelo con ID '{modelId}' no existe.");

        if (!await _context.Airlines.AsNoTracking().AnyAsync(x => x.Id == airlineId, cancellationToken))
            throw new InvalidOperationException($"La aerolínea con ID '{airlineId}' no existe.");

        var aircraft = AircraftAggregate.Create(modelId, airlineId, registration, manufacturedDate, isActive);
        await _repository.AddAsync(aircraft, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return await _repository.GetByRegistrationAsync(registrationVo, cancellationToken)
            ?? throw new InvalidOperationException("La aeronave fue creada pero no pudo recuperarse.");
    }
}
