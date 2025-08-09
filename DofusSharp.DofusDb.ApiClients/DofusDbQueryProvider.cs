﻿using DofusSharp.DofusDb.Models.Items;

namespace DofusSharp.DofusDb.ApiClients;

public class DofusDbQueryProvider(DofusDbApiClientsFactory factory)
{
    public DofusDbQuery<Item> Items() => new(factory.Items());
    public DofusDbQuery<ItemType> ItemTypes() => new(factory.ItemTypes());
    public DofusDbQuery<ItemSuperType> ItemSuperTypes() => new(factory.ItemSuperTypes());
    public DofusDbQuery<ItemSet> ItemSets() => new(factory.ItemSets());
}
