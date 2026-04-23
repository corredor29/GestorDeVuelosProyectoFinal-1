using GestorDeVuelosProyectoFinal.src.Moduls.Rates.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Infrastructure.Entity;
namespace GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Infrastructure.Entity;

public sealed class PassengerTypeEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int? MinAge { get; set; }
    public int? MaxAge { get; set; }
    public ICollection<RateEntity> Rates { get; set; } = new List<RateEntity>();
    public ICollection<PassengersEntity> Passengers { get; set; } = new List<PassengersEntity>();
}
