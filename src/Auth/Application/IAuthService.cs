namespace GestorDeVuelosProyectoFinal.Auth.Application;

public interface IAuthService
{
    Task EnsureSeededAsync(CancellationToken cancellationToken = default);

    Task<bool> TryLoginAsync(string username, string password, CancellationToken cancellationToken = default);

    Task RegisterUserAsync(string username, string password, CancellationToken cancellationToken = default);
}
