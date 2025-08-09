using DofusSharp.DofusDb.Models.Items;

namespace DofusSharp.DofusDb.ApiClients;

public class DofusDbApiClientsFactory(Uri baseUrl)
{
    public IDofusDbApiClient<Item> Items() => new DofusDbApiClient<Item>(new Uri(baseUrl, "items/"));
    public IDofusDbApiClient<ItemType> ItemTypes() => new DofusDbApiClient<ItemType>(new Uri(baseUrl, "item-types/"));
    public IDofusDbApiClient<ItemSuperType> ItemSuperTypes() => new DofusDbApiClient<ItemSuperType>(new Uri(baseUrl, "item-super-types/"));
    public IDofusDbApiClient<ItemSet> ItemSets() => new DofusDbApiClient<ItemSet>(new Uri(baseUrl, "item-sets/"));
}
