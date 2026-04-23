using GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Infrastructure.Persistence.Seeders;

public sealed class PhoneCodeSeeder
{
    private readonly AppDbContext _context;

    private static readonly (string CountryCode, string CountryName)[] DefaultPhoneCodes =
    {
        ("+1", "Estados Unidos"),
        ("+34", "España"),
        ("+52", "México"),
        ("+54", "Argentina"),
        ("+55", "Brasil"),
        ("+56", "Chile"),
        ("+57", "Colombia"),
        ("+58", "Venezuela")
    };

    public PhoneCodeSeeder(AppDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var existingCodes = await _context.PhoneCodes
            .Select(x => x.CountryCode)
            .ToHashSetAsync(StringComparer.OrdinalIgnoreCase, cancellationToken);

        foreach (var (countryCode, countryName) in DefaultPhoneCodes)
        {
            if (existingCodes.Contains(countryCode))
                continue;

            await _context.PhoneCodes.AddAsync(new PhoneCodeEntity
            {
                CountryCode = countryCode,
                CountryName = countryName
            }, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
