using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.Moduls.CabinConfiguration.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;
using CabinConfigurationAggregate = GestorDeVuelosProyectoFinal.Moduls.CabinConfiguration.Domain.Aggregate.CabinConfiguration;

namespace GestorDeVuelosProyectoFinal.src.Moduls.CabinConfiguration.Infrastructure.Persistence.seeders;

public sealed class CabinConfigurationSeeder
{
    private readonly AppDbContext _context;
    private readonly ICabinConfigurationRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CabinConfigurationSeeder(
        AppDbContext context,
        ICabinConfigurationRepository repository,
        IUnitOfWork unitOfWork)
    {
        _context = context;
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task SeedAsync()
    {
        var aircraftIds = await _context.Aircrafts
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Select(x => x.Id)
            .Take(3)
            .ToListAsync();

        var cabinTypes = await _context.CabinTypes
            .AsNoTracking()
            .Select(x => new { x.Id, x.Name })
            .ToListAsync();

        if (aircraftIds.Count == 0 || cabinTypes.Count == 0)
            return;

        foreach (var aircraftId in aircraftIds)
        {
            var startRow = 1;

            foreach (var cabinType in cabinTypes.OrderBy(x => GetSortOrder(x.Name)))
            {
                var duplicate = await _repository.GetByAircraftAndCabinTypeAsync(aircraftId, cabinType.Id);
                if (duplicate is not null)
                {
                    startRow += GetRowBlockSize(cabinType.Name);
                    continue;
                }

                var rowBlock = GetRowBlockSize(cabinType.Name);
                var seatsPerRow = GetSeatsPerRow(cabinType.Name);
                var seatLetters = GetSeatLetters(cabinType.Name);

                var configuration = CabinConfigurationAggregate.Create(
                    aircraftId,
                    cabinType.Id,
                    startRow,
                    startRow + rowBlock - 1,
                    seatsPerRow,
                    seatLetters);

                await _repository.SaveAsync(configuration);
                startRow += rowBlock;
            }
        }

        await _unitOfWork.SaveChangesAsync();
    }

    private static int GetSortOrder(string cabinTypeName) => cabinTypeName.ToUpperInvariant() switch
    {
        "VIP" => 1,
        "FIRST CLASS" => 2,
        "BUSINESS" => 3,
        _ => 4
    };

    private static int GetRowBlockSize(string cabinTypeName) => cabinTypeName.ToUpperInvariant() switch
    {
        "VIP" => 3,
        "FIRST CLASS" => 4,
        "BUSINESS" => 6,
        _ => 12
    };

    private static int GetSeatsPerRow(string cabinTypeName) => cabinTypeName.ToUpperInvariant() switch
    {
        "VIP" => 3,
        "FIRST CLASS" => 4,
        "BUSINESS" => 6,
        _ => 6
    };

    private static string GetSeatLetters(string cabinTypeName) => cabinTypeName.ToUpperInvariant() switch
    {
        "VIP" => "ABC",
        "FIRST CLASS" => "ACDF",
        "BUSINESS" => "ABCDEF",
        _ => "ABCDEF"
    };
}
