using GestorDeVuelosProyectoFinal.Moduls.Continents.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.Continents.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Countries.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.Countries.Application.UseCases;
using GestorDeVuelosProyectoFinal.Moduls.Countries.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.Countries.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.Countries.Application.Services;

public sealed class CountryService : ICountryService
{
    // Además de los casos de uso, aquí se inyecta continentes porque país depende de ese catálogo.
    private readonly GetCountriesUseCase _getAll;
    private readonly CreateCountryUseCase _create;
    private readonly UpdateCountryUseCase _update;
    private readonly DeleteCountryUseCase _delete;
    private readonly IContinentService _continents;

    public CountryService(
        GetCountriesUseCase getAll,
        CreateCountryUseCase create,
        UpdateCountryUseCase update,
        DeleteCountryUseCase delete,
        IContinentService continents)
    {
        _getAll = getAll;
        _create = create;
        _update = update;
        _delete = delete;
        _continents = continents;
    }

    public Task<IEnumerable<Country>> GetAllAsync()
        => _getAll.ExecuteAsync();

    public Task<IEnumerable<Country>> GetByContinentIdAsync(int continentId)
        => _getAll.GetByContinentAsync(ContinentsId.Create(continentId));

    public async Task<IEnumerable<Country>> GetByContinentNameAsync(string continentName)
    {
        // Primero resuelve el continente real y después delega el filtro por id.
        var cont = await _continents.GetByNameAsync(continentName.Trim())
            ?? throw new InvalidOperationException($"No existe el continente '{continentName}'.");
        return await _getAll.GetByContinentAsync(ContinentsId.Create(cont.Id.Value));
    }

    public Task<Country?> GetByIsoCodeAsync(string isoCode)
        => _getAll.GetByIsoCodeAsync(CountryIsoCode.Create(isoCode));

    public Task<Country?> GetByNameAsync(string name)
        => _getAll.GetByNameAsync(name);

    public Task CreateAsync(string name, string isoCode, int continentId)
        => _create.ExecuteAsync(name, isoCode, continentId);

    public Task UpdateAsync(string currentIsoCode, string newName, string newIsoCode, int newContinentId)
        => _update.ExecuteAsync(currentIsoCode, newName, newIsoCode, newContinentId);

    public Task DeleteByNameAsync(string name)
        => _delete.DeleteByNameAsync(name);

    public Task DeleteByIsoCodeAsync(string isoCode)
        => _delete.DeleteByIsoCodeAsync(isoCode);
}
