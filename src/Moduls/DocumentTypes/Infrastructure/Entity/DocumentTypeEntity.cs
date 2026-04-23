using GestorDeVuelosProyectoFinal.Moduls.People.Infrastructure.Persistence.Entities;
namespace GestorDeVuelosProyectoFinal.Moduls.DocumentTypes.Infrastructure.Persistence.Entities;

public sealed class DocumentTypeEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;
    public  ICollection<PersonEntity> Person { get; set; } = new List<PersonEntity>();
}