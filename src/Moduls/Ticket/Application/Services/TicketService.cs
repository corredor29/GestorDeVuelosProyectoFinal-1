using System;
using GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Application.Services;

public sealed class TicketService : ITicketService
{
    private readonly ITicketRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public TicketService(
        ITicketRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TicketAggregate> CreateAsync(
        string code,
        DateTime issueDate,
        int reservationPassengerId,
        int statusId,
        CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        var ticket = TicketAggregate.Create(
            0,
            code,
            issueDate,
            now,
            now,
            reservationPassengerId,
            statusId
        );

        await _repository.SaveAsync(ticket, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ticket;
    }

    public async Task<TicketAggregate?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var ticketId = TicketId.Create(id);
        return await _repository.GetByIdAsync(ticketId, cancellationToken);
    }

    public Task<IEnumerable<TicketAggregate>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return _repository.GetAllAsync(cancellationToken);
    }

    public async Task<TicketAggregate> UpdateAsync(
        int id,
        string? code,
        DateTime? issueDate,
        int? reservationPassengerId,
        int? statusId,
        CancellationToken cancellationToken = default)
    {
        var ticketId = TicketId.Create(id);

        var existing = await _repository.GetByIdAsync(ticketId, cancellationToken);

        if (existing is null)
            throw new KeyNotFoundException($"Ticket with id '{id}' was not found.");

        if (code is not null)
            existing.UpdateCode(code);

        if (issueDate is not null)
            existing.UpdateIssueDate(issueDate.Value);

        if (reservationPassengerId is not null)
            existing.UpdateReservationPassenger(reservationPassengerId.Value);

        if (statusId is not null)
            existing.UpdateStatus(statusId.Value);

        await _repository.UpdateAsync(existing, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return existing;
    }

    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var ticketId = TicketId.Create(id);

        var existing = await _repository.GetByIdAsync(ticketId, cancellationToken);

        if (existing is null)
            return false;

        await _repository.DeleteAsync(ticketId, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}