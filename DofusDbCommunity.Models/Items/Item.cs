using System.Text.Json.Serialization;
using DofusDbCommunity.Models.Common;

namespace DofusDbCommunity.Models.Items;

/// <summary>
///     An item in the game.
/// </summary>
[JsonDerivedType(typeof(Weapon))]
public class Item : DofusDbEntity
{
    /// <summary>
    ///     The unique identifier of the type of the item.
    /// </summary>
    public int? TypeId { get; init; }

    /// <summary>
    ///     The type of the item.
    /// </summary>
    public ItemType? Type { get; init; }

    /// <summary>
    ///     The name of the item.
    /// </summary>
    public MultiLangString? Name { get; init; }

    /// <summary>
    ///     The name of the item as lower-case and without diacritics.
    /// </summary>
    public MultiLangString? Slug { get; init; }

    /// <summary>
    ///     The description of the item.
    /// </summary>
    public MultiLangString? Description { get; init; }

    /// <summary>
    ///     The description of the item.
    /// </summary>
    public MultiLangString? ImportantNotice { get; init; }

    /// <summary>
    ///     The level of the item.
    /// </summary>
    public int? Level { get; init; }

    /// <summary>
    ///     The weight of the item, in pods.
    /// </summary>
    public int? RealWeight { get; init; }

    /// <summary>
    ///     The base price of the item, in kamas.
    /// </summary>
    public int? Price { get; init; }

    /// <summary>
    ///     The condition that must be met by a player to use the item.
    /// </summary>
    public string? Criteria { get; init; }

    /// <summary>
    ///     The creation date of the item in the database.
    /// </summary>
    public DateTimeOffset? CreatedAt { get; init; }

    /// <summary>
    ///     The last update date of the item in the database.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; init; }

    /// <summary>
    ///     The URL of the icon of the item.
    /// </summary>
    public string? Img { get; init; }

    #region Item set

    /// <summary>
    ///     The unique identifier of the item set that the item belongs to.
    ///     If the item does not belong to any set, this will be -1.
    /// </summary>
    public int? ItemSetId { get; init; }

    /// <summary>
    ///     Whether the item belongs to an item set.
    /// </summary>
    public ValueOrFalse<ItemSetMinimal>? ItemSet { get; init; }

    #endregion

    #region Effects

    /// <summary>
    ///     Whether the effects of the item should be hidden.
    /// </summary>
    public bool? HideEffects { get; init; }

    /// <summary>
    ///     The characteristics of the item.
    /// </summary>
    public IReadOnlyCollection<ItemEffect>? Effects { get; init; }

    /// <summary>
    ///     The unique identifiers of the evolutive effects of the item.
    /// </summary>
    public IReadOnlyCollection<int>? EvolutiveEffectIds { get; init; }

    /// <summary>
    ///     The unique identifiers of the sub-areas where the item has increased effects.
    /// </summary>
    public IReadOnlyCollection<int>? FavoriteSubAreas { get; init; }

    /// <summary>
    ///     The bonus to the effects of the item when used in a favorite sub-area.
    /// </summary>
    public int? FavoriteSubAreasBonus { get; init; }

    #endregion

    #region Appearance

    /// <summary>
    ///     The unique identifier of the icon of the item.
    /// </summary>
    public int? IconId { get; init; }

    /// <summary>
    ///     The unique identifier of the appearance of the item.
    /// </summary>
    public int? AppearanceId { get; init; }

    /// <summary>
    ///     Whether the colors of the item change to match the colors of the player when equipped.
    /// </summary>
    public bool? IsColorable { get; init; }

    /// <summary>
    ///     ???
    /// </summary>
    public bool? HasLivingObjectSkinJntMood { get; init; }

    #endregion

    #region Craft

    /// <summary>
    ///     Whether the item can be crafted.
    /// </summary>
    public bool? HasRecipe { get; init; }

    /// <summary>
    ///     The number of recipe slots that the item has.
    /// </summary>
    public int? RecipeSlots { get; init; }

    /// <summary>
    ///     The unique identifiers of the recipes that can be used to craft the item.
    /// </summary>
    public IReadOnlyCollection<int>? RecipeIds { get; init; }

    /// <summary>
    ///     The unique identifiers of the recipes that use the item as a resource.
    /// </summary>
    public IReadOnlyCollection<int>? RecipesThatUse { get; init; }

    /// <summary>
    ///     The ratio to apply to the experience gained from crafting the item.
    ///     If the ratio should be the default one, this will be -1.
    /// </summary>
    public double? CraftXpRatio { get; init; }

    /// <summary>
    ///     The condition that must be met by a player to see the item in the crafting interface.
    /// </summary>
    public string? CraftVisible { get; init; }

    /// <summary>
    ///     The condition that must be met by a player to be able to craft the item.
    /// </summary>
    public string? CraftConditional { get; init; }

    /// <summary>
    ///     The condition that must be met by a player to be able to craft the item.
    /// </summary>
    public string? CraftFeasible { get; init; }

    /// <summary>
    ///     Whether the recipe for the item is a secret recipe.
    /// </summary>
    public bool? SecretRecipe { get; init; }

    /// <summary>
    ///     Whether the bonus of the item is a secret bonus.
    /// </summary>
    public bool? BonusIsSecret { get; init; }

    #endregion

    #region Consumables

    /// <summary>
    ///     Whether the item is usable outside of combat.
    /// </summary>
    public bool? Usable { get; init; }

    /// <summary>
    ///     Whether the item is targetable on other players or entities.
    /// </summary>
    public bool? Targetable { get; init; }

    /// <summary>
    ///     The condition that must be met by a player to be the target of the item.
    /// </summary>
    public string? CriteriaTarget { get; init; }

    /// <summary>
    ///     Whether the item should not be usable on another player or entity.
    /// </summary>
    public bool? NonUsableOnAnother { get; init; }

    /// <summary>
    ///     Whether the use of the item requires confirmation from the player.
    /// </summary>
    public bool? NeedUseConfirm { get; init; }

    /// <summary>
    ///     The unique identifier of the animation that should be played when a player uses the item.
    /// </summary>
    public int? UseAnimationId { get; init; }

    #endregion

    #region Harvestables

    /// <summary>
    ///     The number of instances of the resources that yields the item, per sub-area.
    /// </summary>
    public IReadOnlyCollection<(int SubArea, int ResourceCount)>? ResourcesBySubarea { get; init; }

    #endregion

    #region Drop

    /// <summary>
    ///     The unique identifiers of the monsters that can drop the item.
    /// </summary>
    public IReadOnlyCollection<int>? DropMonsterIds { get; init; }

    /// <summary>
    ///     The unique identifiers of the monsters that can drop the item, when the server is in Temporis mode.
    /// </summary>
    public IReadOnlyCollection<int>? DropTemporisMonsterIds { get; init; }

    /// <summary>
    ///     The unique identifiers of the sub-areas where the item can be dropped.
    /// </summary>
    public IReadOnlyCollection<int>? DropSubAreaIds { get; init; }

    #endregion

    #region Nuggets

    /// <summary>
    ///     The number of nuggets that can be obtained by recycling the item.
    /// </summary>
    public double? RecyclingNuggets { get; init; }

    /// <summary>
    ///     The unique identifiers of the sub-areas where the recycling of the item yields more nuggets.
    /// </summary>
    public IReadOnlyCollection<int>? FavoriteRecyclingSubareas { get; init; }

    #endregion

    #region Quests

    /// <summary>
    ///     The unique identifiers of the quests that require the item.
    /// </summary>
    public IReadOnlyCollection<int>? QuestsThatUse { get; init; }

    /// <summary>
    ///     The unique identifiers of the quests that reward the item.
    /// </summary>
    public IReadOnlyCollection<int>? QuestsThatReward { get; init; }

    #endregion

    #region Treasure hunts

    /// <summary>
    ///     The unique identifier of the treasure hunt that can be started with the item.
    ///     If no treasure hunt can be started with the item, this will be 0.
    /// </summary>
    public int? StartLegendaryTreasureHunt { get; init; }

    /// <summary>
    ///     The unique identifier of the treasure hunt that rewards the item.
    ///     If no treasure hunt can reward the item, this will be 0.
    /// </summary>
    public int? LegendaryTreasureHuntThatReward { get; init; }

    #endregion

    #region Other flags

    /// <summary>
    ///     The flags of the item, represented as a bitmask.
    /// </summary>
    [JsonPropertyName("m_flags")]
    public long? Flags { get; init; }

    /// <summary>
    ///     Whether the item is cursed.
    /// </summary>
    public bool? Cursed { get; init; }

    /// <summary>
    ///     Whether the item is exchangeable between players.
    /// </summary>
    public bool? Exchangeable { get; init; }

    /// <summary>
    ///     Whether the item is etheral.
    /// </summary>
    public bool? Etheral { get; init; }

    /// <summary>
    ///     Whether the item is enhanceable.
    /// </summary>
    public bool? Enhanceable { get; init; }

    /// <summary>
    ///     Whether the item is displayed on the website's encyclopedia.
    /// </summary>
    public bool? ObjectIsDisplayOnWeb { get; init; }

    /// <summary>
    ///     Whether the item can be destroyed.
    /// </summary>
    public bool? IsDestructible { get; init; }

    /// <summary>
    ///     Whether the item can be sold.
    /// </summary>
    public bool? IsSaleable { get; init; }

    /// <summary>
    ///     Whether the item is legendary.
    /// </summary>
    public bool? IsLegendary { get; init; }

    #endregion

    #region Unknown

    /// <summary>
    ///     ???
    /// </summary>
    public string? Visibility { get; init; }

    /// <summary>
    ///     ???
    /// </summary>
    public string? ChangeVersion { get; init; }

    /// <summary>
    ///     ???
    /// </summary>
    public long? TooltipExpirationDate { get; init; }

    #endregion
}
