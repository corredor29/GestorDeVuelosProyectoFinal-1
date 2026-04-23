using GestorDeVuelosProyectoFinal.Moduls.CabinTypes.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.CabinTypes.Domain.Aggregate;

public sealed class CabinType
{
    public CabinTypesId Id { get; private set; } = null!;
    public CabinTypesName Name { get; private set; } = null!;

    private CabinType() { }

    // Constructor privado para consistencia interna
    private CabinType(CabinTypesId id, CabinTypesName name)
    {
        Id = id;
        Name = name;
    }

    // Para crear una nueva cabina (sin ID aún, ya que es autoincremental)
    public static CabinType Create(string name)
    {
        return new CabinType
        {
            Name = CabinTypesName.Create(name)
        };
    }

    // Para reconstruir la entidad desde el Repositorio (cuando ya existe en la BD)
    public static CabinType FromPrimitives(int id, string name)
    {
        return new CabinType(
            CabinTypesId.Create(id),
            CabinTypesName.Create(name)
        );
    }
    public void UpdateName(string newName)
{
    Name = CabinTypesName.Create(newName);
}
    
}
