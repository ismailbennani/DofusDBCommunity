using DofusDbCommunity.ApiClients;
using DofusDbCommunity.ApiClients.Search;
using DofusDbCommunity.Models.Items;
using FluentAssertions;
using Moq;

namespace Tests.UnitTests.ApiClients;

public class DofusDbQueryTest
{
    readonly Mock<IDofusDbApiClient<Item>> _clientMock;
    readonly DofusDbQuery<Item> _builder;

    public DofusDbQueryTest()
    {
        _clientMock = new Mock<IDofusDbApiClient<Item>>();
        _clientMock.Setup(c => c.SearchAsync(It.IsAny<SearchQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new SearchResult<Item>
                {
                    Total = 0, Limit = 0, Skip = 0, Data = []
                }
            );

        _builder = new DofusDbQuery<Item>(_clientMock.Object);
    }

    [Fact]
    public async Task ShouldSetLimitParameter()
    {
        await _builder.Take(123).ExecuteAsync().ToArrayAsync();

        _clientMock.Verify(c => c.SearchAsync(It.IsAny<SearchQuery>(), It.IsAny<CancellationToken>()));

        SearchQuery query = (SearchQuery)_clientMock.Invocations.Single().Arguments[0];
        query.Should().BeEquivalentTo(new SearchQuery { Limit = 123 });
    }

    [Fact]
    public async Task ShouldSetSkipParameter()
    {
        await _builder.Skip(123).ExecuteAsync().ToArrayAsync();

        _clientMock.Verify(c => c.SearchAsync(It.IsAny<SearchQuery>(), It.IsAny<CancellationToken>()));

        SearchQuery query = (SearchQuery)_clientMock.Invocations.Single().Arguments[0];
        query.Should().BeEquivalentTo(new SearchQuery { Skip = 123 });
    }

    [Fact]
    public async Task ShouldSetSortParameter_Ascending()
    {
        await _builder.SortByAscending(i => i.AppearanceId).ExecuteAsync().ToArrayAsync();

        _clientMock.Verify(c => c.SearchAsync(It.IsAny<SearchQuery>(), It.IsAny<CancellationToken>()));

        SearchQuery query = (SearchQuery)_clientMock.Invocations.Single().Arguments[0];
        query.Should().BeEquivalentTo(new SearchQuery { Sort = new Dictionary<string, SearchQuerySortOrder> { { "appearanceId", SearchQuerySortOrder.Ascending } } });
    }

    [Fact]
    public async Task ShouldSetSortParameter_Descending()
    {
        await _builder.SortByDescending(i => i.AppearanceId).ExecuteAsync().ToArrayAsync();

        _clientMock.Verify(c => c.SearchAsync(It.IsAny<SearchQuery>(), It.IsAny<CancellationToken>()));

        SearchQuery query = (SearchQuery)_clientMock.Invocations.Single().Arguments[0];
        query.Should().BeEquivalentTo(new SearchQuery { Sort = new Dictionary<string, SearchQuerySortOrder> { { "appearanceId", SearchQuerySortOrder.Descending } } });
    }

    [Fact]
    public async Task ShouldSetSortParameter_NestedProperty()
    {
        await _builder.SortByDescending(i => i.Name.Fr).ExecuteAsync().ToArrayAsync();

        _clientMock.Verify(c => c.SearchAsync(It.IsAny<SearchQuery>(), It.IsAny<CancellationToken>()));

        SearchQuery query = (SearchQuery)_clientMock.Invocations.Single().Arguments[0];
        query.Should().BeEquivalentTo(new SearchQuery { Sort = new Dictionary<string, SearchQuerySortOrder> { { "name.fr", SearchQuerySortOrder.Descending } } });
    }

    [Fact]
    public async Task ShouldSetSortParameter_ForMultipleFields()
    {
        await _builder.SortByAscending(i => i.Name).SortByDescending(i => i.AppearanceId).ExecuteAsync().ToArrayAsync();

        _clientMock.Verify(c => c.SearchAsync(It.IsAny<SearchQuery>(), It.IsAny<CancellationToken>()));

        SearchQuery query = (SearchQuery)_clientMock.Invocations.Single().Arguments[0];
        query.Should()
            .BeEquivalentTo(
                new SearchQuery
                {
                    Sort = new Dictionary<string, SearchQuerySortOrder> { { "name", SearchQuerySortOrder.Ascending }, { "appearanceId", SearchQuerySortOrder.Descending } }
                }
            );
    }

    [Fact]
    public async Task ShouldSetSelectParameter()
    {
        await _builder.Select(i => i.AppearanceId).ExecuteAsync().ToArrayAsync();

        _clientMock.Verify(c => c.SearchAsync(It.IsAny<SearchQuery>(), It.IsAny<CancellationToken>()));

        SearchQuery query = (SearchQuery)_clientMock.Invocations.Single().Arguments[0];
        query.Should().BeEquivalentTo(new SearchQuery { Select = ["appearanceId"] });
    }

    [Fact]
    public async Task ShouldSetSelectParameter_ForNestedFields()
    {
        await _builder.Select(i => i.Name.Fr).ExecuteAsync().ToArrayAsync();

        _clientMock.Verify(c => c.SearchAsync(It.IsAny<SearchQuery>(), It.IsAny<CancellationToken>()));

        SearchQuery query = (SearchQuery)_clientMock.Invocations.Single().Arguments[0];
        query.Should().BeEquivalentTo(new SearchQuery { Select = ["name.fr"] });
    }

    [Fact]
    public async Task ShouldSetPredicateParameter_Eq()
    {
        await _builder.Where(i => i.AppearanceId == 1).ExecuteAsync().ToArrayAsync();

        _clientMock.Verify(c => c.SearchAsync(It.IsAny<SearchQuery>(), It.IsAny<CancellationToken>()));

        SearchQuery query = (SearchQuery)_clientMock.Invocations.Single().Arguments[0];
        query.Should().BeEquivalentTo(new SearchQuery { Predicates = [new SearchPredicate.Eq("appearanceId", "1")] });
    }

    [Fact]
    public async Task ShouldSetPredicateParameter_Neq()
    {
        await _builder.Where(i => i.AppearanceId != 1).ExecuteAsync().ToArrayAsync();

        _clientMock.Verify(c => c.SearchAsync(It.IsAny<SearchQuery>(), It.IsAny<CancellationToken>()));

        SearchQuery query = (SearchQuery)_clientMock.Invocations.Single().Arguments[0];
        query.Should().BeEquivalentTo(new SearchQuery { Predicates = [new SearchPredicate.NotEq("appearanceId", "1")] });
    }

    [Fact]
    public async Task ShouldSetPredicateParameter_In()
    {
        List<int?> collection = [1, 2];
        await _builder.Where(i => collection.Contains(i.AppearanceId)).ExecuteAsync().ToArrayAsync();

        _clientMock.Verify(c => c.SearchAsync(It.IsAny<SearchQuery>(), It.IsAny<CancellationToken>()));

        SearchQuery query = (SearchQuery)_clientMock.Invocations.Single().Arguments[0];
        query.Should().BeEquivalentTo(new SearchQuery { Predicates = [new SearchPredicate.In("appearanceId", "1", "2")] });
    }

    [Fact]
    public async Task ShouldSetPredicateParameter_Nin()
    {
        List<int?> collection = [1, 2];
        await _builder.Where(i => !collection.Contains(i.AppearanceId)).ExecuteAsync().ToArrayAsync();

        _clientMock.Verify(c => c.SearchAsync(It.IsAny<SearchQuery>(), It.IsAny<CancellationToken>()));

        SearchQuery query = (SearchQuery)_clientMock.Invocations.Single().Arguments[0];
        query.Should().BeEquivalentTo(new SearchQuery { Predicates = [new SearchPredicate.NotIn("appearanceId", "1", "2")] });
    }

    [Fact]
    public async Task ShouldSetPredicateParameter_Gt()
    {
        await _builder.Where(i => i.AppearanceId > 1).ExecuteAsync().ToArrayAsync();

        _clientMock.Verify(c => c.SearchAsync(It.IsAny<SearchQuery>(), It.IsAny<CancellationToken>()));

        SearchQuery query = (SearchQuery)_clientMock.Invocations.Single().Arguments[0];
        query.Should().BeEquivalentTo(new SearchQuery { Predicates = [new SearchPredicate.GreaterThan("appearanceId", "1")] });
    }

    [Fact]
    public async Task ShouldSetPredicateParameter_Gte()
    {
        await _builder.Where(i => i.AppearanceId >= 1).ExecuteAsync().ToArrayAsync();

        _clientMock.Verify(c => c.SearchAsync(It.IsAny<SearchQuery>(), It.IsAny<CancellationToken>()));

        SearchQuery query = (SearchQuery)_clientMock.Invocations.Single().Arguments[0];
        query.Should().BeEquivalentTo(new SearchQuery { Predicates = [new SearchPredicate.GreaterThanOrEqual("appearanceId", "1")] });
    }

    [Fact]
    public async Task ShouldSetPredicateParameter_Lt()
    {
        await _builder.Where(i => i.AppearanceId < 1).ExecuteAsync().ToArrayAsync();

        _clientMock.Verify(c => c.SearchAsync(It.IsAny<SearchQuery>(), It.IsAny<CancellationToken>()));

        SearchQuery query = (SearchQuery)_clientMock.Invocations.Single().Arguments[0];
        query.Should().BeEquivalentTo(new SearchQuery { Predicates = [new SearchPredicate.LessThan("appearanceId", "1")] });
    }

    [Fact]
    public async Task ShouldSetPredicateParameter_Lte()
    {
        await _builder.Where(i => i.AppearanceId <= 1).ExecuteAsync().ToArrayAsync();

        _clientMock.Verify(c => c.SearchAsync(It.IsAny<SearchQuery>(), It.IsAny<CancellationToken>()));

        SearchQuery query = (SearchQuery)_clientMock.Invocations.Single().Arguments[0];
        query.Should().BeEquivalentTo(new SearchQuery { Predicates = [new SearchPredicate.LessThanOrEquals("appearanceId", "1")] });
    }

    [Fact]
    public async Task ShouldSetPredicateParameter_And()
    {
        await _builder.Where(i => i.AppearanceId == 1 && i.AppearanceId != 2).ExecuteAsync().ToArrayAsync();

        _clientMock.Verify(c => c.SearchAsync(It.IsAny<SearchQuery>(), It.IsAny<CancellationToken>()));

        SearchQuery query = (SearchQuery)_clientMock.Invocations.Single().Arguments[0];
        query.Should()
            .BeEquivalentTo(
                new SearchQuery { Predicates = [new SearchPredicate.And(new SearchPredicate.Eq("appearanceId", "1"), new SearchPredicate.NotEq("appearanceId", "2"))] }
            );
    }

    [Fact]
    public async Task ShouldSetPredicateParameter_Or()
    {
        await _builder.Where(i => i.AppearanceId == 1 || i.AppearanceId != 2).ExecuteAsync().ToArrayAsync();

        _clientMock.Verify(c => c.SearchAsync(It.IsAny<SearchQuery>(), It.IsAny<CancellationToken>()));

        SearchQuery query = (SearchQuery)_clientMock.Invocations.Single().Arguments[0];
        query.Should()
            .BeEquivalentTo(new SearchQuery { Predicates = [new SearchPredicate.Or(new SearchPredicate.Eq("appearanceId", "1"), new SearchPredicate.NotEq("appearanceId", "2"))] });
    }

    [Fact]
    public async Task ShouldSetPredicateParameter_Not_Eq()
    {
        await _builder.Where(i => !(i.AppearanceId == 1)).ExecuteAsync().ToArrayAsync();

        _clientMock.Verify(c => c.SearchAsync(It.IsAny<SearchQuery>(), It.IsAny<CancellationToken>()));

        SearchQuery query = (SearchQuery)_clientMock.Invocations.Single().Arguments[0];
        query.Should().BeEquivalentTo(new SearchQuery { Predicates = [new SearchPredicate.NotEq("appearanceId", "1")] });
    }

    [Fact]
    public async Task ShouldSetPredicateParameter_Not_Neq()
    {
        await _builder.Where(i => !(i.AppearanceId != 1)).ExecuteAsync().ToArrayAsync();

        _clientMock.Verify(c => c.SearchAsync(It.IsAny<SearchQuery>(), It.IsAny<CancellationToken>()));

        SearchQuery query = (SearchQuery)_clientMock.Invocations.Single().Arguments[0];
        query.Should().BeEquivalentTo(new SearchQuery { Predicates = [new SearchPredicate.Eq("appearanceId", "1")] });
    }

    [Fact]
    public async Task ShouldSetPredicateParameter_Not_In()
    {
        List<int?> collection = [1, 2];
        await _builder.Where(i => !collection.Contains(i.AppearanceId)).ExecuteAsync().ToArrayAsync();

        _clientMock.Verify(c => c.SearchAsync(It.IsAny<SearchQuery>(), It.IsAny<CancellationToken>()));

        SearchQuery query = (SearchQuery)_clientMock.Invocations.Single().Arguments[0];
        query.Should().BeEquivalentTo(new SearchQuery { Predicates = [new SearchPredicate.NotIn("appearanceId", "1", "2")] });
    }

    [Fact]
    public async Task ShouldSetPredicateParameter_Not_Nin()
    {
        List<int?> collection = [1, 2];
        await _builder.Where(i => !!collection.Contains(i.AppearanceId)).ExecuteAsync().ToArrayAsync();

        _clientMock.Verify(c => c.SearchAsync(It.IsAny<SearchQuery>(), It.IsAny<CancellationToken>()));

        SearchQuery query = (SearchQuery)_clientMock.Invocations.Single().Arguments[0];
        query.Should().BeEquivalentTo(new SearchQuery { Predicates = [new SearchPredicate.In("appearanceId", "1", "2")] });
    }

    [Fact]
    public async Task ShouldSetPredicateParameter_Not_Gt()
    {
        await _builder.Where(i => !(i.AppearanceId > 1)).ExecuteAsync().ToArrayAsync();

        _clientMock.Verify(c => c.SearchAsync(It.IsAny<SearchQuery>(), It.IsAny<CancellationToken>()));

        SearchQuery query = (SearchQuery)_clientMock.Invocations.Single().Arguments[0];
        query.Should().BeEquivalentTo(new SearchQuery { Predicates = [new SearchPredicate.LessThanOrEquals("appearanceId", "1")] });
    }

    [Fact]
    public async Task ShouldSetPredicateParameter_Not_Gte()
    {
        await _builder.Where(i => i.AppearanceId >= 1).ExecuteAsync().ToArrayAsync();

        _clientMock.Verify(c => c.SearchAsync(It.IsAny<SearchQuery>(), It.IsAny<CancellationToken>()));

        SearchQuery query = (SearchQuery)_clientMock.Invocations.Single().Arguments[0];
        query.Should().BeEquivalentTo(new SearchQuery { Predicates = [new SearchPredicate.LessThan("appearanceId", "1")] });
    }

    [Fact]
    public async Task ShouldSetPredicateParameter_Not_Lt()
    {
        await _builder.Where(i => i.AppearanceId < 1).ExecuteAsync().ToArrayAsync();

        _clientMock.Verify(c => c.SearchAsync(It.IsAny<SearchQuery>(), It.IsAny<CancellationToken>()));

        SearchQuery query = (SearchQuery)_clientMock.Invocations.Single().Arguments[0];
        query.Should().BeEquivalentTo(new SearchQuery { Predicates = [new SearchPredicate.GreaterThanOrEqual("appearanceId", "1")] });
    }

    [Fact]
    public async Task ShouldSetPredicateParameter_Not_Lte()
    {
        await _builder.Where(i => i.AppearanceId <= 1).ExecuteAsync().ToArrayAsync();

        _clientMock.Verify(c => c.SearchAsync(It.IsAny<SearchQuery>(), It.IsAny<CancellationToken>()));

        SearchQuery query = (SearchQuery)_clientMock.Invocations.Single().Arguments[0];
        query.Should().BeEquivalentTo(new SearchQuery { Predicates = [new SearchPredicate.GreaterThan("appearanceId", "1")] });
    }

    [Fact]
    public async Task ShouldSetPredicateParameter_Not_And()
    {
        await _builder.Where(i => !(i.AppearanceId == 1 && i.AppearanceId != 2)).ExecuteAsync().ToArrayAsync();

        _clientMock.Verify(c => c.SearchAsync(It.IsAny<SearchQuery>(), It.IsAny<CancellationToken>()));

        SearchQuery query = (SearchQuery)_clientMock.Invocations.Single().Arguments[0];
        query.Should()
            .BeEquivalentTo(new SearchQuery { Predicates = [new SearchPredicate.Or(new SearchPredicate.NotEq("appearanceId", "1"), new SearchPredicate.Eq("appearanceId", "2"))] });
    }

    [Fact]
    public async Task ShouldSetPredicateParameter_Not_Or()
    {
        await _builder.Where(i => !(i.AppearanceId == 1 || i.AppearanceId != 2)).ExecuteAsync().ToArrayAsync();

        _clientMock.Verify(c => c.SearchAsync(It.IsAny<SearchQuery>(), It.IsAny<CancellationToken>()));

        SearchQuery query = (SearchQuery)_clientMock.Invocations.Single().Arguments[0];
        query.Should()
            .BeEquivalentTo(
                new SearchQuery { Predicates = [new SearchPredicate.And(new SearchPredicate.NotEq("appearanceId", "1"), new SearchPredicate.Eq("appearanceId", "2"))] }
            );
    }

    [Fact]
    public async Task ShouldSetPredicateParameter_MultiplePredicates()
    {
        await _builder.Where(i => i.AppearanceId == 1).Where(i => i.AppearanceId == 2).ExecuteAsync().ToArrayAsync();

        _clientMock.Verify(c => c.SearchAsync(It.IsAny<SearchQuery>(), It.IsAny<CancellationToken>()));

        SearchQuery query = (SearchQuery)_clientMock.Invocations.Single().Arguments[0];
        query.Should().BeEquivalentTo(new SearchQuery { Predicates = [new SearchPredicate.Eq("appearanceId", "1"), new SearchPredicate.Eq("appearanceId", "2")] });
    }

    [Fact]
    public async Task ShouldSetPredicateParameter_Complex()
    {
        List<int?> firstContainer = [1, 2];
        List<string?> secondContainer = ["value1", "value2"];
        List<string?> thirdContainer = ["value3", "value4"];

        await _builder.Where(i => firstContainer.Contains(i.AppearanceId) || i.BonusIsSecret == true && i.Level > 50 && !secondContainer.Contains(i.Name.Fr))
            .Where(i => thirdContainer.Contains(i.Criteria))
            .ExecuteAsync()
            .ToArrayAsync();

        _clientMock.Verify(c => c.SearchAsync(It.IsAny<SearchQuery>(), It.IsAny<CancellationToken>()));

        SearchQuery query = (SearchQuery)_clientMock.Invocations.Single().Arguments[0];
        query.Should()
            .BeEquivalentTo(
                new SearchQuery
                {
                    Predicates =
                    [
                        new SearchPredicate.Or(
                            new SearchPredicate.In("appearanceId", "1", "2"),
                            new SearchPredicate.And(
                                new SearchPredicate.Eq("bonusIsSecret", "true"),
                                new SearchPredicate.GreaterThan("level", "50"),
                                new SearchPredicate.NotIn("name.fr", "value1", "value2")
                            )
                        ),
                        new SearchPredicate.In("criteria", "value3", "value4")
                    ]
                }
            );
    }
}
