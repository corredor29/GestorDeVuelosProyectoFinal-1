using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.Moduls.CabinTypes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.CabinTypes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.CabinTypes.Application.UseCases;

public sealed class DeleteCabinTypeUseCase
{
    private readonly ICabinTypesRepository _repository;
    private readonly AppDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCabinTypeUseCase(ICabinTypesRepository repository, AppDbContext context, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteByIdAsync(int id)
    {
        var cabinTypeId = CabinTypesId.Create(id);
        var existing = await _repository.GetByIdAsync(cabinTypeId);
        if (existing is null)
            throw new InvalidOperationException($"No se encontró el tipo de cabina con ID {id}.");

        if (await _context.CabinConfigurations.AsNoTracking().AnyAsync(x => x.CabinTypeId == id))
            throw new InvalidOperationException("No se puede eliminar el tipo de cabina porque tiene configuraciones asociadas.");

        await _repository.DeleteAsync(cabinTypeId);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ExecuteByNameAsync(string name)
    {
        var existing = await _repository.GetByNameStringAsync(name);
        if (existing is null)
            throw new InvalidOperationException($"No se encontró el tipo de cabina con nombre '{name}'.");

        await ExecuteByIdAsync(existing.Id.Value);
    }
}
