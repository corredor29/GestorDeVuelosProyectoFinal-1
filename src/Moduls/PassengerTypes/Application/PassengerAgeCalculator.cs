namespace GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Application;

public static class PassengerAgeCalculator
{
    /// <summary>Edad en años cumplidos a la fecha de referencia (solo calendario, UTC date-safe).</summary>
    public static int ComputeAgeInWholeYears(DateTime birthDate, DateTime referenceDate)
    {
        var birth = birthDate.Date;
        var reference = referenceDate.Date;
        if (birth > reference)
            throw new ArgumentException("La fecha de nacimiento no puede ser posterior a la fecha de referencia.");

        var age = reference.Year - birth.Year;
        if (birth > reference.AddYears(-age))
            age--;

        return age;
    }
}
