using System;
using GestorDeVuelosProyectoFinal.Moduls.DocumentTypes.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.DocumentTypes.Domain.Aggregate;

public sealed class DocumentType
{
    public DocumentTypesId Id { get; private set; } = null!;
    public DocumentTypesName Name { get; private set; } = null!;
    public DocumentTypesCode Code { get; private set; } = null!;

    private DocumentType() { }

    private DocumentType(DocumentTypesId id, DocumentTypesName name, DocumentTypesCode code)
    {
        Id = id;
        Name = name;
        Code = code;
    }

    public static DocumentType Create(string name, string code)
    {
        return new DocumentType
        {
            Name = DocumentTypesName.Create(name),
            Code = DocumentTypesCode.Create(code)
        };
    }

    public static DocumentType FromPrimitives(int id, string name, string code)
    {
        return new DocumentType(
            DocumentTypesId.Create(id),
            DocumentTypesName.Create(name),
            DocumentTypesCode.Create(code)
        );
    }

    public void Update(string name, string code)
    {
        Name = DocumentTypesName.Create(name);
        Code = DocumentTypesCode.Create(code);
    }
}
