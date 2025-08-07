namespace DofusDbCommunity.ApiClients.Search;

/// <summary>
///     A query for searching items in the Dofus DB API.
/// </summary>
public class SearchQuery
{
    /// <summary>
    ///     The maximum number of items to return in the result set.
    /// </summary>
    public int? Limit { get; init; }

    /// <summary>
    ///     The number of items to skip before starting to collect the result set.
    /// </summary>
    public int? Skip { get; init; }

    /// <summary>
    ///     The order in which to sort the results.
    /// </summary>
    public IReadOnlyDictionary<string, SearchQuerySortOrder> Sort { get; init; } = new Dictionary<string, SearchQuerySortOrder>();

    /// <summary>
    ///     The fields to include in the result set.
    /// </summary>
    public IReadOnlyCollection<string> Select { get; init; } = [];

    /// <summary>
    ///     The predicate to apply to the search query, allowing for complex filtering of results.
    /// </summary>
    public IReadOnlyCollection<SearchPredicate> Predicates { get; init; } = [];
}

public static class SearchQueryExtensions
{
    /// <summary>
    ///     Converts the search query to a string representation for use in API requests.
    /// </summary>
    /// <param name="query">The search query to convert.</param>
    /// <returns>A string representation of the search query.</returns>
    public static string ToQueryString(this SearchQuery query)
    {
        SearchRequestQueryParamsBuilder builder = new();
        return builder.BuildQueryParams(query);
    }
}
