using System;
using GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.PassengerReservations.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.TicketStates.Domain.ValueObject;
namespace GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Domain.Aggregate;

public sealed class TicketAggregate
{
    public TicketId Id { get; private set; } = null!;
    public TicketCode Code { get; private set; } = null!;
    public TicketIssueDate IssueDate { get; private set; } = null!;
    public TicketCreatedAt CreatedAt { get; private set; } = null!;
    public TicketUpdateAt UpdateAt { get; private set; } = null!;

    public PassengerReservationId ReservationPassengerId { get; private set; } = null!;

    public TicketStatesId StatesId { get; private set; } = null!;

    private TicketAggregate() { }

    private TicketAggregate(
        TicketId id,
        TicketCode code,
        TicketIssueDate issueDate,
        TicketCreatedAt createdAt,
        TicketUpdateAt updateAt,
        PassengerReservationId reservationPassengerId,
        TicketStatesId statesId)
    {
        Id = id;
        Code = code;
        IssueDate = issueDate;
        CreatedAt = createdAt;
        UpdateAt = updateAt;
        ReservationPassengerId = reservationPassengerId;
        StatesId = statesId;
    }

    public static TicketAggregate Create(
        int id,
        string code,
        DateTime issueDate,
        DateTime createdAt,
        DateTime updatedAt,
        int reservationPassengerId,
        int statusId)
    {
        return new TicketAggregate(
            TicketId.Create(id),
            TicketCode.Create(code),
            TicketIssueDate.Create(issueDate),
            TicketCreatedAt.Create(createdAt),
            TicketUpdateAt.Create(updatedAt),
            PassengerReservationId.Create(reservationPassengerId),
            TicketStatesId.Create(statusId)
        );
    }
    internal void SetId(int id)
    {
        Id = TicketId.Create(id);
    }

    public void UpdateCode(string newCode)
    {
        Code = TicketCode.Create(newCode);
    }

    public void UpdateIssueDate(DateTime newIssueDate)
    {
        IssueDate = TicketIssueDate.Create(newIssueDate);
    }

    public void UpdateStatus(int newStatusId)
    {
        StatesId = TicketStatesId.Create(newStatusId);
    }

    public void UpdateReservationPassenger(int newReservationPassengerId)
    {
        ReservationPassengerId = PassengerReservationId.Create(newReservationPassengerId);
    }
}