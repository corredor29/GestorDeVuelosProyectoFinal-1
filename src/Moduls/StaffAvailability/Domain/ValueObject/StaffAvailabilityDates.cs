using System;

namespace GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.Domain.ValueObject;

public sealed class StaffAvailabilityDates
{
    public DateTime StartDate { get; }
    public DateTime EndDate { get; }

    private StaffAvailabilityDates(DateTime startDate, DateTime endDate)
    {
        StartDate = startDate;
        EndDate = endDate;
    }

    public static StaffAvailabilityDates Create(DateTime startDate, DateTime endDate)
    {
        if (startDate == default || endDate == default)
            throw new ArgumentException("Las fechas no pueden estar vacías.");

        if (endDate <= startDate)
            throw new ArgumentException("La fecha de fin debe ser mayor a la fecha de inicio.");

        return new StaffAvailabilityDates(startDate, endDate);
    }
}