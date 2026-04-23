using GestorDeVuelosProyectoFinal.src.Moduls.Baggage.Infrastructure.Entity;
namespace GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Infrastructure.Entity;

public class BaggageTypesEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal MaxWeightKg { get; set; }
    public decimal BasePrice { get; set; }
    public ICollection<BaggageEntity> Baggage { get; set; } = new List<BaggageEntity>();
}