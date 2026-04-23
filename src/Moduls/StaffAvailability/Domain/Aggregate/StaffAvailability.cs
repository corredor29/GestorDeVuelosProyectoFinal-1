using System;
using GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Personal.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.Domain.Aggregate;

public sealed class StaffAvailabilityRecord
{
    public StaffAvailabilityId? Id { get; private set; }
    public PersonalId StaffId { get; private set; } = null!;
    public AvailabilityStatesId StateId { get; private set; } = null!;
    public StaffAvailabilityDates Dates { get; private set; } = null!;
    public StaffAvailabilityObservation Observation { get; private set; } = null!;

    private StaffAvailabilityRecord() { }

    private StaffAvailabilityRecord(
        StaffAvailabilityId? id,
        PersonalId staffId,
        AvailabilityStatesId stateId,
        StaffAvailabilityDates dates,
        StaffAvailabilityObservation observation)
    {
        Id = id;
        StaffId = staffId;
        StateId = stateId;
        Dates = dates;
        Observation = observation;
    }

    public static StaffAvailabilityRecord Create(
        int staffId,
        int stateId,
        DateTime startDate,
        DateTime endDate,
        string? observation)
    {
        return new StaffAvailabilityRecord(
            id: null,
            staffId: PersonalId.Create(staffId),
            stateId: AvailabilityStatesId.Create(stateId),
            dates: StaffAvailabilityDates.Create(startDate, endDate),
            observation: StaffAvailabilityObservation.Create(observation));
    }

    public static StaffAvailabilityRecord FromPrimitives(
        int id,
        int staffId,
        int stateId,
        DateTime startDate,
        DateTime endDate,
        string? observation)
    {
        return new StaffAvailabilityRecord(
            id: StaffAvailabilityId.Create(id),
            staffId: PersonalId.Create(staffId),
            stateId: AvailabilityStatesId.Create(stateId),
            dates: StaffAvailabilityDates.Create(startDate, endDate),
            observation: StaffAvailabilityObservation.Create(observation));
    }

    public void Update(
        int staffId,
        int stateId,
        DateTime startDate,
        DateTime endDate,
        string? observation)
    {
        StaffId = PersonalId.Create(staffId);
        StateId = AvailabilityStatesId.Create(stateId);
        Dates = StaffAvailabilityDates.Create(startDate, endDate);
        Observation = StaffAvailabilityObservation.Create(observation);
    }
}
