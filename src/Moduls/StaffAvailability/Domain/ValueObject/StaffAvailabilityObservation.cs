namespace GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.Domain.ValueObject;

public sealed class StaffAvailabilityObservation
{
    public string? Value { get; }
    private StaffAvailabilityObservation(string? value) => Value = value;
    public static StaffAvailabilityObservation Create(string? value)
    {
        if (value?.Length > 255)
            throw new ArgumentException("La observación no puede superar los 255 caracteres.");
        return new StaffAvailabilityObservation(value?.Trim());
    }
}