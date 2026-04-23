using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.Moduls.CabinConfiguration.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.CabinConfiguration.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.CabinConfiguration.Application.UseCases;

public sealed class UpdateCabinConfigurationUseCase
{
    private readonly ICabinConfigurationRepository _repository;
    private readonly AppDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCabinConfigurationUseCase(
        ICabinConfigurationRepository repository,
        AppDbContext context,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(
        int id,
        int aircraftId,
        int cabinTypeId,
        int rowStart,
        int rowEnd,
        int seatsPerRow,
        string seatLetters)
    {
        var configuration = await _repository.GetByIdAsync(CabinConfigurationId.Create(id))
            ?? throw new InvalidOperationException($"La configuración de cabina con id {id} no existe.");

        await EnsureForeignKeysExistAsync(aircraftId, cabinTypeId);

        var duplicate = await _repository.GetByAircraftAndCabinTypeAsync(aircraftId, cabinTypeId);
        if (duplicate is not null && duplicate.Id.Value != id)
            throw new InvalidOperationException(
                $"Ya existe otra configuración para el avión {aircraftId} y el tipo de cabina {cabinTypeId}.");

        configuration.Update(aircraftId, cabinTypeId, rowStart, rowEnd, seatsPerRow, seatLetters);

        await _repository.UpdateAsync(configuration);
        await _unitOfWork.SaveChangesAsync();
    }

    private async Task EnsureForeignKeysExistAsync(int aircraftId, int cabinTypeId)
    {
        var aircraftExists = await _context.Aircrafts
            .AsNoTracking()
            .AnyAsync(x => x.Id == aircraftId);

        if (!aircraftExists)
            throw new InvalidOperationException($"El aircraft con id {aircraftId} no existe.");

        var cabinTypeExists = await _context.CabinTypes
            .AsNoTracking()
            .AnyAsync(x => x.Id == cabinTypeId);

        if (!cabinTypeExists)
            throw new InvalidOperationException($"El cabin type con id {cabinTypeId} no existe.");
    }
}
