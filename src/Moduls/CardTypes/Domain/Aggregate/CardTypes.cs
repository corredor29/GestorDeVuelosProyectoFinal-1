using System;
using GestorDeVuelosProyectoFinal.src.Moduls.CardTypes.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.CardTypes.Domain.Aggregate;

public sealed class CardTypes
{
    // Propiedades con set privado para garantizar el encapsulamiento
    public CardTypesId Id { get; private set; } = null!;
    public CardTypesName Name { get; private set; } = null!;

    // Constructor vacío para ORMs como Entity Framework (necesario a veces)
    private CardTypes() { }

    // Constructor principal privado
    private CardTypes(
        CardTypesId id,
        CardTypesName name)
    {
        Id = id;
        Name = name;
    }

    public static CardTypes Create(
        int id,
        string name)
    {
        return new CardTypes(
            CardTypesId.Create(id),
            CardTypesName.Create(name)
        );
    }

    public void UpdateName(string newName)
    {
        // El Value Object CardTypesName se encarga de validar (longitud, números, etc.)
        Name = CardTypesName.Create(newName);
    }
}
