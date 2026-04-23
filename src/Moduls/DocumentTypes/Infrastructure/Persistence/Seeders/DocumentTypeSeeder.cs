using GestorDeVuelosProyectoFinal.Moduls.DocumentTypes.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.Moduls.DocumentTypes.Infrastructure.Persistence.Seeders;

public sealed class DocumentTypeSeeder
{
    private readonly AppDbContext _context;

    private static readonly (string Name, string Code)[] DefaultDocumentTypes =
    {
        ("Pasaporte", "PASSPORT"),
        ("Cédula de Ciudadanía", "CC"),
        ("Documento Nacional de Identidad", "DNI"),
        ("Tarjeta de Identidad", "TI"),
        ("Cédula de Extranjería", "CE")
    };

    public DocumentTypeSeeder(AppDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var existingCodes = await _context.DocumentTypes
            .Select(x => x.Code)
            .ToHashSetAsync(StringComparer.OrdinalIgnoreCase, cancellationToken);

        foreach (var (name, code) in DefaultDocumentTypes)
        {
            if (existingCodes.Contains(code))
                continue;

            await _context.DocumentTypes.AddAsync(new DocumentTypeEntity
            {
                Name = name,
                Code = code
            }, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
