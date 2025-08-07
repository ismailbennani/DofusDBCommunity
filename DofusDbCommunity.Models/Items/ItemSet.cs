using DofusDbCommunity.Models.Common;

namespace DofusDbCommunity.Models.Items;

/// <summary>
///     A set of items that can be equipped together to gain bonuses.
/// </summary>
public class ItemSet : DofusDbEntity
{
    /// <summary>
    ///     The name of the item set.
    /// </summary>
    public MultiLangString? Name { get; init; }

    /// <summary>
    ///     The name of the item set as lower-case and without diacritics.
    /// </summary>
    public MultiLangString? Slug { get; init; }

    /// <summary>
    ///     The level of the item set. This is the level of the highest-level item in the set.
    /// </summary>
    public int? Level { get; init; }

    /// <summary>
    ///     The unique identifiers of the items that belong to the item set.
    /// </summary>
    public IReadOnlyCollection<Item>? Items { get; init; }

    /// <summary>
    ///     The unique identifiers of the item types of the items that belong to the item set.
    /// </summary>
    public IReadOnlyCollection<int>? TypeIds { get; init; }

    /// <summary>
    ///     Whether the bonus of the item set is a secret bonus.
    /// </summary>
    public bool? BonusIsSecret { get; init; }

    /// <summary>
    ///     The effects of the item set, applied when multiple items from the set are equipped together.
    ///     The collection of effects at index N is applied when N items from the set are equipped.
    /// </summary>
    public IReadOnlyList<IReadOnlyCollection<ItemEffect>>? Effects { get; init; }

    /// <summary>
    ///     Whether the items in the item set are cosmetic only.
    /// </summary>
    public bool? IsCosmetic { get; init; }

    /// <summary>
    ///     The creation date of the item in the database.
    /// </summary>
    public DateTimeOffset? CreatedAt { get; init; }

    /// <summary>
    ///     The last update date of the item in the database.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; init; }
}
