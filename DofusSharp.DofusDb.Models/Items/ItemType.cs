using DofusSharp.DofusDb.Models.Common;

namespace DofusSharp.DofusDb.Models.Items;

/// <summary>
///     A type of item, used for categorization.
/// </summary>
public class ItemType : DofusDbEntity
{
    /// <summary>
    ///     The name of the item type.
    /// </summary>
    public MultiLangString? Name { get; init; }

    /// <summary>
    ///     The unique identifier of the super type of the item type.
    /// </summary>
    public int? SuperTypeId { get; init; }

    /// <summary>
    ///     The super type of the item type.
    /// </summary>
    public ItemSuperType? SuperType { get; init; }

    /// <summary>
    ///     The unique identifier of the category of the item type.
    /// </summary>
    public int? CategoryId { get; init; }

    /// <summary>
    ///     Whether the item type is in the website's encyclopedia.
    /// </summary>
    public bool? IsInEncyclopedia { get; init; }

    /// <summary>
    ///     Whether the name of the item type is plural or singular.
    /// </summary>
    public bool? Plural { get; init; }

    /// <summary>
    ///     The gender of the name of the item type.
    /// </summary>
    public Gender? Gender { get; init; }

    /// <summary>
    ///     The zone of effect associated with the item type.
    /// </summary>
    public string? RawZone { get; init; }

    /// <summary>
    ///     Whether the items of this type can be mimicked.
    /// </summary>
    public bool? Mimickable { get; init; }

    /// <summary>
    ///     The ratio to apply to the experience gained from crafting items of this type.
    ///     If the ratio should be the default one, this will be -1.
    /// </summary>
    public int? CraftXpRatio { get; init; }

    /// <summary>
    ///     The possible slots where items of this type can be equipped.
    /// </summary>
    public IReadOnlyCollection<int>? PossiblePositions { get; init; }

    /// <summary>
    ///     The creation date of the item in the database.
    /// </summary>
    public DateTimeOffset? CreatedAt { get; init; }

    /// <summary>
    ///     The last update date of the item in the database.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; init; }

    /// <summary>
    ///     ???
    /// </summary>
    public int? EvolutiveTypeId { get; init; }
}
