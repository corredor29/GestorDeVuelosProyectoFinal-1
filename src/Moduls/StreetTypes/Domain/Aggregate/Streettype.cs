using GestorDeVuelosProyectoFinal.Moduls.StreetTypes.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.StreetTypes.Domain.Aggregate;

public sealed class StreetType
{
    // Ahora el ID es de tipo int internamente en el ValueObject
    public StreetTypeId Id { get; private set; } = null!;
    public StreetTypeName Name { get; private set; } = null!;

    private StreetType() { }

    private StreetType(StreetTypeId id, StreetTypeName name)
    {
        Id = id;
        Name = name;
    }

    // Cambiamos el parámetro de entrada de Guid a int
    public static StreetType Create(int id, string name)
    {
        return new StreetType(
            StreetTypeId.Create(id),
            StreetTypeName.Create(name)
        );
    }

    public void UpdateName(string newName)
    {
        Name = StreetTypeName.Create(newName);
    }
}