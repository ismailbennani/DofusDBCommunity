using DofusSharp.DofusDb.ApiClients;
using DofusSharp.DofusDb.ApiClients.Search;
using DofusSharp.DofusDb.Models.Items;
using FluentAssertions;

namespace Tests.EndToEnd.DofusDb.ApiClients;

public class ItemTypesClientTest
{
    [Fact]
    public async Task ItemTypesClient_Should_GetItemType()
    {
        IDofusDbApiClient<ItemType> client = DofusDbApiClient.Beta().ItemTypes();
        ItemType value = await client.GetAsync(1);
        await Verify(value);
    }

    [Fact]
    public async Task ItemTypesClient_Should_SearchItemTypes()
    {
        IDofusDbApiClient<ItemType> client = DofusDbApiClient.Beta().ItemTypes();

        // we don't want to assert results here because they might change with each update, we just want to ensure that all the items are parsed correctly
        // which means that no exception is thrown during the search
        SearchQuery query = new();
        ItemType[] results = await client.MultiQuerySearchAsync(query).ToArrayAsync();
        int count = await client.CountAsync();

        results.Length.Should().Be(count);
    }
}
