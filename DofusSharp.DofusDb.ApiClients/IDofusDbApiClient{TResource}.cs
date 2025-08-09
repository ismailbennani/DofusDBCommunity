using DofusSharp.DofusDb.ApiClients.Search;
using DofusSharp.DofusDb.Models;

namespace DofusSharp.DofusDb.ApiClients;

public interface IDofusDbApiClient<TResource> where TResource: DofusDbEntity
{
    /// <summary>
    ///     Fetch the resource with the specified ID from the API.
    /// </summary>
    /// <param name="id">The unique identifier of the resource to fetch.</param>
    /// <param name="cancellationToken">The cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>The resource with the specified ID.</returns>
    Task<TResource> GetAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Fetch the number of resources available in the API.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>The total count of resources.</returns>
    Task<int> CountAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Fetch a paginated list of resources from the API based on the provided search query.
    /// </summary>
    /// <param name="query">The search query containing pagination parameters.</param>
    /// <param name="cancellationToken">The cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>The search result containing the resources matching the query.</returns>
    Task<SearchResult<TResource>> SearchAsync(SearchQuery query, CancellationToken cancellationToken = default);
}
