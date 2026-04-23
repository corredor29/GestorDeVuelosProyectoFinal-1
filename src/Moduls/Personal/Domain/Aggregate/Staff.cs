using GestorDeVuelosProyectoFinal.Moduls.Airlines.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Airports.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.People.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Personal.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.Personal.Domain.Aggregate;

public sealed class Staff
{
    public PersonalId? Id { get; private set; }
    public PeopleId PersonId { get; private set; } = null!;
    public PersonalPositionsId PositionId { get; private set; } = null!;
    public AirlinesId? AirlineId { get; private set; }
    public AirportsId? AirportId { get; private set; }
    public PersonalHireDate HireDate { get; private set; } = null!;
    public PersonalIsActive IsActive { get; private set; } = null!;
    public PersonalCreatedIn CreatedIn { get; private set; } = null!;
    public PersonalUpdatedIn UpdatedIn { get; private set; } = null!;

    private Staff() { }

    private Staff(
        PersonalId? id,
        PeopleId personId,
        PersonalPositionsId positionId,
        AirlinesId? airlineId,
        AirportsId? airportId,
        PersonalHireDate hireDate,
        PersonalIsActive isActive,
        PersonalCreatedIn createdIn,
        PersonalUpdatedIn updatedIn)
    {
        Id = id;
        PersonId = personId;
        PositionId = positionId;
        AirlineId = airlineId;
        AirportId = airportId;
        HireDate = hireDate;
        IsActive = isActive;
        CreatedIn = createdIn;
        UpdatedIn = updatedIn;
    }

    public static Staff Create(int personId, int positionId, int? airlineId, int? airportId, DateTime hireDate, bool isActive = true)
    {
        var now = DateTime.UtcNow;

        return new Staff(
            id: null,
            personId: PeopleId.Create(personId),
            positionId: PersonalPositionsId.Create(positionId),
            airlineId: airlineId.HasValue ? AirlinesId.Create(airlineId.Value) : null,
            airportId: airportId.HasValue ? AirportsId.Create(airportId.Value) : null,
            hireDate: PersonalHireDate.Create(hireDate),
            isActive: PersonalIsActive.Create(isActive),
            createdIn: PersonalCreatedIn.Create(now),
            updatedIn: PersonalUpdatedIn.Create(now));
    }

    public static Staff FromPrimitives(
        int id,
        int personId,
        int positionId,
        int? airlineId,
        int? airportId,
        DateTime hireDate,
        bool isActive,
        DateTime createdIn,
        DateTime updatedIn)
    {
        return new Staff(
            id: PersonalId.Create(id),
            personId: PeopleId.Create(personId),
            positionId: PersonalPositionsId.Create(positionId),
            airlineId: airlineId.HasValue ? AirlinesId.Create(airlineId.Value) : null,
            airportId: airportId.HasValue ? AirportsId.Create(airportId.Value) : null,
            hireDate: PersonalHireDate.Create(hireDate),
            isActive: PersonalIsActive.Create(isActive),
            createdIn: PersonalCreatedIn.Create(createdIn),
            updatedIn: PersonalUpdatedIn.Create(updatedIn));
    }

    public void Update(int personId, int positionId, int? airlineId, int? airportId, DateTime hireDate, bool isActive)
    {
        PersonId = PeopleId.Create(personId);
        PositionId = PersonalPositionsId.Create(positionId);
        AirlineId = airlineId.HasValue ? AirlinesId.Create(airlineId.Value) : null;
        AirportId = airportId.HasValue ? AirportsId.Create(airportId.Value) : null;
        HireDate = PersonalHireDate.Create(hireDate);
        IsActive = PersonalIsActive.Create(isActive);
        UpdatedIn = PersonalUpdatedIn.Create(DateTime.UtcNow);
    }

    public void Deactivate()
    {
        IsActive = PersonalIsActive.Inactive();
        UpdatedIn = PersonalUpdatedIn.Create(DateTime.UtcNow);
    }

    public void Reactivate()
    {
        IsActive = PersonalIsActive.Active();
        UpdatedIn = PersonalUpdatedIn.Create(DateTime.UtcNow);
    }
}
