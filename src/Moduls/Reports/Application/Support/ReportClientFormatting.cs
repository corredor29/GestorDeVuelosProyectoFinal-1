namespace GestorDeVuelosProyectoFinal.src.Moduls.Reports.Application.Support;

internal static class ReportClientFormatting
{
    /// <summary>
    /// Evita "Cliente usuario": si coincide con alta portal (Cliente + apellido = login), muestra solo el usuario.
    /// </summary>
    public static string BuildClientDisplayName(string firstName, string lastName, string? username)
    {
        var f = (firstName ?? "").Trim();
        var l = (lastName ?? "").Trim();
        var u = (username ?? "").Trim();

        if (u.Length > 0
            && f.Equals("Cliente", StringComparison.OrdinalIgnoreCase)
            && l.Equals(u, StringComparison.OrdinalIgnoreCase))
            return u;

        return $"{f} {l}".Trim();
    }

    public static string BuildIdentityDocument(string? docTypeCode, string documentNumber)
    {
        var n = (documentNumber ?? "").Trim();
        if (n.Length == 0)
            return "—";

        var code = (docTypeCode ?? "").Trim();
        return code.Length > 0 ? $"{code} {n}" : n;
    }
}
