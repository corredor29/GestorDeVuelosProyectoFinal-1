using GestorDeVuelosProyectoFinal.Moduls.Personal.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.Domain.Repositories;

namespace GestorDeVuelosProyectoFinal.Moduls.FlightAssignments.Application.UseCases;

// verificar disponibilidad del personal
public sealed class CheckStaffAvailableUseCase
{
    // La clase depende de un repositorio (IStaffAvailabilityRepository) que se encarga de acceder a los datos.
    private readonly IStaffAvailabilityRepository _staffAvailabilityRepository;

    // El constructor recibe el repositorio como parámetro.
    //Se inyecta por constructor
    public CheckStaffAvailableUseCase(IStaffAvailabilityRepository staffAvailabilityRepository)
    {
        _staffAvailabilityRepository = staffAvailabilityRepository;
    }

    // La función verifica si el personal está disponible en un rango de fechas.
    // Método principal
    public Task<bool> ExecuteAsync(int staffId, DateTime startsAt, DateTime endsAt)
        => _staffAvailabilityRepository.HasBlockingAvailabilityAsync(PersonalId.Create(staffId), startsAt, endsAt);
}
