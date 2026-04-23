using System;
using GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Domain.Aggregate;

public sealed class InvoiceItemType
{
    public InvoiceItemTypesId Id { get; private set; } = null!;
    public InvoiceItemTypesName Name { get; private set; } = null!;

    private InvoiceItemType() { }

    private InvoiceItemType(InvoiceItemTypesId id, InvoiceItemTypesName name)
    {
        Id = id;
        Name = name;
    }

    public static InvoiceItemType Create(int id, string name)
    {
        return new InvoiceItemType(
            InvoiceItemTypesId.Create(id),
            InvoiceItemTypesName.Create(name)
        );
    }
    internal void SetId(int id) => Id = InvoiceItemTypesId.Create(id);

    public void UpdateName(string newName)
    {
        Name = InvoiceItemTypesName.Create(newName);
    }
}
