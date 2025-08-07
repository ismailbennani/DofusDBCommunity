using System.Runtime.CompilerServices;
using System.Text.Json;
using DofusDbCommunity.ApiClients.Search;
using DofusDbCommunity.Models;

namespace DofusDbCommunity.ApiClients;

public static class DofusDbApiClientExtensions
{
    /// <summary>
    ///     Fetch ressources matching the search query from the API.
    ///     Performs multiple queries if necessary to fetch the requested number of results.
    /// </summary>
    /// <param name="client">The client instance to use for the requests.</param>
    /// <param name="query">The search query containing pagination parameters.</param>
    /// <param name="cancellationToken">The cancellation token to observe while waiting for the task to complete.</param>
    /// <typeparam name="TResource">The type of resource to fetch from the API.</typeparam>
    /// <returns>The search result containing all resources matching the query.</returns>
    public static async IAsyncEnumerable<TResource> MultiQuerySearchAsync<TResource>(
        this IDofusDbApiClient<TResource> client,
        SearchQuery query,
        [EnumeratorCancellation] CancellationToken cancellationToken = default
    ) where TResource: DofusDbEntity
    {
        int? requested = query.Limit;
        int? offset = query.Skip;

        SearchQuery firstQuery = new() { Limit = requested, Skip = offset, Sort = query.Sort, Select = query.Select, Predicates = query.Predicates };
        SearchResult<TResource> firstResults = await SearchImplAsync(client, firstQuery, cancellationToken);

        foreach (TResource result in firstResults.Data)
        {
            yield return result;
        }

        offset += firstResults.Data.Count;
        requested -= firstResults.Data.Count;
        int total = firstResults.Total;

        if (offset >= total)
        {
            yield break;
        }

        while (requested > 0 && offset < total)
        {
            SearchQuery currentQuery = new() { Limit = requested, Skip = offset, Sort = query.Sort, Select = query.Select, Predicates = query.Predicates };

            SearchResult<TResource> results = await SearchImplAsync(client, currentQuery, cancellationToken);

            foreach (TResource result in results.Data)
            {
                yield return result;
            }

            offset += results.Data.Count;
            requested -= results.Data.Count;
        }
    }

    static async Task<SearchResult<TResource>> SearchImplAsync<TResource>(IDofusDbApiClient<TResource> client, SearchQuery currentQuery, CancellationToken cancellationToken)
        where TResource: DofusDbEntity
    {
        try
        {
            return await client.SearchAsync(currentQuery, cancellationToken);
        }
        catch (Exception e)
        {
            throw new Exception($"Error while executing query {JsonSerializer.Serialize(currentQuery)}", e);
        }
    }
}
