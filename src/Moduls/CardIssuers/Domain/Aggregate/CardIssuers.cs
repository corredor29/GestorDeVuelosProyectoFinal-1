using GestorDeVuelosProyectoFinal.Moduls.CardIssuers.Domain.ValueObject;
namespace GestorDeVuelosProyectoFinal.Moduls.CardIssuers.Domain.Aggregate;

public sealed class CardIssuer
{
    private CardIssuersName _name = null!;
    private CardIssuersIssuerNumber _issuerNumber = null!;

    public CardIssuersId Id { get; } = null!;
    public CardIssuersName Name => _name;
    public CardIssuersIssuerNumber IssuerNumber => _issuerNumber;

    private CardIssuer() { }

    private CardIssuer(CardIssuersId id, CardIssuersName name, CardIssuersIssuerNumber issuerNumber)
    {
        Id            = id;
        _name         = name;
        _issuerNumber = issuerNumber;
    }

    public static CardIssuer Create(int id, string name, string issuerNumber)
    {
        return new CardIssuer(
            CardIssuersId.Create(id),
            CardIssuersName.Create(name),
            CardIssuersIssuerNumber.Create(issuerNumber)
        );
    }

    public void UpdateName(string newName)        => _name         = CardIssuersName.Create(newName);
    public void UpdateIssuerNumber(string newNum) => _issuerNumber = CardIssuersIssuerNumber.Create(newNum);
}