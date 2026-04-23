using GestorDeVuelosProyectoFinal.Moduls.Addresses.Infrastructure.Persistence.Entities;
namespace GestorDeVuelosProyectoFinal.Moduls.StreetTypes.Infrastructure.Persistence.Entities;

public sealed class StreetTypeEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public  ICollection<AddressEntity> Addresses { get; set; } = new List<AddressEntity>();
    
}