using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GestorDeVuelosProyectoFinal.Moduls.CabinConfiguration.Domain.ValueObject;
using CabinConfigurationAggregate = GestorDeVuelosProyectoFinal.Moduls.CabinConfiguration.Domain.Aggregate.CabinConfiguration;

namespace GestorDeVuelosProyectoFinal.Moduls.CabinConfiguration.Domain.Repositories;

public interface ICabinConfigurationRepository
{
    Task<CabinConfigurationAggregate?> GetByIdAsync(CabinConfigurationId id);
    Task<CabinConfigurationAggregate?> GetByAircraftAndCabinTypeAsync(int aircraftId, int cabinTypeId);

    Task<IEnumerable<CabinConfigurationAggregate>> GetByAircraftIdAsync(int aircraftId);

    Task<IEnumerable<CabinConfigurationAggregate>> GetAllAsync();

    Task SaveAsync(CabinConfigurationAggregate configuration);
    Task UpdateAsync(CabinConfigurationAggregate configuration);
    Task DeleteAsync(CabinConfigurationId id);
}
