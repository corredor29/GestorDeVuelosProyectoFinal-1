using GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Domain.Repositories;

public interface IPassengerTypesRepository
{
    Task<PassengerType?> GetByIdAsync(PassengerTypesId id, CancellationToken cancellationToken = default);

    Task<PassengerType?> GetByNameAsync(PassengerTypesName name, CancellationToken cancellationToken = default);


    Task<PassengerType?> GetByAgeAsync(int ageInYears, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<PassengerType>> GetAllAsync(CancellationToken cancellationToken = default);

    Task SaveAsync(PassengerType passengerType, CancellationToken cancellationToken = default);

    Task UpdateAsync(PassengerType passengerType, CancellationToken cancellationToken = default);

    Task DeleteAsync(PassengerTypesId id, CancellationToken cancellationToken = default);

    Task<bool> DeleteByNameAsync(PassengerTypesName name, CancellationToken cancellationToken = default);

    Task<int> DeleteByAgeAsync(int ageInYears, CancellationToken cancellationToken = default);
}
