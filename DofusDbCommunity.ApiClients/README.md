# DofusDB Community - API Clients

Provides API clients for the DofusDB API. It offers both a low-level client (`DofusDbApiClient<TResource>`) for maximum control and a high-level, fluent interface (`DofusDbQuery<TResource>`) for building requests using LINQ-like statements.

## Installation

Install via NuGet:

```
dotnet add package DofusDbCommunity.ApiClients
```

## Usage

In both the examples below we will fetch the items from level 50 to 100, that are not consumables, select only the `name` field and order them by `realWeight` in descending order.

### Query interface (recommended)

The query interface returns an `IAsyncEnumerable<TResource>` that will fetch all the available pages automatically while iterating over the results.

```csharp
DofusDbQuery<Item> query = DofusDbQuery.Production().Items()
    .Select(i => i.Name)
    .OrderByDescending(i => i.RealWeight)
    .Where(i => i.Level >= 50 && i.Level <= 100 && i.Usable == false);
Item[] items = await query.ExecuteAsync().ToArrayAsync();
```

> ![Note] **Known issue**
> All model fields are nullable because the API supports a `select` operator for partial field selection. 
> As a result, enabling nullable analysis may cause compiler warnings about possible null references in expression subtrees. 
> However, these warnings are safe to ignore in this context, since the expressions are only used to determine property names for request parameters and will not cause null reference exceptions at runtime.

### Low-level client

The low-level client grants direct access to the request parameters exposed by `FeatherJS`.

```csharp
IDofusDbApiClient<Item> client = DofusDbApiClient.Production().Items();
SearchResult<Item> items = await client.SearchAsync(
    new SearchQuery
    {
        Limit = 50,
        Select = ["name"],
        Sort = new Dictionary<string, SearchQuerySortOrder> { { "realWeight", SearchQuerySortOrder.Descending } }, 
        Predicates =
        [
            new SearchPredicate.GreaterThanOrEqual("level", "50"),
            new SearchPredicate.LessThanOrEqual("level", "100"),
            new SearchPredicate.Eq("usable", "false")
        ]
    }
);
```

**Note:** the query string generated from a `SearchQuery` can be computed by calling the `ToQueryString` extension method on the `SearchQuery` object. This can be useful for debugging or logging purposes. 
For example the search query above would generate the following query string:

```
limit=50&select=name&sort[realWeight]=desc&level[$gte]=50&level[$lte]=100&usable=false
```

## Contributing

Contributions are welcome! Please open issues or submit pull requests for bug fixes, features, or documentation improvements.

## License

This project is licensed under the MIT License. See the `LICENSE` file for details.