namespace LeetCode;

public class AsyncCachedDictionary<TKey, TValue> where TKey : notnull where TValue : new()
{
    public readonly Dictionary<TKey, TValue> _dict = new Dictionary<TKey, TValue>();

    private readonly SemaphoreSlim _slimLock = new SemaphoreSlim(1, 1);

    public async Task<TValue> AddOrUpdate(TKey key, TValue value)
    {
        await _slimLock.WaitAsync();

        try
        {
            if (_dict.TryGetValue(key, out var existingValue))
            {
                return existingValue;
            }

            TValue newValue = await DummyValueAsync(key);

            _dict.TryAdd(key, newValue);
            return newValue;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            _slimLock.Release();
        }
    }

    private async Task<TValue> DummyValueAsync(TKey key)
    {
        return new TValue();
    }
}