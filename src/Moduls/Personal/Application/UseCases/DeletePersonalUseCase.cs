using GestorDeVuelosProyectoFinal.Moduls.Personal.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Personal.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.Personal.Application.UseCases;

public sealed class DeletePersonalUseCase
{
    private readonly IPersonalRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeletePersonalUseCase(IPersonalRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteByIdAsync(int id)
    {
        var existing = await _repository.GetByIdAsync(PersonalId.Create(id));
        if (existing is null)
            throw new InvalidOperationException($"No se encontró el empleado con ID {id}.");

        await _repository.DeleteAsync(PersonalId.Create(id));
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<int> ExecuteByPersonNameAsync(string personName)
    {
        var affected = await _repository.DeleteByPersonNameAsync(personName);
        if (affected == 0)
            throw new InvalidOperationException("No se encontraron empleados con ese nombre.");

        await _unitOfWork.SaveChangesAsync();
        return affected;
    }

    public async Task<int> ExecuteByPositionNameAsync(string positionName)
    {
        var affected = await _repository.DeleteByPositionNameAsync(positionName);
        if (affected == 0)
            throw new InvalidOperationException("No se encontraron empleados con ese cargo.");

        await _unitOfWork.SaveChangesAsync();
        return affected;
    }

    public async Task<int> ExecuteByAirlineNameAsync(string airlineName)
    {
        var affected = await _repository.DeleteByAirlineNameAsync(airlineName);
        if (affected == 0)
            throw new InvalidOperationException("No se encontraron empleados en esa aerolínea.");

        await _unitOfWork.SaveChangesAsync();
        return affected;
    }

    public async Task<int> ExecuteByAirportNameAsync(string airportName)
    {
        var affected = await _repository.DeleteByAirportNameAsync(airportName);
        if (affected == 0)
            throw new InvalidOperationException("No se encontraron empleados en ese aeropuerto.");

        await _unitOfWork.SaveChangesAsync();
        return affected;
    }
}
