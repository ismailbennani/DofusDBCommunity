using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using DofusDbCommunity.ApiClients.Search;
using DofusDbCommunity.Models;

namespace DofusDbCommunity.ApiClients;

public class DofusDbQuery<TResource>(IDofusDbApiClient<TResource> client) where TResource: DofusDbEntity
{
    int? _limit;
    int? _skip;
    readonly Dictionary<string, SearchQuerySortOrder> _sort = [];
    readonly List<string> _select = [];
    readonly List<SearchPredicate> _predicates = [];

    /// <summary>
    ///     Sets the maximum number of items to return in the result set.
    /// </summary>
    /// <param name="count">The maximum number of items to return.</param>
    /// <returns>The current builder, for chaining.</returns>
    public DofusDbQuery<TResource> Take(int count)
    {
        _limit = count;
        return this;
    }

    /// <summary>
    ///     Sets the number of items to skip before starting to collect the result set.
    /// </summary>
    /// <param name="count">The number of items to skip.</param>
    /// <returns>The current builder, for chaining.</returns>
    public DofusDbQuery<TResource> Skip(int count)
    {
        _skip = count;
        return this;
    }

    /// <summary>
    ///     Sort the data by the specified property in ascending order.
    /// </summary>
    /// <param name="expression">The expression representing the property to sort by.</param>
    /// <returns>The current builder, for chaining.</returns>
    public DofusDbQuery<TResource> SortByAscending(Expression<Func<TResource, object?>> expression)
    {
        string propertyName = ExtractPropertyName(expression);
        _sort[propertyName] = SearchQuerySortOrder.Ascending;
        return this;
    }

    /// <summary>
    ///     Sort the data by the specified property in descending order.
    /// </summary>
    /// <param name="expression">The expression representing the property to sort by.</param>
    /// <returns>The current builder, for chaining.</returns>
    public DofusDbQuery<TResource> SortByDescending(Expression<Func<TResource, object?>> expression)
    {
        string propertyName = ExtractPropertyName(expression);
        _sort[propertyName] = SearchQuerySortOrder.Descending;
        return this;
    }

    /// <summary>
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public DofusDbQuery<TResource> Select(Expression<Func<TResource, object?>> expression)
    {
        string propertyName = ExtractPropertyName(expression);
        _select.Add(propertyName);
        return this;
    }

    /// <summary>
    ///     Adds a predicate to the search query, allowing for complex filtering of results.
    /// </summary>
    /// <param name="expression">The expression representing the predicate to apply.</param>
    /// <returns>The current builder, for chaining.</returns>
    public DofusDbQuery<TResource> Where(Expression<Func<TResource, bool>> expression)
    {
        SearchPredicate predicate = ExtractPredicate(expression);
        _predicates.Add(predicate);
        return this;
    }

    /// <summary>
    ///     Executes the search query and returns an asynchronous enumerable of resources matching the query.
    ///     This method will perform as many requests as necessary to retrieve the requested number of results.
    /// </summary>
    /// <returns>The search result containing all resources matching the query.</returns>
    public IAsyncEnumerable<TResource> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        SearchQuery query = BuildQuery();
        return client.MultiQuerySearchAsync(query, cancellationToken);
    }

    SearchQuery BuildQuery() =>
        new()
        {
            Limit = _limit,
            Skip = _skip,
            Sort = _sort,
            Select = _select,
            Predicates = _predicates
        };

    static string ExtractPropertyName(Expression<Func<TResource, object?>> expression) => ExtractPropertyChain(expression.Parameters.Single(), expression.Body);

    static string ExtractPropertyChain(ParameterExpression root, Expression expression)
    {
        string[] chain = ExtractPropertyChainRecursive(root, expression, []);
        return string.Join('.', chain.Select(p => p.ToCamelCase()));

        static string[] ExtractPropertyChainRecursive(ParameterExpression root, Expression expression, string[] path)
        {
            if (expression == root)
            {
                return path;
            }

            switch (expression)
            {
                case UnaryExpression { NodeType: ExpressionType.Convert } unaryExpression:
                    return ExtractPropertyChainRecursive(root, unaryExpression.Operand, path);
                case MemberExpression memberExpression:
                    string[] newPath = [memberExpression.Member.Name, ..path];
                    return memberExpression.Expression == null ? newPath : ExtractPropertyChainRecursive(root, memberExpression.Expression, newPath);
            }

            throw new ArgumentException("Expression must be a property chain.", nameof(expression));
        }
    }

    static SearchPredicate ExtractPredicate(Expression<Func<TResource, bool>> expression) => ExtractPredicate(expression.Parameters.Single(), expression.Body);

    static SearchPredicate ExtractPredicate(ParameterExpression root, Expression expression)
    {
        switch (expression)
        {
            case BinaryExpression { NodeType: ExpressionType.Equal } e:
            {
                string left = ExtractPropertyChain(root, e.Left);
                string right = ExtractValueAsString(e.Right);
                return new SearchPredicate.Eq(left, right);
            }
            case BinaryExpression { NodeType: ExpressionType.NotEqual } e:
            {
                string left = ExtractPropertyChain(root, e.Left);
                string right = ExtractValueAsString(e.Right);
                return new SearchPredicate.NotEq(left, right);
            }
            case MethodCallExpression { Method.Name: "Contains" } e:
            {
                if (e.Object is null)
                {
                    throw new ArgumentException("The 'Contains' method must be called on a collection.", nameof(expression));
                }

                string left = ExtractPropertyChain(root, e.Arguments.Single());
                string[] right = ExtractCollectionValuesAsString(e.Object);
                return new SearchPredicate.In(left, right);
            }
            case UnaryExpression { NodeType: ExpressionType.Not } e:
            {
                SearchPredicate predicate = ExtractPredicate(root, e.Operand);
                return NegatePredicate(predicate);
            }
            case BinaryExpression { NodeType: ExpressionType.GreaterThan } e:
            {
                string left = ExtractPropertyChain(root, e.Left);
                string right = ExtractValueAsString(e.Right);
                return new SearchPredicate.GreaterThan(left, right);
            }
            case BinaryExpression { NodeType: ExpressionType.GreaterThanOrEqual } e:
            {
                string left = ExtractPropertyChain(root, e.Left);
                string right = ExtractValueAsString(e.Right);
                return new SearchPredicate.GreaterThanOrEqual(left, right);
            }
            case BinaryExpression { NodeType: ExpressionType.LessThan } e:
            {
                string left = ExtractPropertyChain(root, e.Left);
                string right = ExtractValueAsString(e.Right);
                return new SearchPredicate.LessThan(left, right);
            }
            case BinaryExpression { NodeType: ExpressionType.LessThanOrEqual } e:
            {
                string left = ExtractPropertyChain(root, e.Left);
                string right = ExtractValueAsString(e.Right);
                return new SearchPredicate.LessThanOrEquals(left, right);
            }
            case BinaryExpression { NodeType: ExpressionType.AndAlso } e:
            {
                SearchPredicate left = ExtractPredicate(root, e.Left);
                SearchPredicate right = ExtractPredicate(root, e.Right);
                return new SearchPredicate.And(left, right);
            }
            case BinaryExpression { NodeType: ExpressionType.OrElse } e:
            {
                SearchPredicate left = ExtractPredicate(root, e.Left);
                SearchPredicate right = ExtractPredicate(root, e.Right);
                return new SearchPredicate.Or(left, right);
            }
        }

        throw new ArgumentException($"Could not extract predicate from expression {expression}.", nameof(expression));
    }

    static SearchPredicate NegatePredicate(SearchPredicate predicate) =>
        predicate switch
        {
            SearchPredicate.Eq p => new SearchPredicate.NotEq(p.Field, p.Value),
            SearchPredicate.NotEq p => new SearchPredicate.Eq(p.Field, p.Value),
            SearchPredicate.In p => new SearchPredicate.NotIn(p.Field, p.Value),
            SearchPredicate.NotIn p => new SearchPredicate.In(p.Field, p.Value),
            SearchPredicate.GreaterThan p => new SearchPredicate.LessThanOrEquals(p.Field, p.Value),
            SearchPredicate.GreaterThanOrEqual p => new SearchPredicate.LessThan(p.Field, p.Value),
            SearchPredicate.LessThan p => new SearchPredicate.GreaterThanOrEqual(p.Field, p.Value),
            SearchPredicate.LessThanOrEquals p => new SearchPredicate.GreaterThan(p.Field, p.Value),
            SearchPredicate.And p => new SearchPredicate.Or(p.Predicates.Select(NegatePredicate).ToArray()),
            SearchPredicate.Or p => new SearchPredicate.And(p.Predicates.Select(NegatePredicate).ToArray()),
            _ => throw new ArgumentOutOfRangeException(nameof(predicate))
        };

    static string ExtractValueAsString(Expression expression) =>
        expression switch
        {
            ConstantExpression constantExpression => constantExpression.Value?.ToString() ?? "null",
            UnaryExpression { NodeType: ExpressionType.Convert } unaryExpression => ExtractValueAsString(unaryExpression.Operand),
            _ => throw new ArgumentException($"Could not evaluate expression {expression}.", nameof(expression))
        };

    static string[] ExtractCollectionValuesAsString(Expression expression)
    {
        switch (expression)
        {
            case ConstantExpression constantExpression:
                if (constantExpression.Value is IEnumerable enumerable)
                {
                    return enumerable.Cast<object?>().Select(i => i?.ToString() ?? "null").ToArray();
                }

                if (constantExpression.Type.Name.StartsWith("<>"))
                {
                    // type is generated by the compiler, e.g. <>c__DisplayClass0_0
                    foreach (FieldInfo field in constantExpression.Type.GetFields())
                    {
                        if (field.FieldType.IsAssignableTo(typeof(IEnumerable)))
                        {
                            IEnumerable? value = field.GetValue(constantExpression.Value) as IEnumerable;
                            return value?.Cast<object?>().Select(i => i?.ToString() ?? "null").ToArray() ?? [];
                        }
                    }
                }

                throw new ArgumentException($"Expected a collection, but got {constantExpression.Value?.GetType().Name ?? "null"}.", nameof(expression));
            case MemberExpression memberExpression:
                if (memberExpression.Expression is not null)
                {
                    return ExtractCollectionValuesAsString(memberExpression.Expression);
                }
                break;
        }

        throw new ArgumentException($"Could not evaluate collection {expression}.", nameof(expression));
    }
}
