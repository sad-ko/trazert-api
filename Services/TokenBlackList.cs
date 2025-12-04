using System.Collections.Concurrent;

namespace Trazert_API.Services;

public class TokenBlackList
{
    private readonly ConcurrentDictionary<string, DateTime> _tokens = new();

    public void Add(string token, DateTime expiration)
    {
        _tokens[token] = expiration;
        CleanExpired();
    }

    public bool IsBlacklisted(string token)
    {
        return _tokens.ContainsKey(token) && _tokens[token] > DateTime.UtcNow;
    }

    private void CleanExpired()
    {
        var expired = _tokens.Where(x => x.Value <= DateTime.UtcNow).ToList();
        foreach (var item in expired)
            _tokens.TryRemove(item.Key, out _);
    }
}