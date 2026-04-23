using System;

namespace GestorDeVuelosProyectoFinal.Moduls.CabinConfiguration.Domain.ValueObject;

public sealed class CabinRowRange
{
    public int StartRow { get; }
    public int EndRow { get; }

    private CabinRowRange(int start, int end)
    {
        StartRow = start;
        EndRow = end;
    }

    public static CabinRowRange Create(int start, int end)
    {
        if (start <= 0 || end <= 0)
            throw new ArgumentException("Las filas deben ser números positivos mayores a cero.");

        if (end < start)
            throw new ArgumentException("La fila de fin no puede ser menor a la fila de inicio.");

        return new CabinRowRange(start, end);
    }
}
