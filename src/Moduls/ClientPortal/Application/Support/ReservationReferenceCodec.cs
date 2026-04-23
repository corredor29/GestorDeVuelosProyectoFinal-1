using System.Buffers.Binary;
using System.Globalization;
using System.Linq;

namespace GestorDeVuelosProyectoFinal.src.Moduls.ClientPortal.Application.Support;

/// <summary>
/// Crea un código de reserva corto y “aleatorio” a partir del id interno, verificable sin columna extra en BD.
/// </summary>
public static class ReservationReferenceCodec
{
    // Alfabeto Crockford Base32 (sin I, L, O, U) — legible en consola.
    private const string Alphabet = "0123456789ABCDEFGHJKMNPQRSTVWXYZ";

    /// <summary>
    /// Quita espacios/guiones y corrige confusiones típicas al copiar (p. ej. O vs 0).
    /// </summary>
    public static string NormalizeInput(string? code)
    {
        if (string.IsNullOrWhiteSpace(code))
            return "";

        var s = code.Trim()
            .Replace(" ", "", StringComparison.Ordinal)
            .Replace("-", "", StringComparison.Ordinal)
            .Replace("_", "", StringComparison.Ordinal);

        // Crockford no usa la letra O; suele copiarse como cero.
        s = s.Replace("O", "0", StringComparison.OrdinalIgnoreCase);

        return s.ToUpperInvariant();
    }

    /// <summary>
    /// Compara lo que escribió el usuario con el código oficial de una reserva (longitud 13).
    /// </summary>
    public static bool MatchesBookingCode(string input, int bookingId)
    {
        var n = NormalizeInput(input);
        if (n.Length == 0)
            return false;

        var expected = Encode(bookingId);
        if (string.Equals(n, expected, StringComparison.Ordinal))
            return true;

        if (n.Length <= 13 && n.All(ch => Alphabet.IndexOf(ch) >= 0))
            return string.Equals(n.PadLeft(13, '0'), expected, StringComparison.Ordinal);

        return false;
    }

    /// <summary>
    /// Obtiene el id interno de reserva a partir del texto (número o código Base32).
    /// </summary>
    public static bool TryParseReservationCode(string input, out int bookingId)
    {
        bookingId = 0;
        var n = NormalizeInput(input);
        if (n.Length == 0)
            return false;

        if (int.TryParse(n, NumberStyles.Integer, CultureInfo.InvariantCulture, out var numeric) && numeric > 0)
        {
            bookingId = numeric;
            return true;
        }

        foreach (var variant in EnumerateDecodeVariants(n))
        {
            if (TryDecode(variant, out var decoded) && decoded > 0)
            {
                bookingId = decoded;
                return true;
            }
        }

        return false;
    }

    private static IEnumerable<string> EnumerateDecodeVariants(string normalized)
    {
        yield return normalized;

        if (normalized.Length is > 0 and < 13 && normalized.All(ch => Alphabet.IndexOf(ch) >= 0))
            yield return normalized.PadLeft(13, '0');
    }

    private static ushort Crc16CcittFalse(ushort seed, ReadOnlySpan<byte> data)
    {
        var crc = seed;
        foreach (var b in data)
        {
            crc ^= (ushort)(b << 8);
            for (var i = 0; i < 8; i++)
                crc = (crc & 0x8000) != 0 ? (ushort)((crc << 1) ^ 0x1021) : (ushort)(crc << 1);
        }
        return crc;
    }

    public static string Encode(int bookingId)
    {
        if (bookingId <= 0)
            throw new ArgumentOutOfRangeException(nameof(bookingId));

        Span<byte> payload = stackalloc byte[4];
        BinaryPrimitives.WriteInt32BigEndian(payload, bookingId);

        var crc = Crc16CcittFalse(0xFFFF, payload);
        var value = ((ulong)crc << 32) | (uint)bookingId;

        Span<char> chars = stackalloc char[13];
        var idx = chars.Length - 1;
        do
        {
            chars[idx--] = Alphabet[(int)(value % 32)];
            value /= 32;
        } while (value > 0 && idx >= 0);

        while (idx >= 0)
            chars[idx--] = Alphabet[0];

        return new string(chars);
    }

    public static bool TryDecode(string code, out int bookingId)
    {
        bookingId = 0;
        var trimmed = (code ?? "").Trim().Replace(" ", "", StringComparison.Ordinal).Replace("-", "", StringComparison.Ordinal);
        if (trimmed.Length == 0)
            return false;

        ulong value = 0;
        foreach (var ch in trimmed.ToUpperInvariant())
        {
            var i = Alphabet.IndexOf(ch);
            if (i < 0)
                return false;
            value = (value * 32) + (ulong)i;
        }

        var id = (int)(value & 0xFFFFFFFFu);
        var crc = (ushort)(value >> 32);
        if (id <= 0)
            return false;

        Span<byte> payload = stackalloc byte[4];
        BinaryPrimitives.WriteInt32BigEndian(payload, id);
        return Crc16CcittFalse(0xFFFF, payload) == crc;
    }
}
