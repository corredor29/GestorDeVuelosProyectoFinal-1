using GestorDeVuelosProyectoFinal.Moduls.EmailDomains.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.EmailDomains.Application.UseCases;

public sealed class DeleteEmailDomainUseCase
{
    private readonly IEmailDomainRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteEmailDomainUseCase(IEmailDomainRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(string domain)
    {
        var existing = await _repository.GetByDomainAsync(domain)
            ?? throw new InvalidOperationException($"Email domain '{domain}' not found.");

        await _repository.DeleteByDomainAsync(existing.Domain.Value);
        await _unitOfWork.SaveChangesAsync();
    }
}
