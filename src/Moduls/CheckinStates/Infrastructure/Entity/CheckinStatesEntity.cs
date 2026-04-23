using GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Infrastructure.Entity;
namespace GestorDeVuelosProyectoFinal.src.Moduls.CheckinStates.Infrastructure.Entity;

public class CheckinStatesEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<CheckinEntity>? Checkins { get; set; }
}
