using GestorDeVuelosProyectoFinal.Moduls.CabinTypes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.CabinTypes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.CabinTypes.Application.UseCases;

public class UpdateCabinTypeUseCase
{
    private readonly ICabinTypesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCabinTypeUseCase(ICabinTypesRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(int id, string newName)
    {
        // 1. Buscar la entidad existente
        var cabinType = await _repository.GetByIdAsync(CabinTypesId.Create(id));
        
        if (cabinType == null)
            throw new InvalidOperationException("El tipo de cabina no existe.");

        // 2. Validar que el nuevo nombre no esté ocupado por otra cabina
        var duplicate = await _repository.GetByNameStringAsync(newName);
        if (duplicate != null && duplicate.Id.Value != id)
            throw new InvalidOperationException($"Ya existe otro tipo de cabina con el nombre '{newName}'.");

        // 3. Actualizar el estado interno de la entidad
        cabinType.UpdateName(newName);

        // 4. Persistir cambios
        await _repository.UpdateAsync(cabinType);
        await _unitOfWork.SaveChangesAsync();
    }
}
