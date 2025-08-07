using System.Diagnostics.CodeAnalysis;

namespace DofusDbCommunity.Models.Common;

/// <summary>
///     Either a value of type T, or false.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
public class ValueOrFalse<T>
{
    /// <summary>
    ///     The value of the object, or null if it is false.
    /// </summary>
    public T? Value { get; init; }

    /// <summary>
    ///     Whether the value is false.
    /// </summary>
    [MemberNotNullWhen(false, nameof(Value))]
    public bool IsFalse => Value is null;

    public static implicit operator T?(ValueOrFalse<T> valueOrFalse) => valueOrFalse.Value;
    public static implicit operator ValueOrFalse<T>(T value) => new() { Value = value };
}
