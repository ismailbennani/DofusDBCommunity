namespace DofusDbCommunity.ApiClients;

public static class DofusDbQuery
{
    public static DofusDbQueryProvider Production() => new(DofusDbApiClient.Production());
    public static DofusDbQueryProvider Beta() => new(DofusDbApiClient.Beta());
}
