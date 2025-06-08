namespace TradingSystem;

public interface IRepository<T>
{
    void Add(T entity);
    bool TryGet(Guid id, out T entity);
    IEnumerable<T> All();
}