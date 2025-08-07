using DofusDbCommunity.ApiClients;
using DofusDbCommunity.ApiClients.Search;
using DofusDbCommunity.Models.Items;
using FluentAssertions;

namespace Tests.EndToEnd.ApiClients;

public class ItemSuperTypesClientTest
{
    [Fact]
    public async Task ItemSuperTypesClient_Should_GetItemSuperType()
    {
        IDofusDbApiClient<ItemSuperType> client = DofusDbApiClient.Beta().ItemSuperTypes();
        ItemSuperType value = await client.GetAsync(1);
        await Verify(value);
    }

    [Fact]
    public async Task ItemSuperTypesClient_Should_SearchItemSuperTypes()
    {
        IDofusDbApiClient<ItemSuperType> client = DofusDbApiClient.Beta().ItemSuperTypes();

        // we don't want to assert results here because they might change with each update, we just want to ensure that all the items are parsed correctly
        // which means that no exception is thrown during the search
        SearchQuery query = new();
        ItemSuperType[] results = await client.MultiQuerySearchAsync(query).ToArrayAsync().AsTask();
        int count = await client.CountAsync();

        results.Length.Should().Be(count);
    }
}
