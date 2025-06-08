using System.Collections.Concurrent;

namespace TradingSystem;

public abstract class InMemoryRepository<T> : IRepository<T> where T : class
{
    protected readonly ConcurrentDictionary<Guid, T> store = new();
    public void Add(T entity)
    {
        var id = (Guid)typeof(T).GetProperty("Id")!.GetValue(entity)!;
        store[id] = entity;
    }
    public bool TryGet(Guid id, out T entity) => store.TryGetValue(id, out entity!);
    public IEnumerable<T> All() => store.Values;
}