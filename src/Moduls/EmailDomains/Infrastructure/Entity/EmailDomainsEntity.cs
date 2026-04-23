using GestorDeVuelosProyectoFinal.src.Moduls.PeopleEmails.Infrastructure.Entity;

namespace GestorDeVuelosProyectoFinal.src.Moduls.EmailDomains.Infrastructure.Entity;

public sealed class EmailDomainsEntity
{
    public int Id { get; set; }
    public string Domain { get; set; } = string.Empty;
    public  ICollection<PersonEmailEntity> PersonEmails { get; set; } = new List<PersonEmailEntity>();
}
