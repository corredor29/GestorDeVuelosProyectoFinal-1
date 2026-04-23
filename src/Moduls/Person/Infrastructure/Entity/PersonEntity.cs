using GestorDeVuelosProyectoFinal.Moduls.Addresses.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.DocumentTypes.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.src.Moduls.PeopleEmails.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.src.Moduls.Customers.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.Moduls.Personal.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.Users.Infrastructure.Entity;
namespace GestorDeVuelosProyectoFinal.Moduls.People.Infrastructure.Persistence.Entities;

public sealed class PersonEntity
{
    public int Id { get; set; }
    public int DocumentTypeId { get; set; }
    public string DocumentNumber { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime? BirthDate { get; set; }
    public char? Gender { get; set; }
    public int? AddressId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DocumentTypeEntity DocumentType { get; set; } = null!;
    public AddressEntity? Address { get; set; }
    public ICollection<PersonEmailEntity> PersonEmails { get; set; } = new List<PersonEmailEntity>();
    public ICollection<PeoplePhoneEntity> PersonPhones { get; set; } = new List<PeoplePhoneEntity>();

    // 1:1 (o 1:1 opcional) según el DDL.
    public CustomerEntity? Client { get; set; }
    public StaffEntity? StaffMember { get; set; }
    public PassengersEntity? Passenger { get; set; }
    public UsersEntity? User { get; set; }

}
