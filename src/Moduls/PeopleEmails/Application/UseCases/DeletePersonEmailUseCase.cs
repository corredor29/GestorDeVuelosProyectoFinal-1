using GestorDeVuelosProyectoFinal.Moduls.PersonEmails.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.PersonEmails.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PeopleEmails.Application.UseCases;

public sealed class DeletePersonEmailUseCase
{
    private readonly IPersonEmailsRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeletePersonEmailUseCase(IPersonEmailsRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(int id)
    {
        var personEmail = await _repository.GetByIdAsync(PersonEmailsId.Create(id))
            ?? throw new InvalidOperationException($"Person email with id '{id}' not found.");

        await _repository.DeleteAsync(personEmail.Id);
        await _unitOfWork.SaveChangesAsync();
    }
}
