using DofusSharp.DofusDb.ApiClients;
using DofusSharp.DofusDb.ApiClients.Search;
using DofusSharp.DofusDb.Models.Items;
using FluentAssertions;

namespace Tests.EndToEnd.DofusDb.ApiClients;

public class ItemSetsClientTest
{
    [Fact]
    public async Task ItemSetsClient_Should_GetItemSet()
    {
        IDofusDbApiClient<ItemSet> client = DofusDbApiClient.Beta().ItemSets();
        ItemSet value = await client.GetAsync(1);
        await Verify(value);
    }

    [Fact]
    public async Task ItemSetsClient_Should_SearchItemSets()
    {
        IDofusDbApiClient<ItemSet> client = DofusDbApiClient.Beta().ItemSets();

        // we don't want to assert results here because they might change with each update, we just want to ensure that all the items are parsed correctly
        // which means that no exception is thrown during the search
        SearchQuery query = new();
        ItemSet[] results = await client.MultiQuerySearchAsync(query).ToArrayAsync();
        int count = await client.CountAsync();

        results.Length.Should().Be(count);
    }
}
