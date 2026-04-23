using GestorDeVuelosProyectoFinal.src.Moduls.Customers.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Customers.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Customers.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Customers.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Customers.Infrastructure.Repository;

// Repositorio de clientes.
// La mayoría de sus búsquedas son simples, salvo la de documento que necesita cruzar con personas.
public sealed class CustomersRepository : ICustomersRepository
{
    private readonly AppDbContext _context;

    public CustomersRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Customer?> GetByIdAsync(CustomersId id)
    {
        var entity = await _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<Customer?> GetByPersonIdAsync(int personId)
    {
        var entity = await _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PersonId == personId);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<Customer?> GetByDocumentNumberAsync(string documentNumber)
    {
        var normalized = documentNumber.Trim();

        // El documento realmente vive en Persons, por eso aquí hacemos un join.
        var result = await (
            from customer in _context.Customers.AsNoTracking()
            join person in _context.Persons.AsNoTracking()
                on customer.PersonId equals person.Id
            where person.DocumentNumber == normalized
            select customer)
            .FirstOrDefaultAsync();

        return result is null ? null : MapToDomain(result);
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        var entities = await _context.Customers
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task SaveAsync(Customer customer)
    {
        await _context.Customers.AddAsync(MapToEntity(customer));
    }

    public async Task DeleteAsync(CustomersId id)
    {
        var entity = await _context.Customers
            .FirstOrDefaultAsync(x => x.Id == id.Value);

        if (entity is null)
            // Aquí preferimos fallar explícitamente porque desde admin sí tiene sentido saber
            // que el registro ya no existe.
            throw new InvalidOperationException($"Customer with id '{id.Value}' not found.");

        _context.Customers.Remove(entity);
    }

    private static Customer MapToDomain(CustomerEntity entity)
    {
        return Customer.FromPrimitives(entity.Id, entity.PersonId, entity.CreatedAt);
    }

    private static CustomerEntity MapToEntity(Customer customer)
    {
        return new CustomerEntity
        {
            Id = customer.Id.Value,
            PersonId = customer.PersonId,
            CreatedAt = customer.CreatedAt.Value
        };
    }
}
