using GestorDeVuelosProyectoFinal.Moduls.Seasons.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Seasons.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Rates.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Rates.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Rates.Application.UseCases;

public sealed class CalculateRatePriceUseCase
{
    private readonly IRatesRepository _repository;
    private readonly ISeasonsRepository _seasonsRepository;

    public CalculateRatePriceUseCase(IRatesRepository repository, ISeasonsRepository seasonsRepository)
    {
        _repository = repository;
        _seasonsRepository = seasonsRepository;
    }

    public async Task<decimal> ExecuteAsync(int id)
    {
        var rate = await _repository.GetByIdAsync(RatesId.Create(id));
        if (rate is null)
            throw new InvalidOperationException($"No se encontro la tarifa con ID {id}.");

        var season = await _seasonsRepository.GetByIdAsync(SeasonsId.Create(rate.SeasonId.Value));
        if (season is null)
            throw new InvalidOperationException($"No se encontro la temporada con ID {rate.SeasonId.Value}.");

        return decimal.Round(rate.BasePrice.Value * season.PriceFactor.Value, 2, MidpointRounding.AwayFromZero);
    }
}
