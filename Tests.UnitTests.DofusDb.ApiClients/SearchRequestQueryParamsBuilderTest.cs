using DofusSharp.DofusDb.ApiClients.Search;
using FluentAssertions;

namespace Tests.UnitTests.DofusDb.ApiClients;

public class SearchRequestQueryParamsBuilderTest
{
    readonly SearchRequestQueryParamsBuilder _builder = new();

    [Fact]
    public void ShouldSetLimitParameter()
    {
        SearchQuery query = new() { Limit = 123 };
        string queryParams = _builder.BuildQueryParams(query);
        queryParams.Should().Be("$limit=123");
    }

    [Fact]
    public void ShouldSetSkipParameter()
    {
        SearchQuery query = new() { Skip = 123 };
        string queryParams = _builder.BuildQueryParams(query);
        queryParams.Should().Be("$skip=123");
    }

    [Theory]
    [InlineData(SearchQuerySortOrder.Ascending, 1)]
    [InlineData(SearchQuerySortOrder.Descending, -1)]
    public void ShouldSetSortParameter(SearchQuerySortOrder sortOrder, int expectedValue)
    {
        SearchQuery query = new() { Sort = new Dictionary<string, SearchQuerySortOrder> { { "parameter", sortOrder } } };
        string queryParams = _builder.BuildQueryParams(query);
        queryParams.Should().Be($"$sort[parameter]={expectedValue}");
    }

    [Fact]
    public void ShouldSkipSortParameter_WhenOrderIsNone()
    {
        SearchQuery query = new() { Sort = new Dictionary<string, SearchQuerySortOrder> { { "parameter", SearchQuerySortOrder.None } } };
        string queryParams = _builder.BuildQueryParams(query);
        queryParams.Should().BeEmpty();
    }

    [Fact]
    public void ShouldSetSortParameter_ForMultipleFields()
    {
        SearchQuery query = new()
            { Sort = new Dictionary<string, SearchQuerySortOrder> { { "parameter1", SearchQuerySortOrder.Ascending }, { "parameter2", SearchQuerySortOrder.Descending } } };
        string queryParams = _builder.BuildQueryParams(query);
        queryParams.Should().Be("$sort[parameter1]=1&$sort[parameter2]=-1");
    }

    [Fact]
    public void ShouldSetSelectParameter()
    {
        SearchQuery query = new() { Select = ["parameter"] };
        string queryParams = _builder.BuildQueryParams(query);
        queryParams.Should().Be("$select[]=parameter");
    }

    [Fact]
    public void ShouldSetSelectParameter_ForMultipleFields()
    {
        SearchQuery query = new() { Select = ["parameter1", "parameter2"] };
        string queryParams = _builder.BuildQueryParams(query);
        queryParams.Should().Be("$select[]=parameter1&$select[]=parameter2");
    }

    [Fact]
    public void ShouldSetPredicateParameter_Eq()
    {
        SearchQuery query = new() { Predicates = [new SearchPredicate.Eq("parameter", "value")] };
        string queryParams = _builder.BuildQueryParams(query);
        queryParams.Should().Be("parameter[$eq]=value");
    }

    [Fact]
    public void ShouldSetPredicateParameter_Neq()
    {
        SearchQuery query = new() { Predicates = [new SearchPredicate.NotEq("parameter", "value")] };
        string queryParams = _builder.BuildQueryParams(query);
        queryParams.Should().Be("parameter[$neq]=value");
    }

    [Fact]
    public void ShouldSetPredicateParameter_In()
    {
        SearchQuery query = new() { Predicates = [new SearchPredicate.In("parameter", "value1", "value2")] };
        string queryParams = _builder.BuildQueryParams(query);
        queryParams.Should().Be("parameter[$in][]=value1&parameter[$in][]=value2");
    }

    [Fact]
    public void ShouldSetPredicateParameter_Nin()
    {
        SearchQuery query = new() { Predicates = [new SearchPredicate.NotIn("parameter", "value1", "value2")] };
        string queryParams = _builder.BuildQueryParams(query);
        queryParams.Should().Be("parameter[$nin][]=value1&parameter[$nin][]=value2");
    }

    [Fact]
    public void ShouldSetPredicateParameter_Gt()
    {
        SearchQuery query = new() { Predicates = [new SearchPredicate.GreaterThan("parameter", "value")] };
        string queryParams = _builder.BuildQueryParams(query);
        queryParams.Should().Be("parameter[$gt]=value");
    }

    [Fact]
    public void ShouldSetPredicateParameter_Gte()
    {
        SearchQuery query = new() { Predicates = [new SearchPredicate.GreaterThanOrEqual("parameter", "value")] };
        string queryParams = _builder.BuildQueryParams(query);
        queryParams.Should().Be("parameter[$gte]=value");
    }

    [Fact]
    public void ShouldSetPredicateParameter_Lt()
    {
        SearchQuery query = new() { Predicates = [new SearchPredicate.LessThan("parameter", "value")] };
        string queryParams = _builder.BuildQueryParams(query);
        queryParams.Should().Be("parameter[$lt]=value");
    }

    [Fact]
    public void ShouldSetPredicateParameter_Lte()
    {
        SearchQuery query = new() { Predicates = [new SearchPredicate.LessThanOrEquals("parameter", "value")] };
        string queryParams = _builder.BuildQueryParams(query);
        queryParams.Should().Be("parameter[$lte]=value");
    }

    [Fact]
    public void ShouldSetPredicateParameter_And()
    {
        SearchQuery query = new() { Predicates = [new SearchPredicate.And(new SearchPredicate.Eq("parameter1", "value1"), new SearchPredicate.NotEq("parameter2", "value2"))] };
        string queryParams = _builder.BuildQueryParams(query);
        queryParams.Should().Be("$and[0][parameter1][$eq]=value1&$and[1][parameter2][$neq]=value2");
    }

    [Fact]
    public void ShouldSetPredicateParameter_Or()
    {
        SearchQuery query = new() { Predicates = [new SearchPredicate.Or(new SearchPredicate.Eq("parameter1", "value1"), new SearchPredicate.NotEq("parameter2", "value2"))] };
        string queryParams = _builder.BuildQueryParams(query);
        queryParams.Should().Be("$or[0][parameter1][$eq]=value1&$or[1][parameter2][$neq]=value2");
    }

    [Fact]
    public void ShouldSetPredicateParameter_Complex()
    {
        SearchQuery query = new()
        {
            Predicates =
            [
                new SearchPredicate.Or(
                    new SearchPredicate.In("parameter1", "value11", "value12"),
                    new SearchPredicate.And(
                        new SearchPredicate.Eq("parameter2", "value2"),
                        new SearchPredicate.GreaterThan("parameter3", "value3"),
                        new SearchPredicate.NotIn("parameter4", "value41", "value42")
                    )
                ),
                new SearchPredicate.In("parameter5", "value51", "value52")
            ]
        };
        string queryParams = _builder.BuildQueryParams(query);
        queryParams.Should()
            .Be(
                "$or[0][parameter1][$in][]=value11&"
                + "$or[0][parameter1][$in][]=value12&"
                + "$or[1][$and][0][parameter2][$eq]=value2&"
                + "$or[1][$and][1][parameter3][$gt]=value3&"
                + "$or[1][$and][2][parameter4][$nin][]=value41&"
                + "$or[1][$and][2][parameter4][$nin][]=value42&"
                + "parameter5[$in][]=value51&"
                + "parameter5[$in][]=value52"
            );
    }
}
