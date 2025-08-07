namespace DofusDbCommunity.ApiClients.Search;

/// <summary>
///     A paginated search result.
/// </summary>
/// <typeparam name="TData">The type of data contained in the search result.</typeparam>
public class SearchResult<TData>
{
    /// <summary>
    ///     The total number of items that match the search criteria.
    /// </summary>
    public required int Total { get; init; }

    /// <summary>
    ///     The maximum number of items to return in the result set.
    /// </summary>
    public required int Limit { get; init; }

    /// <summary>
    ///     The number of items to skip before starting to collect the result set.
    /// </summary>
    public required int Skip { get; init; }

    /// <summary>
    ///     The data items returned in the search result.
    /// </summary>
    public required IReadOnlyList<TData> Data { get; init; }
}
