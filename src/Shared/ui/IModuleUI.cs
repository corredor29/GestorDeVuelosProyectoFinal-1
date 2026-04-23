namespace GestorDeVuelosProyectoFinal.src.Shared.ui;

// Contrato base para los módulos que se ejecutan desde la consola.
// Así todos comparten una firma parecida y pueden registrarse de forma consistente.
public interface IModuleUI
{
    string Key { get; }
    string Title { get; }
    Task RunAsync(CancellationToken cancellationToken = default);
}
