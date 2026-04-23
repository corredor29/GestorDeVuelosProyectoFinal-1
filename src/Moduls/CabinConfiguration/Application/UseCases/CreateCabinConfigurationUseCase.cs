using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.Moduls.CabinConfiguration.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;
using CabinConfigurationAggregate = GestorDeVuelosProyectoFinal.Moduls.CabinConfiguration.Domain.Aggregate.CabinConfiguration;

namespace GestorDeVuelosProyectoFinal.Moduls.CabinConfiguration.Application.UseCases;

public sealed class CreateCabinConfigurationUseCase
{
    private readonly ICabinConfigurationRepository _repository;
    private readonly AppDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCabinConfigurationUseCase(
        ICabinConfigurationRepository repository,
        AppDbContext context,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(
        int aircraftId,
        int cabinTypeId,
        int rowStart,
        int rowEnd,
        int seatsPerRow,
        string seatLetters)
    {
        await EnsureForeignKeysExistAsync(aircraftId, cabinTypeId);

        // Esta validación replica la restricción del índice único de EF/BD.
        // La dejamos en application para dar un error claro antes de llegar a la base.
        var duplicate = await _repository.GetByAircraftAndCabinTypeAsync(aircraftId, cabinTypeId);
        if (duplicate is not null)
            throw new InvalidOperationException(
                $"Ya existe una configuración para el avión {aircraftId} y el tipo de cabina {cabinTypeId}.");

        var configuration = CabinConfigurationAggregate.Create(
            aircraftId,
            cabinTypeId,
            rowStart,
            rowEnd,
            seatsPerRow,
            seatLetters);

        await _repository.SaveAsync(configuration);
        await _unitOfWork.SaveChangesAsync();
    }

    private async Task EnsureForeignKeysExistAsync(int aircraftId, int cabinTypeId)
    {
        // Validación de la relación 1:N Aircraft -> CabinConfiguration.
        // Complementa la FK física configurada en EF.
        var aircraftExists = await _context.Aircrafts
            .AsNoTracking()
            .AnyAsync(x => x.Id == aircraftId);

        if (!aircraftExists)
            throw new InvalidOperationException($"El aircraft con id {aircraftId} no existe.");

        // Validación de la relación 1:N CabinType -> CabinConfiguration.
        // Complementa la FK física configurada en EF.
        var cabinTypeExists = await _context.CabinTypes
            .AsNoTracking()
            .AnyAsync(x => x.Id == cabinTypeId);

        if (!cabinTypeExists)
            throw new InvalidOperationException($"El cabin type con id {cabinTypeId} no existe.");
    }
}
