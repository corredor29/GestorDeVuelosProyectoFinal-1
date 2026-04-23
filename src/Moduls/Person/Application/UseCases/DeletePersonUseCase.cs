using GestorDeVuelosProyectoFinal.Moduls.People.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.People.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.People.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Personal.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.Moduls.People.Application.UseCases;

public sealed class DeletePersonUseCase
{
    private readonly IPeopleRepository _repository;
    private readonly IPersonalRepository _personalRepository;
    private readonly AppDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public DeletePersonUseCase(IPeopleRepository repository, IPersonalRepository personalRepository, AppDbContext context, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _personalRepository = personalRepository;
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteByIdAsync(int id)
    {
        var personId = PeopleId.Create(id);
        var person = await _repository.GetByIdAsync(personId);

        if (person is null)
            throw new InvalidOperationException($"No se encontró la persona con ID {id}.");

        await EnsureNotLinkedAsync(person);
        await _repository.DeleteAsync(personId);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<int> ExecuteByFirstNameAsync(string firstName)
    {
        var people = (await _repository.GetByFirstNameAsync(PeopleNames.Create(firstName))).ToList();
        return await DeleteManyAsync(people, p => _repository.DeleteByFirstNameAsync(PeopleNames.Create(firstName)));
    }

    public async Task<int> ExecuteByLastNameAsync(string lastName)
    {
        var people = (await _repository.GetByLastNameAsync(PeopleLastNames.Create(lastName))).ToList();
        return await DeleteManyAsync(people, p => _repository.DeleteByLastNameAsync(PeopleLastNames.Create(lastName)));
    }

    public async Task<int> ExecuteByDocumentNumberAsync(string documentNumber)
    {
        var people = (await _repository.GetByDocumentNumberAsync(PeopleDocumentNumber.Create(documentNumber))).ToList();
        return await DeleteManyAsync(people, p => _repository.DeleteByDocumentNumberAsync(PeopleDocumentNumber.Create(documentNumber)));
    }

    private async Task<int> DeleteManyAsync(IReadOnlyCollection<Person> people, Func<IReadOnlyCollection<Person>, Task<int>> deleteAction)
    {
        if (people.Count == 0)
            throw new InvalidOperationException("No se encontraron personas para eliminar.");

        foreach (var person in people)
            await EnsureNotLinkedAsync(person);

        var deleted = await deleteAction(people);
        await _unitOfWork.SaveChangesAsync();
        return deleted;
    }

    private async Task EnsureNotLinkedAsync(Person person)
    {
        var linkedStaff = await _personalRepository.GetByPersonIdAsync(person.Id);
        if (linkedStaff is not null)
            throw new InvalidOperationException("No se puede eliminar la persona porque está vinculada a un empleado.");

        var linkedCustomer = await _context.Customers.FirstOrDefaultAsync(x => x.PersonId == person.Id.Value);
        if (linkedCustomer is not null)
        {
            var hasBookings = await _context.Bookings.AnyAsync(x => x.ClientId == linkedCustomer.Id);
            if (hasBookings)
                throw new InvalidOperationException("No se puede eliminar la persona porque está vinculada a un cliente con reservas.");

            _context.Customers.Remove(linkedCustomer);
        }

        var linkedPassenger = await _context.Passengers.FirstOrDefaultAsync(x => x.PersonId == person.Id.Value);
        if (linkedPassenger is not null)
        {
            var hasPassengerReservations = await _context.PassengerReservations.AnyAsync(x => x.Passenger_Id == linkedPassenger.Id);
            if (hasPassengerReservations)
                throw new InvalidOperationException("No se puede eliminar la persona porque está vinculada a un pasajero con reservas.");

            _context.Passengers.Remove(linkedPassenger);
        }

        var linkedUser = await _context.Users.FirstOrDefaultAsync(x => x.Person_Id == person.Id.Value);
        if (linkedUser is not null)
            _context.Users.Remove(linkedUser);

        var linkedEmails = await _context.PersonEmails.Where(x => x.PersonId == person.Id.Value).ToListAsync();
        if (linkedEmails.Count > 0)
            _context.PersonEmails.RemoveRange(linkedEmails);

        var linkedPhones = await _context.PeoplePhones.Where(x => x.PersonId == person.Id.Value).ToListAsync();
        if (linkedPhones.Count > 0)
            _context.PeoplePhones.RemoveRange(linkedPhones);
    }
}
