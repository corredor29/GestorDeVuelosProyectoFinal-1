using System;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.Domain.ValueObject;
namespace GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.Domain.Aggregate;

public sealed class PaymentStatuse
{
    public PaymentStatuseId Id { get; private set; } = null!;
    public PaymentStatuseName Name { get; private set; } = null!;

    private PaymentStatuse() { }

    private PaymentStatuse(
        PaymentStatuseId id,
        PaymentStatuseName name)
    {
        Id = id;
        Name = name;
    }

    public static PaymentStatuse Create(
        int id,
        string name)
    {
        return new PaymentStatuse(
            PaymentStatuseId.Create(id),
            PaymentStatuseName.Create(name)
        );
    }

    public void UpdateName(string newName)
    {
        // El Value Object PaymentStatuseName se encarga de validar (longitud, números, etc.)
        Name = PaymentStatuseName.Create(newName);
    }
}
