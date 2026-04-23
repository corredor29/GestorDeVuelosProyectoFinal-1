using GestorDeVuelosProyectoFinal.Moduls.EmailDomains.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.EmailDomains.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.EmailDomains.Application.UseCases;

public sealed class CreateEmailDomainUseCase
{
    private readonly IEmailDomainRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateEmailDomainUseCase(IEmailDomainRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(string domain)
    {
        var existing = await _repository.GetByDomainAsync(domain);
        if (existing is not null)
            throw new InvalidOperationException($"An email domain named '{domain}' already exists.");

        var emailDomain = EmailDomain.Create(domain);

        await _repository.SaveAsync(emailDomain);
        await _unitOfWork.SaveChangesAsync();
    }
}
