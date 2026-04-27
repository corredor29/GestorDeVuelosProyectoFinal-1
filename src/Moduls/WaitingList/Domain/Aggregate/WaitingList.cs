namespace GestorDeVuelosProyectoFinal.src.Moduls.WaitingList.Domain.Aggregate;

public sealed class WaitingLis
{
    // definimos las propiedades de la clase, y los campos de cada dato
    public int? Id { get; private set; }
    public int BookingId { get; private set; }
    public int FlightId { get; private set; }
    public DateTime RequestedAt { get; private set; }
    public int Priority { get; private set; }
    public string Status { get; private set; } = null!;
    // constructor que evita qie se creen objetos invalidos 
    private WaitingLis() { }
    // Constructor que fuerza el uso de metodos controlados y asegura consistencia 

    private WaitingLis(
        int? id,
        int bookingId,
        int flightId,
        DateTime requestedAt,
        int priority,
        string status)
    {
        Id = id;
        BookingId = bookingId;
        FlightId = flightId;
        RequestedAt = requestedAt;
        Priority = priority;
        Status = status;
    }
    // metodo de creacion de la instancia 
    // se encarga primero de validar datos antes de crear el objeto
    // se asegura que el objeto siempre este en un estado valido y consistente
    // asigna valores a las propiedades de forma controlada y centralizada
    public static WaitingLis Create(int bookingId, int flightId, int priority)
    {
        if (bookingId <= 0)
            throw new ArgumentException("El id de la reserva no es válido.", nameof(bookingId));
        if (flightId <= 0)
            throw new ArgumentException("El id del vuelo no es válido.", nameof(flightId));
        if (priority < 0)
            throw new ArgumentException("La prioridad no puede ser negativa.", nameof(priority));

        return new WaitingLis(null, bookingId, flightId, DateTime.UtcNow, priority, "Waiting");
    }
    // metodo que re construye la entidad a partir de los datos primarios ya hechos
    // se reconstruye el objeto para poder volver a tenerlo en memoria
    // no se realizan validaciones por q asume que todos los datos ya son validos 
    public static WaitingLis FromPrimitives(
        int id,
        int bookingId,
        int flightId,
        DateTime requestedAt,
        int priority,
        string status)
        => new(id, bookingId, flightId, requestedAt, priority, status);
    
    // modifican el estado del objeto de forma controlada
    public void Promote() => Status = "Promoted";
    public void Cancel()  => Status = "Cancelled";
}