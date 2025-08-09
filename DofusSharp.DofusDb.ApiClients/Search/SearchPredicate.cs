namespace DofusSharp.DofusDb.ApiClients.Search;

public abstract record SearchPredicate
{
    public record Eq(string Field, string Value) : SearchPredicate;

    public record NotEq(string Field, string Value) : SearchPredicate;

    public record In(string Field, params IReadOnlyCollection<string> Value) : SearchPredicate;

    public record NotIn(string Field, params IReadOnlyCollection<string> Value) : SearchPredicate;

    public record LessThan(string Field, string Value) : SearchPredicate;

    public record LessThanOrEquals(string Field, string Value) : SearchPredicate;

    public record GreaterThan(string Field, string Value) : SearchPredicate;

    public record GreaterThanOrEqual(string Field, string Value) : SearchPredicate;

    public record And(params IReadOnlyList<SearchPredicate> Predicates) : SearchPredicate;

    public record Or(params IReadOnlyList<SearchPredicate> Predicates) : SearchPredicate;
}
