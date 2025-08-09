namespace DofusSharp.DofusDb.Models.Items;

/// <summary>
///     A zone of effect associated with an item.
/// </summary>
public class ItemZone
{
    /// <summary>
    ///     The unique identifier of the shape of the zone of effect.
    /// </summary>
    public int? Shape { get; init; }

    /// <summary>
    ///     The first parameters of the zone of effect.
    ///     The interpretation of this parameter depends on the shape of the zone.
    /// </summary>
    public int? Param1 { get; init; }

    /// <summary>
    ///     The second parameters of the zone of effect.
    ///     The interpretation of this parameter depends on the shape of the zone.
    /// </summary>
    public int? Param2 { get; init; }

    /// <summary>
    ///     The damage decrease per cell of distance from the center of the zone of effect, in percent.
    /// </summary>
    public int? DamageDecreaseStepPercent { get; init; }

    /// <summary>
    ///     The max number of times the damage decrease can be applied to a cell in the zone of effect.
    /// </summary>
    public int? MaxDamageDecreaseApplyCount { get; init; }

    /// <summary>
    ///     ???
    /// </summary>
    public bool? IsStopAtTarget { get; init; }

    /// <summary>
    ///     The unique identifier of the cells that are affected by the zone of effect.
    /// </summary>
    public IReadOnlyCollection<int>? CellIds { get; init; }
}
