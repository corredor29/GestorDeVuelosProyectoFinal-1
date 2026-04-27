using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.ReschedulingHistory.Domain.Aggregate;

public sealed class ReschedulingHistor
{
    // Definimos las propiedades de la clase 
    public int? Id {get; private set; }
    public int BookingId {get; private set;}
    public int PreviousFlightId {get; private set;}
    public int NewFlightId {get; private set;}
    public DateTime ChangedAt {get; private set;}
    public string Reason {get; private set;} = null!;
    // fin de la definicion 
    
    // constructor que evita qie se creen objetos invalidos 
    private ReschedulingHistor() {}
    // Constructor que fuerza el uso de metodos controlados y asegura consistencia 
    private ReschedulingHistor(
        int? id,
        int bookingId,
        int previousFlightId,
        int newFlighId,
        DateTime changedAt,
        string reason)
    {
        Id = id;
        BookingId = bookingId;
        PreviousFlightId = previousFlightId;
        NewFlightId = newFlighId;
        ChangedAt = changedAt;
        Reason = reason;
    }
    // metodo de creacion de la instancia 
    // se encarga primero de validar datos antes de crear el objeto
    // se asegura que el objeto siempre este en un estado valido y consistente
    // asigna valores a las propiedades de forma controlada y centralizada
    public static ReschedulingHistor Create(
        int bookingId,
        int previousFlightId,
        int newFlighId,
        string reason)
    {
        if (bookingId <= 0)
            throw new ArgumentException ("el id de la reserva no es valido",nameof(bookingId));
        if (previousFlightId <= 0)
            throw new ArgumentException("el id del vuelo no es valido", nameof(previousFlightId));
        if (newFlighId <= 0)
            throw new ArgumentException("el id del nuevo vuelo no es valido", nameof(newFlighId));
        if (previousFlightId == newFlighId)
            throw new ArgumentException("el vuelo nuevo no puede ser igual al vuelo anterior");
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("el motivo del cambio es obligatorio", nameof(reason));
        return new ReschedulingHistor( null, bookingId, previousFlightId, newFlighId, DateTime.UtcNow, reason.Trim());
    }
    // metodo que re construye la entidad a partir de los datos primarios ya hechos
    // se reconstruye el objeto para poder volver a tenerlo en memoria
    // no se realizan validaciones por q asume que todos los datos ya son validos 
    public static ReschedulingHistor FromPrimitives(
        int id,
        int bookingId,
        int previousFlightId,
        int newFlighId,
        DateTime changedAt,
        string reason)
        => new (id,bookingId,previousFlightId,newFlighId,changedAt,reason);

}
