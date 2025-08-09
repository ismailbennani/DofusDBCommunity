using DofusSharp.DofusDb.Models.Common;

namespace DofusSharp.DofusDb.Models.Items;

/// <summary>
///     A super type of item, used for categorization.
/// </summary>
public class ItemSuperType : DofusDbEntity
{
    /// <summary>
    ///     The name of the item super type.
    /// </summary>
    public MultiLangString? Name { get; init; }

    /// <summary>
    ///     The possible slots where items of this super type can be equipped.
    /// </summary>
    public IReadOnlyCollection<int>? Positions { get; init; }

    /// <summary>
    ///     The creation date of the item in the database.
    /// </summary>
    public DateTimeOffset? CreatedAt { get; init; }

    /// <summary>
    ///     The last update date of the item in the database.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; init; }
}
