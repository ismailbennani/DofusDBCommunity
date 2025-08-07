using System.Text;
using System.Web;

namespace DofusDbCommunity.ApiClients.Search;

class SearchRequestQueryParamsBuilder
{
    public string BuildQueryParams(SearchQuery query)
    {
        QueryParamsBuilder builder = new();

        if (query.Limit is not null)
        {
            builder.Add("$limit", $"{query.Limit}");
        }

        if (query.Skip is not null)
        {
            builder.Add("$skip", $"{query.Skip}");
        }

        foreach ((string sortParam, SearchQuerySortOrder sortOrder) in query.Sort)
        {
            switch (sortOrder)
            {
                case SearchQuerySortOrder.Ascending:
                    builder.Add($"$sort[{sortParam.ToCamelCase()}]", "1");
                    break;
                case SearchQuerySortOrder.Descending:
                    builder.Add($"$sort[{sortParam.ToCamelCase()}]", "-1");
                    break;
            }
        }

        foreach (string selectParam in query.Select)
        {
            builder.Add("$select[]", selectParam.ToCamelCase());
        }

        foreach (SearchPredicate predicate in query.Predicates)
        {
            AddPredicateParams(builder, predicate, []);
        }

        return builder.Build();
    }

    static void AddPredicateParams(QueryParamsBuilder builder, SearchPredicate queryPredicate, string[] path)
    {
        switch (queryPredicate)
        {
            case SearchPredicate.Eq p:
                builder.Add(FormatNestedFieldName([..path, p.Field, "$eq"]), p.Value);
                break;
            case SearchPredicate.NotEq p:
                builder.Add(FormatNestedFieldName([..path, p.Field, "$neq"]), p.Value);
                break;
            case SearchPredicate.In p:
                foreach (string value in p.Value)
                {
                    builder.Add(FormatNestedFieldName([..path, p.Field, "$in", ""]), value);
                }
                break;
            case SearchPredicate.NotIn p:
                foreach (string value in p.Value)
                {
                    builder.Add(FormatNestedFieldName([..path, p.Field, "$nin", ""]), value);
                }
                break;
            case SearchPredicate.GreaterThan p:
                builder.Add(FormatNestedFieldName([..path, p.Field, "$gt"]), p.Value);
                break;
            case SearchPredicate.GreaterThanOrEqual p:
                builder.Add(FormatNestedFieldName([..path, p.Field, "$gte"]), p.Value);
                break;
            case SearchPredicate.LessThan p:
                builder.Add(FormatNestedFieldName([..path, p.Field, "$lt"]), p.Value);
                break;
            case SearchPredicate.LessThanOrEquals p:
                builder.Add(FormatNestedFieldName([..path, p.Field, "$lte"]), p.Value);
                break;
            case SearchPredicate.And p:
                for (int index = 0; index < p.Predicates.Count; index++)
                {
                    AddPredicateParams(builder, p.Predicates[index], [..path, "$and", $"{index}"]);
                }
                break;
            case SearchPredicate.Or p:
                for (int index = 0; index < p.Predicates.Count; index++)
                {
                    AddPredicateParams(builder, p.Predicates[index], [..path, "$or", $"{index}"]);
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(queryPredicate));
        }
    }

    static string FormatNestedFieldName(params string[] path) =>
        path.Length == 0 ? "" : $"{path[0].ToCamelCase()}{string.Join("", path.Skip(1).Select(p => $"[{p.ToCamelCase()}]"))}";

    class QueryParamsBuilder
    {
        readonly StringBuilder _stringBuilder = new();

        public QueryParamsBuilder Add(string key, string value)
        {
            if (_stringBuilder.Length > 0)
            {
                _stringBuilder.Append('&');
            }

            _stringBuilder.Append($"{key}={HttpUtility.UrlEncode(value)}");
            return this;
        }

        public string Build() => _stringBuilder.ToString();
    }
}
