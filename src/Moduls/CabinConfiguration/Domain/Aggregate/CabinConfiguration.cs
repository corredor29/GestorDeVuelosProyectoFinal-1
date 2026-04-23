using System;
using GestorDeVuelosProyectoFinal.Moduls.CabinConfiguration.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.CabinTypes.Domain.ValueObject;


namespace GestorDeVuelosProyectoFinal.Moduls.CabinConfiguration.Domain.Aggregate;

public sealed class CabinConfiguration 
{
    public CabinConfigurationId Id { get; private set; } = null!;
    public int AircraftId { get; private set; }
    public CabinTypesId CabinTypeId { get; private set; } = null!;
    public CabinRowRange RowRange { get; private set; } = null!;
    public CabinSeatsPerRow SeatsPerRow { get; private set; } = null!;
    public CabinSeatLetters SeatLetters { get; private set; } = null!;

    private CabinConfiguration() { }

    private CabinConfiguration(
        CabinConfigurationId id,
        int aircraftId,
        CabinTypesId cabinTypeId,
        CabinRowRange rowRange,
        CabinSeatsPerRow seatsPerRow,
        CabinSeatLetters seatLetters)
    {
        Id = id;
        AircraftId = aircraftId;
        CabinTypeId = cabinTypeId;
        RowRange = rowRange;
        SeatsPerRow = seatsPerRow;
        SeatLetters = seatLetters;
    }

    public static CabinConfiguration Create(
        int aircraftId,
        int cabinTypeId,
        int startRow,
        int endRow,
        int seatsPerRow,
        string seatLetters)
    {
        var seatsVO = CabinSeatsPerRow.Create(seatsPerRow);

        if (aircraftId <= 0)
            throw new ArgumentException("El aircraft_id debe ser mayor que cero.", nameof(aircraftId));

        return new CabinConfiguration(
            CabinConfigurationId.Create(0),
            aircraftId,
            CabinTypesId.Create(cabinTypeId),
            CabinRowRange.Create(startRow, endRow),
            seatsVO,
            CabinSeatLetters.Create(seatLetters, seatsVO.Value));
    }

    public static CabinConfiguration FromPrimitives(
        int id, int aircraftId, int cabinTypeId, int startRow, int endRow, int seatsPerRow, string seatLetters)
    {
        var seatsVO = CabinSeatsPerRow.Create(seatsPerRow);
        return new CabinConfiguration(
            CabinConfigurationId.Create(id),
            aircraftId,
            CabinTypesId.Create(cabinTypeId),
            CabinRowRange.Create(startRow, endRow),
            seatsVO,
            CabinSeatLetters.Create(seatLetters, seatsVO.Value));
    }

    public void Update(
        int aircraftId,
        int cabinTypeId,
        int startRow,
        int endRow,
        int seatsPerRow,
        string seatLetters)
    {
        var seatsVO = CabinSeatsPerRow.Create(seatsPerRow);

        AircraftId = aircraftId;
        CabinTypeId = CabinTypesId.Create(cabinTypeId);
        RowRange = CabinRowRange.Create(startRow, endRow);
        SeatsPerRow = seatsVO;
        SeatLetters = CabinSeatLetters.Create(seatLetters, seatsVO.Value);
    }

    public IEnumerable<string> GenerateSeatDesignators()
    {
        for (var row = RowRange.StartRow; row <= RowRange.EndRow; row++)
        {
            foreach (var letter in SeatLetters.Value)
                yield return $"{row}{letter}";
        }
    }
}
            
