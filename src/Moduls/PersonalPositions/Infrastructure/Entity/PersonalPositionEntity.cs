using GestorDeVuelosProyectoFinal.Moduls.Personal.Infrastructure.Persistence.Entities;
namespace GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Infrastructure.Persistence.Entities;

public sealed class PersonalPositionEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public  ICollection<StaffEntity> Staff { get; set; } = new List<StaffEntity>();

}
