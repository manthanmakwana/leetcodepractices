namespace LeetCode;

public class ThreadSafeCache <TKey, TValue> where TKey : notnull
{
    private readonly Dictionary<TKey, TValue> _dic = new Dictionary<TKey, TValue>();
    private readonly object _lock = new object();

    public void AddOrUpdate(TKey key, TValue value)
    {
        lock (_lock)
        {
            _dic[key] = value;
        }
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        lock (_lock)
        {
            return _dic.TryGetValue(key, out value!);
        }
    }
}