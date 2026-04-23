using GestorDeVuelosProyectoFinal.Moduls.Addresses.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.Moduls.Addresses.Infrastructure.Persistence.Seeders;

public sealed class AddressSeeder
{
    private readonly AppDbContext _context;

    /// <summary>Ejemplos: requiere tipos de vía y ciudades ya sembrados.</summary>
    private static readonly (string StreetTypeName, string Street, string? Number, string? Complement, string CityName, string? Postal)[] Rows =
    {
        ("Calle", "50", "12", "Apto 301", "Medellín", "050021"),
        ("Carrera", "7", "71", null, "Bogotá", "110221"),
        ("Avenida", "El Dorado", "68", "Oficina 10", "Bogotá", null),
        ("Calle", "Mayor", "4", null, "Madrid", "28013")
    };

    public AddressSeeder(AppDbContext context) => _context = context;

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if (await _context.Addresses.AnyAsync(cancellationToken))
            return;

        var streetTypes = await _context.StreetTypes
            .ToDictionaryAsync(x => x.Name, x => x.Id, StringComparer.OrdinalIgnoreCase, cancellationToken);
        var cities = await _context.Cities
            .ToDictionaryAsync(x => x.Name, x => x.Id, StringComparer.OrdinalIgnoreCase, cancellationToken);

        foreach (var row in Rows)
        {
            if (!streetTypes.TryGetValue(row.StreetTypeName, out var stId))
                continue;
            if (!cities.TryGetValue(row.CityName, out var cityId))
                continue;

            await _context.Addresses.AddAsync(new AddressEntity
            {
                StreetTypeId = stId,
                StreetTypeId1 = stId,
                StreetName = row.Street,
                Number = row.Number,
                Complement = row.Complement,
                CityId = cityId,
                CityId1 = cityId,
                PostalCode = row.Postal
            }, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
