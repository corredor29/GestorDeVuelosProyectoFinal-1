using GestorDeVuelosProyectoFinal.src.Moduls.Rates.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Rates.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Rates.Application.UseCases;

public sealed class DeleteRateUseCase
{
    private readonly IRatesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteRateUseCase(IRatesRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteByIdAsync(int id)
    {
        var existing = await _repository.GetByIdAsync(RatesId.Create(id));
        if (existing is null)
            throw new InvalidOperationException($"No se encontro la tarifa con ID {id}.");

        await _repository.DeleteByIdAsync(RatesId.Create(id));
        await _unitOfWork.SaveChangesAsync();
    }
}
