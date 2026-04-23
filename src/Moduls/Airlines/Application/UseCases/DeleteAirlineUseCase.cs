using GestorDeVuelosProyectoFinal.Moduls.Airlines.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Airlines.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Countries.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.Airlines.Application.UseCases;

public sealed class DeleteAirlineUseCase
{
    private readonly IAirlinesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAirlineUseCase(IAirlinesRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteByIdAsync(int id)
    {
        var existing = await _repository.GetByIdAsync(AirlinesId.Create(id));
        if (existing is null)
            throw new InvalidOperationException($"No se encontró la aerolínea con ID {id}.");

        await _repository.DeleteAsync(AirlinesId.Create(id));
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ExecuteByNameAsync(string name)
    {
        var existing = await _repository.GetByNameAsync(AirlinesName.Create(name));
        if (existing is null)
            throw new InvalidOperationException($"No se encontró la aerolínea con nombre '{name}'.");

        await _repository.DeleteByNameAsync(AirlinesName.Create(name));
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ExecuteByIataCodeAsync(string iataCode)
    {
        var code = AirlinesIataCode.Create(iataCode);
        var existing = await _repository.GetByIataCodeAsync(code);
        if (existing is null)
            throw new InvalidOperationException($"No se encontró la aerolínea con código IATA '{code.Value}'.");

        await _repository.DeleteByIataCodeAsync(code);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<int> ExecuteByOriginCountryIdAsync(int originCountryId)
    {
        var countryId = CountryId.Create(originCountryId);
        var airlines = (await _repository.GetByOriginCountryIdAsync(countryId)).ToList();
        if (airlines.Count == 0)
            throw new InvalidOperationException($"No se encontraron aerolíneas para el país con ID {originCountryId}.");

        var affected = await _repository.DeleteByOriginCountryIdAsync(countryId);
        await _unitOfWork.SaveChangesAsync();
        return affected;
    }
}
