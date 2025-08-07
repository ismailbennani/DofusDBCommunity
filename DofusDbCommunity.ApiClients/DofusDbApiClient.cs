namespace DofusDbCommunity.ApiClients;

public static class DofusDbApiClient
{
    public static DofusDbApiClientsFactory Production() => new(new Uri("https://api.dofusdb.fr/"));
    public static DofusDbApiClientsFactory Beta() => new(new Uri("https://api.beta.dofusdb.fr/"));
}
