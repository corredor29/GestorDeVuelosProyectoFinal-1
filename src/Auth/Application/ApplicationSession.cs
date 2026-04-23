namespace GestorDeVuelosProyectoFinal.Auth.Application;

public static class ApplicationSession
{
    public static int? UserId { get; set; }
    public static string? Username { get; set; }
    public static string? Role { get; set; }

    public static bool IsAuthenticated => UserId.HasValue && !string.IsNullOrWhiteSpace(Role);

    public static void Clear()
    {
        UserId = null;
        Username = null;
        Role = null;
    }
}
