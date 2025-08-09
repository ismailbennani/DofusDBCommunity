namespace DofusSharp.DofusDb.ApiClients.Search;

public static class StringExtensions
{
    public static string ToCamelCase(this string name) => string.IsNullOrWhiteSpace(name) ? "" : $"{char.ToLowerInvariant(name[0])}{name[1..]}";
}
