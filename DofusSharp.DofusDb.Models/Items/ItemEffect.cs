namespace DofusSharp.DofusDb.Models.Items;

/// <summary>
///     An effect of an item, applied when the item is used.
/// </summary>
public class ItemEffect
{
    /// <summary>
    ///     The minimum value of the characteristic.
    /// </summary>
    public int? From { get; init; }

    /// <summary>
    ///     The maximum value of the characteristic.
    /// </summary>
    public int? To { get; init; }

    /// <summary>
    ///     The unique identifier of the element associated with the effect.
    /// </summary>
    public int? ElementId { get; init; }

    /// <summary>
    ///     The unique identifier of the effect.
    /// </summary>
    public int? EffectId { get; init; }

    /// <summary>
    ///     The unique identifier of the characteristic associated with the effect.
    ///     If no characteristic is associated, this will be -1.
    /// </summary>
    public int? Characteristic { get; init; }

    /// <summary>
    ///     The unique identifier of the category associated with the effect.
    /// </summary>
    public int? Category { get; init; }
}
