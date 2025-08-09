namespace DofusSharp.DofusDb.Models;

public abstract class DofusDbEntity
{
    /// <summary>
    ///     The unique identifier of the entity.
    /// </summary>
    public int? Id { get; init; }
}
