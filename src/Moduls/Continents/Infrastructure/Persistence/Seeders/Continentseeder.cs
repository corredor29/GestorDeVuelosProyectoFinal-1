using GestorDeVuelosProyectoFinal.src.Shared.Contracts;
using GestorDeVuelosProyectoFinal.Moduls.Continents.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Continents.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.Moduls.Continents.Infrastructure.Persistence;
public sealed class ContinentSeeder
{
    private readonly IContinentsRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    private static readonly string[] _continents = new string[]
    {
        "Americas",
        "Europe",
        "Asia",
        "Africa",
        "Oceania",
        "Antarctica"
    };

    public ContinentSeeder(IContinentsRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task SeedAsync()
    {
        var existing = await _repository.GetAllAsync();
        var existingNames = existing.Select(c => c.Name.Value).ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var name in _continents)
        {
            if (existingNames.Contains(name)) continue;

            var continent = Continent.Create(0, name);
            await _repository.SaveAsync(continent);
        }

        await _unitOfWork.SaveChangesAsync();
    }
}