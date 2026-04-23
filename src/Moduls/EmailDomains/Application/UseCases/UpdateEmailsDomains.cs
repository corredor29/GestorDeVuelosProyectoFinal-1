using GestorDeVuelosProyectoFinal.Moduls.EmailDomains.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.EmailDomains.Application.UseCases;

public sealed class UpdateEmailDomainUseCase
{
    private readonly IEmailDomainRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateEmailDomainUseCase(IEmailDomainRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(string currentDomain, string newDomain)
    {
        var emailDomain = await _repository.GetByDomainAsync(currentDomain)
            ?? throw new InvalidOperationException($"Email domain '{currentDomain}' not found.");

        if (!string.Equals(currentDomain, newDomain, StringComparison.OrdinalIgnoreCase))
        {
            var duplicate = await _repository.GetByDomainAsync(newDomain);
            if (duplicate is not null)
                throw new InvalidOperationException($"An email domain named '{newDomain}' already exists.");
        }

        emailDomain.UpdateDomain(newDomain);

        await _repository.UpdateAsync(emailDomain);
        await _unitOfWork.SaveChangesAsync();
    }
}
