using System;
using GestorDeVuelosProyectoFinal.Moduls.Continents.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.Continents.Domain.Aggregate;

public sealed class Continent
{
    // El agregado mantiene el id y el nombre como value objects para no saltarse validaciones.
    public ContinentsId Id { get; private set; } = null!;
    public ContinentName Name { get; private set; } = null!;

    // EF necesita un constructor vacío para reconstruir la entidad desde base de datos.
    public Continent() { }

    // El constructor real queda privado para obligar a pasar por el factory.
    public Continent(ContinentsId id, ContinentName name)
    {
        Id = id;
        Name = name;
    }

    // Este factory concentra la creación normal y deja la validación en los value objects.
    public static Continent Create(int id, string name)
    {
        return new Continent(
            ContinentsId.Create(id),
            ContinentName.Create(name)
        );
    }


    public void UpdateName(string newName)
    {
        // Al volver a crear el value object evitamos guardar nombres inválidos por accidente.
        Name = ContinentName.Create(newName);
    }
}
