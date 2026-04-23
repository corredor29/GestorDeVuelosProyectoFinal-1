namespace GestorDeVuelosProyectoFinal.Moduls.Personal.Domain.ValueObject;

public sealed class PersonalIsActive
{
    public bool Value { get; }

    private PersonalIsActive(bool value) => Value = value;

    public static PersonalIsActive Create(bool value)
    {
        // En un booleano no hay "nulos", pero podrías añadir lógica si, 
        // por ejemplo, el sistema prohibiera crear personal inactivo desde el inicio.
        
        return new PersonalIsActive(value);
    }

    // Métodos de ayuda para que el código sea más legible (Syntactic Sugar)
    public static PersonalIsActive Active() => new PersonalIsActive(true);
    public static PersonalIsActive Inactive() => new PersonalIsActive(false);

    public override string ToString() => Value ? "Personal Activo" : "Personal Inactivo";
}