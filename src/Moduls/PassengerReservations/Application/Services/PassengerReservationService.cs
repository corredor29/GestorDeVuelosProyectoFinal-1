using System;
using GestorDeVuelosProyectoFinal.src.Moduls.PassengerReservations.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.PassengerReservations.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.PassengerReservations.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.PassengerReservations.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PassengerReservations.Application.Services;

public sealed class PassengerReservationService : IPassengerReservationService
{
    private readonly IPassengerReservationsRepository _PassengerReservationsrepository;
    private readonly IUnitOfWork _unitOfWork;

    public PassengerReservationService(
        IPassengerReservationsRepository PassengerReservationsrepository,
        IUnitOfWork unitOfWork)
    {
        _PassengerReservationsrepository = PassengerReservationsrepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<PassengerReservation> CreateAsync(
        int flightReservationId,
        int passengerId,
        CancellationToken cancellationToken = default)
    {
        var passengerReservation = PassengerReservation.Create(
            flightReservationId,
            passengerId
        );

        await _PassengerReservationsrepository.SaveAsync(passengerReservation, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return passengerReservation;
    }

    public Task<PassengerReservation?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var passengerReservationId = PassengerReservationId.Create(id);
        return _PassengerReservationsrepository.GetByIdAsync(passengerReservationId, cancellationToken);
    }

    public Task<IReadOnlyCollection<PassengerReservation>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return _PassengerReservationsrepository.GetAllAsync(cancellationToken);
    }

    public async Task<PassengerReservation> UpdateAsync(
        int id,
        int flightReservationId,
        int passengerId,
        CancellationToken cancellationToken = default)
    {
        var passengerReservationId = PassengerReservationId.Create(id);

        var existing = await _PassengerReservationsrepository.GetByIdAsync(passengerReservationId, cancellationToken);

        if (existing is null)
        {
            throw new KeyNotFoundException($"PassengerReservation with id '{id}' was not found.");
        }

        existing.Update(flightReservationId, passengerId);

        await _PassengerReservationsrepository.UpdateAsync(existing, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return existing;
    }

    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var passengerReservationId = PassengerReservationId.Create(id);

        var existing = await _PassengerReservationsrepository.GetByIdAsync(passengerReservationId, cancellationToken);

        if (existing is null)
        {
            return false;
        }

        await _PassengerReservationsrepository.DeleteAsync(passengerReservationId, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}