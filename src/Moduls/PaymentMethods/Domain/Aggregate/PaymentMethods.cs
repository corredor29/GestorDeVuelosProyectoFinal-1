using System;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.Domain.Aggregate;

public sealed class PaymentMethod
{
    // Propiedades con set privado para garantizar el encapsulamiento
    public PaymentMethodsId Id { get; private set; } = null!;
    public PaymentMethodsDisplayName DisplayName { get; private set; } = null!;

    // Constructor vacío para ORMs como Entity Framework (necesario a veces)
    private PaymentMethod() { }

    // Constructor principal privado
    private PaymentMethod(
        PaymentMethodsId id,
        PaymentMethodsDisplayName displayName)
    {
        Id = id;
        DisplayName = displayName;
    }

    public static PaymentMethod Create(
        int id,
        string displayName)
    {
        return new PaymentMethod(
            PaymentMethodsId.Create(id),
            PaymentMethodsDisplayName.Create(displayName)
        );
    }

    public void UpdateDisplayName(string newDisplayName)
    {
        // El Value Object PaymentMethodsDisplayName se encarga de validar (longitud, números, etc.)
        DisplayName = PaymentMethodsDisplayName.Create(newDisplayName);
    }
    public void SetId(int id)
    {
        Id = PaymentMethodsId.Create(id);
    }
}
