using System.Security.Cryptography.X509Certificates;
using GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Infrastructure.Entity;
namespace GestorDeVuelosProyectoFinal.src.Moduls.Baggage.Infrastructure.Entity;

public class BaggageEntity
{
    public int Id { get; set; }
    public int CheckinId { get; set; }
    public int BaggageTypeId { get; set; }
    public decimal WeightKg { get; set; }
    public decimal ChargedPrice { get; set; }
    public CheckinEntity? Checkin { get; set; }
    public BaggageTypesEntity? BaggageType { get; set; }
}