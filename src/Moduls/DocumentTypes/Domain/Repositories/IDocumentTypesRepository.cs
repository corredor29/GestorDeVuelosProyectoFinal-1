using System.Collections.Generic;
using System.Threading.Tasks;
using GestorDeVuelosProyectoFinal.Moduls.DocumentTypes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.DocumentTypes.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.DocumentTypes.Domain.Repositories;

public interface IDocumentTypesRepository
{
    Task<DocumentType?> GetByIdAsync(DocumentTypesId id);
    Task<DocumentType?> GetByNameAsync(DocumentTypesName name);
    Task<DocumentType?> GetByCodeAsync(DocumentTypesCode code);
    Task<DocumentType?> GetByNameStringAsync(string name);
    Task<DocumentType?> GetByCodeStringAsync(string code);
    Task<IEnumerable<DocumentType>> GetAllAsync();
    Task SaveAsync(DocumentType documentType);
    Task UpdateAsync(DocumentType documentType);
    Task DeleteAsync(DocumentTypesId id);
    Task DeleteByNameAsync(DocumentTypesName name);
    Task DeleteByCodeAsync(DocumentTypesCode code);
}
