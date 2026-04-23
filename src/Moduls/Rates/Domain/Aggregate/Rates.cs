using GestorDeVuelosProyectoFinal.Moduls.CabinTypes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Routes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Seasons.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Rates.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Rates.Domain.Aggregate;

public sealed class Rate
{
    public RatesId? Id { get; private set; }
    public RouteId RouteId { get; private set; } = null!;
    public CabinTypesId CabinTypeId { get; private set; } = null!;
    public PassengerTypesId PassengerTypeId { get; private set; } = null!;
    public SeasonsId SeasonId { get; private set; } = null!;
    public RatesBasePrice BasePrice { get; private set; } = null!;
    public RatesValidFrom ValidFrom { get; private set; } = null!;
    public RatesValidTo ValidUntil { get; private set; } = null!;

    private Rate() { }

    private Rate(
        RatesId? id,
        RouteId routeId,
        CabinTypesId cabinTypeId,
        PassengerTypesId passengerTypeId,
        SeasonsId seasonId,
        RatesBasePrice basePrice,
        RatesValidFrom validFrom,
        RatesValidTo validUntil)
    {
        Id = id;
        RouteId = routeId;
        CabinTypeId = cabinTypeId;
        PassengerTypeId = passengerTypeId;
        SeasonId = seasonId;
        BasePrice = basePrice;
        ValidFrom = validFrom;
        ValidUntil = validUntil;
    }

    public static Rate Create(
        int routeId,
        int cabinTypeId,
        int passengerTypeId,
        int seasonId,
        decimal basePrice,
        DateOnly? validFrom,
        DateOnly? validUntil)
    {
        EnsureValidRange(validFrom, validUntil);

        return new Rate(
            id: null,
            routeId: RouteId.Create(routeId),
            cabinTypeId: CabinTypesId.Create(cabinTypeId),
            passengerTypeId: PassengerTypesId.Create(passengerTypeId),
            seasonId: SeasonsId.Create(seasonId),
            basePrice: RatesBasePrice.Create(basePrice),
            validFrom: RatesValidFrom.Create(validFrom),
            validUntil: RatesValidTo.Create(validUntil));
    }

    public static Rate FromPrimitives(
        int id,
        int routeId,
        int cabinTypeId,
        int passengerTypeId,
        int seasonId,
        decimal basePrice,
        DateOnly? validFrom,
        DateOnly? validUntil)
    {
        EnsureValidRange(validFrom, validUntil);

        return new Rate(
            id: RatesId.Create(id),
            routeId: RouteId.Create(routeId),
            cabinTypeId: CabinTypesId.Create(cabinTypeId),
            passengerTypeId: PassengerTypesId.Create(passengerTypeId),
            seasonId: SeasonsId.Create(seasonId),
            basePrice: RatesBasePrice.Create(basePrice),
            validFrom: RatesValidFrom.Create(validFrom),
            validUntil: RatesValidTo.Create(validUntil));
    }

    public void Update(
        int routeId,
        int cabinTypeId,
        int passengerTypeId,
        int seasonId,
        decimal basePrice,
        DateOnly? validFrom,
        DateOnly? validUntil)
    {
        EnsureValidRange(validFrom, validUntil);

        RouteId = RouteId.Create(routeId);
        CabinTypeId = CabinTypesId.Create(cabinTypeId);
        PassengerTypeId = PassengerTypesId.Create(passengerTypeId);
        SeasonId = SeasonsId.Create(seasonId);
        BasePrice = RatesBasePrice.Create(basePrice);
        ValidFrom = RatesValidFrom.Create(validFrom);
        ValidUntil = RatesValidTo.Create(validUntil);
    }

    private static void EnsureValidRange(DateOnly? validFrom, DateOnly? validUntil)
    {
        if (validFrom.HasValue && validUntil.HasValue && validUntil.Value < validFrom.Value)
            throw new ArgumentException("La fecha valid_until no puede ser menor que valid_from.");
    }
}
