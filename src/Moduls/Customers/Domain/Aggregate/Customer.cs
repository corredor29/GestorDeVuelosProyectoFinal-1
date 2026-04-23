using GestorDeVuelosProyectoFinal.src.Moduls.Customers.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Customers.Domain.Aggregate;

public sealed class Customer
{
    public CustomersId Id { get; private set; } = null!;
    public int PersonId { get; private set; }
    public CustomersCreatedAt CreatedAt { get; private set; } = null!;

    private Customer() { }

    private Customer(CustomersId id, int personId, CustomersCreatedAt createdAt)
    {
        Id = id;
        PersonId = personId;
        CreatedAt = createdAt;
    }

    public static Customer Create(int personId, DateTime createdAt)
    {
        return new Customer(
            CustomersId.Create(0),
            personId,
            CustomersCreatedAt.Create(createdAt));
    }

    public static Customer FromPrimitives(int id, int personId, DateTime createdAt)
    {
        return new Customer(
            CustomersId.Create(id),
            personId,
            CustomersCreatedAt.Create(createdAt));
    }
}
