using System;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes.Domain.Aggregate;

public sealed class PaymentMediumType
{
    // Propiedades con set privado para garantizar el encapsulamiento
    public PaymentMediumTypesId Id { get; private set; } = null!;
    public PaymentMediumTypesName Name { get; private set; } = null!;

    // Constructor vacío para ORMs como Entity Framework (necesario a veces)
    private PaymentMediumType() { }

    // Constructor principal privado
    private PaymentMediumType(
        PaymentMediumTypesId id,
        PaymentMediumTypesName name)
    {
        Id = id;
        Name = name;
    }   

    public static PaymentMediumType Create(
        int id,
        string name)
    {
        return new PaymentMediumType(
            PaymentMediumTypesId.Create(id),
            PaymentMediumTypesName.Create(name)
        );
    }

    public void UpdateName(string newName)
    {
        // El Value Object PaymentMediumTypesName se encarga de validar (longitud, números, etc.)
        Name = PaymentMediumTypesName.Create(newName);
    }
}
