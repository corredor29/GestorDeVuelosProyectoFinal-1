using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Domain.Aggregate;

public sealed class BookingStatuses
{
    public BookingStatusesId Id { get; private set; } = null!;
    public BookingStatusesName Name { get; private set; } = null!;

    private BookingStatuses() { }

    private BookingStatuses(
        BookingStatusesId id,
        BookingStatusesName name)
    {
        Id = id;
        Name = name;
    }

    public static BookingStatuses Create(
        string name)
    {
        return new BookingStatuses(
            null!, // El ID lo genera la base de datos
            BookingStatusesName.Create(name)
        );
    }
    public static BookingStatuses FromPrimitives(int id, string name)
    {
        return new BookingStatuses(
            BookingStatusesId.Create(id),
            BookingStatusesName.Create(name)
        );
    }
    public void UpdateName(string newName)
    {
        Name = BookingStatusesName.Create(newName);
    }
}