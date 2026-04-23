using GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Application.UseCases;

public sealed class ReactivateAirportAirlineUseCase
{
    private readonly IAirportAirlineRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ReactivateAirportAirlineUseCase(IAirportAirlineRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(int id)
    {
        var existing = await _repository.GetByIdAsync(AirportAirlineId.Create(id));
        if (existing is null)
            throw new InvalidOperationException($"No se encontró la operación con ID {id}.");

        await _repository.ReactivateAsync(AirportAirlineId.Create(id));
        await _unitOfWork.SaveChangesAsync();
    }
}
