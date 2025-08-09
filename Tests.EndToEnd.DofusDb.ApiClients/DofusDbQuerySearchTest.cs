using DofusSharp.DofusDb.ApiClients;
using DofusSharp.DofusDb.Models.Items;
using FluentAssertions;

namespace Tests.EndToEnd.DofusDb.ApiClients;

public class DofusDbQuerySearchTest
{
    [Fact]
    public async Task ShouldLimitSearchResults()
    {
        DofusDbQuery<Item> query = DofusDbQuery.Beta().Items();
        Item[] items = await query.Take(12).ExecuteAsync().ToArrayAsync();
        items.Length.Should().Be(12);
    }

    [Fact]
    public async Task ShouldSkipSearchResults()
    {
        DofusDbQuery<Item> query = DofusDbQuery.Beta().Items();
        Item[] firstAndSecondItems = await query.Take(2).ExecuteAsync().ToArrayAsync();

        Item[] secondItem = await query.Skip(1).Take(1).ExecuteAsync().ToArrayAsync();

        secondItem.Should().BeEquivalentTo([firstAndSecondItems[1]]);
    }

    [Fact]
    public async Task ShouldSortSearchResults()
    {
        DofusDbQuery<Item> query = DofusDbQuery.Beta().Items();

        Item[] sortedSearchResults = await query.Take(50).SortByDescending(i => i.RealWeight).ExecuteAsync().ToArrayAsync();

        // expect the results to be sorted by name.fr in descending order
        IOrderedEnumerable<Item> sortedData = sortedSearchResults.OrderByDescending(d => d.RealWeight);
        sortedSearchResults.Should().BeEquivalentTo(sortedData, opt => opt.WithStrictOrdering());
    }

    [Fact]
    public async Task ShouldSelectSearchResults()
    {
        DofusDbQuery<Item> query = DofusDbQuery.Beta().Items();

        Item[] results = await query.Take(1).Select(i => i.RealWeight).ExecuteAsync().ToArrayAsync();

        results[0]
            .Should()
            .BeEquivalentTo(
                new Item
                {
                    // the selected field is not null
                    RealWeight = 1,
                    // Img is not null for whatever reason, looks like a bug in the API
                    Img = "https://api.beta.dofusdb.fr/img/items/undefined.png",
                    // all the other fields are null
                    Id = null,
                    TypeId = null,
                    Type = null,
                    Name = null,
                    Slug = null,
                    Description = null,
                    ImportantNotice = null,
                    Level = null,
                    Price = null,
                    Criteria = null,
                    CreatedAt = null,
                    UpdatedAt = null,
                    ItemSetId = null,
                    ItemSet = null,
                    HideEffects = null,
                    Effects = null,
                    EvolutiveEffectIds = null,
                    FavoriteSubAreas = null,
                    FavoriteSubAreasBonus = null,
                    IconId = null,
                    AppearanceId = null,
                    IsColorable = null,
                    HasLivingObjectSkinJntMood = null,
                    HasRecipe = null,
                    RecipeSlots = null,
                    RecipeIds = null,
                    RecipesThatUse = null,
                    CraftXpRatio = null,
                    CraftVisible = null,
                    CraftConditional = null,
                    CraftFeasible = null,
                    SecretRecipe = null,
                    BonusIsSecret = null,
                    Usable = null,
                    Targetable = null,
                    CriteriaTarget = null,
                    NonUsableOnAnother = null,
                    NeedUseConfirm = null,
                    UseAnimationId = null,
                    ResourcesBySubarea = null,
                    DropMonsterIds = null,
                    DropTemporisMonsterIds = null,
                    DropSubAreaIds = null,
                    RecyclingNuggets = null,
                    FavoriteRecyclingSubareas = null,
                    QuestsThatUse = null,
                    QuestsThatReward = null,
                    StartLegendaryTreasureHunt = null,
                    LegendaryTreasureHuntThatReward = null,
                    Flags = null,
                    Cursed = null,
                    Exchangeable = null,
                    Etheral = null,
                    Enhanceable = null,
                    ObjectIsDisplayOnWeb = null,
                    IsDestructible = null,
                    IsSaleable = null,
                    IsLegendary = null,
                    Visibility = null,
                    ChangeVersion = null,
                    TooltipExpirationDate = null
                }
            );
    }

    [Fact]
    public async Task ShouldFilterSearchResults()
    {
        DofusDbQuery<Item> query = DofusDbQuery.Beta().Items();

        Item[] sortedSearchResults = await query.Where(i => i.Level > 27 && i.Level < 29).ExecuteAsync().ToArrayAsync();

        // expect the results to be sorted by name.fr in descending order
        sortedSearchResults.Should().AllSatisfy(d => d.Level.Should().Be(28));
    }
}
