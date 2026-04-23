using GestorDeVuelosProyectoFinal.Moduls.Airlines.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Airlines.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Airports.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Airports.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.People.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.People.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Personal.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.Personal.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.Personal.Application.UseCases;

public sealed class CreatePersonalUseCase
{
    private readonly IPersonalRepository _repository;
    private readonly IPeopleRepository _peopleRepository;
    private readonly IPersonalPositionsRepository _positionsRepository;
    private readonly IAirlinesRepository _airlinesRepository;
    private readonly IAirportsRepository _airportsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePersonalUseCase(
        IPersonalRepository repository,
        IPeopleRepository peopleRepository,
        IPersonalPositionsRepository positionsRepository,
        IAirlinesRepository airlinesRepository,
        IAirportsRepository airportsRepository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _peopleRepository = peopleRepository;
        _positionsRepository = positionsRepository;
        _airlinesRepository = airlinesRepository;
        _airportsRepository = airportsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(int personId, int positionId, int? airlineId, int? airportId, DateTime hireDate, bool isActive)
    {
        var existing = await _repository.GetByPersonIdAsync(PeopleId.Create(personId));
        if (existing is not null)
            throw new InvalidOperationException($"La persona con ID {personId} ya está vinculada a un empleado.");

        await EnsureReferencesExistAsync(personId, positionId, airlineId, airportId);

        var staff = Staff.Create(personId, positionId, airlineId, airportId, hireDate, isActive);
        await _repository.SaveAsync(staff);
        await _unitOfWork.SaveChangesAsync();
    }

    private async Task EnsureReferencesExistAsync(int personId, int positionId, int? airlineId, int? airportId)
    {
        var person = await _peopleRepository.GetByIdAsync(PeopleId.Create(personId));
        if (person is null)
            throw new InvalidOperationException($"No se encontró la persona con ID {personId}.");

        var position = await _positionsRepository.GetByIdAsync(PersonalPositionsId.Create(positionId));
        if (position is null)
            throw new InvalidOperationException($"No se encontró el cargo con ID {positionId}.");

        if (airlineId.HasValue)
        {
            var airline = await _airlinesRepository.GetByIdAsync(AirlinesId.Create(airlineId.Value));
            if (airline is null)
                throw new InvalidOperationException($"No se encontró la aerolínea con ID {airlineId.Value}.");
        }

        if (airportId.HasValue)
        {
            var airport = await _airportsRepository.GetByIdAsync(AirportsId.Create(airportId.Value));
            if (airport is null)
                throw new InvalidOperationException($"No se encontró el aeropuerto con ID {airportId.Value}.");
        }
    }
}
