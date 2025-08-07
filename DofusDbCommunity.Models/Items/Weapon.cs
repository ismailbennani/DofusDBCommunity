namespace DofusDbCommunity.Models.Items;

/// <summary>
///     An item in the game.
/// </summary>
public class Weapon : Item
{
    /// <summary>
    ///     The probability of a critical hit when using the item, in percent.
    /// </summary>
    public int? CriticalHitProbability { get; init; }

    /// <summary>
    ///     The bonus damages applied to critical hits when using the item.
    /// </summary>
    public int? CriticalHitBonus { get; init; }

    /// <summary>
    ///     The probability of a critical failure when using the item, in percent.
    /// </summary>
    public int? CriticalFailureProbability { get; init; }

    /// <summary>
    ///     The minimum range of the item when used in combat.
    /// </summary>
    public int? MinRange { get; init; }

    /// <summary>
    ///     The maximum range of the item when used in combat.
    /// </summary>
    public int? Range { get; init; }

    /// <summary>
    ///     Whether casting this item in combat can be done in line only.
    /// </summary>
    public bool? CastInLine { get; init; }

    /// <summary>
    ///     Whether casting this item in combat can be done in diagonal only.
    /// </summary>
    public bool? CastInDiagonal { get; init; }

    /// <summary>
    ///     Whether casting this item in combat requires line of sight to the target.
    /// </summary>
    public bool? CastTestLos { get; init; }

    /// <summary>
    ///     The maximum number of times the item can be cast in a single turn.
    /// </summary>
    public int? MaxCastPerTurn { get; init; }

    /// <summary>
    ///     The cost in action points (AP) to use the item.
    /// </summary>
    public int? ApCost { get; init; }

    /// <summary>
    ///     Whether the item occupies both hands.
    /// </summary>
    public bool? TwoHanded { get; init; }
}
