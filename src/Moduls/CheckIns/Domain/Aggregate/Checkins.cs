using GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.CheckinStates.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Domain.Aggregate;

public sealed class Checkin
{
    private CheckinStatesId _checkinStatusId;

    public CheckinsId Id { get; }
    public TicketId TicketId { get; }
    public StaffAvailabilityId StaffId { get; }
    public FlightSeatsId FlightSeatId { get; }
    public CheckinsDate CheckedInAt { get; }
    public CheckinStatesId CheckinStatusId => _checkinStatusId;
    public CheckinsBoardingPassNumber BoardingPassNumber { get; }

    private Checkin(
        CheckinsId id,
        TicketId ticketId,
        StaffAvailabilityId staffId,
        FlightSeatsId flightSeatId,
        CheckinsDate checkedInAt,
        CheckinStatesId checkinStatusId,
        CheckinsBoardingPassNumber boardingPassNumber)
    {
        Id                 = id;
        TicketId           = ticketId;
        StaffId            = staffId;
        FlightSeatId       = flightSeatId;
        CheckedInAt        = checkedInAt;
        _checkinStatusId   = checkinStatusId;
        BoardingPassNumber = boardingPassNumber;
    }

    public static Checkin Create(
        int id,
        int ticketId,
        int staffId,
        int flightSeatId,
        DateTime checkedInAt,
        int checkinStatusId,
        string boardingPassNumber)
    {
        return new Checkin(
            CheckinsId.Create(id),
            TicketId.Create(ticketId),
            StaffAvailabilityId.Create(staffId),
            FlightSeatsId.Create(flightSeatId),
            CheckinsDate.Create(checkedInAt),
            CheckinStatesId.Create(checkinStatusId),
            CheckinsBoardingPassNumber.Create(boardingPassNumber)
        );
    }

    public void UpdateCheckinStatus(int newStatusId)
        => _checkinStatusId = CheckinStatesId.Create(newStatusId);
}