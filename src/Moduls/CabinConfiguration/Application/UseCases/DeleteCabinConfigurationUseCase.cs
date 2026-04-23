using GestorDeVuelosProyectoFinal.Moduls.CabinConfiguration.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.CabinConfiguration.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.CabinConfiguration.Application.UseCases;

public sealed class DeleteCabinConfigurationUseCase
{
    private readonly ICabinConfigurationRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCabinConfigurationUseCase(
        ICabinConfigurationRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(int id)
    {
        var configuration = await _repository.GetByIdAsync(CabinConfigurationId.Create(id));
        if (configuration is null)
            throw new InvalidOperationException($"La configuración de cabina con id {id} no existe.");

        await _repository.DeleteAsync(configuration.Id);
        await _unitOfWork.SaveChangesAsync();
    }
}
